using CrossWebHost.EntityFramework;
using CrossWebHost.EntityFramework.MiddleWares;
using CrossWebHost.Services;
using CrossWebHost.ViewModels;
using CrossWebHost.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;

namespace CrossWebHost;

public partial class App : Avalonia.Application
{
    private IAbpApplicationWithInternalServiceProvider? _abpApplication;
    private PicoServerService? _picoServer;

    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    public static Action<IServiceCollection>? ConfigurePlatformServices { get; set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", (Serilog.Events.LogEventLevel)LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File($"Logs/logs_{DateTime.Now:yyyyMMdd}.txt"))
                .CreateLogger();

        var webViewModel = new WebHostViewModel();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Exit += OnDesktopExit;
            desktop.MainWindow = new MainWindow
            {
                DataContext = webViewModel
            };
        }
        else if (ApplicationLifetime is IActivityApplicationLifetime activityLifetime)
        {
            activityLifetime.MainViewFactory = () => new WebHostView
            {
                DataContext = webViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new WebHostView
            {
                DataContext = webViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
        _ = InitializeHostAsync(webViewModel);
    }

    private async Task InitializeHostAsync(WebHostViewModel webViewModel)
    {
        try
        {
            // 根据平台确定可写的数据库目录，并在 ABP 初始化前注入连接字符串
            var dbDir = GetDatabaseDirectory();
            if (!Directory.Exists(dbDir))
                Directory.CreateDirectory(dbDir);
            var dbConnStr = $"Data Source={Path.Combine(dbDir, "CrossWebHost.db")}";

            _abpApplication = await AbpApplicationFactory.CreateAsync<CrossWebHostModule>(options =>
            {
                options.UseAutofac();
                options.Services.ReplaceConfiguration(
                    BuildAppConfiguration(dbConnStr)
                );

                // 注册平台特定服务
                ConfigurePlatformServices?.Invoke(options.Services);
            });
            await _abpApplication.InitializeAsync();
            ServiceProvider = _abpApplication.ServiceProvider;

            // 自动迁移数据库
            ServiceProvider.UseAutoMigration<CrossWebHostDbContext>();

            _picoServer = ServiceProvider.GetRequiredService<PicoServerService>();
            await _picoServer.StartAsync();
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            await Dispatcher.UIThread.InvokeAsync(() =>
                webViewModel.SetReady($"{PicoServerService.BaseAddress}/"));
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "应用初始化失败");
            await Dispatcher.UIThread.InvokeAsync(() =>
                webViewModel.SetInitializationFailed(ex.Message));
        }
    }

    private void OnDesktopExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        StopPicoServer();
    }

    private void OnProcessExit(object? sender, EventArgs e)
    {
        StopPicoServer();
    }

    private void StopPicoServer()
    {
        AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
        _picoServer?.Stop();
        Log.CloseAndFlush();
    }

    private static string GetDatabaseDirectory()
    {
        // Android 上 SpecialFolder.Personal 对应应用私有可写目录
        if (OperatingSystem.IsAndroid())
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "db");
        return Path.Combine(AppContext.BaseDirectory, "db");
    }

    /// <summary>
    /// 从资源中构建appsettings，避免Android平台appsetting.json不更新的问题
    /// </summary>
    /// <param name="dbConnStr"></param>
    /// <returns></returns>
    private static IConfiguration BuildAppConfiguration(string dbConnStr)
    {
        var builder = new ConfigurationBuilder();
        var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        MemoryStream? embeddedConfigStream = null;

        if (File.Exists(appSettingsPath))
        {
            builder.SetBasePath(AppContext.BaseDirectory)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
        }
        else
        {
            var configUri = new Uri("avares://CrossWebHost/appsettings.json");
            using var stream = AssetLoader.Open(configUri);
            if (stream != null)
            {
                embeddedConfigStream = new MemoryStream();
                stream.CopyTo(embeddedConfigStream);
                embeddedConfigStream.Position = 0;
                builder.AddJsonStream(embeddedConfigStream);
            }
        }

        builder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:Default"] = dbConnStr
        });

        var configuration = builder.Build();
        embeddedConfigStream?.Dispose();
        return configuration;
    }
}
