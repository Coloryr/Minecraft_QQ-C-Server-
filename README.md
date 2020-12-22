# Minecraft服务器与QQ群聊天的插件  
Minecraft_QQ 插件本体(Cmd/Gui)

[机器人使用ColorMirai](https://github.com/Coloryr/ColorMirai) 

[MCBBS帖子](http://www.mcbbs.net/thread-788137-1-1.html)

## 部署教程：
1. 下载

> [ColorMirai](https://github.com/Coloryr/ColorMirai/actions) 
> 
> [Minecraft_QQ_Cmd/Gui](https://github.com/HeartAge/Minecraft_QQ-C-Server-/actions)
>
> [Minecraft_QQ插件](https://github.com/HeartAge/Minecraft_QQ/actions)

2. 安装

> 安装[.Net 5](https://dotnet.microsoft.com/download/dotnet/5.0)
>
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
> 将插件放进服务器的插件文件夹，或者放进`Mods`文件夹(Forge1.12.2专属)  
> 重启服务器  
> 连接`Minecraft_QQ_Cmd/Gui`

## 第一次使用配置

> Gui下，添加主群即可
>
> Cmd下，打开`MainConfig.json`添加群
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
> 重启Cmd

## 配置文件

未完待续