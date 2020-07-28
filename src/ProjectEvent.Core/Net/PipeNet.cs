using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Net
{
    /// <summary>
    /// 基于pipe的双向通讯类
    /// </summary>
    public class PipeNet
    {
        public delegate void PipeNetMsgEventHandler(PipeNet sender, string msg);

        /// <summary>
        /// 接收到消息时
        /// </summary>
        public event PipeNetMsgEventHandler OnReceiveMsg;

        /// <summary>
        /// 服务端
        /// </summary>
        private NamedPipeServerStream server;
        /// <summary>
        /// 客户端
        /// </summary>
        private NamedPipeClientStream client;

        /// <summary>
        /// 客户端已连接通知
        /// </summary>
        private const string connectedTag = "[000]";

        /// <summary>
        /// 系统消息
        /// </summary>
        private const string systemMsg = "[000]";
        /// <summary>
        /// 普通消息
        /// </summary>
        private const string normalMsg = "[001]";

        private StreamReader reader;
        private StreamWriter writer;

        private string serverName;
        private bool isConnected = false;
        private bool isServerOpened = false;
        public PipeNet(string serverName)
        {
            this.serverName = serverName;
        }

        /// <summary>
        /// 启动主服务
        /// </summary>
        public void StartMain()
        {
            if (isServerOpened)
            {
                return;
            }
            StartServer(serverName);
        }

        /// <summary>
        /// 启动次服务（必须在主服务已启动成功后调用）
        /// </summary>
        public void Start()
        {
            if (isServerOpened)
            {
                return;
            }
            Task.Factory.StartNew(() =>
            {
                Connect(serverName);
                StartClientReceiveServer();
            });
        }
        public void Send(string content)
        {
            SendMsg(content);
        }
        public void Close()
        {
            isServerOpened = false;
            isConnected = false;
            server?.Close();
            client?.Close();
            reader?.Dispose();
        }
        private void Connect(string server)
        {
            client = new NamedPipeClientStream(server);
            client.Connect();
            writer = new StreamWriter(client);
            writer.AutoFlush = true;
            isConnected = true;
            Debug.WriteLine("已成功连接到：" + server);
        }
        private void StartServer(string name)
        {
            Task.Factory.StartNew(() =>
            {
                server = new NamedPipeServerStream(name);
                server.WaitForConnection();
                reader = new StreamReader(server);
                isServerOpened = true;
                Debug.WriteLine("已成功启动服务：" + name);
                try
                {
                    string temp;
                    while (isServerOpened && (temp = reader.ReadLine()) != null)
                    {
                        HandleReceive(temp);
                    }
                }
                catch (IOException e)
                {
                    isServerOpened = false;
                    Console.WriteLine("ERROR: {0}", e.Message);
                }

            });
        }
        private void StartClientReceiveServer()
        {
            //启动客户端接收服务
            string server = $"{serverName}_receive";
            StartServer(server);
            //通知服务端
            SendMsg($"{connectedTag}{server}", true);
        }
        private void SendMsg(string content, bool isSystemMsg = false)
        {
            if (isConnected)
            {
                string pre = isSystemMsg ? systemMsg : normalMsg;
                writer.WriteLine($"{pre}{content}");
            }
        }
        private void HandleReceive(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                string type = content.Substring(0, 5);
                if (type == systemMsg)
                {
                    //系统消息
                    string msg = content.Substring(5, 5);
                    if (msg == connectedTag)
                    {
                        //客户端连接成功回复
                        //客户端的服务器名
                        string clientServerName = content.Substring(10);
                        //连接到客户端的接收服务
                        Connect(clientServerName);
                        Debug.WriteLine("客户端已连接，接收服务：" + clientServerName);
                    }
                }
                else
                {
                    //普通消息
                    string msg = content.Substring(5);
                    //响应事件
                    OnReceiveMsg?.Invoke(this, msg);
                    Debug.WriteLine("收到消息：" + msg);
                }
            }
        }
    }
}
