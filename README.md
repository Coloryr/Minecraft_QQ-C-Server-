# Minecraft服务器与QQ群聊天的插件  
Minecraft_QQ 插件本体(Cmd/Gui)

[机器人使用ColorMirai](https://github.com/Coloryr/ColorMirai)   
[MCBBS帖子](http://www.mcbbs.net/thread-788137-1-1.html)

## 连接说明

链接顺序不要搞错了  
Minecraft_QQ->Minecraft_QQ_Cmd/Gui->ColorMirai  
Minecraft_QQ->Minecraft_QQ_Cmd/Gui->ColorMirai  
Minecraft_QQ->Minecraft_QQ_Cmd/Gui->ColorMirai  
请不要拿Minecraft_QQ直连ColorMirai，连不上的

## 部署教程：
1. 下载

> [ColorMirai](https://github.com/Coloryr/ColorMirai/actions)   
> [Minecraft_QQ_Cmd/Gui](https://github.com/HeartAge/Minecraft_QQ-C-Server-/actions)  
> [Minecraft_QQ插件](https://github.com/HeartAge/Minecraft_QQ/actions)

2. 安装

> 安装[.Net 6](https://dotnet.microsoft.com/download/dotnet/6.0)  
> 安装[Java](https://adoptopenjdk.net/)

3. 启动

> 将`ColorMirai`放到一个文件夹，根据[步骤](https://github.com/Coloryr/ColorMirai/#%E5%90%AF%E5%8A%A8)启动
>
> 将`Minecraft_QQ_Cmd/Gui`放到一个文件夹，选择启动`Cmd`或者是`Gui`  
> Linux下使用
> ```
> dotnet Minecraft_QQ_Cmd.dll
> ```  
> 
> 启动后进行[第一次使用配置](#第一次使用配置)  
> 连接`ColorMirai`
>
> 将插件放进服务器的插件文件夹
> 重启服务器  
> 连接`Minecraft_QQ_Cmd/Gui`

## 第一次使用配置

1. 群设置
> `Gui`下，添加主群即可  
> 有弹窗一样可以点主界面  
> `右键`就能修改/添加
>
> `Cmd`下，打开`Group.json`调整配置
> ```json
> {
>  "群列表": {
>    "123456789": {
>      "群号": "123456789",
>      "启用命令": true,
>      "开启对话": true,
>      "主群": true
>    }
>  }
>}
> ```
2. 运行QQ号设置
> Gui修改`插件配置`下的`机器人账户`  
> Cmd修改`MainConfig.json`下的`QQ机器人账户`  
> 填写运行的QQ号即可
>

3. 保存重启
> `Cmd`的配置文件修改后需要重启
> `Gui`点右上角保存即可

## 配置文件

> `Ask.json`  
> `自动应答`配置文件
> ```json
>{
>  "自动应答列表": {
>    "服务器菜单": "服务器查询菜单：\r\n【#绑定： ID】可以绑定你的游戏ID。\r\n【#在线人数】可以查询服务器在线人数。\r\n【#服务器状态】可以查询服务器是否在运行。\r\n【#服务器： 内容】可以向服务器里发送消息。（使用前请确保已经绑定了ID，）"
>  }
>}
> ```
> 可以自行添加，注意json格式就行
> ```json
>{
>  "自动应答列表": {
>    "a": "xxx",
>   ......
>    "b": "xxx"
>  }
>}
> ```

> `Command.json`  
> `自定义命令`配置文件
> ```json
>{
>  "命令列表": {
>    "插件帮助": {
>      "命令": "qq help",
>      "玩家使用": false,
>      "玩家发送": false,
>      "服务器使用": []
>    },
>    "查钱": {
>      "命令": "money {arg:name}",
>      "玩家使用": true,
>      "玩家发送": false,
>      "服务器使用": []
>    },
>    "禁言": {
>      "命令": "mute {arg1}",
>      "玩家使用": false,
>      "玩家发送": false,
>      "服务器使用": []
>    },
>    "传送": {
>      "命令": "tpa {arg:at}",
>      "玩家使用": true,
>      "玩家发送": false,
>      "服务器使用": []
>    },
>    "给权限": {
>      "命令": "lp user {arg:at} permission set {arg1} true",
>      "玩家使用": false,
>      "玩家发送": false,
>      "服务器使用": []
>    }
>  }
>}
> ```
> 命令可以自己添加，注意json格式
> - `命令`：发送到服务器的格式
> - `玩家使用`：该命令是否非管理员可用
> - `玩家发送`：命令执行是否是玩家
> - `服务器使用`：发送给的服务器，服务器名字记得加上`"`标起来
>
> 参数说明
> - `{arg:at}`：将会被替换为@QQ之后的游戏ID
> - `{arg:name}`：将会被替换为自己的游戏ID
> - `{arg:qq}`：将会被替换为自己的QQ
> - `{arg:atqq}`：将会被替换为@QQ之后的QQ
> - `{arg[1\2\3\...]}`：将会被替换为后面的参数
> - `{argx}`：将剩下的参数直接填进去
> 例如
> ```json
> "禁言": {
>      "命令": "mute {arg1}",
>      "玩家使用": false,
>      "玩家发送": false,
>      "服务器使用": []
>}
> ```
>在群里输入 `#禁言 Color_yr` 
>那么发送到服务器的指令为 `mute Color_yr`  
> ```json
>"给权限": {
>     "命令": "lp user {arg:at} permission set {arg1} true",
>     "玩家使用": false,
>     "玩家发送": false,
>     "服务器使用": []
>}
> ```
>在群里输入 `#给权限 @恋恋 admin.*`(@一个群成员)  
>那么发送到服务器的指令为 `lp user 该群员绑定的ID permission set admin.* true`  
>参数注意空格，不注意会错乱
> ```json
>"说话": {
>     "命令": "say {argx}",
>     "玩家使用": false,
>     "玩家发送": false,
>     "服务器使用": []
>}
> ```
>在群里输入 `#说话 test test`  
>那么发送到服务器的指令为 `say test test`  

>`Player.json`玩家绑定储存
>```json
>{
>  "禁止绑定列表": [
>    "Color_yr",
>    "id"
>  ],
>  "禁言列表": [
>    "playerid"
>  ],
>  "玩家列表": {
>    "402067010": {
>      "名字": "测试",
>      "昵称": "昵称",
>      "QQ号": 402067010,
>      "管理员": true
>    }
>  }
>}
>```
>自行添加注意格式

## 端口说明

ColorMirai的默认端口为23333  
Minecraft_QQ_Cmd/Gui的默认端口为25555  
如果没有必要，请不要随便改这两个端口

## 不在一台机器上部署

如果你有公网IP，直接在防火墙开放端口就行了  
然后Minecraft_QQ的IP设置填你机器的公网IP
如果你没有公网IP，那就去用端口映射，能映射出去就行了  
然后Minecraft_QQ_Cmd/Gui的绑定IP改为0.0.0.0

## 自己写Minecraft_QQ
1. 首先确定你的环境是.net5
2. 在你的项目里面导入`Minecraft_QQ_Core.dll`  
如果你导入的是`ref`文件夹里面的dll，请另外安装[Newtonsoft.Json](https://www.newtonsoft.com/json)

3. 核心启动  
首先设置回调，然后使用`Start`方法
```C#
private static void Message(string message)
{
    Console.WriteLine(message);
}
IMinecraft_QQ IMinecraft_QQ = new();
IMinecraft_QQ.ShowMessageCall = new IMinecraft_QQ.ShowMessage(Message);
IMinecraft_QQ.LogCall = new IMinecraft_QQ.Log(Message);
IMinecraft_QQ.Start();
```
获取核心
```C#
Minecraft_QQ Minecraft_QQ = IMinecraft_QQ.Main;
```
获取核心后就获取一些东西
```C#
/// <summary>
/// 配置文件路径
/// </summary>
public string Path { get; init; } = AppContext.BaseDirectory + "Minecraft_QQ/";
/// <summary>
/// Mysql启用
/// </summary>
public bool MysqlOK = false;
/// <summary>
/// 主群群号
/// </summary>
public long GroupSetMain { get; set; } = 0;
/// <summary>
/// 主配置文件
/// </summary>
public MainConfig MainConfig { get; set; }
/// <summary>
/// 玩家储存配置
/// </summary>
public PlayerConfig PlayerConfig { get; set; }
/// <summary>
/// 群储存配置
/// </summary>
public GroupConfig GroupConfig { get; set; }
/// <summary>
/// 自动应答储存
/// </summary>
public AskConfig AskConfig { get; set; }
/// <summary>
/// 自定义指令
/// </summary>
public CommandConfig CommandConfig { get; set; }

/// <summary>
/// Socket服务器
/// </summary>
public readonly MySocketServer Server;
/// <summary>
/// Mysql
/// </summary>
public readonly MyMysql Mysql;
/// <summary>
/// 机器人
/// </summary>
public readonly RobotSocket Robot;
/// <summary>
/// 发送群消息
/// </summary>
public readonly SendGroup SendGroup;
```

Minecraft_QQ核心方法API
```C#
/// <summary>
/// QQ号取玩家
/// </summary>
/// <param name="qq">qq号</param>
/// <returns>玩家信息</returns>
public PlayerObj GetPlayer(long qq);
/// <summary>
/// ID取玩家
/// </summary>
/// <param name="id">玩家ID</param>
/// <returns>玩家信息</returns>
public PlayerObj GetPlayer(string id);
/// <summary>
/// 设置玩家昵称
/// </summary>
/// <param name="qq">qq号</param>
/// <param name="nick">昵称</param>
public void SetNick(long qq, string nick);
/// <summary>
/// 设置玩家ID，如果存在直接修改，不存在创建
/// </summary>
/// <param name="qq">qq号</param>
/// <param name="name">玩家ID</param>
public void SetPlayerName(long qq, string name);
/// <summary>
/// 禁言玩家
/// </summary>
/// <param name="qq">QQ号</param>
public void MutePlayer(long qq);
/// <summary>
/// 禁言玩家
/// </summary>
/// <param name="name">名字</param>
public void MutePlayer(string name);
/// <summary>
/// 解除禁言
/// </summary>
/// <param name="qq">玩家QQ号</param>
public void UnmutePlayer(long qq);
/// <summary>
/// 解除禁言
/// </summary>
/// <param name="name">玩家ID</param>
public void UnmutePlayer(string name);
/// <summary>
/// 设置维护模式状态
/// </summary>
/// <param name="open">状态</param>
public void FixModeChange(bool open);
/// <summary>
/// 重载配置
/// </summary>
public bool Reload();
/// <summary>
/// 插件启动
/// </summary>
public void Start();
/// <summary>
/// 插件停止
/// </summary>
public void Stop();
```
具体内容看代码

如果你要通过Minecraft_QQ发送群消息，可以这样写
```C#
Minecraft_QQ.SendGroup.AddSend(new()
{
    Group = 123456,
    Message = "text"
});
```
又或者
```C#
Minecraft_QQ.Robot.SendGroupMessage(123456, "text");
```
推荐使用上面的方法，可以控制发送群消息的数量。
