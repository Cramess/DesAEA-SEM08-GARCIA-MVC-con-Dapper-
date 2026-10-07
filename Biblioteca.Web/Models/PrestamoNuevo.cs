using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class PrestamoNuevo
{
    [Display(Name = "Socio")]
    [Required(ErrorMessage = "Debe seleccionar un socio.")]
    public int SocioId { get; set; }

    [Display(Name = "Libro")]
    [Required(ErrorMessage = "Debe seleccionar un libro.")]
    public int LibroId { get; set; }

    [Display(Name = "Fecha de préstamo")]
    [Required(ErrorMessage = "La fecha de préstamo es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime FechaPrestamo { get; set; } = DateTime.Today;

    [Display(Name = "Fecha límite de devolución")]
    [Required(ErrorMessage = "La fecha límite es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime FechaLimite { get; set; } = DateTime.Today.AddDays(14);
}
