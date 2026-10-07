using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Web.Controllers;

public class PrestamosController : Controller
{
    private readonly ISocioRepositorio _socioRepositorio;
    private readonly ILibroRepositorio _libroRepositorio;

    public PrestamosController(ISocioRepositorio socioRepositorio, ILibroRepositorio libroRepositorio)
    {
        _socioRepositorio = socioRepositorio;
        _libroRepositorio = libroRepositorio;
    }

    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
    {
        var fechaDesde = desde ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var fechaHasta = hasta ?? DateTime.Today;
        var reporte = await _socioRepositorio.ObtenerReporteAsync(fechaDesde, fechaHasta);
        ViewData["Title"] = "Reporte de Préstamos";
        ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");
        ViewData["Total"] = reporte.Count();
        return View(reporte);
    }

    public async Task<IActionResult> Create()
    {
        await CargarCombosAsync();
        ViewData["Title"] = "Nuevo Préstamo";
        return View(new PrestamoNuevo());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PrestamoNuevo model)
    {
        if (model.FechaLimite < model.FechaPrestamo)
        {
            ModelState.AddModelError("FechaLimite", "La fecha límite no puede ser anterior a la fecha de préstamo.");
        }
        if (!ModelState.IsValid)
        {
            await CargarCombosAsync(model.SocioId, model.LibroId);
            ViewData["Title"] = "Nuevo Préstamo";
            return View(model);
        }
        await _socioRepositorio.InsertarPrestamoAsync(model.SocioId, model.LibroId, model.FechaPrestamo, model.FechaLimite);
        TempData["Mensaje"] = "Préstamo registrado correctamente.";
        TempData["TipoMensaje"] = "success";
        return RedirectToAction(nameof(Reporte));
    }

    private async Task CargarCombosAsync(int? socioId = null, int? libroId = null)
    {
        var socios = await _socioRepositorio.ListarAsync();
        var libros = await _libroRepositorio.ListarAsync();
        ViewData["Socios"] = new SelectList(socios, "SocioId", "Nombre", socioId);
        ViewData["Libros"] = new SelectList(libros, "LibroId", "Titulo", libroId);
    }
}
