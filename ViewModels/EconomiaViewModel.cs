using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CorIncrescendo.Models;
using CorIncrescendo.Services;
using System.Collections.ObjectModel;
using System.Transactions;

namespace CorIncrescendo.ViewModels
{
    public partial class EconomiaViewModel : ObservableObject
    {
        private readonly EconomiaService _economiaService;
        private readonly AuthService _authService;

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
            CarregarDades();
        }

        [RelayCommand]
        private void SeleccionarPeriode(string periode)
        {
            PeriodeDia = periode == "dia";
            PeriodeMes = periode == "mes";
            PeriodeCurs = periode == "curs";
            CarregarDades();
        }

        [RelayCommand]
        private void MesAnterior()
        {
            MesActual = MesActual.AddMonths(-1);
            OnPropertyChanged(nameof(MesActualText));
            CarregarDades();
        }

        [RelayCommand]
        private void MesSeguent()
        {
            MesActual = MesActual.AddMonths(1);
            OnPropertyChanged(nameof(MesActualText));
            CarregarDades();
        }

        [RelayCommand]
        private async Task AfegirAsync()
        {
            await Shell.Current.GoToAsync("AfegirTransaccioPage");
        }

        [RelayCommand]
        private void Eliminar(Transaccio t)
        {
            _economiaService.EliminarTransaccio(t.Id);
            CarregarDades();
        }

        public void CarregarDades()
        {
            List<Transaccio> list;

            if (PeriodeDia)
                list = _economiaService.GetPerDia(DiaSeleccionat);
            else if (PeriodeMes)
                list = _economiaService.GetPerMes(MesActual.Year, MesActual.Month);
            else
                list = _economiaService.GetPerCurs(CursInici, CursFi);

            Transaccions = new ObservableCollection<Transaccio>(
                list.OrderByDescending(t => t.Data));

            TotalIngressos = _economiaService.TotalIngressos(list);
            TotalGastos = _economiaService.TotalGastos(list);
            Balanc = _economiaService.Balanc(list);
            BalancColor = Balanc >= 0 ? Color.FromArgb("#2E7D32") : Color.FromArgb("#C62828");
        }

        // Recargar al volver de AfegirTransaccioPage
        public void OnAppearing() => CarregarDades();
    }
}


