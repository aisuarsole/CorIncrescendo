using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CorIncrescendo.Services;

namespace CorIncrescendo.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty] private string salutacio = "Benvingut/da!";

        public MainViewModel(AuthService authService)
        {
            _authService = authService;
            var user = _authService.GetCurrentUser();
            if (user != null)
                Salutacio = $"Hola, {user.Email.Split('@')[0]}!";
        }

        [RelayCommand]
        private async Task AnarEconomiaAsync()
        {
            await Shell.Current.GoToAsync("EconomiaPage");
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            _authService.Logout();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }

}

