using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class RegisterViewModel(MainWindowViewModel viewModel) : ViewModelBase
{
    [ObservableProperty]
    private string _login = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;
    
    [ObservableProperty]
    private string _confirmPassword = string.Empty;
    
    [ObservableProperty]
    private string _email = string.Empty;
    
    [ObservableProperty]
    private string _errorMessage = string.Empty;

    private MainWindowViewModel _mvMain = viewModel;

    [RelayCommand]
    public void Register()
    {
        Sound.PlaySfx("click");
        if (Login.Length < 6)
        {
            ErrorMessage = Localization.Instance["Register_LowerUserNameError"];
            return;
        }

        if (!Email.Contains("@"))
        {
            ErrorMessage = Localization.Instance["Register_ValidationEmailError"];
            return;
        }
        
        if (Password.Length < 8)
        {
            ErrorMessage = Localization.Instance["Register_ValidationPasswordError"];
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = Localization.Instance["Register_PasswordError"];
            return;
        }

        if (Authorize.FindUser(Login, Email))
        {
            ErrorMessage = Localization.Instance["Register_Error"];
            return;
        }
        
        Authorize.Register(Login, Password, Email);        
        ErrorMessage = string.Empty;
        _mvMain.CurrentPage = new MenuViewModel(_mvMain);
    }
    
    [RelayCommand]
    public void OpenLoginMenu()
    {
        Sound.PlaySfx("click");
        _mvMain.CurrentPage = new LoginViewModel(_mvMain);
    }
}