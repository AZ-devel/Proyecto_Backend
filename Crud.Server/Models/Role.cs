using System;
using System.Collections.Generic;

namespace Crud.Server.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string? Rol { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
