using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NAS_AutoHelper;

public class TcpConnect
{
    private void InitTcpConnect()
    {
        BuildTcpServer();
        SendTcpServerMessage();
    }

    /// <summary>
    /// 发送数据到本地TCP服务
    /// </summary>
    private static void SendTcpServerMessage()
    {
        string token = "@UPS!ShutdownCanceled";
        string ipAddress = "127.0.0.1";  
        int port = 12345;  // 服务端的端口号

        try
        {
            using var tcpClient = new TcpClient(ipAddress, port);
            // 获取网络流
            NetworkStream networkStream = tcpClient.GetStream();
            // 将消息转换为字节数组
            var data = Encoding.UTF8.GetBytes(token);

            // 发送数据
            networkStream.Write(data, 0, data.Length);
            // 关闭 客户端
            networkStream.Close();
            tcpClient.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("send不过 "+ e);
            throw;
        }
    }

    /// <summary>
    /// 构建本地TCP连接服务端 监听后续请求
    /// </summary>
    private static void BuildTcpServer()
    {
        Task.Run(() =>
        {
            try
            {
                int serverPort = 12345; // 监听的端口号
                // 创建 TcpListener 实例并开始监听
                var tcpListener = new TcpListener(IPAddress.Loopback, serverPort);
                tcpListener.Start();
                Console.WriteLine("Server is listening...");

                // 接受客户端连接
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                Console.WriteLine("Client connected.");
                // 获取网络流
                NetworkStream networkStream = tcpClient.GetStream();

                // 读取数据
                var buffer = new byte[1024];  // 创建一个缓冲区来存放接收到的数据
                var bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                var receivedMessage = Encoding.UTF8.GetString(buffer);
                Console.WriteLine($"Received message: {receivedMessage}");

                // 关闭连接
                networkStream.Close();
                tcpClient.Close();
                tcpListener.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("的bug++ "+e);
            }
            
        });
    }
}