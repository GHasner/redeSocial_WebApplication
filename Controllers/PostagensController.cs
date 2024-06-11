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
            var contexto = _context.Postagens.Include(p => p.usuario);
            List<Postagem>? postagens = await contexto.ToListAsync();
            if (postagens != null && postagens.Count > 0)
            {
                List<ArquivoMidia> arquivos = await _context.Arquivos.ToListAsync();
                foreach (ArquivoMidia arquivo in arquivos)
                {
                    int i = postagens.FindIndex(x => x.postID == arquivo.postID);
                    postagens[i].midia = arquivo;
                }

            }
            return View(postagens);
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
            var arquivo = await _context.Arquivos.FirstOrDefaultAsync(x => x.postID == postagem.postID);
            if (arquivo != null)
            {
                postagem.midia = arquivo;
            }

            return View(postagem);
        }

        public void UploadArquivoMidia(ArquivoMidia midia, IFormFile arquivo)
        {
            // NOME DE ARMAZENAMENTO DO ARQUIVO
            string extensaoArquivo = Path.GetExtension(arquivo.FileName).ToLower();
            string nomeArmazenamento = "postID" + Auxiliar.fixedDigits(midia.postID, 8) + "arqID" + Auxiliar.fixedDigits(midia.arquivoID, 8) + extensaoArquivo;


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
            }
            else if (imgExt.Contains(extensaoArquivo))
            {
                midia.tipoArquivo = "video";
            }
            else
            {
                midia.tipoArquivo = "";
            }

            midia.nomeArmazenamento = nomeArmazenamento;
            //Grava dados no Banco de dados.
            _context.Arquivos.Add(midia);
            _context.SaveChanges();
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
        public async Task<IActionResult> Create([Bind("postID,conteudoTxt,usuarioID")] Postagem postagem, IFormFile arquivo, ArquivoMidia? midia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postagem);

                var arqRecebido = Request.Form.Files["arquivo"];
                if (arqRecebido != null && arqRecebido.Length > 0)
                {
                    ArquivoMidiasController arqController = new ArquivoMidiasController(_context);
                    midia.postID = postagem.postID;
                    midia.post = postagem;
                    UploadArquivoMidia(midia, arqRecebido);
                }
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
            var arquivo = await _context.Arquivos.FirstOrDefaultAsync(x => x.postID == postagem.postID);
            if (arquivo != null)
            {
                postagem.midia = arquivo;
            }
            return View(postagem);
        }

        // POST: Postagens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("postID,conteudoTxt,usuarioID")] Postagem postagem, IFormFile arquivo, ArquivoMidia? midia)
        {
            if (id != postagem.postID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var arquivoOld = await _context.Arquivos.FirstOrDefaultAsync(x => x.postID == postagem.postID);
                    if (arquivoOld != null) // TINHA mídia
                    {
                        var arqRecebido = Request.Form.Files["arquivo"];
                        if (arqRecebido != null && arqRecebido.Length > 0) // Tem mídia
                        {
                            if (arqRecebido.Equals(arquivoOld))
                            {
                                // Não houve alteração
                            } else
                            {
                                // Delete ArquivoMidia
                                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "arquivos", arquivoOld.nomeArmazenamento!));
                                _context.Arquivos.Remove(arquivoOld);
                                // Insert ArquivoMidia
                                midia.postID = postagem.postID;
                                midia.post = postagem;
                                UploadArquivoMidia(midia, arqRecebido);
                            }
                        }
                        else // Não tem mídia
                        {
                            // Delete ArquivoMidia
                            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "arquivos", arquivoOld.nomeArmazenamento!));
                            _context.Arquivos.Remove(arquivoOld);
                        }
                    } else // NÃO TINHA mídia
                    {
                        var arqRecebido = Request.Form.Files["arquivo"];
                        if (arqRecebido != null && arqRecebido.Length > 0) // Tem midia
                        {
                            // Insert ArquivoMidia
                            midia.postID = postagem.postID;
                            midia.post = postagem;
                            UploadArquivoMidia(midia, arqRecebido);
                        }
                    }
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
            
            // Substituir
            var arquivoAtual = await _context.Arquivos.FirstOrDefaultAsync(x => x.postID == postagem.postID);
            if (arquivoAtual != null)
            {
                postagem.midia = arquivoAtual;
            }
            //
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

            var arquivo = await _context.Arquivos.FirstOrDefaultAsync(x => x.postID == postagem.postID);
            if (arquivo != null)
            {
                postagem.midia = arquivo;
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
                var arquivo = await _context.Arquivos.FirstOrDefaultAsync(x => x.postID == postagem.postID);
                if (arquivo != null)
                {
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "arquivos", arquivo.nomeArmazenamento!));
                    _context.Arquivos.Remove(arquivo);
                }
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
