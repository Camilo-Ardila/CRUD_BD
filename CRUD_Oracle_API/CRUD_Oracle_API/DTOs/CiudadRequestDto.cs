// DTOs/CiudadRequestDto.cs
namespace CRUD_Oracle_API.DTOs
{
    /// <summary>
    /// Usado para crear y actualizar ciudades
    /// </summary>
    public class CiudadRequestDto
    {
        public string NombreCiudad { get; set; } = null!;
        public int IdDepartamento { get; set; }
    }
}