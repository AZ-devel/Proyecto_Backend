using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Shared
{
    public class CambiarPasswordDTO
    {
        public int UsuarioID { get; set; }
        public string? PasswordActual { get; set; }
        public string? Password { get; set; }
        public string? ConfirmarPassword { get; set; }
    }
}
