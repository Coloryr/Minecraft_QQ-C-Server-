﻿using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minecraft_QQ_Core.Robot
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
            Name = "ColoryrSDK",
            Reg = new List<byte>()
            { 49, 50, 51 }
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
                                    Console.WriteLine("id = " + pack.id);
                                    Console.WriteLine("fid = " + pack.fid);
                                    Console.WriteLine("name = " + pack.name);
                                    Console.WriteLine("message = " + pack.message);
                                    Console.WriteLine();
                                    break;
                                case 50:
                                    var pack1 = JsonConvert.DeserializeObject<TempMessageEventPack>(task.data);
                                    Console.WriteLine("id = " + pack1.id);
                                    Console.WriteLine("fid = " + pack1.fid);
                                    Console.WriteLine("name = " + pack1.name);
                                    Console.WriteLine("message = " + pack1.message);
                                    Console.WriteLine();
                                    break;
                                case 51:
                                    var pack2 = JsonConvert.DeserializeObject<FriendMessageEventPack>(task.data);
                                    Console.WriteLine("id = " + pack2.id);
                                    Console.WriteLine("time = " + pack2.time);
                                    Console.WriteLine("name = " + pack2.name);
                                    Console.WriteLine("message = " + pack2.message);
                                    Console.WriteLine();
                                    break;
                            }
                        }
                        Thread.Sleep(10);
                    }
                    catch (Exception e)
                    {
                        ServerMain.LogError(e);
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
                        else if (Socket.Poll(1000, SelectMode.SelectRead))
                        {
                            ServerMain.LogOut("机器人连接中断");
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
                        ServerMain.LogError("机器人连接失败");
                        ServerMain.LogError(e);
                        IsConnect = false;
                        ServerMain.LogError("机器人20秒后重连");
                        Thread.Sleep(20000);
                        ServerMain.LogError("机器人重连中");
                    }
                }
            });
            ReadThread.Start();
            IsRun = true;
            ReadTest();
        }

        private static void ReadTest()
        {
            while (true)
            {
                while (!IsConnect)
                {
                    Thread.Sleep(500);
                }
                SendGroupMessage(571239090, Console.ReadLine());
            }
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
                ServerMain.LogOut("机器人已连接");
                IsConnect = true;
            }
            catch (Exception e)
            {
                ServerMain.LogError("机器人连接失败");
                ServerMain.LogError(e);
            }
        }
        public static void SendGroupMessage(long id, string message)
        {
            var data = BuildPack.Build(new SendGroupMessagePack { id = id, message = message }, 52);
            QueueSend.Add(data);
        }
        public static void SendGroupPrivateMessage(long id, long fid, string message)
        {
            var data = BuildPack.Build(new SendGroupPrivateMessagePack { id = id, fid = fid, message = message }, 53);
            QueueSend.Add(data);
        }
        public static void SendFriendMessage(long id, string message)
        {
            var data = BuildPack.Build(new SendFriendMessagePack { id = id, message = message }, 54);
            QueueSend.Add(data);
        }
        public static void SendGroupImage(long id, string img)
        {
            var data = BuildPack.Build(new SendGroupImagePack { id = id, img = img }, 61);
            QueueSend.Add(data);
        }
        public static void SendGroupPrivateImage(long id, long fid, string img)
        {
            var data = BuildPack.Build(new SendGroupPrivateImagePack { id = id, fid = fid, img = img }, 62);
            QueueSend.Add(data);
        }
        public static void SendFriendImage(long id, string img)
        {
            var data = BuildPack.Build(new SendFriendImagePack { id = id, img = img }, 63);
            QueueSend.Add(data);
        }
        public static void Stop()
        {
            ServerMain.LogOut("机器人正在断开");
            IsRun = false;
            if (Socket != null)
                Socket.Close();
            ServerMain.LogOut("机器人已断开");
        }
    }
}
