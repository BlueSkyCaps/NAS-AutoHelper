using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MsBox.Avalonia;
using NAS_AutoHelper.Model;

namespace NAS_AutoHelper;

public partial class SettingWindow : Window
{
    public SettingWindow()
    {
        this.DataContext = MainWindow.Setting;
        InitializeComponent();
    }

    private void CancelBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private async void OkBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var sTbPingHost = TbPingHost.Text?.Trim();
        var sTbPingTickSeconds = TbPingTickSeconds.Value;
        var sTbCountdownSeconds = TbCountdownSeconds.Value;
        if (!Common.IsValidIPv4(sTbPingHost))
        {
            string msg = "无效的IPv4地址";
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("提示", msg);
                await box.ShowWindowDialogAsync(this);
            });
            return;
        }
        if ( sTbPingTickSeconds < 10)
        {
            string msg = "检测间隔(秒)：不少于10秒";
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("提示", msg);
                await box.ShowWindowDialogAsync(this);
            });
            return;
        }
        if (sTbCountdownSeconds<0)
        {
            string msg = "关机倒计时(秒)：请输入数字";
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("提示", msg);
                await box.ShowWindowDialogAsync(this);
            });
            return;
        }

        if (string.IsNullOrEmpty(TbMacPassword.Text))
        {
            string msg = "您确定你的锁屏密码是空吗？！\n" +
                         "由于macOS的安全机制，执行关机权限需要获得您的登录密码(非AppleID)。\n" +
                         "您必须去[系统设置]里设置有效的登录密码才能使用此应用！";
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("提示", msg);
                await box.ShowWindowDialogAsync(this);
            });
            return;
        }
        MainWindow.Setting.PingHost = sTbPingHost;
        MainWindow.Setting.PingTickSeconds = Convert.ToUInt32(sTbPingTickSeconds);
        MainWindow.Setting.CountdownSeconds = Convert.ToUInt32(sTbCountdownSeconds);
        MainWindow.Setting.MacPassword = TbMacPassword.Text;
        
        // 更新配置文件
        try
        {
            var serialize = System.Text.Json.JsonSerializer.Serialize(MainWindow.Setting);
            if (!File.Exists(Common.UserDataPath))
            {
                // 不存在则创建，表示首次打开应用！并且初始化主窗口
                Directory.CreateDirectory(Common.UserDataDir);
                var toWrite = File.CreateText(Common.UserDataPath);
                await toWrite.WriteAsync(serialize);
                toWrite.Close();
                new MainWindow().Show();
            }
            else
            {
                // 覆盖更新，表示是通过菜单点击的设置
                await File.WriteAllTextAsync(Common.UserDataPath, serialize);
                // 重新初始化主窗口逻辑 更新时间间隔
                MainWindow.InitNetLogic();
            }
        }
        catch(Exception ex)
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var box = MessageBoxManager
                    .GetMessageBoxStandard("致命错误", ""+ex.Message);
                await box.ShowWindowDialogAsync(this);
                Environment.Exit(1);
            });
        }
        this.Close();
    }
}