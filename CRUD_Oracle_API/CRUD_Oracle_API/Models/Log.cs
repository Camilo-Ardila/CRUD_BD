// Models/Log.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_Oracle_API.Models
{
    [Table("LOGS")]
    public class Log
    {
        [Key]
        [Column("IDLOG")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLog { get; set; }

        [Column("CC")]
        public long Cc { get; set; }

        [Column("FECHAINGRESO")]
        public DateTime FechaIngreso { get; set; }

        [Column("TIPOACCION")]
        public string TipoAccion { get; set; } = null!;

        // Navegación
        [ForeignKey("Cc")]
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey("TipoAccion")]
        public Accion Accion { get; set; } = null!;
    }
}