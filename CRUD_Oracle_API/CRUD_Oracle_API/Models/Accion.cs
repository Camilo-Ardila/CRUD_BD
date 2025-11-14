using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("ACCIONES")]
public class Accion
{
    [Key]
    [Column("TIPOACCION")]
    [StringLength(20)]
    [Required]
    public string TipoAccion { get; set; } = string.Empty;
}