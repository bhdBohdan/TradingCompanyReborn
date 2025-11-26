using System.Windows;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF2.ViewModels;

namespace TradingCompany.WPF2.Windows
{
    public partial class ProductDetails : Window
    {
        public ProductDetails(Product product, IProductManager productManager, ICurrentUserService session)
        {
            InitializeComponent();
            var vm = new ProductDetailsViewModel(product, productManager, session);
            vm.CloseAction = () => Dispatcher.Invoke(Close);
            DataContext = vm;
        }
    }
}