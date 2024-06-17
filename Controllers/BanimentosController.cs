using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using redeSocial.Models;

namespace redeSocial_WebApplication.Controllers
{
    public class BanimentosController : Controller
    {
        private readonly Contexto _context;

        public BanimentosController(Contexto context)
        {
            _context = context;
        }

        // GET: Banimentos
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserLoggedIn") != null || HttpContext.Session.GetString("UserLoggedIn") != "")
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserLoggedIn")!);

                var banimentos = await _context.Banimentos.Include(b => b.usuario).Include(b => b.usuarioBan).Where(x => x.usuarioID == userId).ToListAsync();
                return View(banimentos);
            }
            return NotFound();
        }

        // GET: Banimentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Banimentos == null)
            {
                return NotFound();
            }

            var banimentos = await _context.Banimentos
                .Include(b => b.usuario)
                .Include(b => b.usuarioBan)
                .FirstOrDefaultAsync(m => m.banID == id);
            if (banimentos == null)
            {
                return NotFound();
            }

            return View(banimentos);
        }

        // GET: Banimentos/Create
        public IActionResult Create()
        {
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID");
            ViewData["usuarioBanID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID");
            return View();
        }

        // POST: Banimentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("banID,usuarioID,usuarioBanID")] Banimentos banimentos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(banimentos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", banimentos.usuarioID);
            ViewData["usuarioBanID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", banimentos.usuarioBanID);
            return View(banimentos);
        }

        // GET: Banimentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Banimentos == null)
            {
                return NotFound();
            }

            var banimentos = await _context.Banimentos.FindAsync(id);
            if (banimentos == null)
            {
                return NotFound();
            }
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", banimentos.usuarioID);
            ViewData["usuarioBanID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", banimentos.usuarioBanID);
            return View(banimentos);
        }

        // POST: Banimentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("banID,usuarioID,usuarioBanID")] Banimentos banimentos)
        {
            if (id != banimentos.banID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banimentos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BanimentosExists(banimentos.banID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", banimentos.usuarioID);
            ViewData["usuarioBanID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", banimentos.usuarioBanID);
            return View(banimentos);
        }

        // GET: Banimentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Banimentos == null)
            {
                return NotFound();
            }

            var banimentos = await _context.Banimentos
                .Include(b => b.usuario)
                .Include(b => b.usuarioBan)
                .FirstOrDefaultAsync(m => m.banID == id);
            if (banimentos == null)
            {
                return NotFound();
            }

            return View(banimentos);
        }

        // POST: Banimentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Banimentos == null)
            {
                return Problem("Entity set 'Contexto.Banimentos'  is null.");
            }
            var banimentos = await _context.Banimentos.FindAsync(id);
            if (banimentos != null)
            {
                _context.Banimentos.Remove(banimentos);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BanimentosExists(int id)
        {
          return (_context.Banimentos?.Any(e => e.banID == id)).GetValueOrDefault();
        }
    }
}
