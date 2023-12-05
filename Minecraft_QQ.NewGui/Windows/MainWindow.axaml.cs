using Avalonia.Controls;
using System;

namespace Minecraft_QQ.NewGui.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Opened += MainWindow_Opened;
    }

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        CommandSet.Open();
    }
}