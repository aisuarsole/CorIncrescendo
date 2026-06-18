using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorIncrescendo.Models
{
    public enum TipusTransaccio { Ingres, Gasto }

    public class Transaccio
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public TipusTransaccio Tipus { get; set; }
        public decimal Import { get; set; }
        public string Descripcio { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public DateTime Data { get; set; } = DateTime.Now;
        public string UserId { get; set; } = string.Empty;
    }
}


