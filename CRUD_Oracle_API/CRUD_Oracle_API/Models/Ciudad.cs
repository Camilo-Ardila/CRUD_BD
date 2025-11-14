using CRUD_Oracle_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("CIUDAD")]
public class Ciudad
{
    [Key]
    [Column("IDCIUDAD")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCiudad { get; set; }

    [Column("NOMBRECIUDAD")]
    [Required]
    [StringLength(50)]
    public string NombreCiudad { get; set; } = null!;

    [Column("IDDEPARTAMENTO")]
    [Required]
    public int IdDepartamento { get; set; }

    [ForeignKey("IdDepartamento")]
    public Departamento Departamento { get; set; } = null!;

    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}