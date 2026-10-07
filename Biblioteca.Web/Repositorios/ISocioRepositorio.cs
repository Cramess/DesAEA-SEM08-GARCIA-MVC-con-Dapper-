using Biblioteca.Web.Models;

namespace Biblioteca.Web.Repositorios;

public interface ISocioRepositorio
{
    Task<IEnumerable<Socio>> ListarAsync();
    Task InsertarAsync(Socio socio);
    Task<bool> ExisteDNIAsync(string dni, int socioId);
    Task<IEnumerable<PrestamoReporte>> ObtenerReporteAsync(DateTime desde, DateTime hasta);
    Task InsertarPrestamoAsync(int socioId, int libroId, DateTime fechaPrestamo, DateTime fechaLimite);
}
