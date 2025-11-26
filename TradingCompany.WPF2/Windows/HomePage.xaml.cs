using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.WPF2.ViewModels;
using TradingCompany.DTO;

namespace TradingCompany.WPF2.Windows
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        private readonly ICurrentUserService _session;

        public HomePage(ICurrentUserService session)
        {
            InitializeComponent();
            _session = session ?? throw new ArgumentNullException(nameof(session));
            DataContext = session; // session stays window DataContext

            // resolve and attach products VM to the products control
            var productsVm = App.Services!.GetRequiredService<ProductsListViewModel>();
            ProductsListView.DataContext = productsVm;
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var profileWin = App.Services!.GetRequiredService<UserProfile>();
            profileWin.Owner = this;
            profileWin.ShowDialog();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = App.Services!.GetRequiredService<Login>();
            login.Owner = this;
            this.Hide();
            var result = login.ShowDialog();
            this.Show();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var newWin = App.Services!.GetRequiredService<NewProduct>();
            newWin.Owner = this;
            newWin.ShowDialog();

            // refresh products list after dialog closes (best-effort)
            if (ProductsListView?.DataContext is ProductsListViewModel vm)
            {
                vm.Refresh();
            }
        }

        private void ProductsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // 1. Get the ViewModel from the ListView
            if (ProductsListView.DataContext is not ProductsListViewModel vm) return;

            // 2. Check if a product is actually selected
            // (This prevents opening if the user double-clicks empty space or a header)
            if (vm.SelectedProduct == null) return;

            // 3. Resolve dependencies needed for ProductDetails
            // We cannot ask the container for 'ProductDetails' directly because 
            // we need to pass the specific 'SelectedProduct' manually.
            var productManager = App.Services!.GetRequiredService<IProductManager>();
            var session = App.Services!.GetRequiredService<ICurrentUserService>();

            // 4. Create the window manually
            var detailsWindow = new ProductDetails(vm.SelectedProduct, productManager, session);
            detailsWindow.Owner = this;

            // 5. Show the window as a Dialog (modal)
            detailsWindow.ShowDialog();

            
            vm.Refresh();
        }

        // Dev: grant Admin role to target user (selected product owner or current user)
        private void GiveAdminRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_session.CurrentUser == null) return;

            var roleManager = App.Services!.GetRequiredService<IRoleManager>();
            var authManager = App.Services!.GetRequiredService<IAuthManager>();

            int targetUserId;
            if (ProductsListView.DataContext is ProductsListViewModel vm && vm.SelectedProduct != null)
                targetUserId = vm.SelectedProduct.UserId;
            else
                targetUserId = _session.CurrentUser.Id;

            var ok = roleManager.AddRoleToUser(targetUserId, RoleType.ADMIN);
            if (ok)
            {
                // if we updated the currently logged in user, refresh session user info
                if (_session.CurrentUser != null && _session.CurrentUser.Id == targetUserId)
                {
                    var refreshed = authManager.GetUserById(targetUserId);
                    if (refreshed != null) _session.CurrentUser = refreshed;
                }

                // refresh product list in case UI shows role-related info
                if (ProductsListView.DataContext is ProductsListViewModel pvm) pvm.Refresh();

                MessageBox.Show("Admin role granted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to grant Admin role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Dev: remove Admin role from target user (selected product owner or current user)
        private void RemoveAdminRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_session.CurrentUser == null) return;

            var roleManager = App.Services!.GetRequiredService<IRoleManager>();
            var authManager = App.Services!.GetRequiredService<IAuthManager>();

            int targetUserId;
            if (ProductsListView.DataContext is ProductsListViewModel vm && vm.SelectedProduct != null)
                targetUserId = vm.SelectedProduct.UserId;
            else
                targetUserId = _session.CurrentUser.Id;

            var ok = roleManager.RemoveRoleFromUser(targetUserId, RoleType.ADMIN);
            if (ok)
            {
                if (_session.CurrentUser != null && _session.CurrentUser.Id == targetUserId)
                {
                    var refreshed = authManager.GetUserById(targetUserId);
                    if (refreshed != null) _session.CurrentUser = refreshed;
                }

                if (ProductsListView.DataContext is ProductsListViewModel pvm) pvm.Refresh();

                MessageBox.Show("Admin role removed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to remove Admin role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}