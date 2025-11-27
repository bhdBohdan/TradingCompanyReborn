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
                    _registrationSucceeded = false;
                    Close();
                };
            }
            if (DataContext is RegisterViewModel rvm)
            {
                rvm.RegisterSuccessful += () =>
                {
                    _registrationSucceeded = true;
                    // Ensure ShowDialog() returns true to the caller by setting DialogResult.
                    // Setting DialogResult is only valid when shown with ShowDialog(); guard it.
                    try
                    {
                        this.DialogResult = true;
                    }
                    catch (InvalidOperationException)
                    {
                        // fallback if DialogResult cannot be set in this state
                        Close();
                    }
                };
                rvm.RegisterFailed += () =>
                {
                    MessageBox.Show("Registration failed: User already exists", "Error");
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
            var login = App.Services!.GetRequiredService<Login>();
            login.Owner = this.Owner;
            this.Close();
            login.ShowDialog();
        }
    }
}
