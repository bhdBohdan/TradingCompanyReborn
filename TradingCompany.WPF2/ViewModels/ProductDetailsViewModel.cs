using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.WPF2.ViewModels
{
    public class ProductDetailsViewModel : INotifyPropertyChanged
    {
        private readonly IProductManager _productManager;
        private readonly ICurrentUserService _session;
        private readonly Product _original;

        public Action? CloseAction { get; set; }

        public ProductDetailsViewModel(Product product, IProductManager productManager, ICurrentUserService session)
        {
            _original = product ?? throw new ArgumentNullException(nameof(product));
            _productManager = productManager ?? throw new ArgumentNullException(nameof(productManager));
            _session = session ?? throw new ArgumentNullException(nameof(session));

            // copy values for editing
            ProductId = _original.Id;
            ProductName = _original.ProductName;
            Category = _original.Category;
            Description = _original.Description;
            Price = _original.Price;

            var currentUser = _session.CurrentUser;
            var isAdminOrMod = currentUser?.Roles?.Any(r => r.Id == (int)RoleType.ADMIN || r.Id == (int)RoleType.MODERATOR) ?? false;
            CanEdit = isAdminOrMod || (currentUser != null && _original.UserId == currentUser.Id);
            CanDelete = CanEdit; // same rule for both

            SaveCommand = new RelayCommand(_ => Save(), _ => CanEdit && IsDirty);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => CanDelete);
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        // readonly ID
        public int ProductId { get; }

        // editable properties bound from XAML
        private string _productName = string.Empty;
        public string ProductName { get => _productName; set { if (_productName == value) return; _productName = value; OnPropertyChanged(); MarkDirty(); } }

        private string _category = string.Empty;
        public string Category { get => _category; set { if (_category == value) return; _category = value; OnPropertyChanged(); MarkDirty(); } }

        private string? _description;
        public string? Description { get => _description; set { if (_description == value) return; _description = value; OnPropertyChanged(); MarkDirty(); } }

        private decimal _price;
        public decimal Price { get => _price; set { if (_price == value) return; _price = value; OnPropertyChanged(); MarkDirty(); } }

        public bool CanEdit { get; }
        public bool CanDelete { get; }

        private bool _isDirty;
        public bool IsDirty { get => _isDirty; private set { if (_isDirty == value) return; _isDirty = value; OnPropertyChanged(); } }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        private void MarkDirty() => IsDirty = true;

        private void Save()
        {
            try
            {
                // copy edited values back to original DTO and persist
                _original.ProductName = ProductName;
                _original.Category = Category;
                _original.Description = Description;
                _original.Price = Price;

                var updated = _productManager.UpdateProduct(_original);
                if (updated == null)
                {
                    MessageBox.Show("Failed to save product", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                IsDirty = false;
                MessageBox.Show("Product saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete()
        {
            if (MessageBox.Show($"Delete '{ProductName}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            try
            {
                var ok = _productManager.DeleteProduct(ProductId);
                if (!ok)
                {
                    MessageBox.Show("Delete failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Product deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            CloseAction?.Invoke();
        }

        // minimal RelayCommand
        private class RelayCommand : ICommand
        {
            private readonly Action<object?> _exec;
            private readonly Predicate<object?>? _can;
            public RelayCommand(Action<object?> exec, Predicate<object?>? can = null) { _exec = exec; _can = can; }
            public event EventHandler? CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
            public bool CanExecute(object? parameter) => _can?.Invoke(parameter) ?? true;
            public void Execute(object? parameter) => _exec(parameter);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}