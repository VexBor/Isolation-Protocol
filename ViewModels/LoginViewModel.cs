using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class LoginViewModel(MainWindowViewModel viewModel) : ViewModelBase
{
    [ObservableProperty]
    private string _username;
    
    [ObservableProperty]
    private string _password;
    
    [ObservableProperty]
    private string _errorMessage;
    
    private MainWindowViewModel _mainVM = viewModel;

    [RelayCommand]
    public void Login()
    { 
        Sound.PlaySfx("click");
        bool s = Authorize.Login(_username, _password);
        if (s == true)
        {
            _mainVM.CurrentPage = new MenuViewModel(_mainVM);
        }
        else
            ErrorMessage = Localization.Instance["Register_LoginError"];;
    }

    [RelayCommand]
    public void OpenRegisterMenu()
    {
        Sound.PlaySfx("click");
        _mainVM.CurrentPage = new RegisterViewModel(_mainVM);
    }
}