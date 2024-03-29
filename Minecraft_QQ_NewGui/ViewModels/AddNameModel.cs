﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace Minecraft_QQ_NewGui.ViewModels;

public partial class AddNameModel : ObservableObject
{
    [ObservableProperty]
    private string? _text;
    [ObservableProperty]
    private string _title;

    [RelayCommand]
    public void Save()
    {
        DialogHost.Close("Main", true);
    }

    [RelayCommand]
    public void Cancel()
    {
        DialogHost.Close("Main", false);
    }
}
