using System;
using System.Collections.Generic;

namespace Crud.Server.Models;

public partial class DetalleOrden
{
    public int DetalleId { get; set; }

    public int OrdenId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public virtual OrdenVentum Orden { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
