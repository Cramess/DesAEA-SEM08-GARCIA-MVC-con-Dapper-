using System.Data;
using Dapper;
using Biblioteca.Web.Models;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class SocioRepositorio : ISocioRepositorio
{
    private readonly string _connectionString;

    public SocioRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BibliotecaDB")!;
    }

    public async Task<IEnumerable<Socio>> ListarAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Socio>("usp_Socios_Listar", commandType: CommandType.StoredProcedure);
    }

    public async Task InsertarAsync(Socio socio)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("usp_Socios_Insertar", new { socio.DNI, socio.Nombre, socio.Email }, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ExisteDNIAsync(string dni, int socioId)
    {
        using var connection = new SqlConnection(_connectionString);
        int count = await connection.ExecuteScalarAsync<int>("usp_Socios_ExisteDNI", new { DNI = dni, SocioId = socioId }, commandType: CommandType.StoredProcedure);
        return count > 0;
    }

    public async Task<IEnumerable<PrestamoReporte>> ObtenerReporteAsync(DateTime desde, DateTime hasta)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<PrestamoReporte>("usp_Prestamos_Reporte", new { Desde = desde.Date, Hasta = hasta.Date }, commandType: CommandType.StoredProcedure);
    }

    public async Task InsertarPrestamoAsync(int socioId, int libroId, DateTime fechaPrestamo, DateTime fechaLimite)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("usp_Prestamos_Insertar", new { SocioId = socioId, LibroId = libroId, FechaPrestamo = fechaPrestamo.Date, FechaLimite = fechaLimite.Date }, commandType: CommandType.StoredProcedure);
    }
}
