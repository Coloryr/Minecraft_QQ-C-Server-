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
    private bool isreq;

    public MainWindow()
    {
        InitializeComponent();

        GroupDataGrid.CellPointerPressed += GroupDataGrid_CellPointerPressed;
        GroupDataGrid.PointerPressed += GroupDataGrid_PointerPressed;
        ServerDataGrid.CellPointerPressed += ServerDataGrid_CellPointerPressed;
        PlayerDataGrid.CellPointerPressed += PlayerDataGrid_CellPointerPressed;
        PlayerDataGrid.PointerPressed += PlayerDataGrid_PointerPressed;
        NotBindDataGrid.CellPointerPressed += NotBindDataGrid_CellPointerPressed;
        NotBindDataGrid.PointerPressed += NotBindDataGrid_PointerPressed;
        MuteDataGrid.CellPointerPressed += MuteDataGrid_CellPointerPressed;
        MuteDataGrid.PointerPressed += MuteDataGrid_PointerPressed;
        AskDataGrid.CellPointerPressed += AskDataGrid_CellPointerPressed;
        AskDataGrid.PointerPressed += AskDataGrid_PointerPressed;
        CommandDataGrid.CellPointerPressed += CommandDataGrid_CellPointerPressed;
        CommandDataGrid.PointerPressed += CommandDataGrid_PointerPressed;
        DataContextChanged += MainWindow_DataContextChanged;
        Closing += MainWindow_Closing;

        IMinecraft_QQ.ShowMessageCall = (data) =>
        {
            using var sem = new Semaphore(0, 2);
            Dispatcher.UIThread.Post(() =>
            {
                isreq = true;
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
            isreq = false;
        };
        IMinecraft_QQ.ConfigInitCall = () =>
        {
            isreq = true;
            using var sem = new Semaphore(0, 2);
            Dispatcher.UIThread.Post(async () =>
            {
                var model = (DataContext as WindowModel)!;
                await DialogHost.Show(new AddGroupModel(model), "Main");
                sem.Release();
            });
            sem.WaitOne();
            isreq = false;
        };
        IMinecraft_QQ.GuiCall = (state) =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                var model = (DataContext as WindowModel)!;
                switch (state)
                {
                    case GuiCallType.ServerList:
                        model.UpdateServer();
                        break;
                    case GuiCallType.PlayerList:
                        model.LoadPlayer();
                        break;
                }
            });
        };
    }

    private void CommandDataGrid_PointerPressed(object? sender, PointerPressedEventArgs e)
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
                new CommandFlyout((sender as Control)!, model, null);
            }
        });
    }

    private void CommandDataGrid_CellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
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
                new CommandFlyout((sender as Control)!, model, model.CommandItem);
            }
        });
    }

    private void AskDataGrid_PointerPressed(object? sender, PointerPressedEventArgs e)
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
                new AskFlyout((sender as Control)!, model, null);
            }
        });
    }

    private void AskDataGrid_CellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
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
                new AskFlyout((sender as Control)!, model, model.AskItem);
            }
        });
    }

    private void MuteDataGrid_PointerPressed(object? sender, PointerPressedEventArgs e)
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
                new MuteFlyout((sender as Control)!, model, null);
            }
        });
    }

    private void MuteDataGrid_CellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
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
                new MuteFlyout((sender as Control)!, model, model.MuteItem);
            }
        });
    }

    private void NotBindDataGrid_PointerPressed(object? sender, PointerPressedEventArgs e)
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
                new NotBindFlyout((sender as Control)!, model, null);
            }
        });
    }

    private void NotBindDataGrid_CellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
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
                new NotBindFlyout((sender as Control)!, model, model.NotBindItem);
            }
        });
    }

    private void PlayerDataGrid_PointerPressed(object? sender, PointerPressedEventArgs e)
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
                new PlayerFlyout((sender as Control)!, model, null);
            }
        });
    }

    private void PlayerDataGrid_CellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
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
                new PlayerFlyout((sender as Control)!, model, model.PlayerItem);
            }
        });
    }

    private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (isreq)
        {
            e.Cancel = true;
            return;
        }
        Minecraft_QQ.Stop();
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