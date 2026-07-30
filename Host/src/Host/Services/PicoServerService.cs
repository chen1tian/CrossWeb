using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
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
    public const string AccessibleUrlsPath = "/api/host/urls";

    private readonly object _lifecycleLock = new();
    private readonly WebAPIServer _server;
    private string? _webRootDirectory;
    private bool _disposed;
    private bool _started;

    public PicoServerService()
    {
        _server = new WebAPIServer();
        _server.AddCors();
        _server.MapGet(PingPath, PingAsync);
        _server.MapGet(AccessibleUrlsPath, AccessibleUrlsAsync);
    }

    public bool IsRunning => _server.IsRunning;

    public async Task StartAsync()
    {
        _webRootDirectory = await ExtractStaticWebAsync();

        lock (_lifecycleLock)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_started)
                return;

            _server.AddStaticFiles("/", _webRootDirectory);
            _server.StartServer(Port);
            _started = true;
        }

        if (!await WaitUntilReachableAsync())
        {
            lock (_lifecycleLock)
                _started = false;

            throw new InvalidOperationException(
                $"PicoServer 无法监听 {ListenerPrefix}。"
                + "请确认应用已通过管理员权限启动，并且端口未被占用。"
            );
        }

        Log.Information(
            "PicoServer 已启动：{ListenerPrefix}；前端目录：{WebRootDirectory}",
            ListenerPrefix,
            _webRootDirectory
        );
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

    private static Task AccessibleUrlsAsync(
        HttpListenerRequest _,
        HttpListenerResponse response
    )
    {
        var payload = JsonSerializer.Serialize(new
        {
            urls = GetAccessibleUrls()
        });
        return response.WriteAsync(payload, WebAPIServer.ContentType.ApplicationJson);
    }

    private static IReadOnlyList<string> GetAccessibleUrls()
    {
        var urls = new List<string>
        {
            $"{BaseAddress}/"
        };

        try
        {
            urls.AddRange(
                NetworkInterface.GetAllNetworkInterfaces()
                    .Where(network =>
                        network.OperationalStatus == OperationalStatus.Up
                        && network.NetworkInterfaceType is not
                            NetworkInterfaceType.Loopback
                        && network.NetworkInterfaceType is not
                            NetworkInterfaceType.Tunnel)
                    .SelectMany(network =>
                        network.GetIPProperties().UnicastAddresses)
                    .Select(address => address.Address)
                    .Where(address =>
                        address.AddressFamily == AddressFamily.InterNetwork
                        && !IPAddress.IsLoopback(address)
                        && !address.ToString().StartsWith(
                            "169.254.",
                            StringComparison.Ordinal))
                    .Select(address => $"http://{address}:{Port}/")
            );
        }
        catch (NetworkInformationException exception)
        {
            Log.Warning(exception, "无法枚举可访问的局域网地址");
        }

        return urls
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(url => url, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static async Task<string> ExtractStaticWebAsync()
    {
        var manifestUri = new Uri(
            "avares://Host/Assets/Web/avweb-host-web-manifest.json"
        );
        if (!AssetLoader.Exists(manifestUri))
        {
            throw new InvalidOperationException(
                "未找到内嵌前端资源。请先在 Frontend 目录运行 npm run build:host。"
            );
        }

        var appDataRoot = OperatingSystem.IsAndroid()
            ? Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var targetRoot = Path.Combine(appDataRoot, "AvWeb", "host-web");

        if (Directory.Exists(targetRoot))
            Directory.Delete(targetRoot, recursive: true);
        Directory.CreateDirectory(targetRoot);

        await using var manifestStream = AssetLoader.Open(manifestUri);
        using var manifest = await JsonDocument.ParseAsync(manifestStream);
        if (!manifest.RootElement.TryGetProperty("files", out var files)
            || files.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("前端资源清单缺少 files 数组。");
        }

        foreach (var file in files.EnumerateArray())
        {
            var relativePath = file.GetString()?.Replace('\\', '/').TrimStart('/');
            if (string.IsNullOrWhiteSpace(relativePath)
                || relativePath.Split('/', StringSplitOptions.RemoveEmptyEntries)
                    .Any(segment => string.Equals(segment, "..", StringComparison.Ordinal)))
            {
                throw new InvalidOperationException("前端资源清单包含不安全路径。");
            }

            var assetPath = string.Join(
                "/",
                relativePath.Split('/').Select(Uri.EscapeDataString)
            );
            var sourceUri = new Uri($"avares://Host/Assets/Web/{assetPath}");
            if (!AssetLoader.Exists(sourceUri))
                throw new FileNotFoundException($"内嵌前端资源不存在：{relativePath}");

            var targetPath = Path.Combine(
                targetRoot,
                relativePath.Replace('/', Path.DirectorySeparatorChar)
            );
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
            await using var sourceStream = AssetLoader.Open(sourceUri);
            await using var targetStream = File.Create(targetPath);
            await sourceStream.CopyToAsync(targetStream);
        }

        return targetRoot;
    }

    private static async Task<bool> WaitUntilReachableAsync()
    {
        using var client = new HttpClient
        {
            Timeout = TimeSpan.FromMilliseconds(500)
        };
        var deadline = DateTime.UtcNow.AddSeconds(5);

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                using var response = await client.GetAsync($"{BaseAddress}{PingPath}");
                var body = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode
                    && string.Equals(body.Trim(), "pong", StringComparison.Ordinal))
                {
                    return true;
                }
            }
            catch (HttpRequestException)
            {
            }
            catch (TaskCanceledException)
            {
            }

            await Task.Delay(100);
        }

        return false;
    }
}
