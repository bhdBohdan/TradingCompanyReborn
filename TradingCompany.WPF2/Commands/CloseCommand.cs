
using System.Windows.Input;
using TradingCompany.WPF2.Interfaces;

namespace TradingCompany.WPF2.Commands
{
    internal class CloseCommand : ICommand
    {
        private ICloseable _vm;
        public CloseCommand(ICloseable vm)
        {
            _vm = vm;
        }

        #region ICommand
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.Close?.Invoke();
        }
        #endregion
    }
}
