using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Web.Controllers;

public class LibrosController : Controller
{
    private readonly ILibroRepositorio _libroRepositorio;

    public LibrosController(ILibroRepositorio libroRepositorio)
    {
        _libroRepositorio = libroRepositorio;
    }

    public async Task<IActionResult> Index(string? titulo)
    {
        IEnumerable<Libro> libros;
        if (!string.IsNullOrWhiteSpace(titulo))
        {
            libros = await _libroRepositorio.BuscarPorTituloAsync(titulo);
            ViewData["Titulo"] = titulo;
        }
        else
        {
            libros = await _libroRepositorio.ListarAsync();
        }
        ViewData["Title"] = "Libros";
        return View(libros);
    }

    public async Task<IActionResult> Details(int id)
    {
        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null) return NotFound();
        ViewData["Title"] = "Detalle del Libro";
        return View(libro);
    }

    public async Task<IActionResult> Create()
    {
        await CargarAutoresAsync();
        ViewData["Title"] = "Nuevo Libro";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            ViewData["Title"] = "Nuevo Libro";
            return View(libro);
        }
        await _libroRepositorio.InsertarAsync(libro);
        TempData["Mensaje"] = "Libro registrado correctamente.";
        TempData["TipoMensaje"] = "success";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null) return NotFound();
        await CargarAutoresAsync(libro.AutorId);
        ViewData["Title"] = "Editar Libro";
        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Libro libro)
    {
        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            ViewData["Title"] = "Editar Libro";
            return View(libro);
        }
        await _libroRepositorio.ActualizarAsync(libro);
        TempData["Mensaje"] = "Libro actualizado correctamente.";
        TempData["TipoMensaje"] = "success";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
        if (libro == null) return NotFound();
        ViewData["Title"] = "Eliminar Libro";
        return View(libro);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _libroRepositorio.EliminarAsync(id);
        TempData["Mensaje"] = "Libro eliminado correctamente.";
        TempData["TipoMensaje"] = "warning";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarAutoresAsync(int? autorSeleccionado = null)
    {
        var autores = await _libroRepositorio.ListarAutoresAsync();
        ViewData["Autores"] = new SelectList(autores, "AutorId", "Nombre", autorSeleccionado);
    }
}
