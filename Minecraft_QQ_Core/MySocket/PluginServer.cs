using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Minecraft_QQ_Core.Robot;
using Minecraft_QQ_Core.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Minecraft_QQ_Core.MySocket;

public static class PluginServer
{

    public readonly static ConcurrentDictionary<string, PluginItem> MCServers = [];
    public readonly static ConcurrentDictionary<IChannel, PluginItem> Contexts = [];

    private static IEventLoopGroup bossGroup;
    private static IEventLoopGroup workerGroup;

    private static IChannel BoundChannel;

    public static bool Start { get; private set; }

    private static void SetState(bool st)
    {
        Start = st;
        IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
    }

    public static bool IsReady()
    {
        return !MCServers.IsEmpty;
    }

    public static void ReadData(IChannel channel, IByteBuffer pack)
    {
        if (Contexts.TryGetValue(channel, out var item))
        {
            item.Read(pack);
        }
        else
        {
            item = new PluginItem(channel);
            Contexts.TryAdd(channel, item);
            item.Read(pack);
        }
    }

    public static async void StartServer()
    {
        try
        {
            Logs.LogOut("[Socket]正在启动端口");

            bossGroup = new MultithreadEventLoopGroup(1);
            workerGroup = new MultithreadEventLoopGroup();

            var bootstrap = new ServerBootstrap();
            bootstrap.Group(bossGroup, workerGroup);

            bootstrap.Channel<TcpServerSocketChannel>();

            bootstrap
                .Option(ChannelOption.SoBacklog, 100)
                .Option(ChannelOption.TcpNodelay, true)
                .Option(ChannelOption.SoKeepalive, true)
                .Handler(new LoggingHandler("Minecraft_QQ"))
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast(new LoggingHandler("Minecraft_QQ"));
                    pipeline.AddLast(new LengthFieldPrepender(4));
                    pipeline.AddLast(new LengthFieldBasedFrameDecoder(1024 * 8, 0, 4, 0, 4));

                    pipeline.AddLast(new PluginServerHandler());
                }));

            BoundChannel = await bootstrap
                .BindAsync(Minecraft_QQ.MainConfig.Socket.Port);

            SetState(true);
            if (Minecraft_QQ.MainConfig.Setting.SendLog)
            {
                RobotCore.SendGroupMessage(Minecraft_QQ.GroupSetMain,
                [
                    $"[Minecraft_QQ]端口已启动{Environment.NewLine}",
                    $"已绑定在：{Minecraft_QQ.MainConfig.Socket.Port}"
                ]);
            }

            Logs.LogOut("[Socket]端口已启动");
        }
        catch (Exception e)
        {
            RobotCore.SendGroupMessage(Minecraft_QQ.GroupSetMain, ["[Minecraft_QQ]启动失败，请看日志/Minecraft_QQ/logs.log"]);
            Logs.LogError(e);
            SetState(false);
            IMinecraft_QQ.ShowMessageCall.Invoke("[Minecraft_QQ]启动失败，请检查设置的端口是否被占用");
        }
    }

    public static void Close(string name)
    {
        if (MCServers.TryRemove(name, out var value))
        {
            value.Stop();
        }
    }

    public static IByteBuffer BuildPack(TranObj info)
    {
        IByteBuffer buffer = Unpooled.Buffer();
        buffer.WriteString(info.group)
            .WriteString(info.message)
            .WriteString(info.player)
            .WriteString(info.command)
            .WriteBoolean(info.isCommand);
        return buffer;
    }

    public static void Send(TranObj info, List<string>? servers = null)
    {
        if (!MCServers.IsEmpty)
        {
            var pack = BuildPack(info);
            if (servers != null)
            {
                foreach (var temp in servers)
                {
                    SendData(MCServers[temp], pack);
                }
            }
            else
            {
                foreach (var socket in new List<PluginItem>(Contexts.Values))
                {
                    SendData(socket, pack);
                }
            }
        }
        else
            RobotCore.SendGroupMessage(Minecraft_QQ.GroupSetMain, ["[Minecraft_QQ]服务器未连接，无法发送"]);
    }
    private static void SendData(PluginItem Client, IByteBuffer data)
    {
        try
        {
            Client.Channel.WriteAndFlushAsync(data);
        }
        catch (Exception e)
        {
            Logs.LogError(e);
            Close(Client.Name);
            IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
            GC.Collect();
            if (MCServers.IsEmpty)
            {
                RobotCore.SendGroupMessage(Minecraft_QQ.GroupSetMain, [$"[Minecraft_QQ]连接已断开，无法发送{Environment.NewLine}{e}"]);
            }
        }
    }
    public static void AddServer(string name, PluginItem receive)
    {
        if (MCServers.TryGetValue(name, out var value))
        {
            RobotCore.SendGroupMessage(Minecraft_QQ.GroupSetMain, [$"[Minecraft_QQ]同名服务器{name}连接，旧连接已断开"]);
            value.StopSame();
            MCServers[name] = receive;
        }
        else
        {
            MCServers.TryAdd(name, receive);
        }
    }
    public static void ServerStop()
    {
        foreach (var item in MCServers)
        {
            item.Value.Stop();
        }
        BoundChannel?.CloseAsync();
        MCServers.Clear();
        Start = false;
    }
}

public class PluginServerHandler : ChannelHandlerAdapter
{
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        if (message is IByteBuffer buffer)
        {
            PluginServer.ReadData(context.Channel, buffer);
        }
    }

    public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

    public override void ExceptionCaught(IChannelHandlerContext ctx, Exception e)
    {
        Console.WriteLine("{0}", e.ToString());
        ctx.CloseAsync();
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        if (PluginServer.Contexts.TryRemove(context.Channel, out var item))
        {
            item.Stop();
            PluginServer.MCServers.TryRemove(item.Name, out _);
            IMinecraft_QQ.GuiCall?.Invoke(GuiFun.ServerList);
        }
    }
}