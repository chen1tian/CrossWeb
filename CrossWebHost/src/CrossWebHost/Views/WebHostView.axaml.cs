using Avalonia.Controls;
using CrossWebHost.ViewModels;
using System;
using System.Text.Json;

namespace CrossWebHost.Views;

public partial class WebHostView : UserControl
{
    public WebHostView()
    {
        InitializeComponent();
        WebView.NavigationStarted += (_, _) =>
            (DataContext as WebHostViewModel)?.ShowLoading();
        WebView.WebMessageReceived += OnWebMessageReceived;
    }

    private void OnWebMessageReceived(object? sender, WebMessageReceivedEventArgs e)
    {
        try
        {
            using var document = JsonDocument.Parse(NormalizeMessageBody(e.Body ?? "{}"));
            var root = document.RootElement;
            var messageType = root.TryGetProperty("type", out var typeElement)
                ? typeElement.GetString()
                : null;

            if (string.Equals(messageType, "crossweb.frontendReady", StringComparison.Ordinal))
                (DataContext as WebHostViewModel)?.SetFrontendReady();
        }
        catch (JsonException)
        {
            // 忽略非 JSON Web 消息；当前桥接协议只处理结构化消息。
        }
    }

    private static string NormalizeMessageBody(string body)
    {
        using var document = JsonDocument.Parse(body);
        return document.RootElement.ValueKind == JsonValueKind.String
            ? document.RootElement.GetString() ?? "{}"
            : body;
    }
}
