using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventual.Models;
[Table("events")]
public class Events
{
    [Key]
    [Column("Id")]
    public int id { get; set; }
    
    [Required(ErrorMessage = "El nombre del evento es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
    [Display(Name = "Nombre del Evento")]
    [Column("Name")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(2000)]
    [Display(Name = "Descripción")]
    [Column("Description")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    [StringLength(300)]
    [Display(Name = "Ubicación")]
    [Column("location")]
    public string Location { get; set; }= string.Empty;
    
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [Display(Name = "Fecha del evento")]
    [Column("EventDate")]
    public DateTime EventDate { get; set; }
    
    [StringLength(500)]
    [Display(Name = "URL del Poster/Imagen")]
    [Column("Poster")]
    public string? Poster { get; set; } = string.Empty;

    // Helper property (not mapped)
    [NotMapped]
    public string FormattedDate => EventDate.ToString("dd MMM yyyy");
    [NotMapped]
    public string FallbackImage => string.IsNullOrWhiteSpace(Poster)
        ? $"https://picsum.photos/seed/{id + 10}/600/400"
        : Poster;

}