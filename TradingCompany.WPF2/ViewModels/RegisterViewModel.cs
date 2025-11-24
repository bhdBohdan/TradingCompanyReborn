using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF2.Commands;
using TradingCompany.WPF2.Interfaces;

namespace TradingCompany.WPF2.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged, ICloseable, IDataErrorInfo
    {
        private readonly IAuthManager _security;
        private readonly IProfileManager _profile;
        private readonly ICurrentUserService _session;

        private string _email = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string? _registerError;

        public RegisterViewModel(IAuthManager security, IProfileManager profile, ICurrentUserService session)
        {
            _security = security;
            _profile = profile;
            _session = session;

            RegisterCommand = new RegisterCommand(this);
            CloseCommand = new CloseCommand(this);
        }

        public User? Register()
        {
            try
            {
                var user = _security.Register(Email, Username, Password);
                if (user != null)
                {
                    _session.CurrentUser = user;
                    _session.Profile = _profile.GetProfileByUserId(user.Id);
                    RegisterSuccessful?.Invoke();
                    return user;
                }

                RegisterError = "Registration failed";
                RegisterFailed?.Invoke();
                return null;
            }
            catch (Exception ex)
            {
                RegisterError = ex.Message;
                RegisterFailed?.Invoke();
                return null;
            }
        }

        public Action RegisterFailed { get; set; }
        public Action RegisterSuccessful { get; set; }

        public ICommand RegisterCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }

        public string Email
        {
            get => _email;
            set
            {
                if (_email == value) return;
                _email = value ?? string.Empty;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Error));
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username == value) return;
                _username = value ?? string.Empty;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Error));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value ?? string.Empty;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Error));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword == value) return;
                _confirmPassword = value ?? string.Empty;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Error));
            }
        }

        public string? RegisterError
        {
            get => _registerError;
            internal set
            {
                if (_registerError == value) return;
                _registerError = value;
                OnPropertyChanged();
            }
        }

        public bool CanRegister => string.IsNullOrEmpty(Error);

        private string ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(Email)) return "Email is required.";
            if (!Email.Contains("@")) return "Email is invalid.";
            return string.Empty;
        }

        private string ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(Username)) return "Username is required.";
            if (Username.Length < 3) return "Username must be at least 3 characters.";
            return string.Empty;
        }

        private string ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password)) return "Password is required.";
            if (Password.Length < 6) return "Password must be at least 6 characters.";
            return string.Empty;
        }

        private string ValidateConfirmPassword()
        {
            if (Password != ConfirmPassword) return "Passwords do not match.";
            return string.Empty;
        }

        private string[] ValidatedProperties => new string[] { nameof(Email), nameof(Password), nameof(ConfirmPassword), nameof(Username) };

        private string GetErrorInfo(string propertyName)
        {
            return propertyName switch
            {
                nameof(Email) => ValidateEmail(),
                nameof(Username) => ValidateUsername(),
                nameof(Password) => ValidatePassword(),
                nameof(ConfirmPassword) => ValidateConfirmPassword(),
                _ => string.Empty
            };
        }

        #region IDataErrorInfo
        public string Error
        {
            get
            {
                foreach (var prop in ValidatedProperties)
                {
                    var err = GetErrorInfo(prop);
                    if (!string.IsNullOrEmpty(err)) return err;
                }
                return string.Empty;
            }
        }

        public string this[string columnName] => GetErrorInfo(columnName);
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion

        #region ICloseable
        public Action Close { get; set; }
        #endregion
    }
}
