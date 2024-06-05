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
    public class UsuariosController : Controller
    {
        private readonly Contexto _context;

        public UsuariosController(Contexto context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            HttpContext.Session.SetString("UserLoggedIn", "");
            var loginTrial = HttpContext.Session.GetString("LoginValidation");
            if (loginTrial != null)
            {
                if (loginTrial == "Fail")
                {
                    ViewBag.Message = "LoginFailed";
                }
                else
                {
                    ViewBag.Message = "";
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            string un = username;
            string p = password;

            // Buscar o usuário no banco de dados pelo nome de usuário (ou outro critério de identificação)
            var user = await _context.Usuarios.FirstOrDefaultAsync(m => m.nomeUsuario == un);

            // Verifica se o usuário foi encontrado e se a senha está correta
            if (user != null && user.senha == p)
            {

                HttpContext.Session.SetString("UserLoggedIn", user.usuarioID.ToString());
                WriteCookie(user.usuarioID.ToString());
                Console.WriteLine("Login Succeded");

                HttpContext.Session.SetString("LoginValidation", "Success");

                // Redireciona para a página inicial após o login
                return RedirectToAction("Index", "Usuarios");
            }

            HttpContext.Session.SetString("LoginValidation", "Fail");
            Console.WriteLine("Login Unsucceded");
            return RedirectToAction("Login", "Usuarios");
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            // Verifica se o usuário está logado
            if (HttpContext.Session.GetString("UserLoggedIn") != null)
            {
                // Salva o userID em uma variável para consultar informações do usuário no BD
                string? userId = HttpContext.Session.GetString("UserLoggedIn");

                if (userId != "" && userId != null)
                {
                    var currentUser = await _context.Usuarios.FirstOrDefaultAsync(m => m.usuarioID == int.Parse(userId));

                    if (currentUser != null)
                    {
                        // Qualquer valor obtido no BD pode ser resgatado aqui
                    }
                    return RedirectToAction("Index", "Usuarios");
                }
            }
            return RedirectToAction("Login", "Usuarios");
        }

        private async Task<IActionResult> ValidateUser()
        {
            // Verifica se o usuário está logado
            if (HttpContext.Session.GetString("UserLoggedIn") != null)
            {
                // Salva o userID em uma variável para consultar informações do usuário no BD
                string? userId = HttpContext.Session.GetString("UserLoggedIn");

                if (userId != "" && userId != null)
                {
                    var currentUser = await _context.Usuarios.FirstOrDefaultAsync(m => m.usuarioID == int.Parse(userId));

                    if (currentUser != null)
                    {
                        return View(currentUser);
                        // Qualquer valor obtido no BD pode ser resgatado aqui
                    }
                }
            }
            return NotFound();
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.usuarioID == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewBag.Message = "NewUserSuccess";
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("usuarioID,nomeUsuario,nome,telefone,email,senha")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("usuarioID,nomeUsuario,nome,telefone,email,senha")] Usuario usuario)
        {
            if (id != usuario.usuarioID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.usuarioID))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var user = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.usuarioID == id);
            if (user == null)
            {
                return NotFound();
            }

            string? currentUserId = HttpContext.Session.GetString("UserLoggedIn");
            Usuario? currentUser = await _context.Usuarios.FirstOrDefaultAsync(m => m.usuarioID == int.Parse(currentUserId!));

            var userSelected = await _context.Usuarios.FindAsync(id);

            if (userSelected!.usuarioID == currentUser!.usuarioID)
            {
                ViewBag.Message = "CurrentUserDeleted";
            }

            return View(user);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'Contexto.Usuarios' is null.");
            }

            string? currentUserId = HttpContext.Session.GetString("UserLoggedIn");
            Usuario? currentUser = await _context.Usuarios.FirstOrDefaultAsync(m => m.usuarioID == int.Parse(currentUserId!));

            var user = await _context.Usuarios.FindAsync(id);

            _context.Usuarios.Remove(user!);
            await _context.SaveChangesAsync();

            if (user!.usuarioID == currentUser!.usuarioID)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            return RedirectToAction("Index", "Usuarios");
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.usuarioID == id)).GetValueOrDefault();
        }

        public IActionResult WriteCookie(string value)
        {
            Response.Cookies.Append("LoggedIn", value, new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddDays(7)
            });
            return Ok("Cookie recorded.");
        }
    }
}
