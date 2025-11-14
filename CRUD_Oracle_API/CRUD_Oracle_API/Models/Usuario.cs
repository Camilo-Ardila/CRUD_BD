using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_Oracle_API.Models
{
    [Table("USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("CC")]
        public long Cc { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; } = null!;

        [Column("FECHANACIMIENTO")]
        public DateTime FechaNacimiento { get; set; }

        [Column("IDCIUDAD")]
        public int IdCiudad { get; set; }

        // Relación
        [ForeignKey("IdCiudad")]
        public Ciudad Ciudad { get; set; } = null!;
    }
}