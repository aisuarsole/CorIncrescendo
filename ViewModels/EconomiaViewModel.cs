using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CorIncrescendo.Models;
using CorIncrescendo.Services;
using IntelliJ.Lang.Annotations;
using System.Collections.ObjectModel;
using System.Transactions;

namespace CorIncrescendo.ViewModels
{
    public partial class EconomiaViewModel : ObservableObject
    {
        private readonly EconomiaService _economiaService;
        private readonly AuthService _authService;

        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private ObservableCollection<Transaccio> transaccions = new();
        [ObservableProperty] private decimal totalIngressos;
        [ObservableProperty] private decimal totalGastos;
        [ObservableProperty] private decimal balanc;
        [ObservableProperty] private Color balancColor = Colors.Green;

        [ObservableProperty] private bool periodeDia = true;
        [ObservableProperty] private bool periodeMes;
        [ObservableProperty] private bool periodeCurs;

        [ObservableProperty] private DateTime diaSeleccionat = DateTime.Today;
        [ObservableProperty] private DateTime mesActual = DateTime.Today;
        [ObservableProperty] private DateTime cursInici = new DateTime(DateTime.Today.Year, 9, 1);
        [ObservableProperty] private DateTime cursFi = new DateTime(DateTime.Today.Year + 1, 6, 30);

        public string MesActualText =>
            MesActual.ToString("MMMM yyyy", new System.Globalization.CultureInfo("ca-ES"));

        public EconomiaViewModel(EconomiaService economiaService, AuthService authService)
        {
            _economiaService = economiaService;
            _authService = authService;
            CarregarDadesAsync();
        }

        [RelayCommand]
        private void SeleccionarPeriode(string periode)
        {
            PeriodeDia = periode == "dia";
            PeriodeMes = periode == "mes";
            PeriodeCurs = periode == "curs";
            CarregarDadesAsync();
        }

        [RelayCommand]
        private void MesAnterior()
        {
            MesActual = MesActual.AddMonths(-1);
            OnPropertyChanged(nameof(MesActualText));
            CarregarDadesAsync();
        }

        [RelayCommand]
        private void MesSeguent()
        {
            MesActual = MesActual.AddMonths(1);
            OnPropertyChanged(nameof(MesActualText));
            CarregarDadesAsync();
        }

        [RelayCommand]
        private async Task AfegirAsync()
        {
            await Shell.Current.GoToAsync("AfegirTransaccioPage");
        }

        public async Task CarregarDadesAsync()
        {
            IsBusy = true;

            List<Transaccio> list;

            if (PeriodeDia)
                list = await _economiaService.GetPerDiaAsync(DiaSeleccionat);
            else if (PeriodeMes)
                list = await _economiaService.GetPerMesAsync(MesActual.Year, MesActual.Month);
            else
                list = await _economiaService.GetPerCursAsync(CursInici, CursFi);

            Transaccions = new ObservableCollection<Transaccio>(
                list.OrderByDescending(t => t.Data));

            TotalIngressos = _economiaService.TotalIngressos(list);
            TotalGastos = _economiaService.TotalGastos(list);
            Balanc = _economiaService.Balanc(list);
            BalancColor = Balanc >= 0
                ? Color.FromArgb("#2E7D32")
                : Color.FromArgb("#C62828");

            IsBusy = false;
        }

        [RelayCommand]
        private async Task EliminarAsync(Transaccio t)
        {
            await _economiaService.EliminarTransaccioAsync(t.Id);
            await CarregarDadesAsync();
        }

        public async Task OnAppearing() => await CarregarDadesAsync();
    }
}


