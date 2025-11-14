// DTOs/UsuarioResponseDto.cs
namespace CRUD_Oracle_API.DTOs
{
    public class UsuarioResponseDto
    {
        public long Cc { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string FechaNacimientoFormateada => FechaNacimiento.ToString("dd/MM/yyyy");
        public int IdCiudad { get; set; }
        public string NombreCiudad { get; set; } = null!;
        public string NombreDepartamento { get; set; } = null!;
    }
}