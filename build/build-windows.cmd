@echo off
setlocal enabledelayedexpansion

mkdir .\build_out

call :build_win_gui win-x64
call :build_win_cmd win-x64

goto :eof

:build_win_gui
echo Minecraft_QQ build %1

dotnet publish .\Minecraft_QQ_NewGui -p:PublishProfile=%1

echo Minecraft_QQ %1 build done
goto :eof

:build_win_cmd
echo Minecraft_QQ build %1

dotnet publish .\Minecraft_QQ_Cmd -p:PublishProfile=%1

echo Minecraft_QQ %1 build done
goto :eof
