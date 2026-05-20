using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class VictoryViewModel : ViewModelBase
{
    private readonly Localization _loc = Localization.Instance;

    public string VictoryTitle => _loc["Victory_Title"];
    public string VictoryLog => _loc["Victory_Log"];
    public string ExitBtnText => _loc["Victory_Exit_Btn"];
}