# 使用MVU模式

Model-View-Update（简称MVU）是一种函数式响应式架构模式，MVU提供了一种简洁、可预测且高效的方式来构建用户界面应用程序。

简单来说就是不需要再写xaml代码，使用函数来完成界面的构建。

## 集成 Avalonia.Markup.Declarative 到项目中

Avalonia.Markup.Declarative文档：[点击访问github项目](https://github.com/AvaloniaUI/Avalonia.Markup.Declarative/tree/master)

1. 添加`Avalonia.Markup.Declarative` NuGet包到项目中。
   ```bash
   dotnet add package Avalonia.Markup.Declarative
   ```
2. 在项目中增加组件，例如：
```csharp
    public class HomeScreen : ComponentBase
    {
        private TextBlock _textBlock1;

        //styles
        protected override StyleGroup? BuildStyles() =>
        [
            new Style<Button>()
            .Margin(6)
            .Background(Brushes.DarkSalmon),
    ];

        //markup part
        protected override object Build() =>
            new StackPanel()
                .Children(
                    new TextBlock()
                        .Ref(out _textBlock1)
                        .Text("Hello world"),
                    new TextBlock()
                        .Text(() => $"Counter: {(Counter == 0 ? "zero" : Counter)}"),
                    new Button()
                        .Content("Click me")                        
                        .OnClick(OnButtonClick),
                    new AvaPlot()
                        .Height(400)
                        .Width(600)
                );

        public int Counter { get; set; } //no need to implement AvaloniaProperty or OnPropertyChanged behaviors, since component has registry of all properties and emits ProperyChanged event after changing state of component.

        private void OnButtonClick(RoutedEventArgs e)
        {
            Counter++;
            _textBlock1.Text = $"Counter: {(Counter == 0 ? "zero" : Counter)}";
            StateHasChanged(); //for now we have to call this method manually. In future there will be some additional triggers like user input, that will rise this method automatically
        }
    }
```

3. 修改App.axaml.cs文件，设置MainWindow的内容为HomeScreen组件：
    ```csharp
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
        // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
        DisableAvaloniaDataAnnotationValidation();
        desktop.MainWindow = new MainWindow().Content(new HomeScreen()); // 修改此处
    }
    else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
    {
        singleViewPlatform.MainView = new MainView().Content(new HomeScreen()); // 修改此处
    }
    ```
4. 删除`Views\MainWindow.axaml`原来的Content，此时已经用HomeScreen代替
    ```xaml
    <Window xmlns="https://github.com/avaloniaui"
            <!--其他代码-->
            Title="CrossWebHost">
            <views:MainView /> // 删除这一行
    </Window>
    ```