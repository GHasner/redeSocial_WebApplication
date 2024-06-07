using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using redeSocial.Models;
using SQLitePCL;

namespace redeSocial_WebApplication.Controllers
{
    public class PostagensController : Controller
    {
        private readonly Contexto _context;

        public PostagensController(Contexto context)
        {
            _context = context;
        }

        // GET: Postagens
        public async Task<IActionResult> Index()
        {
            ViewBag.Arquivo = _context.Arquivos.ToListAsync();
            var contexto = _context.Postagens.Include(p => p.usuario);
            return View(await contexto.ToListAsync());
        }

        public static ArquivoMidia? Arquivo(List<ArquivoMidia> arquivos, int postID)
        {
            int index = arquivos.FindIndex(x => x.postID == postID);
            if (index == -1) return null;
            return arquivos[index];
        }

        // GET: Postagens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Postagens == null)
            {
                return NotFound();
            }

            var postagem = await _context.Postagens
                .Include(p => p.usuario)
                .FirstOrDefaultAsync(m => m.postID == id);
            if (postagem == null)
            {
                return NotFound();
            }

            return View(postagem);
        }

        // GET: Postagens/Create
        public IActionResult Create()
        {
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID");
            return View();
        }

        // POST: Postagens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("postID,conteudoTxt,usuarioID")] Postagem postagem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postagem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", postagem.usuarioID);
            return View(postagem);
        }

        // GET: Postagens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Postagens == null)
            {
                return NotFound();
            }

            var postagem = await _context.Postagens.FindAsync(id);
            if (postagem == null)
            {
                return NotFound();
            }
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", postagem.usuarioID);
            return View(postagem);
        }

        // POST: Postagens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("postID,conteudoTxt,usuarioID")] Postagem postagem)
        {
            if (id != postagem.postID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postagem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostagemExists(postagem.postID))
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
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", postagem.usuarioID);
            return View(postagem);
        }

        // GET: Postagens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Postagens == null)
            {
                return NotFound();
            }

            var postagem = await _context.Postagens
                .Include(p => p.usuario)
                .FirstOrDefaultAsync(m => m.postID == id);
            if (postagem == null)
            {
                return NotFound();
            }

            return View(postagem);
        }

        // POST: Postagens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Postagens == null)
            {
                return Problem("Entity set 'Contexto.Postagens'  is null.");
            }
            var postagem = await _context.Postagens.FindAsync(id);
            if (postagem != null)
            {
                _context.Postagens.Remove(postagem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostagemExists(int id)
        {
          return (_context.Postagens?.Any(e => e.postID == id)).GetValueOrDefault();
        }
    }
}
