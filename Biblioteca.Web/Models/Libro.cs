using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Libro
{
    public int LibroId { get; set; }
    [Display(Name = "Título")]
    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(200, ErrorMessage = "El título no puede tener más de 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;
    [Display(Name = "ISBN")]
    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    [StringLength(20, ErrorMessage = "El ISBN no puede tener más de 20 caracteres.")]
    public string ISBN { get; set; } = string.Empty;
    [Display(Name = "Autor")]
    [Required(ErrorMessage = "Debe seleccionar un autor.")]
    public int AutorId { get; set; }
    [Display(Name = "Autor")]
    public string? NombreAutor { get; set; }
    [Display(Name = "Ejemplares")]
    [Required(ErrorMessage = "El número de ejemplares es obligatorio.")]
    [Range(1, 999, ErrorMessage = "Los ejemplares deben estar entre 1 y 999.")]
    public int Ejemplares { get; set; }
    public bool Activo { get; set; } = true;
}
