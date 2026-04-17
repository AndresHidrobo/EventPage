using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Eventual.Models;

[Table("events")]
public class Events
{
    [Key]
    [Column("Id")]
    public int id { get; set; }

    [Required(ErrorMessage = "El nombre del evento es obligatorio.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 150 caracteres.")]
    [Display(Name = "Nombre del Evento")]
    [Column("Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "La descripción debe tener al menos 10 caracteres.")]
    [Display(Name = "Descripción")]
    [Column("Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ubicación es obligatoria.")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "La ubicación debe tener al menos 3 caracteres.")]
    [Display(Name = "Ubicación")]
    [Column("location")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [FutureDate]
    [Display(Name = "Fecha del evento")]
    [Column("EventDate")]
    public DateTime EventDate { get; set; }

    [StringLength(500)]
    [Display(Name = "Imagen del Poster")]
    [Column("Poster")]
    public string? Poster { get; set; } = string.Empty;

    // Archivo de imagen subido (no se guarda en BD)
    [NotMapped]
    [Display(Name = "Imagen del Poster")]
    public IFormFile? ImageFile { get; set; }

    // Helper properties (not mapped)
    [NotMapped]
    public string FormattedDate => EventDate.ToString("dd MMM yyyy");

    [NotMapped]
    public string FallbackImage => string.IsNullOrWhiteSpace(Poster)
        ? $"https://picsum.photos/seed/{id + 10}/600/400"
        : Poster;
}
