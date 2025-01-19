using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UPS_AutoHelper;

public class ExecuteCommandBase
{
    private ExecuteCommandBase()
    {
        
    }

    public static ExecuteCommandBase Instance { get; } = new();
    
    /// <summary>
    /// 无头模式打开shell执行命令 兼容各个操作系统
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public  (string output, string error) ExecuteCommand(string command)
    {
        // 创建一个新的进程对象
        using Process process = new Process();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments =$"/C {command}";  
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            process.StartInfo.FileName = "/bin/zsh";  // 使用 zsh 来执行命令
            process.StartInfo.Arguments = $"-c \"{command}\"";  // 使用 -c "" 参数传递命令
        }
        process.StartInfo.RedirectStandardOutput = true;  // 重定向标准输出
        process.StartInfo.RedirectStandardError = true;  // 重定向错误输出
        process.StartInfo.UseShellExecute = false;  // 禁用外壳程序执行
        process.StartInfo.CreateNoWindow = true;  // 不创建新窗口

        // 启动进程并等待完成
        process.Start();
        process.WaitForExit();  // 等待进程结束
        string output = process.StandardOutput.ReadToEnd();  // 读取标准输出
        string error = process.StandardError.ReadToEnd();    // 读取标准错误
        return (output, error);
    }
}