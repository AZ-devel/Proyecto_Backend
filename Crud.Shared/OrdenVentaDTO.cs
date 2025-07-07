using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Shared
{
    public class OrdenVentaDTO
    {
        public int OrdenID { get; set; }
        public int ClienteID { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal TotalVenta { get; set; }
        public string? Estado { get; set; }
        public List<DetalleOrdenDTO> ListaOrdenes { get; set; }
    }
}
