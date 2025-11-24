using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using TradingCompany.WPF2;

namespace TradingCompany.WPF2.Windows
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        public HomePage(ICurrentUserService session)
        {
            InitializeComponent();
            DataContext = session; // now XAML can bind to CurrentUser and ProfileImage
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var profileWin = App.Services!.GetRequiredService<UserProfile>();
            profileWin.Owner = this;
            profileWin.ShowDialog();





        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // show login window modally (DI returns a fresh instance because Login is registered transient)
            var login = App.Services!.GetRequiredService<Login>();
            login.Owner = this;
            this.Hide();
            var result = login.ShowDialog();
            // after dialog closes, session (ICurrentUserService) will be updated by the LoginViewModel if successful
            this.Show();
        }


        //private void AddButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var editViewModel = ActivatorUtilities.CreateInstance<MovieDetailsSimpleViewModel>(App.Services!, new Movie());
        //    var editWin = new MovieDetailsSimple(editViewModel);
        //    editWin.Owner = this;
        //    editWin.ShowDialog();
        //    _viewModel.Refresh();
        //}

        //private void EditButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_viewModel.SelectedMovie == null)
        //        return;

        //    // Pass the selected Movie into the constructor chain. ActivatorUtilities will resolve other services.
        //    var editViewModel = ActivatorUtilities.CreateInstance<MovieDetailsSimpleViewModel>(App.Services!, _viewModel.SelectedMovie);
        //    var editWin = new MovieDetailsSimple(editViewModel);
        //    editWin.Owner = this;
        //    editWin.ShowDialog();
        //    _viewModel.Refresh();
        //}
    }
}