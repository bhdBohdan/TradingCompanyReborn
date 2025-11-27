using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Commands;
using TradingCompany.WPF2.Services;

namespace TradingCompany.WPF2.ViewModels
{
    public class NewProductViewModel : INotifyPropertyChanged, TradingCompany.WPF2.Interfaces.ICloseable, IDataErrorInfo
    {
        private readonly IProductManager _productManager;
        private readonly ICurrentUserService _session;

        public NewProductViewModel(IProductManager productManager, ICurrentUserService session)
        {
            _productManager = productManager ?? throw new ArgumentNullException(nameof(productManager));
            _session = session ?? throw new ArgumentNullException(nameof(session));

            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        // simple properties bound from XAML
        private string _productName = string.Empty;
        public string ProductName { get => _productName; set { if (_productName == value) return; _productName = value; OnPropertyChanged(); OnPropertyChanged(nameof(Error)); } }

        private string _category = string.Empty;
        public string Category { get => _category; set { if (_category == value) return; _category = value; OnPropertyChanged(); OnPropertyChanged(nameof(Error)); } }

        private string _description = string.Empty;
        public string Description { get => _description; set { if (_description == value) return; _description = value; OnPropertyChanged(); } }

        private string _priceText = string.Empty;
        public string PriceText { get => _priceText; set { if (_priceText == value) return; _priceText = value; OnPropertyChanged(); OnPropertyChanged(nameof(Error)); } }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // ICloseable
        public Action Close { get; set; }

        private bool CanSave()
        {
            return string.IsNullOrEmpty(Error) && _session.CurrentUser != null;
        }

        private void Save()
        {
            if (_session.CurrentUser == null)
            {
                MessageBox.Show("No user in session - cannot add product.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(PriceText, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var price))
            {
                MessageBox.Show("Invalid price.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var p = new Product
                {
                    ProductName = ProductName,
                    Category = Category,
                    Description = Description,
                    Price = price,
                    CreatedDate = DateTime.UtcNow,
                    UserId = _session.CurrentUser.Id,
                    User = null
                };

                var added = _productManager.AddProduct(p);
                // best-effort inform user
                MessageBox.Show("Product added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel() => Close?.Invoke();

        #region IDataErrorInfo
        public string Error
        {
            get
            {
                // return first encountered error (simple), could aggregate
                var e = this[nameof(ProductName)];
                if (!string.IsNullOrEmpty(e)) return e;
                e = this[nameof(PriceText)];
                if (!string.IsNullOrEmpty(e)) return e;
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                return columnName switch
                {
                    nameof(ProductName) => DataValidators.ValidateProductName(ProductName),
                    nameof(PriceText) => DataValidators.ValidatePriceString(PriceText),
                    _ => string.Empty
                };
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion

        // minimal local RelayCommand to avoid repeating implementations
        private class RelayCommand : ICommand
        {
            private readonly Action<object?> _exec;
            private readonly Predicate<object?>? _can;
            public RelayCommand(Action<object?> exec, Predicate<object?>? can = null) { _exec = exec; _can = can; }
            public event EventHandler? CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
            public bool CanExecute(object? parameter) => _can?.Invoke(parameter) ?? true;
            public void Execute(object? parameter) => _exec(parameter);
        }
    }
}