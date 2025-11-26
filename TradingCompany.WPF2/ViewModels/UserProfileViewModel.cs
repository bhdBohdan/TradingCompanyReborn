using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using TradingCompany.DTO;
using TradingCompany.WPF2.Windows;

namespace TradingCompany.WPF2.ViewModels
{
    public class UserProfileViewModel : INotifyPropertyChanged, TradingCompany.WPF2.Interfaces.ICloseable
    {
        private readonly ICurrentUserService _session;
        private readonly TradingCompany.BLL.Interfaces.IProfileManager _profileManager;

        // editable backing model copy
        private DTO.UserProfile? _editedProfile;
        private User? _editedUser;

        // Commands
        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand UploadPhotoCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand LogoutCommand { get; }    // new

        public UserProfileViewModel(ICurrentUserService session, TradingCompany.BLL.Interfaces.IProfileManager profileManager)
        {
            _session = session;
            _profileManager = profileManager;

            // initialize
            LoadFromSession();

            EditCommand = new RelayCommand(_ => IsEditing = true, _ => !IsEditing);
            SaveCommand = new RelayCommand(_ => Save(), _ => IsDirty);
            CancelCommand = new RelayCommand(_ => { LoadFromSession(); IsEditing = false; }, _ => IsEditing);
            UploadPhotoCommand = new RelayCommand(_ => UploadPhoto());
            CloseCommand = new RelayCommand(_ => Close?.Invoke());
            LogoutCommand = new RelayCommand(_ => Logout()); // new

            _session.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_session.Profile) || string.IsNullOrEmpty(e.PropertyName))
                {
                    LoadFromSession();
                }
            };
        }

        private void LoadFromSession()
        {
            _editedUser = _session.CurrentUser == null ? null : new User
            {
                Id = _session.CurrentUser.Id,
                Username = _session.CurrentUser.Username,
                Email = _session.CurrentUser.Email,
                Roles = _session.CurrentUser.Roles.ToList(),
                RegistrationDate = _session.CurrentUser.RegistrationDate,
                UpdatedAt = _session.CurrentUser.UpdatedAt,
                PasswordHash = _session.CurrentUser.PasswordHash,
                RestoreKeyword = _session.CurrentUser.RestoreKeyword,
            };

            var p = _session.Profile;
            if (p != null)
            {
                _editedProfile = new DTO.UserProfile
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    Address = p.Address,
                    Phone = p.Phone,
                    BankCardNumber = p.BankCardNumber,
                    ProfilePicture = p.ProfilePicture,
                    UpdatedAt = p.UpdatedAt
                };
            }
            else
            {
                _editedProfile = null;
            }

            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(Gender));
            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(Phone));
            OnPropertyChanged(nameof(BankCardNumber));
            OnPropertyChanged(nameof(ProfileImage));
            IsEditing = false;
            IsDirty = false;
        }

        public string? Username => _editedUser?.Username;
        public string? Email => _editedUser?.Email;

        public string Roles =>
     _editedUser?.Roles is null
         ? string.Empty
         : string.Join(", ", _editedUser.Roles.Select(r => r.RoleName));

        public string? FirstName
        {
            get => _editedProfile?.FirstName;
            set { if (_editedProfile == null) return; if (_editedProfile.FirstName == value) return; _editedProfile.FirstName = value ?? string.Empty; OnPropertyChanged(nameof(FirstName)); MarkDirty(); }
        }
        public string? LastName
        {
            get => _editedProfile?.LastName;
            set { if (_editedProfile == null) return; if (_editedProfile.LastName == value) return; _editedProfile.LastName = value ?? string.Empty; OnPropertyChanged(nameof(LastName)); MarkDirty(); }
        }
        public string? Gender
        {
            get => _editedProfile?.Gender;
            set { if (_editedProfile == null) return; if (_editedProfile.Gender == value) return; _editedProfile.Gender = value; OnPropertyChanged(nameof(Gender)); MarkDirty(); }
        }
        public string? Address
        {
            get => _editedProfile?.Address;
            set { if (_editedProfile == null) return; if (_editedProfile.Address == value) return; _editedProfile.Address = value; OnPropertyChanged(nameof(Address)); MarkDirty(); }
        }
        public string? Phone
        {
            get => _editedProfile?.Phone;
            set { if (_editedProfile == null) return; if (_editedProfile.Phone == value) return; _editedProfile.Phone = value; OnPropertyChanged(nameof(Phone)); MarkDirty(); }
        }
        public string? BankCardNumber
        {
            get => _editedProfile?.BankCardNumber;
            set { if (_editedProfile == null) return; if (_editedProfile.BankCardNumber == value) return; _editedProfile.BankCardNumber = value; OnPropertyChanged(nameof(BankCardNumber)); MarkDirty(); }
        }

        // Prefer the edited profile's bytes while editing / dirty; otherwise fall back to session image
        public object? ProfileImage => BytesToImage(_editedProfile?.ProfilePicture) ?? _session.ProfileImage;

        private void MarkDirty()
        {
            IsDirty = true;
            OnPropertyChanged(nameof(IsDirty));
            // notify profile image in case it changed
            OnPropertyChanged(nameof(ProfileImage));
        }

        private ImageSource? BytesToImage(byte[]? bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            try
            {
                var img = new BitmapImage();
                using var ms = new MemoryStream(bytes);
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = ms;
                img.EndInit();
                img.Freeze();
                return img;
            }
            catch
            {
                return null;
            }
        }

        private void Save()
        {
            if (_editedProfile == null) return;
            try
            {
                _editedProfile.UpdatedAt = DateTime.UtcNow;
                var saved = _profileManager.UpdateProfile(_editedProfile);
                // update session so UI reflects persisted data (ProfileImage, etc.)
                _session.Profile = saved;
                IsEditing = false;
                IsDirty = false;
                MessageBox.Show("Profile saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save profile: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UploadPhoto()
        {
            var dlg = new OpenFileDialog
            {
                Title = "Select profile picture",
                Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp"
            };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var bytes = File.ReadAllBytes(dlg.FileName);
                    if (_editedProfile == null) _editedProfile = new DTO.UserProfile { UserId = _editedUser?.Id ?? 0, Id = _session.Profile?.Id ?? 0 };
                    // copy to avoid possible shared buffer issues
                    _editedProfile.ProfilePicture = bytes.ToArray();
                    OnPropertyChanged(nameof(ProfileImage));
                    MarkDirty();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to load image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Logout()
        {
            _session.CurrentUser = null;
            _session.Profile = null;

          
            Close?.Invoke();

            var loginWindow = App.Services!.GetRequiredService<Windows.Login>();
            loginWindow.Show();

        }


        // UI state
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set { if (_isEditing == value) return; _isEditing = value; OnPropertyChanged(); }
        }

        private bool _isDirty;
        public bool IsDirty
        {
            get => _isDirty;
            set { if (_isDirty == value) return; _isDirty = value; OnPropertyChanged(); }
        }

        // ICloseable
        public Action Close { get; set; }
        
        // add this to the UserProfileViewModel class (e.g. near other public properties)
        public IList<string> GenderOptions { get; } = new List<string> { "Male", "Female", "Other" };

        // Simple relay
        private class RelayCommand : ICommand
        {
            private readonly Action<object?> _exec;
            private readonly Predicate<object?>? _can;
            public RelayCommand(Action<object?> exec, Predicate<object?>? can = null) { _exec = exec; _can = can; }
            public event EventHandler? CanExecuteChanged { add { CommandManager.RequerySuggested += value; } remove { CommandManager.RequerySuggested -= value; } }
            public bool CanExecute(object? parameter) => _can?.Invoke(parameter) ?? true;
            public void Execute(object? parameter) => _exec(parameter);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion
    }
}