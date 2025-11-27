using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF.Commands;

namespace TradingCompany.WPF2.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private readonly ICurrentUserService _session;
        private readonly IProductManager _productManager;
        private readonly IRoleManager _roleManager;
        private readonly IAuthManager _authManager;
        public ProductsListViewModel ProductsVm { get; }

        // set by the Window after construction so view-model can own dialogs
        public Window? OwnerWindow { get; set; }

        public HomePageViewModel(
            ICurrentUserService session,
            IProductManager productManager,
            IRoleManager roleManager,
            IAuthManager authManager,
            ProductsListViewModel productsVm)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _productManager = productManager ?? throw new ArgumentNullException(nameof(productManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
            ProductsVm = productsVm ?? throw new ArgumentNullException(nameof(productsVm));

            // propagate session change notifications
            _session.PropertyChanged += Session_PropertyChanged;

            GiveAdminRoleCommand = new RelayCommand(_ => GiveAdminRole(), _ => _session.CurrentUser != null);
            RemoveAdminRoleCommand = new RelayCommand(_ => RemoveAdminRole(), _ => _session.CurrentUser != null);
            OpenProfileCommand = new RelayCommand(_ => OpenProfile(), _ => _session.CurrentUser != null);
            AddProductCommand = new RelayCommand(_ => AddProduct(), _ => _session.CurrentUser != null);
        }

        private void Session_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // When session raises an empty/NULL property name it means "many things changed" — refresh both
            if (string.IsNullOrEmpty(e.PropertyName))
            {
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(ProfileImage));
                return;
            }

            // forward specific session changes to bound VM properties
            if (e.PropertyName == nameof(_session.CurrentUser))
                OnPropertyChanged(nameof(CurrentUser));

            if (e.PropertyName == nameof(_session.Profile) || e.PropertyName == nameof(_session.ProfileImage))
                OnPropertyChanged(nameof(ProfileImage));
        }

        // expose session-backed properties for binding (HomePage XAML uses CurrentUser / ProfileImage)
        public User? CurrentUser => _session.CurrentUser;
        public object? ProfileImage => _session.ProfileImage;

        public ICommand GiveAdminRoleCommand { get; }
        public ICommand RemoveAdminRoleCommand { get; }
        public ICommand OpenProfileCommand { get; }
        public ICommand AddProductCommand { get; }

        // Open selected product details (used by code-behind double-click)
        public void OpenSelectedProduct()
        {
            var selected = ProductsVm.SelectedProduct;
            if (selected == null) return;

            var productManager = App.Services!.GetRequiredService<IProductManager>();
            var session = App.Services!.GetRequiredService<ICurrentUserService>();

            var detailsWindow = new Windows.ProductDetails(selected, productManager, session);
            if (OwnerWindow != null) detailsWindow.Owner = OwnerWindow;
            detailsWindow.ShowDialog();

            ProductsVm.Refresh();
        }

        private void OpenProfile()
        {
            var profileWin = App.Services!.GetRequiredService<Windows.UserProfile>();
            if (OwnerWindow != null) profileWin.Owner = OwnerWindow;
            profileWin.ShowDialog();
        }

        private void AddProduct()
        {
            var newWin = App.Services!.GetRequiredService<Windows.NewProduct>();
            if (OwnerWindow != null) newWin.Owner = OwnerWindow;
            newWin.ShowDialog();
            ProductsVm.Refresh();
        }

        // if selection exists use its owner, otherwise current user
        private int TargetUserId()
        {
            return ProductsVm.SelectedProduct != null ? ProductsVm.SelectedProduct.UserId : (_session.CurrentUser?.Id ?? 0);
        }

        private void GiveAdminRole()
        {
            if (_session.CurrentUser == null) return;
            var userId = TargetUserId();
            var ok = _roleManager.AddRoleToUser(userId, RoleType.ADMIN);
            if (ok)
            {
                if (_session.CurrentUser.Id == userId)
                {
                    var refreshed = _authManager.GetUserById(userId);
                    if (refreshed != null) _session.CurrentUser = refreshed;
                }
                ProductsVm.Refresh();
                MessageBox.Show("Admin role granted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to grant Admin role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveAdminRole()
        {
            if (_session.CurrentUser == null) return;
            var userId = TargetUserId();
            var ok = _roleManager.RemoveRoleFromUser(userId, RoleType.ADMIN);
            if (ok)
            {
                if (_session.CurrentUser.Id == userId)
                {
                    var refreshed = _authManager.GetUserById(userId);
                    if (refreshed != null) _session.CurrentUser = refreshed;
                }
                ProductsVm.Refresh();
                MessageBox.Show("Admin role removed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to remove Admin role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}
