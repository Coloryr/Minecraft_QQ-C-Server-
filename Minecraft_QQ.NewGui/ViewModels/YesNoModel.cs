using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class YesNoModel(string text, Action yes, Action no) : ObservableObject
{
    public string Text { get; init; } = text;

    [ObservableProperty]
    private bool _enable = true;
    [ObservableProperty]
    private bool _cancel = true;

    [RelayCommand]
    public void Yes()
    {
        yes();
    }

    [RelayCommand]
    public void No()
    {
        no();
    }
}
