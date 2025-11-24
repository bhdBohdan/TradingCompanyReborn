using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.WPF2.Commands;
using TradingCompany.WPF2.Interfaces;

namespace TradingCompany.WPF2.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged, ICloseable, IDataErrorInfo
    {
        private readonly IAuthManager _security;
        private readonly IProfileManager _profile;
        private readonly ICurrentUserService _session;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string? _loginError;
        public LoginViewModel(IAuthManager security, IProfileManager profile, ICurrentUserService session)
        {
            _security = security;
            _profile = profile;
            _session = session;
            LoginCommand = new LoginCommand(this);
            CloseCommand = new CloseCommand(this);
        }

        public User? Login()
        {
            var user = _security.Login(Username, Password);
            if (user != null)
            {
                _session.CurrentUser = user;
                _session.Profile = _profile.GetProfileByUserId(user.Id);
                LoginSuccessful?.Invoke();
                return user;
            }

            LoginError = "Invalid credentials";
            LoginFailed?.Invoke();
            return null;
        }

        public Action LoginFailed { get; set; }
        public Action LoginSuccessful { get; set; }

        public ICommand LoginCommand
        {
            get;
            private set;
        }

        public ICommand CloseCommand
        {
            get;
            private set;
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

        public string? LoginError
        {
            get => _loginError;
            internal set
            {
                if (_loginError == value) return;
                _loginError = value;
                OnPropertyChanged();
            }
        }

        public bool CanLogin =>
            string.IsNullOrEmpty(Error);

        private string ValidateUsername()
        {
            if (string.IsNullOrWhiteSpace(Username))
                return "Username is required.";
            if (Username.Length < 3)
                return "Username must be at least 3 characters.";
            return string.Empty;
        }

        private string ValidatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
                return "Password is required.";
            if (Password.Length < 6)
                return "Password must be at least 6 characters.";
            return string.Empty;
        }

        private string[] ValidatedProperties
        {
            get
            {
                return new string[] { nameof(Password), nameof(Username) };
            }
        }

        private string GetErrorInfo(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Password):
                    return ValidatePassword();
                case nameof(Username):
                    return ValidateUsername();
                default:
                    return "";
            }
        }
        #region IDataErrorInfo
        public string Error
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var property in ValidatedProperties)
                {
                    var err = GetErrorInfo(property);
                    if (!string.IsNullOrWhiteSpace(err))
                    {
                        sb.AppendLine(err);
                    }
                }
                return sb.ToString();
            }
        }
        public string this[string propertyName]
        {
            get
            {
                return GetErrorInfo(propertyName);
            }
        }
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
