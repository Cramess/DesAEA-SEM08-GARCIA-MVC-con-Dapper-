using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Socio
{
    public int SocioId { get; set; }
    [Display(Name = "DNI")]
    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(15, ErrorMessage = "El DNI no puede tener más de 15 caracteres.")]
    public string DNI { get; set; } = string.Empty;
    [Display(Name = "Nombre completo")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
    [Display(Name = "Correo electrónico")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(100, ErrorMessage = "El correo no puede tener más de 100 caracteres.")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    public bool Activo { get; set; } = true;
}
