using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class SociosController : Controller
{
    private readonly ISocioRepositorio _socioRepositorio;

    public SociosController(ISocioRepositorio socioRepositorio)
    {
        _socioRepositorio = socioRepositorio;
    }

    public async Task<IActionResult> Index()
    {
        var socios = await _socioRepositorio.ListarAsync();
        ViewData["Title"] = "Socios";
        return View(socios);
    }

    public IActionResult Create()
    {
        ViewData["Title"] = "Nuevo Socio";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Socio socio)
    {
        if (await _socioRepositorio.ExisteDNIAsync(socio.DNI, 0))
        {
            ModelState.AddModelError("DNI", "Ya existe un socio con ese DNI.");
        }
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Nuevo Socio";
            return View(socio);
        }
        await _socioRepositorio.InsertarAsync(socio);
        TempData["Mensaje"] = "Socio registrado correctamente.";
        TempData["TipoMensaje"] = "success";
        return RedirectToAction(nameof(Index));
    }
}
