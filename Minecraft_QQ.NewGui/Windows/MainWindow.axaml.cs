using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Minecraft_QQ_NewGui.ViewModels;
using System;

namespace Minecraft_QQ_NewGui.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Opened += MainWindow_Opened;

        GroupDataGrid.CellPointerPressed += GroupDataGrid_CellPointerPressed;
        GroupDataGrid.PointerPressed += GroupDataGrid_PointerPressed;
    }

    private void GroupDataGrid_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var po = e.GetCurrentPoint(this);
        if (po.Properties.IsRightButtonPressed == false)
        {
            return;
        }

        Dispatcher.UIThread.Post(() =>
        {
            if (DataContext is WindowModel model)
            {
                new GroupFlyout(this, model, model.GroupItem);
            }
        });
    }

    private void GroupDataGrid_CellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
    {
        var po = e.PointerPressedEventArgs.GetCurrentPoint(this);
        if (po.Properties.IsRightButtonPressed == false)
        {
            return;
        }

        Dispatcher.UIThread.Post(() =>
        {
            if (DataContext is WindowModel model)
            {
                new GroupFlyout(this, model, model.GroupItem);
            }
        });
    }

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        
    }
}