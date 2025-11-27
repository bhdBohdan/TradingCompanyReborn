using System;
using System.Windows.Input;
using TradingCompany.WPF2.ViewModels;

internal class LoginCommand : ICommand
{
    private readonly LoginViewModel _vm;
    public LoginCommand(LoginViewModel vm) => _vm = vm;

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => _vm.CanLogin;

    public void Execute(object? parameter)
    {
        _vm.LoginError = null;
        var user = _vm.Login();
        if (user != null)
        {
            // LoginViewModel.Login already sets session and raises LoginSuccessful
             
            _vm.LoginSuccessful?.Invoke();
        }
        else
        {
            _vm.LoginFailed?.Invoke();
        }
    }
}
