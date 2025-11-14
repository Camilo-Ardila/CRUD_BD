// DTOs/DepartamentoRequestDto.cs
namespace CRUD_Oracle_API.DTOs
{
    /// <summary>
    /// Usado tanto para crear como para actualizar un departamento
    /// </summary>
    public class DepartamentoRequestDto
    {
        public string NombreDepartamento { get; set; } = null!;
    }
}