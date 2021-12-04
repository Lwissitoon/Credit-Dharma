using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Credit_Dharma.Data;
using Credit_Dharma.Models;
using Credit_Dharma.Helper;

namespace Credit_Dharma.Controllers
{
    public class RegistrosController : Controller
    {
        private readonly Credit_DharmaContext _context;

        public RegistrosController(Credit_DharmaContext context)
        {
            _context = context;
        }

        // GET: Registros
        public async Task<IActionResult> Index()
        {
            if(Session.Loggedin)
            { 
            return View(await _context.Registro.ToListAsync());
        }
            else
            {
                return RedirectToAction("Index","Home");
    }
}

        // GET: Registros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (Session.Loggedin)
            {


                if (id == null)
                {
                    return NotFound();
                }

                var registro = await _context.Registro
                    .FirstOrDefaultAsync(m => m.IdNotification == id);
                if (registro == null)
                {
                    return NotFound();
                }

                return View(registro);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        // GET: Registros/Create
        public IActionResult Create()
        {
            if(Session.Loggedin)
            { 
            return View();
        }
            else
            {
                return RedirectToAction("Index","Home");
    }
}

        // POST: Registros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNotification,NotificationDate,UserAccountNumber,NotificationDetails,Username,FlowStep")] Registro registro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registro);
        }

        // GET: Registros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (false)
            {

            
            if (id == null)
            {
                return NotFound();
            }

            var registro = await _context.Registro.FindAsync(id);
            if (registro == null)
            {
                return NotFound();
            }
            return View(registro);
        }
            else
            {
                return RedirectToAction("Index","Home");
             }
           }

        // POST: Registros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNotification,NotificationDate,UserAccountNumber,NotificationDetails,Username")] Registro registro)
        {
            if (id != registro.IdNotification)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroExists(registro.IdNotification))
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
            return View(registro);
        }

        // GET: Registros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (false)
            {
            if (id == null)
            {
                return NotFound();
            }

            var registro = await _context.Registro
                .FirstOrDefaultAsync(m => m.IdNotification == id);
            if (registro == null)
            {
                return NotFound();
            }

            return View(registro);
        }
            else
            {
                return RedirectToAction("Index","Home");
    }
       }

        // POST: Registros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registro = await _context.Registro.FindAsync(id);
            _context.Registro.Remove(registro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistroExists(int id)
        {
            return _context.Registro.Any(e => e.IdNotification == id);
        }
    }
}
