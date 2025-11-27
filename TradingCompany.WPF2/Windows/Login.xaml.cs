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
                    
                    try
                    {
                        DialogResult = true;

                    }
                    catch (InvalidOperationException)
                    {
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
            register.Owner = this;

            var regResult = register.ShowDialog();

            if (regResult == true)
            {
                this.DialogResult = true;
            }
        }

    }
}