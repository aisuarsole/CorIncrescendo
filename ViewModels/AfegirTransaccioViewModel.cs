using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CorIncrescendo.Models;
using CorIncrescendo.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CorIncrescendo.ViewModels
{
    public partial class AfegirTransaccioViewModel : ObservableObject
    {
        private readonly EconomiaService _economiaService;
        private readonly AuthService _authService;

        [ObservableProperty] private bool esIngres = true;
        [ObservableProperty] private bool esGasto;
        [ObservableProperty] private string import = string.Empty;
        [ObservableProperty] private string descripcio = string.Empty;
        [ObservableProperty] private string categoria = "General";
        [ObservableProperty] private DateTime data = DateTime.Today;
        [ObservableProperty] private string errorMessage = string.Empty;
        [ObservableProperty] private bool hasError;

        public List<string> Categories { get; } = new()
    {
        "General", "Quotes socis", "Concerts", "Assajos",
        "Material", "Desplaçaments", "Administració", "Altres"
    };

        public AfegirTransaccioViewModel(EconomiaService economiaService, AuthService authService)
        {
            _economiaService = economiaService;
            _authService = authService;
        }

        [RelayCommand]
        private void SeleccionarTipus(string tipus)
        {
            EsIngres = tipus == "ingres";
            EsGasto = tipus == "gasto";
        }

        [RelayCommand]
        private async Task GuardarAsync()
        {
            if (!decimal.TryParse(Import.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var importDecimal) || importDecimal <= 0)
            {
                ErrorMessage = "Introdueix un import vàlid.";
                HasError = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(Descripcio))
            {
                ErrorMessage = "La descripció és obligatòria.";
                HasError = true;
                return;
            }

            var user = _authService.GetCurrentUser();
            var t = new Transaccio
            {
                Tipus = EsIngres ? TipusTransaccio.Ingres : TipusTransaccio.Gasto,
                Import = importDecimal,
                Descripcio = Descripcio,
                Categoria = Categoria,
                Data = Data,
                UserId = user?.Id ?? string.Empty
            };

            await _economiaService.AfegirTransaccioAsync(t);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancellarAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}


