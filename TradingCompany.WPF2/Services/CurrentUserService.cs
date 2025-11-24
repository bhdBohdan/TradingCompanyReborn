using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TradingCompany.DTO;
using TradingCompany.WPF2; // adjust namespace for your User DTO

public interface ICurrentUserService : INotifyPropertyChanged
{
    User? CurrentUser { get; set; }
    UserProfile? Profile { get; set; }
    ImageSource? ProfileImage { get; }
}

public class CurrentUserService : ICurrentUserService
{
    private User? _currentUser;
    private UserProfile? _profile;
    public User? CurrentUser
    {
        get => _currentUser;
        set
        {
            if (_currentUser == value) return;
            _currentUser = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ProfileImage));
        }
    }

    public UserProfile? Profile
    {
        get => _profile;
        set
        {
            if (_profile == value) return;
            _profile = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ProfileImage));
        }
    }

    public ImageSource? ProfileImage
    {
        get
        {
            var bytes = _profile?.ProfilePicture;
            if (bytes == null || bytes.Length == 0) return null;
            try
            {
                using var ms = new MemoryStream(bytes);
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.StreamSource = ms;
                bmp.EndInit();
                bmp.Freeze();
                return bmp;
            }
            catch { return null; }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}