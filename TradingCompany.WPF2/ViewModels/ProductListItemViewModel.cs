//using System;
//using System.Windows.Input;
//using TradingCompany.DTO;

//namespace TradingCompany.WPF2.ViewModels
//{
//    public class ProductListItemViewModel
//    {
//        public Product Product { get; }
//        public bool CanDelete { get; }

//        public ICommand DeleteCommand { get; }

//        public ProductListItemViewModel(Product product, bool canDelete, Action<Product> deleteAction)
//        {
//            Product = product ?? throw new ArgumentNullException(nameof(product));
//            CanDelete = canDelete;
//            DeleteCommand = new RelayCommand(_ => deleteAction?.Invoke(Product));
//        }

//        // lightweight RelayCommand so this file is self-contained
//        private class RelayCommand : ICommand
//        {
//            private readonly Action<object?> _exec;
//            private readonly Predicate<object?>? _can;
//            public RelayCommand(Action<object?> exec, Predicate<object?>? can = null) { _exec = exec; _can = can; }
//            public event EventHandler? CanExecuteChanged { add { } remove { } }
//            public bool CanExecute(object? parameter) => _can?.Invoke(parameter) ?? true;
//            public void Execute(object? parameter) => _exec(parameter);
//        }
//    }
//}