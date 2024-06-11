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
    public class ComentariosController : Controller
    {
        private readonly Contexto _context;

        public ComentariosController(Contexto context)
        {
            _context = context;
        }

        // GET: Comentarios
        public async Task<IActionResult> Index(Postagem postagem)
        {
            var comentarios = await _context.Comentarios.Include(c => c.post).Include(c => c.usuario).Where(c => c.postID == postagem.postID).ToListAsync();
            postagem.comentarios = comentarios;
            if (HttpContext.Session.GetString("UserLoggedIn") != null || HttpContext.Session.GetString("UserLoggedIn") != "")
            {
                // Salva o userID em uma variável para consultar informações do usuário no BD
                int userId = int.Parse(HttpContext.Session.GetString("UserLoggedIn")!);

                if (postagem.usuarioID == userId)
                {
                    postagem.pertenceAoUsuario = true;
                    return View(postagem);
                }
            }
            postagem.pertenceAoUsuario = false;
            return View(postagem);
        }

        // GET: Comentarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comentarios == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios
                .Include(c => c.post)
                .Include(c => c.usuario)
                .FirstOrDefaultAsync(m => m.comentID == id);
            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // GET: Comentarios/Create
        public IActionResult Create()
        {
            ViewData["postID"] = new SelectList(_context.Postagens, "postID", "postID");
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID");
            return View();
        }

        // POST: Comentarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("comentID,postID,usuarioID,comentario,visible")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comentario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["postID"] = new SelectList(_context.Postagens, "postID", "postID", comentario.postID);
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", comentario.usuarioID);
            return View(comentario);
        }

        // GET: Comentarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comentarios == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }
            ViewData["postID"] = new SelectList(_context.Postagens, "postID", "postID", comentario.postID);
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", comentario.usuarioID);
            return View(comentario);
        }

        // POST: Comentarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("comentID,postID,usuarioID,comentario,visible")] Comentario comentario)
        {
            if (id != comentario.comentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comentario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComentarioExists(comentario.comentID))
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
            ViewData["postID"] = new SelectList(_context.Postagens, "postID", "postID", comentario.postID);
            ViewData["usuarioID"] = new SelectList(_context.Usuarios, "usuarioID", "usuarioID", comentario.usuarioID);
            return View(comentario);
        }

        // GET: Comentarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comentarios == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios
                .Include(c => c.post)
                .Include(c => c.usuario)
                .FirstOrDefaultAsync(m => m.comentID == id);
            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // POST: Comentarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comentarios == null)
            {
                return Problem("Entity set 'Contexto.Comentarios'  is null.");
            }
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario != null)
            {
                _context.Comentarios.Remove(comentario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComentarioExists(int id)
        {
          return (_context.Comentarios?.Any(e => e.comentID == id)).GetValueOrDefault();
        }
    }
}
