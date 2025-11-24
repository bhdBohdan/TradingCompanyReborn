using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TradingCompany.WPF2.ViewModels;

namespace TradingCompany.WPF2.Windows
{
    public partial class UserProfile : Window
    {
        public UserProfile(UserProfileViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            
            vm.Close = () => this.Close();
        }

      
    }
}