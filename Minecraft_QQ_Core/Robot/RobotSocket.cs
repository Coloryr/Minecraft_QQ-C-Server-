using Minecraft_QQ.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Minecraft_QQ.Robot
{
    class RobotSocket
    {
        private static Socket Socket;
        private static Thread ReadThread;
        private static Thread DoThread;
        private static bool IsRun;
        private static bool IsConnect;
        private static ConcurrentBag<RobotTask> QueueRead;
        private static ConcurrentBag<byte[]> QueueSend;
        private static PackStart PackStart = new PackStart
        {
            Name = "Minecraft_QQ",
            Reg = new List<byte>()
            { 49 }
        };
        public static void Start()
        {
            QueueRead = new ConcurrentBag<RobotTask>();
            QueueSend = new ConcurrentBag<byte[]>();
            DoThread = new Thread(() =>
            {
                RobotTask task;
                while (IsRun)
                {
                    try
                    {
                        if (QueueRead.TryTake(out task))
                        {
                            switch (task.index)
                            {
                                case 49:
                                    var pack = JsonConvert.DeserializeObject<GroupMessageEventPack>(task.data);
                                    Minecraft_QQ.GroupMessage(pack.id, pack.fid, pack.message);
                                    break;
                            }
                        }
                        Thread.Sleep(10);
                    }
                    catch (Exception e)
                    {
                        Logs.LogError(e);
                    }
                }
            });

            ReadThread = new Thread(() =>
            {
                while (!IsRun)
                {
                    Thread.Sleep(100);
                }
                DoThread.Start();
                byte[] Send;
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
                            var type = data[data.Length - 1];
                            data[data.Length - 1] = 0;
                            QueueRead.Add(new RobotTask
                            {
                                index = type,
                                data = Encoding.UTF8.GetString(data)
                            });
                        }
                        else if (Socket.Poll(-1, SelectMode.SelectRead))
                        {
                            Logs.LogOut("机器人连接中断");
                            IsConnect = false;
                        }
                        else if (QueueSend.TryTake(out Send))
                        {
                            Socket.Send(Send);
                        }
                        Thread.Sleep(50);
                    }
                    catch (Exception e)
                    {
                        Logs.LogError("机器人连接失败");
                        Logs.LogError(e);
                        IsConnect = false;
                        Logs.LogError("机器人20秒后重连");
                        Thread.Sleep(20000);
                        Logs.LogError("机器人重连中");
                    }
                }
            });
            ReadThread.Start();
            IsRun = true;
        }
        private static void ReConnect()
        {
            if (Socket != null)
                Socket.Close();
            try
            {
                Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                Socket.Connect(IPAddress.Parse("127.0.0.1"), 23333);

                var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PackStart) + " ");
                data[data.Length - 1] = 0;

                Socket.Send(data);

                QueueRead.Clear();
                QueueSend.Clear();
                Logs.LogOut("机器人已连接");
                IsConnect = true;
            }
            catch (Exception e)
            {
                Logs.LogError("机器人连接失败");
                Logs.LogError(e);
            }
        }
        public static void SendGroupMessage(long id, string message)
        {
            SendGroupMessage(id, new List<string>() { message });
        }
        public static void SendGroupMessage(long id, List<string> message)
        {
            var data = BuildPack.Build(new SendGroupMessagePack { id = id, message = message }, 52);
            QueueSend.Add(data);
        }
        public static void SendGroupPrivateMessage(long id, long fid, string message)
        {
            var data = BuildPack.Build(new SendGroupPrivateMessagePack
            { id = id, fid = fid, message = new List<string>() { message } }, 53);
            QueueSend.Add(data);
        }
        public static void Stop()
        {
            Logs.LogOut("机器人正在断开");
            IsRun = false;
            if (Socket != null)
                Socket.Close();
            Logs.LogOut("机器人已断开");
        }
    }
}
