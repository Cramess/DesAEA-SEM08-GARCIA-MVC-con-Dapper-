namespace Biblioteca.Web.Models;

public class PrestamoReporte
{
    public int PrestamoId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaLimite { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string NombreSocio { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string TituloLibro { get; set; } = string.Empty;
    public string NombreAutor { get; set; } = string.Empty;
    public DateTime? FechaDevolucion { get; set; }
    public decimal? Multa { get; set; }
    public bool EstaAtrasado => Estado != "Devuelto" && FechaLimite < DateTime.Today;
}
