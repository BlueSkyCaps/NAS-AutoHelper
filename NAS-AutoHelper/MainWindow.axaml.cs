using System;
using System.IO;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using NAS_AutoHelper.Model;

namespace NAS_AutoHelper;

/// <summary>
/// 在后台一直隐藏运行 无法正常关闭的主窗口
/// </summary>
public partial class MainWindow : Window
{
    private static Timer? _mainPingTimer;
    private static Timer? _shutdownTimer;
    public static SettingModel Setting { get; set; } = new();
    public MainWindow()
    {
        // 确保当前只有一个此应用实例在运行
        var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        if (System.Diagnostics.Process.GetProcessesByName(processName).Length>1)
        {
            Console.WriteLine(processName);
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                string msg = "已有一个UPS自动关机应用正在运行，无法重复启动。\n" +
                             "可在桌面 [菜单栏/托盘] 中点击应用图标进行操作。\n" +
                             "如果你无法找到应用图标，请在 [系统设置] 里让它重新显示，然后重启电脑。";
                var box = MessageBoxManager
                    .GetMessageBoxStandard("提示", msg);
                var y = await box.ShowAsync();
                // 到这一步 即使点击了是，程序也不会被退出。因为主窗口守护已经创建了。只能等待提示框被点击后主动退出当前程序
                Environment.Exit(0);
            });
            // 直接return 不让他执行后续的初始化逻辑
            return;
        }
        //  显示托盘
        TrayIcon.GetIcons(Application.Current).FirstOrDefault().IsVisible = true;
        InitializeComponent();
        // 开始网络监测
        InitNetLogic();
        
    }

    /// <summary>
    /// 启动网络监测： 当检测到断网时停止、当点击撤销关机按钮后可被再次调用启动
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public static void InitNetLogic()
    {
        Console.WriteLine("InitNetLogic");
        // 无论如何，先关闭关机的Timer。即使第一次它还没被创建。用于后续点击撤销关机按钮时统一关闭
        _shutdownTimer?.Dispose();
        _mainPingTimer?.Dispose();
        // 启动：每倒计时秒钟触发一次Tick方法 ping路由 
        _mainPingTimer = new Timer(PingTickAction, null, TimeSpan.FromSeconds(Setting.PingTickSeconds), TimeSpan.FromSeconds(Setting.PingTickSeconds)); 
    }
    
    private static void PingTickAction(object? state)
    {
        // 先暂停网络监测 避免执行重复 等到当前的ping操作执行完毕，根据结果再判断是否重启监测
        _mainPingTimer?.Dispose();
        // ping 光猫IP是最直接有效的
        var status = Common.ExecutePing();
        Console.WriteLine(status.Message);
        if (! status.Success)
        {
            // 断网 执行倒计时关机
            Console.WriteLine("网络未连接");
            // 启动UI倒计时提示界面
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                new TickWindow().Show();
            });
            // X秒后执行的关机timer，无限期间隔等待意味它只会执行一次
            _shutdownTimer = new Timer(ShutdownOnceTickAction,null, TimeSpan.FromSeconds(Setting.CountdownSeconds), Timeout.InfiniteTimeSpan);
        }
        else
        {
            // 正常，立即重启网络监测Timer
            InitNetLogic();
        }
    }
    
    private static void ShutdownOnceTickAction(object? state)
    {

        Console.WriteLine("开始执行关机 ShutdownOnceTickAction");
        // Mac 使用 sudo 执行关机命令，需要管理员权限。windows无需
        var status = Common.ExecuteShutdown();
        if (status.Success)
        {
            Console.WriteLine("关机执行成功，返回数据："+status.Message);
        }
        else
        {
            Console.WriteLine("执行关机失败 ShutdownOnceTickAction"+status.Message);
            _mainPingTimer?.Dispose();
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                // 执行关机命令失败 弹出提示
                string msg = "当您看到此信息，意味着您的设备已经过断电断网，但是我们无法正常关机。\n" +
                             "请查看您的电源是否电量不足，若是，请立即手动关机以防止断电造成的设备损害。\n"+
                           $"(错误提示：{status.Message})";
                var box = MessageBoxManager
                    .GetMessageBoxStandard("UPS奔溃", msg);
                await box.ShowAsync();
                Environment.Exit(1);
            });
        }
    }

    protected override void OnOpened(EventArgs e)
    {
        this.ShowActivated = false;
        this.ShowInTaskbar = false;
        //  当打开窗口时立即隐藏主窗口 IsVisible在Mac无效
        this.Hide();
        base.OnOpened(e);
    }

    /// <summary>
    /// 当窗口关闭时，阻止
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;
    }
}