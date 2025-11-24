using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using TradingCompany.WPF2.Interfaces;
using TradingCompany.WPF2.ViewModels;

namespace TradingCompany.WPF2.Windows
{
    public partial class Register : Window
    {
        private bool _registrationSucceeded;
        public bool RegistrationSucceeded => _registrationSucceeded;

        public Register(RegisterViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
            Loaded += Register_Loaded;
        }

        private void Register_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICloseable cvm)
            {
                cvm.Close += () =>
                {
                    // Don't set DialogResult here because the window may not be shown with ShowDialog()
                    _registrationSucceeded = false;
                    Close();
                };
            }
            if (DataContext is RegisterViewModel rvm)
            {
                rvm.RegisterSuccessful += () =>
                {
                    // Record success and close. Caller should inspect RegistrationSucceeded after the window closes.
                    _registrationSucceeded = true;
                    Close();
                };
                rvm.RegisterFailed += () =>
                {
                    MessageBox.Show("Registration failed", "Error");
                };
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Open login window
            var login = App.Services!.GetRequiredService<Login>();
            login.Owner = this.Owner; // keep same owner
            this.Close();
            login.ShowDialog();
        }
    }
}
