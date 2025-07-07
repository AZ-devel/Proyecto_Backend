using System;
using System.Collections.Generic;

namespace Crud.Server.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public bool Descontinuado { get; set; }

    public virtual ICollection<DetalleOrden> DetalleOrdens { get; } = new List<DetalleOrden>();

    public virtual ICollection<ImgsProducto> ImgsProductos { get; } = new List<ImgsProducto>();
}
