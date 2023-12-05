using Avalonia.Controls;
using ColorMC.Gui.Utils;
using Minecraft_QQ.NewGui.ViewModels;
using System;
using System.ComponentModel;
using System.Threading;

namespace Minecraft_QQ.NewGui.Controls;

public partial class CommandSetControl : UserControl
{
    public CommandSetControl()
    {
        InitializeComponent();

        DataContextChanged += CommandSetControl_DataContextChanged;
    }

    private void CommandSetControl_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is WindowModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "CommandSetDisplay")
        {
            App.CrossFade300.Start(null, this, CancellationToken.None);
        }
        else if (e.PropertyName == "CommandSetHide")
        {
            App.CrossFade300.Start(this, null, CancellationToken.None);
        }
    }

    public void Open()
    {
        DataGrid1.SetFontColor();
    }
}
