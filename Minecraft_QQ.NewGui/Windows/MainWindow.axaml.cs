using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Threading;
using DialogHostAvalonia;
using Minecraft_QQ_Core;
using Minecraft_QQ_NewGui.ViewModels;
using System;
using System.ComponentModel;
using System.Threading;

namespace Minecraft_QQ_NewGui.Windows;

public partial class MainWindow : Window
{
    private WindowNotificationManager notificationManager;

    public MainWindow()
    {
        InitializeComponent();

        GroupDataGrid.CellPointerPressed += GroupDataGrid_CellPointerPressed;
        GroupDataGrid.PointerPressed += GroupDataGrid_PointerPressed;
        ServerDataGrid.CellPointerPressed += ServerDataGrid_CellPointerPressed;
        DataContextChanged += MainWindow_DataContextChanged;

        IMinecraft_QQ.ShowMessageCall = (data) =>
        {
            using var sem = new Semaphore(0, 2);
            Dispatcher.UIThread.Post(() =>
            {
                var model = (DataContext as WindowModel)!;
                var model1 = new YesNoModel(data, () =>
                {
                    model.Cancel();
                    sem.Release();
                }, model.Cancel)
                { 
                    Cancel = false
                };
                DialogHost.Show(model1, "Main");
            });
            sem.WaitOne();
        };
        IMinecraft_QQ.ConfigInitCall = () =>
        {
            using var sem = new Semaphore(0, 2);
            Dispatcher.UIThread.Post(async () =>
            {
                var model = (DataContext as WindowModel)!;
                await DialogHost.Show(new AddGroupModel(model), "Main");
                sem.Release();
            });
            sem.WaitOne();
        };
        IMinecraft_QQ.GuiCall = (state) =>
        {
            Dispatcher.UIThread.Post(async () =>
            {
                var model = (DataContext as WindowModel)!;
                switch (state)
                {
                    case GuiFun.ServerList:
                        model.UpdateServer();
                        break;
                }
            });
        };
    }

    private void ServerDataGrid_CellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
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
                new GroupFlyout((sender as Control)!, model, model.GroupItem);
            }
        });
    }

    private async void MainWindow_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is WindowModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
            await Minecraft_QQ.Start();
            model.Load();
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (DataContext is not WindowModel model)
        {
            return;
        }
        if (e.PropertyName == "Notify")
        {
            notificationManager.Show(model.Notify);
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        notificationManager = new(this)
        {
            MaxItems = 5
        };
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
                new GroupFlyout((sender as Control)!, model, model.GroupItem);
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
                new GroupFlyout((sender as Control)!, model, model.GroupItem);
            }
        });
    }
}