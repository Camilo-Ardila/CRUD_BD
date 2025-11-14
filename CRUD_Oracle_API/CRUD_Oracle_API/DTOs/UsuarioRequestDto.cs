// DTOs/UsuarioRequestDto.cs
using System.ComponentModel.DataAnnotations;

namespace CRUD_Oracle_API.DTOs
{
    /// <summary>
    /// Usado para crear y actualizar usuarios
    /// </summary>
    public class UsuarioRequestDto
    {
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [Range(10000000, 9999999999, ErrorMessage = "La cédula debe tener 8 o 10 dígitos.")]
        public long Cc { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public int IdCiudad { get; set; }
    }
}