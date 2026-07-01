using CorIncrescendo.Models;
using Plugin.Firebase.Firestore;

namespace CorIncrescendo.Services;

public class EconomiaService
{
    private readonly IFirebaseFirestore _db;
    private const string Col = "transaccions";

    public EconomiaService(IFirebaseFirestore db)
    {
        _db = db;
    }

    public async Task AfegirTransaccioAsync(Transaccio t)
    {
        await _db.Collection(Col).Document(t.Id).SetDataAsync(t);
    }

    public async Task EliminarTransaccioAsync(string id)
    {
        await _db.Collection(Col).Document(id).DeleteDocumentAsync();
    }

    public async Task<List<Transaccio>> GetTransaccionsAsync()
    {
        var snapshot = await _db.Collection(Col).GetDocumentsAsync<Transaccio>();
        return snapshot.ToList();
    }

    // Filtres
    public async Task<List<Transaccio>> GetPerDiaAsync(DateTime dia)
    {
        var all = await GetTransaccionsAsync();
        return all.Where(t => t.Data.Date == dia.Date).ToList();
    }

    public async Task<List<Transaccio>> GetPerMesAsync(int any, int mes)
    {
        var all = await GetTransaccionsAsync();
        return all.Where(t => t.Data.Year == any && t.Data.Month == mes).ToList();
    }

    public async Task<List<Transaccio>> GetPerCursAsync(DateTime inici, DateTime fi)
    {
        var all = await GetTransaccionsAsync();
        return all.Where(t => t.Data.Date >= inici.Date && t.Data.Date <= fi.Date).ToList();
    }

    // Càlculs
    public decimal TotalIngressos(List<Transaccio> list) =>
        list.Where(t => t.Tipus == TipusTransaccio.Ingres).Sum(t => t.Import);

    public decimal TotalGastos(List<Transaccio> list) =>
        list.Where(t => t.Tipus == TipusTransaccio.Gasto).Sum(t => t.Import);

    public decimal Balanc(List<Transaccio> list) =>
        TotalIngressos(list) - TotalGastos(list);
}