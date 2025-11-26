using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TradingCompany.WPF2.ViewModels;

namespace TradingCompany.WPF2.Windows
{
    public partial class NewProduct : Window
    {
        public NewProduct(NewProductViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();

            if (DataContext is NewProductViewModel npvm)
            {
                npvm.Close += () => this.Close();
            }
        }
    }
}
