namespace Host.ViewModels;

public sealed class WebHostViewModel : ViewModelBase
{
    private string _webFrontendUrl = "about:blank";
    private string _statusText = "正在启动本地服务…";
    private bool _isWebViewVisible;
    private bool _isLoadingVisible = true;
    private bool _hasInitializationError;

    public string WebFrontendUrl
    {
        get => _webFrontendUrl;
        private set => SetProperty(ref _webFrontendUrl, value);
    }

    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    public bool IsWebViewVisible
    {
        get => _isWebViewVisible;
        private set => SetProperty(ref _isWebViewVisible, value);
    }

    public bool IsLoadingVisible
    {
        get => _isLoadingVisible;
        private set => SetProperty(ref _isLoadingVisible, value);
    }

    public bool HasInitializationError
    {
        get => _hasInitializationError;
        private set => SetProperty(ref _hasInitializationError, value);
    }

    public void SetReady(string webFrontendUrl)
    {
        HasInitializationError = false;
        StatusText = "正在载入前端…";
        WebFrontendUrl = webFrontendUrl;
    }

    public void ShowLoading()
    {
        if (HasInitializationError)
            return;

        IsLoadingVisible = true;
        IsWebViewVisible = false;
        StatusText = "正在载入前端…";
    }

    public void SetFrontendReady()
    {
        HasInitializationError = false;
        IsLoadingVisible = false;
        IsWebViewVisible = true;
        StatusText = "前端已就绪";
    }

    public void SetInitializationFailed(string message)
    {
        HasInitializationError = true;
        IsLoadingVisible = true;
        IsWebViewVisible = false;
        StatusText = $"启动失败：{message}";
    }
}
