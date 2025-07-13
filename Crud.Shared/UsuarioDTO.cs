using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Shared
{
    public class UsuarioDTO
    {
        public int UsuarioID { get; set; }
        public string? Cedula { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Password { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
        public byte[]? Foto{ get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? RolID { get; set; }
        public string? PasswordActual { get; set; }
        public string? ConfirmarPassword { get; set; }
    }
}
