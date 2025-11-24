using System;
using System.Windows.Input;
using TradingCompany.WPF2.ViewModels;

internal class RegisterCommand : ICommand
{
    private readonly RegisterViewModel _vm;
    public RegisterCommand(RegisterViewModel vm) => _vm = vm;

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => _vm.CanRegister;

    public void Execute(object? parameter)
    {
        _vm.RegisterError = null;
        var user = _vm.Register();
        if (user != null)
        {
            _vm.RegisterSuccessful?.Invoke();
        }
        else
        {
            _vm.RegisterFailed?.Invoke();
        }
    }
}
