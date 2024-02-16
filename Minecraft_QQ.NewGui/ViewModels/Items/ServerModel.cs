using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_QQ_NewGui.ViewModels.Items;

public partial class ServerModel : ObservableObject
{
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _addr;

    public void Disconnect()
    { 
    
    }

    public void EditConfig()
    { 
    
    }
}
