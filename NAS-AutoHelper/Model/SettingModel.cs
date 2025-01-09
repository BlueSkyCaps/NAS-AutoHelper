using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NAS_AutoHelper.Model;

public class SettingModel:INotifyPropertyChanged
{
    private string _pingHost;
    private string _macPassword;
    private bool _already = false;
    private uint _pingTickSeconds = 20;
    private uint _countdownSeconds = 60;

    public string PingHost
    {
        get => _pingHost;
        set
        {
            if (value == _pingHost) return;
            _pingHost = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Mac系统需要征得获取密码
    /// </summary>
    public string MacPassword
    {
        get => _macPassword;
        set
        {
            if (value == _macPassword) return;
            _macPassword = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 是否第一次打开填写过设置
    /// </summary>
    public bool Already
    {
        get => _already;
        set
        {
            if (value == _already) return;
            _already = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 设置检测网络的时间间隔
    /// </summary>
    public uint PingTickSeconds
    {
        get => _pingTickSeconds;
        set
        {
            if (value == _pingTickSeconds) return;
            _pingTickSeconds = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 设置倒计时关机的秒数
    /// </summary>
    public uint CountdownSeconds
    {
        get => _countdownSeconds;
        set
        {
            if (value == _countdownSeconds) return;
            _countdownSeconds = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}