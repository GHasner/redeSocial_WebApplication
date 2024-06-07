using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using redeSocial.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace redeSocial_WebApplication.Controllers
{
    public class ArquivoMidiasController : Controller
    {
        private readonly Contexto _context;

        public ArquivoMidiasController(Contexto context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Upload(ArquivoMidia midia, IFormFile arquivo)
        {
            // NOME DE ARMAZENAMENTO DO ARQUIVO
            string nomeArquivo = Path.GetFileName(arquivo.FileName);
            string extensaoArquivo = Path.GetExtension(arquivo.FileName).ToLower();
            string nomeArmazenamento = nomeArquivo + "-ID" + midia.arquivoID + extensaoArquivo;


            // Aponta para onde o arquivo será gravado(Não esqueça de criar o diretório)
            string caminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "arquivos", nomeArmazenamento);

            // Grava o arquivo no servidor
            using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
            {
                arquivo.CopyTo(stream);
            }

            // Extensões de vídeo
            List<string> videoExt = new List<string>
            {
                ".asf",
                ".wma",
                ".wmv",
                ".wmz",
                ".mp4",
                ".mov",
                ".mkv",
                ".webm",
                ".flv",
                ".swf",
                ".3gp",
                ".aac",
                ".m4v",
                ".ogg",
                ".vob",
                ".wmvhd",
                ".amv",
                ".amv3",
                ".asx",
                ".divx",
                ".dvr",
                ".f4v",
                ".gif",
                ".m2ts",
                ".m4a",
                ".mts",
                ".mxp",
                ".ogv",
                ".qt",
                ".ts",
                ".vp8",
                ".vp9",
                ".xavc"
            };

            // Extensões de imagem
            List<string> imgExt = new List<string>
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".bmp",
                ".tiff",
                ".ico",
                ".pcx",
                ".pict",
                ".wmf",
                ".emf",
                ".svg",
                ".psd"
            };

            if (videoExt.Contains(extensaoArquivo))
            {
                midia.tipoArquivo = "img";
            } else if (imgExt.Contains(extensaoArquivo))
            {
                midia.tipoArquivo = "video";
            } else
            {
                midia.tipoArquivo = "";
            }

            midia.nomeArmazenamento = nomeArmazenamento;
            //Grava dados no Banco de dados.
            _context.Add(midia);
            _context.SaveChanges();
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
