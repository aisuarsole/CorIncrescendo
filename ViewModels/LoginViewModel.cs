using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CorIncrescendo.Services;

namespace CorIncrescendo.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty] private string email = string.Empty;
        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private string errorMessage = string.Empty;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool hasError;
        [ObservableProperty] private string nom = string.Empty;
        [ObservableProperty] private string cognoms = string.Empty;
        [ObservableProperty] private bool mostrarCampsRegistre; // controla visibilitat dels camps extra

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MostrarError("Omple tots els camps.");
                return;
            }

            IsBusy = true;
            HasError = false;

            var (ok, error) = await _authService.LoginAsync(Email, Password);

            IsBusy = false;

            if (ok)
                await Shell.Current.GoToAsync("//MainPage");
            else
                MostrarError(error);
        }

        [RelayCommand]
        private async Task RegistrarAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MostrarError("Omple tots els camps.");
                return;
            }
            if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Cognoms))
            {
                MostrarError("Introdueix el teu nom i cognoms.");
                return;
            }
            if (Password.Length < 6)
            {
                MostrarError("La contrasenya ha de tenir mínim 6 caràcters.");
                return;
            }

            IsBusy = true;
            HasError = false;

            var (ok, error) = await _authService.RegistrarAsync(Email, Password, Nom, Cognoms);

            IsBusy = false;

            if (ok)
                await Shell.Current.GoToAsync("//MainPage");
            else
                MostrarError(error);
        }

        private void MostrarError(string msg)
        {
            ErrorMessage = msg;
            HasError = true;
        }

        [RelayCommand]
        private void MostrarRegistre()
        {
            MostrarCampsRegistre = true;
            HasError = false;
        }
    }
}