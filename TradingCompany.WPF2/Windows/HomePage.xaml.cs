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
        public HomePage()
        {
            InitializeComponent();

            var vm = App.Services!.GetRequiredService<HomePageViewModel>();
            vm.OwnerWindow = this;

            DataContext = vm;

            ProductsListView.DataContext = vm.ProductsVm;
        }

        private void ProductsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is HomePageViewModel vm)
            {
                vm.OpenSelectedProduct();
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomePageViewModel vm && vm.OpenProfileCommand?.CanExecute(null) == true)
            {
                vm.OpenProfileCommand.Execute(null);
                return;
            }

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

            //refresh products after login/logout
            if (ProductsListView?.DataContext is ProductsListViewModel pvm)
            {
                pvm.Refresh();
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomePageViewModel vm && vm.AddProductCommand?.CanExecute(null) == true)
            {
                vm.AddProductCommand.Execute(null);
                return;
            }

            var newWin = App.Services!.GetRequiredService<NewProduct>();
            newWin.Owner = this;
            newWin.ShowDialog();

            if (ProductsListView?.DataContext is ProductsListViewModel pvm)
            {
                pvm.Refresh();
            }
        }

        // Dev: grant Admin role to target user (selected product owner or current user)
        private void GiveAdminRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomePageViewModel vm)
            {
                vm.GiveAdminRoleCommand?.Execute(null);
            }

        }

        // Dev: remove Admin role from target user (selected product owner or current user)
        private void RemoveAdminRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomePageViewModel vm)
            {

                vm.RemoveAdminRoleCommand?.Execute(null);
            }



        }
    }
}