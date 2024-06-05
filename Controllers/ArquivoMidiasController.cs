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
    public class ArquivoMidiasController : Controller
    {
        private readonly Contexto _context;

        public ArquivoMidiasController(Contexto context)
        {
            _context = context;
        }

        // GET: ArquivoMidias
        public async Task<IActionResult> Index()
        {
            var contexto = _context.Arquivos.Include(a => a.post);
            return View(await contexto.ToListAsync());
        }

        // GET: ArquivoMidias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Arquivos == null)
            {
                return NotFound();
            }

            var arquivoMidia = await _context.Arquivos
                .Include(a => a.post)
                .FirstOrDefaultAsync(m => m.arquivoID == id);
            if (arquivoMidia == null)
            {
                return NotFound();
            }

            return View(arquivoMidia);
        }

        // GET: ArquivoMidias/Create
        public IActionResult Create()
        {
            ViewData["postID"] = new SelectList(_context.Postagens, "postID", "postID");
            return View();
        }

        // POST: ArquivoMidias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("arquivoID,tipoArquivo,nomeArmazenamento,postID")] ArquivoMidia arquivoMidia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arquivoMidia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["postID"] = new SelectList(_context.Postagens, "postID", "postID", arquivoMidia.postID);
            return View(arquivoMidia);
        }

        // GET: ArquivoMidias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Arquivos == null)
            {
                return NotFound();
            }

            var arquivoMidia = await _context.Arquivos.FindAsync(id);
            if (arquivoMidia == null)
            {
                return NotFound();
            }
            ViewData["postID"] = new SelectList(_context.Postagens, "postID", "postID", arquivoMidia.postID);
            return View(arquivoMidia);
        }

        // POST: ArquivoMidias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("arquivoID,tipoArquivo,nomeArmazenamento,postID")] ArquivoMidia arquivoMidia)
        {
            if (id != arquivoMidia.arquivoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(arquivoMidia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArquivoMidiaExists(arquivoMidia.arquivoID))
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
            ViewData["postID"] = new SelectList(_context.Postagens, "postID", "postID", arquivoMidia.postID);
            return View(arquivoMidia);
        }

        // GET: ArquivoMidias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Arquivos == null)
            {
                return NotFound();
            }

            var arquivoMidia = await _context.Arquivos
                .Include(a => a.post)
                .FirstOrDefaultAsync(m => m.arquivoID == id);
            if (arquivoMidia == null)
            {
                return NotFound();
            }

            return View(arquivoMidia);
        }

        // POST: ArquivoMidias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Arquivos == null)
            {
                return Problem("Entity set 'Contexto.Arquivos'  is null.");
            }
            var arquivoMidia = await _context.Arquivos.FindAsync(id);
            if (arquivoMidia != null)
            {
                _context.Arquivos.Remove(arquivoMidia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArquivoMidiaExists(int id)
        {
          return (_context.Arquivos?.Any(e => e.arquivoID == id)).GetValueOrDefault();
        }
    }
}
