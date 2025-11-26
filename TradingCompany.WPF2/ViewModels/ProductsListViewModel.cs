using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;

namespace TradingCompany.WPF2.ViewModels
{
    public class ProductsListViewModel : INotifyPropertyChanged
    {
        private readonly IProductManager _productManager;
        private readonly ICurrentUserService _session;

        private ObservableCollection<Product> _products = new();
        public ObservableCollection<Product> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(nameof(Products)); }
        }

        public ICollectionView ProductsView { get; private set; }

        private string _filterText = string.Empty;
        public string FilterText
        {
            get => _filterText;
            set
            {
                if (_filterText == value) return;
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                ProductsView?.Refresh();
            }
        }

        private Product? _selectedProduct;
        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set { if (_selectedProduct == value) return; _selectedProduct = value; OnPropertyChanged(nameof(SelectedProduct)); }
        }

        public ProductsListViewModel(IProductManager productManager, ICurrentUserService session)
        {
            _productManager = productManager ?? throw new ArgumentNullException(nameof(productManager));
            _session = session ?? throw new ArgumentNullException(nameof(session));

            ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = FilterPredicate;

            // refresh when login/logout happens
            _session.PropertyChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(_session.CurrentUser))
                    Refresh();
            };

            Refresh();
        }

        private bool FilterPredicate(object? obj)
        {
            if (string.IsNullOrWhiteSpace(FilterText)) return true;
            if (obj is Product p)
            {
                var q = FilterText.Trim();
             
                return (p.ProductName?.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                       || (p.Category?.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                       || (p.Description?.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return false;
        }

        public void Refresh()
        {
            if (_session.CurrentUser == null)
            {
                Products = new ObservableCollection<Product>();
                ProductsView = CollectionViewSource.GetDefaultView(Products);
                ProductsView.Filter = FilterPredicate;
                OnPropertyChanged(nameof(ProductsView));
                return;
            }

            var list = _productManager.GetProducts() ?? new List<Product>();
            Products = new ObservableCollection<Product>(list);
            ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = FilterPredicate;
            OnPropertyChanged(nameof(ProductsView));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}


