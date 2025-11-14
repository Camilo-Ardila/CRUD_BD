using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("DEPARTAMENTO")]
public class Departamento
{
    [Key]
    [Column("IDDEPARTAMENTO")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdDepartamento { get; set; }

    [Column("NOMBREDEPARTAMENTO")]
    [Required]
    [StringLength(50)]
    public string NombreDepartamento { get; set; } = null!;

    public ICollection<Ciudad> Ciudades { get; set; } = new List<Ciudad>();
}