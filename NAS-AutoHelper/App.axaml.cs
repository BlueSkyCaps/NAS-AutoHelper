using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MsBox.Avalonia;

namespace NAS_AutoHelper;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {

        InitSetting();
    }
    
    /// <summary>
    /// 初始化加载配置等
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private  void InitSetting()
    {
        // 加载配置文件
        if (!Path.Exists(Common.UserDataPath))
        {
            // 无配置文件，则是首次打开：弹出设置窗口！
           SettingWindow settingWindow = new SettingWindow();
           settingWindow.Show();
        }
        else
        {
            var settingText = File.ReadAllText(Common.UserDataPath);
            MainWindow.Setting = System.Text.Json.JsonSerializer.Deserialize<Model.SettingModel>(settingText);
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }

    private void ExitItem_OnClick(object? sender, EventArgs e)
    {
        Environment.Exit(0);
    }

    private void SettingItem_OnClick(object? sender, EventArgs e)
    {
        new SettingWindow().Show();
    }

    private void AboutItem_OnClick(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync( () =>
        {
            string msg = "此应用常驻后台，若你的电脑断网，则会自动关机，防止意外断电造成设备损害。\n" +
                         "若你的NAS没有提供Windows或macOS的自动关机程序，这是简单的代替品。\n" +
                         "可在桌面 [菜单栏/托盘] 中点击应用图标进行操作。\n" +
                         "如果你无法找到应用图标，请在 [系统设置] 里让它重新显示，然后重启电脑。\n" +
                         "小红书/抖音/B站关注@芝士虾米。"; 
            var box = MessageBoxManager
                .GetMessageBoxStandard("提示", msg);
             box.ShowAsync();
        });
    }
}