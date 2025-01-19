using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace UPS_AutoHelper;

public partial class TickWindow : Window
{
    private Timer? _timer;
    public   StyledProperty<uint> SecondsProperty =
        AvaloniaProperty.Register<TextBlock, uint>(nameof(Seconds), defaultValue: MainWindow.Setting.CountdownSeconds);
    public uint Seconds
    {
        get => GetValue(SecondsProperty);
        set => SetValue(SecondsProperty, value);
    }

    public TickWindow()
    {
        InitializeComponent();
        // 初始化倒计时的UI计时器⏱️
        InitTimer();
    }

    private void InitTimer()
    {
        _timer = new Timer(Tick, null, 1000, 1000); // 每秒钟触发一次Tick方法
    }

    private void Tick(object? state)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (Seconds > 0)
            {
                Seconds -= 1;
            }
            else
            {
                QuitUi();
            }
        });


    }

    private void QuitUi()
    {
        _timer?.Dispose(); // 停止计时器
        Console.WriteLine("QuitUi");
        CancelBtn.IsEnabled = false;
        // System.Environment.Exit(0);
    }

    protected override void OnOpened(EventArgs e)
    {
        Console.WriteLine();
        base.OnOpened(e);
    }

    /// <summary>
    /// 点击了关机撤销按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CancelBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        _timer?.Dispose(); // 停止计时器
        CancelBtn.IsEnabled = false;
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            CancelBtn.Content = "已阻止关机。";
        });
        
        // 访问主窗口，停止倒计时关机的Timer、重启网络监测
        MainWindow.InitNetLogic();
    }

    /// <summary>
    /// 当窗口关闭时，阻止
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        // 没有点击撤销按钮，阻止关闭。只有点击了撤销按钮，才可以关闭
        if (CancelBtn.IsEnabled)
        {
            e.Cancel = true;
        }
    }
}