using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Credit_Dharma.Data;
using UserManager.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Credit_Dharma.Helper;

namespace Credit_Dharma.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Credit_DharmaContext _context;

        public UsuariosController(Credit_DharmaContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {


            //if (Session.Loggedin )
            //{

            //    if (Session.Admin)
            //    {
            //        return View(await _context.Usuario.ToListAsync());
            //    }
            //    else
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}

            return View();
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Name,Lastname,Role,Password")] Usuario usuario)
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
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Name,Lastname,Role,Password")] Usuario usuario)
        {
            if (id != usuario.UserId)
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
                    if (!UsuarioExists(usuario.UserId))
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
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.UserId == id);
        }


        [HttpPost]
        public async Task<ActionResult> Signin(string Username, string Password)
        {

          //  Response.WriteAsync(await _context.Usuario.ToListAsync().f(m=>m.Username=="").Result.ToString());

            
            TempData.Clear();
            //   Response.WriteAsync(Username + Password);()
            var login = await _context.Usuario.ToListAsync();

           var  usuario = login.FirstOrDefault(m => m.Username.ToUpper().Trim() == Username.ToUpper().Trim() && m.Password.ToUpper().Trim() == Password.ToUpper().Trim());
            //  .FirstOrDefault(m => m.Username.ToUpper().Trim() == Username.ToUpper().Trim() && m.Password.ToUpper().Trim() == Password.ToUpper().Trim()).Result;
            //Response.WriteAsync(usuario.First().Username+ usuario.First().Role);
            if (usuario.Username != null)
            {
                //  var role = _context.Usuario.FirstOrDefaultAsync(m => m.Username == Username);
                TempData["Loggedin"] = "Si";
                Session.Username= usuario.Username;
                Session.Role = usuario.Role;
                Session.Loggedin = true;
                if (usuario.Role.ToUpper().Trim()=="Admin".ToUpper().Trim())
                {
                    Session.Admin = true;
                }
                return RedirectToAction("Index", "Home");
               

            }
            else
            {
                TempData["Loggedin"] = "No";
                Session.Loggedin = false;
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        public ActionResult Signout()
        {
            Session.Admin = false;
            Session.Loggedin = false;
            Session.Role = "N/A";
            Session.Username = "Visitante";
          
            return RedirectToAction("Login");
        }
    }
}
