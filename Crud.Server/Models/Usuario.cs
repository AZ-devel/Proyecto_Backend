using System;
using System.Collections.Generic;

namespace Crud.Server.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Cedula { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Apellido { get; set; }

    public string? Password { get; set; }

    public string? Direccion { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public bool Activo { get; set; }

    public int? IdRolPer { get; set; }
    public byte[]? Foto { get; set; }
    public virtual Role? IdRolPerNavigation { get; set; }

    public virtual ICollection<OrdenVentum> OrdenVenta { get; } = new List<OrdenVentum>();
}
