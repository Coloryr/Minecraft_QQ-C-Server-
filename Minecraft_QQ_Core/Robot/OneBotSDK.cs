using Microsoft.Extensions.Options;
using Minecraft_QQ_Core.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Net.WebSockets;
using Websocket.Client;

namespace Minecraft_QQ_Core.Robot;

public class OneBotSDK
{
    private readonly WebsocketClient client;
    public OneBotSDK(string url, string? auth)
    {
        client = new WebsocketClient(new Uri(url), () => 
        {
            var temp = new ClientWebSocket();
            if (auth != null)
            {
                temp.Options.SetRequestHeader("Authorization", auth);
            }
            return temp;
        })
        {
            ReconnectTimeout = TimeSpan.FromSeconds(10)
        };
        client.ReconnectionHappened.Subscribe(info =>
            Logs.LogOut($"机器人重连, type: {info.Type}"));
        client.DisconnectionHappened.Subscribe(info => Logs.LogOut("机器人链接断开"));

        client.MessageReceived.Subscribe(Message);
    }

    private void Message(ResponseMessage msg)
    {
        if (msg.MessageType == WebSocketMessageType.Text)
        {
            var obj = JObject.Parse(msg.Text!);
            if (obj.TryGetValue("message_type", out var value) && value.ToString() == "group")
            {
                var pack = obj.ToObject<GroupMessagePack>();
                RobotCore.Message(pack!);
            }
        }
    }

    public void Send(string data)
    {
        if (client.IsRunning)
        {
            client.Send(data);
        }
    }

    public void Start()
    {
        client.Start();
    }

    public void Stop()
    {
        client.Stop(WebSocketCloseStatus.Empty, "stop");
        client.Dispose();
    }
}
