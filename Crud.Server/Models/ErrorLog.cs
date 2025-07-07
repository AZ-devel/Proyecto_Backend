using System;
using System.Collections.Generic;

namespace Crud.Server.Models;

public partial class ErrorLog
{
    public string? Tabla { get; set; }

    public string? Error { get; set; }

    public DateTime? FechaHora { get; set; }
}
