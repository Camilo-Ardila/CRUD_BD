// DTOs/CiudadResponseDto.cs
namespace CRUD_Oracle_API.DTOs
{
    public class CiudadResponseDto
    {
        public int IdCiudad { get; set; }
        public string NombreCiudad { get; set; } = null!;
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; } = null!;
    }
}