#!/bin/bash

name="Minecraft_QQ"

mkdir ./build_out

build_linux_gui() {

    echo "build $name $1"

    dotnet publish ./Minecraft_QQ_NewGui -p:PublishProfile=$1

    echo "$name $1 build done"
}

build_linux_cmd() {

    echo "build $name $1"

    dotnet publish ./Minecraft_QQ_Cmd -p:PublishProfile=$1

    echo "$name $1 build done"
}

build_linux_gui linux-x64
build_linux_cmd linux-x64