using Minecraft_QQ_Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Minecraft_QQ_Core.Robot
{
    public class RobotSocket
    {
        private Socket Socket;
        private Thread ReadThread;
        private Thread DoThread;
        private bool IsRun;
        private bool IsConnect;
        private ConcurrentBag<RobotTask> QueueRead;
        private ConcurrentBag<byte[]> QueueSend;
        private readonly PackStart PackStart = new()
        {
            Name = "Minecraft_QQ",
            Reg = new()
            { 49 }
        };

        private readonly Minecraft_QQ Main;

        public RobotSocket(Minecraft_QQ Minecraft_QQ)
        {
            Main = Minecraft_QQ;
        }

        public void Start()
        {
            QueueRead = new();
            QueueSend = new();
            DoThread = new(() =>
            {
                while (IsRun)
                {
                    try
                    {
                        if (QueueRead.TryTake(out RobotTask task))
                        {
                            switch (task.Index)
                            {
                                case 49:
                                    var pack = JsonConvert.DeserializeObject<GroupMessageEventPack>(task.Data);
                                    if (pack.qq == Main.MainConfig.机器人设置.QQ机器人账户)
                                        Main.GroupMessage(pack.id, pack.fid, pack.message);
                                    break;
                            }
                        }
                        Thread.Sleep(30);
                    }
                    catch (Exception e)
                    {
                        Logs.LogError(e);
                    }
                }
            });

            ReadThread = new(() =>
            {
                while (!IsRun)
                {
                    Thread.Sleep(100);
                }
                DoThread.Start();
                int time = 0;
                while (IsRun)
                {
                    try
                    {
                        if (!IsConnect)
                        {
                            ReConnect();
                        }
                        else if (Socket.Available > 0)
                        {
                            var data = new byte[Socket.Available];
                            Socket.Receive(data);
                            var type = data[^1];
                            data[^1] = 0;
                            QueueRead.Add(new()
                            {
                                Index = type,
                                Data = Encoding.UTF8.GetString(data)
                            });
                        }
                        else if (time >= 1000)
                        {
                            time = 0;
                            if (Main.MainConfig.机器人设置.检查是否断开 && Socket.Poll(2000, SelectMode.SelectRead))
                            {
                                Logs.LogOut("机器人连接中断");
                                IsConnect = false;
                            }
                        }
                        else if (QueueSend.TryTake(out byte[] Send))
                        {
                            Socket.Send(Send);
                        }
                        time++;
                        Thread.Sleep(10);
                    }
                    catch (Exception e)
                    {
                        Logs.LogError("机器人连接失败");
                        Logs.LogError(e);
                        IsConnect = false;
                        Logs.LogError("机器人" + Main.MainConfig.机器人设置.自动重连延迟 + "毫秒后重连");
                        Thread.Sleep(Main.MainConfig.机器人设置.自动重连延迟);
                        Logs.LogError("机器人重连中");
                    }
                }
            });
            ReadThread.Start();
            IsRun = true;
        }
        private void ReConnect()
        {
            if (Socket != null)
                Socket.Close();
            Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(IPAddress.Parse(Main.MainConfig.机器人设置.地址),
                Main.MainConfig.机器人设置.端口);

            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PackStart) + " ");
            data[^1] = 0;

            Socket.Send(data);

            while (Socket.Available == 0)
            {
                Thread.Sleep(10);
            }

            data = new byte[Socket.Available];
            Socket.Receive(data);

            QueueRead.Clear();
            QueueSend.Clear();
            Logs.LogOut("机器人已连接");
            IsConnect = true;
        }
        public void SendGroupMessage(long id, string message)
        {
            SendGroupMessage(id, new List<string>() { message });
        }
        public void SendGroupMessage(long id, List<string> message)
        {
            var data = BuildPack.Build(new SendGroupMessagePack { qq = Main.MainConfig.机器人设置.QQ机器人账户, id = id, message = message }, 52);
            QueueSend.Add(data);
        }
        public void SendGroupPrivateMessage(long id, long fid, string message)
        {
            var data = BuildPack.Build(new SendGroupPrivateMessagePack
            { qq = Main.MainConfig.机器人设置.QQ机器人账户, id = id, fid = fid, message = new List<string>() { message } }, 53);
            QueueSend.Add(data);
        }
        public void SendStop()
        {
            var data = BuildPack.Build(new object(), 127);
            Socket.Send(data);
        }
        public void Stop()
        {
            Logs.LogOut("机器人正在断开");
            if (IsConnect)
                SendStop();
            IsRun = false;
            if (Socket != null)
                Socket.Close();
            Logs.LogOut("机器人已断开");
        }
    }
}
