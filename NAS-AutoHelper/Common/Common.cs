using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace NAS_AutoHelper;

public static class Common
{
    public static readonly string UserDataDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".autoHelper-ups");
    public static readonly string UserDataPath = Path.Combine(UserDataDir, "Settings.json");
    
    public static bool IsValidIPv4(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            return false;

        var segments = ipAddress.Split('.');
        if (segments.Length != 4)
            return false;

        foreach (var segment in segments)
        {
            if (!int.TryParse(segment, out int value) || value < 0 || value > 255)
                return false;
        }

        return true;
    }
    /// <summary>
    /// 执行一个ping命令
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Status ExecutePing(string? command=null)
    {
        if (command == null)
        {
            command = $"ping -n 1 -w 3000 {MainWindow.Setting.PingHost}";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                command = $"ping -c1 -t3 {MainWindow.Setting.PingHost}";
            }
        }
        Status status = new();
        try
        {
            var (output, error) = ExecuteCommandBase.Instance.ExecuteCommand(command);

            // 如果错误流有数据，则断开：No route to host
            
            if (!string.IsNullOrWhiteSpace(error))
            {
                throw new Exception("路由器/网线/光猫/宽带与本设备断开连接-"+error);
            }
            /*
             但是存在这种情况：路由器仍在正常连接，但无法接入互联网：光猫/宽带/路由器无网络源
             这种情况是不会输出到错误流的，需要判断字符串是否包含：0 packets received
             */
            if (!string.IsNullOrWhiteSpace(output))
            {
                if (output.ToLower().Contains("0 packets received"))
                {
                    throw new Exception("路由器/网线/光猫/宽带无网络源-0 packets received");
                }
            }
            status.Message = output;
            status.Success = true;
            return status;  // 返回标准输出
        }
        catch (Exception e)
        {
            status.Message = e.Message;
            status.Success = false;
            return status;
        }
    }

    /// <summary>
    /// 执行关机命令
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public static Status ExecuteShutdown(string? command=null)
    {
        if (command == null)
        {
            command = "shutdown /s /f /t 0";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                command = $"echo \"{MainWindow.Setting.MacPassword}\" | sudo -S shutdown -h now";
            }
        }
        Status status = new();
        try
        {
            (string output, string error)  = ExecuteCommandBase.Instance
                .ExecuteCommand(command);
            // 如果错误流有数据
            if (!string.IsNullOrWhiteSpace(error))
            {
                /*
                 但是存在这种情况：mac执行echo sudo命令进行密码的自动填充，即使密码正确
                 但是仍会把“Password:”截断输出到错误流的，因此需要进一步判断
                 */
                if (! (error.ToLower().TrimStart().StartsWith("password") && ! error.ToLower().Contains("try again")))
                {
                    throw new Exception("密码有误，请尝试同步你的密码。"+error);
                }
            }
            // 此处正常早已关机 程序早已终止
            status.Message = output;
            status.Success = true;
            return status; 
        }
        catch (Exception e)
        {
            status.Message = e.Message;
            status.Success = false;
            return status;
        }
    }
}

public class Status
{
    public bool Success { get; set; }
    public int Code { get; set; }
    public string? Message { get; set; }
}