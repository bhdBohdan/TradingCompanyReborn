using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using TradingCompany.WPF2.Interfaces;
using TradingCompany.WPF2.ViewModels;

namespace TradingCompany.WPF2.Windows
{
    public partial class Login : Window
    {
        public Login(LoginViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
            Loaded += Login_Loaded;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password;
            }
        }

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseable cvm)
            {
                cvm.Close += () =>
                {
                    DialogResult = false;
                    Close();
                };
            }
            if (DataContext is LoginViewModel lvm)
            {
                lvm.LoginSuccessful += () =>
                {
                    // Setting DialogResult only valid when window was shown with ShowDialog().
                    // Guard and fallback to Close() if DialogResult cannot be set.
                    try
                    {
                        // prefer setting DialogResult for callers using ShowDialog()
                        DialogResult = true;
                    }
                    catch (InvalidOperationException)
                    {
                        // Not a dialog window (Show was used) or window not in the right state;
                        // simply close the window to proceed.
                        Close();
                    }
                };
                lvm.LoginFailed += () =>
                {
                    MessageBox.Show("Invalid credentials", "Error");
                };
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var register = App.Services!.GetRequiredService<Register>();
            register.Owner = this;            // make login the owner
            this.Hide();                     // hide, do not close
            var regResult = register.ShowDialog();
            if (regResult == true)
            {
                // registration succeeded -> set login result so App continues
                this.DialogResult = true;
                this.Close();
                return;
            }
            // registration canceled/failed -> show login again
            this.Show();
        }
    }
}
