namespace Crud.Server.Models
{
    public partial class HistorialPassword
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
