using Microsoft.AspNetCore.Mvc;
using Eventual.Data;
using Eventual.Models;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Eventual.Controllers;

public class EventsController : Controller
{
    private readonly MysqlDbContext _context;
    private readonly IWebHostEnvironment _env;

    // Tamaño máximo al que se redimensiona la imagen (ancho máximo en px)
    private const int MaxWidth  = 1200;
    private const int MaxHeight = 800;

    public EventsController(MysqlDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // Guarda y redimensiona la imagen subida. Retorna la ruta relativa o null si falla.
    private string? SaveImage(IFormFile file)
    {
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowed.Contains(ext)) return null;

        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsFolder);

        // Siempre guardamos como .jpg para uniformidad
        var fileName = $"{Guid.NewGuid()}.jpg";
        var fullPath = Path.Combine(uploadsFolder, fileName);

        using var image = Image.Load(file.OpenReadStream());

        // Redimensionar solo si supera los límites, manteniendo proporción
        if (image.Width > MaxWidth || image.Height > MaxHeight)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(MaxWidth, MaxHeight),
                Mode = ResizeMode.Max   // mantiene proporción, no recorta
            }));
        }

        image.Save(fullPath, new JpegEncoder { Quality = 85 });

        return $"/uploads/{fileName}";
    }

    // GET: /Events
    public IActionResult Index(string? search)
    {
        var query = _context.events.AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(e =>
                e.Name.Contains(search) ||
                e.Location.Contains(search));

        var events = query.OrderByDescending(e => e.EventDate).ToList();
        ViewBag.Search = search;
        return View(events);
    }

    // GET: /Events/Create
    public IActionResult Create()
    {
        return View(new Events { EventDate = DateTime.Today.AddDays(30) });
    }

    // POST: /Events/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Events ev)
    {
        if (ModelState.IsValid)
        {
            if (ev.ImageFile != null && ev.ImageFile.Length > 0)
            {
                var imagePath = SaveImage(ev.ImageFile);
                if (imagePath != null)
                    ev.Poster = imagePath;
                else
                    ModelState.AddModelError("ImageFile", "Solo se permiten imágenes JPG, PNG, WEBP o GIF.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(ev);
                _context.SaveChanges();
                TempData["Success"] = $"¡Evento \"{ev.Name}\" creado exitosamente!";
                return RedirectToAction(nameof(Index));
            }
        }
        return View(ev);
    }

    // GET: /Events/Edit/5
    public IActionResult Edit(int id)
    {
        var ev = _context.events.Find(id);
        if (ev == null) return NotFound();
        return View(ev);
    }

    // POST: /Events/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Events ev)
    {
        if (id != ev.id) return NotFound();

        if (ModelState.IsValid)
        {
            if (ev.ImageFile != null && ev.ImageFile.Length > 0)
            {
                var newPath = SaveImage(ev.ImageFile);
                if (newPath != null)
                    ev.Poster = newPath;
                else
                    ModelState.AddModelError("ImageFile", "Solo se permiten imágenes JPG, PNG, WEBP o GIF.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ev);
                    _context.SaveChanges();
                    TempData["Success"] = $"¡Evento \"{ev.Name}\" actualizado correctamente!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.events.Any(e => e.id == ev.id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
        }
        return View(ev);
    }

    // GET: /Events/Delete/5
    public IActionResult Delete(int id)
    {
        var ev = _context.events.Find(id);
        if (ev == null) return NotFound();
        return View(ev);
    }

    // POST: /Events/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var ev = _context.events.Find(id);
        if (ev != null)
        {
            _context.events.Remove(ev);
            _context.SaveChanges();
            TempData["Success"] = "Evento eliminado correctamente.";
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: /Events/Detail/5
    public IActionResult Detail(int id)
    {
        var ev = _context.events.Find(id);
        if (ev == null) return NotFound();
        return View(ev);
    }
}
