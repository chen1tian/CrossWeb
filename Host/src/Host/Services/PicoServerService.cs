using System;
using System.Net;
using System.Threading.Tasks;
using PicoServer;
using Serilog;

namespace Host.Services;

public sealed class PicoServerService : IDisposable
{
    public const int Port = 8090;
    public const string BaseAddress = "http://127.0.0.1:8090";
    public const string ListenerPrefix = "http://+:8090/";
    public const string PingPath = "/api/ping";

    private readonly object _lifecycleLock = new();
    private readonly WebAPIServer _server;
    private bool _disposed;
    private bool _started;

    public PicoServerService()
    {
        _server = new WebAPIServer();
        _server.AddCors();
        _server.MapGet(PingPath, PingAsync);
    }

    public bool IsRunning => _server.IsRunning;

    public async Task StartAsync()
    {
        lock (_lifecycleLock)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_started)
                return;

            _server.StartServer(Port);
            _started = true;
        }

        var deadline = DateTime.UtcNow.AddSeconds(3);
        while (!_server.IsRunning && DateTime.UtcNow < deadline)
            await Task.Delay(50);

        if (!_server.IsRunning)
        {
            lock (_lifecycleLock)
                _started = false;

            throw new InvalidOperationException(
                $"PicoServer 无法监听 {ListenerPrefix}。"
                + "Windows 普通用户可能需要先配置对应的 URL ACL。"
            );
        }

        Log.Information("PicoServer 已启动：{ListenerPrefix}", ListenerPrefix);
    }

    public void Stop()
    {
        lock (_lifecycleLock)
        {
            if (!_started)
                return;

            _server.StopServer();
            _started = false;

            Log.Information("PicoServer 已停止");
        }
    }

    public void Dispose()
    {
        lock (_lifecycleLock)
        {
            if (_disposed)
                return;

            _disposed = true;
        }

        Stop();
    }

    private static Task PingAsync(HttpListenerRequest _, HttpListenerResponse response)
    {
        return response.WriteAsync("pong", "text/plain; charset=utf-8");
    }
}
