using CorIncrescendo.Models;
using System.Text.Json;

namespace CorIncrescendo.Services
{
    public class EconomiaService
    {
        private const string TransaccionsKey = "transaccions";

        public List<Transaccio> GetTransaccions()
        {
            var json = Preferences.Get(TransaccionsKey, "[]");
            return JsonSerializer.Deserialize<List<Transaccio>>(json) ?? new();
        }

        public void AfegirTransaccio(Transaccio t)
        {
            var list = GetTransaccions();
            list.Add(t);
            Preferences.Set(TransaccionsKey, JsonSerializer.Serialize(list));
        }

        public void EliminarTransaccio(string id)
        {
            var list = GetTransaccions();
            list.RemoveAll(t => t.Id == id);
            Preferences.Set(TransaccionsKey, JsonSerializer.Serialize(list));
        }

        // Filtres de periode
        public List<Transaccio> GetPerDia(DateTime dia) =>
            GetTransaccions().Where(t => t.Data.Date == dia.Date).ToList();

        public List<Transaccio> GetPerMes(int any, int mes) =>
            GetTransaccions().Where(t => t.Data.Year == any && t.Data.Month == mes).ToList();

        public List<Transaccio> GetPerCurs(DateTime inici, DateTime fi) =>
            GetTransaccions().Where(t => t.Data.Date >= inici.Date && t.Data.Date <= fi.Date).ToList();

        // Resum
        public decimal TotalIngressos(List<Transaccio> list) =>
            list.Where(t => t.Tipus == TipusTransaccio.Ingres).Sum(t => t.Import);

        public decimal TotalGastos(List<Transaccio> list) =>
            list.Where(t => t.Tipus == TipusTransaccio.Gasto).Sum(t => t.Import);

        public decimal Balanc(List<Transaccio> list) =>
            TotalIngressos(list) - TotalGastos(list);
    }
}


