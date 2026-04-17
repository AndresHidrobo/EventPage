using Microsoft.AspNetCore.Mvc;
using Eventual.Data;
using Eventual.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventual.Controllers;

public class EventsController : Controller
{
    private readonly MysqlDbContext _context;
    public EventsController(MysqlDbContext context)
    {
        _context = context;
    }
    
    // GET: /Events — Listado administrativo
    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.events.AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(e =>
                e.Name.Contains(search) ||
                e.Location.Contains(search));

        var events = await query.OrderByDescending(e => e.EventDate).ToListAsync();
        ViewBag.Search = search;
        return View(events);
    }
    // GET: /Events/Create
    public IActionResult Create()
    {
        
        return View(new Events { EventDate = DateTime.Now.AddDays(30) });
    }
    // POST: /Events/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Events ev)
    {
        if (ModelState.IsValid)
        {
            _context.Add(ev);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"¡Evento \"{ev.Name}\" creado exitosamente!";
            return RedirectToAction(nameof(Index));
        }
        return View(ev);
    }
    // GET: /Events/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var ev = await _context.events.FindAsync(id);
        if (ev == null) return NotFound();
        return View(ev);
    }
    // POST: /Events/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Events ev)
    {
        if (id != ev.id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ev);
                await _context.SaveChangesAsync();
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
        return View(ev);
    }
    // GET: /Events/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _context.events.FindAsync(id);
        if (ev == null) return NotFound();
        return View(ev);
    }

    // POST: /Events/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ev = await _context.events.FindAsync(id);
        if (ev != null)
        {
            _context.events.Remove(ev);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Evento eliminado correctamente.";
        }
        return RedirectToAction(nameof(Index));
    }
}