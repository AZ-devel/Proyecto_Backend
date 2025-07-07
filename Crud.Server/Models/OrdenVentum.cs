using System;
using System.Collections.Generic;

namespace Crud.Server.Models;

public partial class OrdenVentum
{
    public int OrdenId { get; set; }

    public int ClienteId { get; set; }

    public DateTime FechaVenta { get; set; }

    public decimal TotalVenta { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Usuario Cliente { get; set; } = null!;

    public virtual ICollection<DetalleOrden> DetalleOrdens { get; } = new List<DetalleOrden>();
}
