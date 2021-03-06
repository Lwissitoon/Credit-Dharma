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
using System.Text;

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

                ViewBag.clientes = (from cliente in _context.Client
                                    select new SelectListItem()
                                    {
                                        Text = cliente.Identification,
                                        Value = cliente.Identification
                                    }).ToList();

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



        public ActionResult ExportToCsv()
        {
            var registros = _context.Registro.ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append("ID");
            sb.Append(",");
            sb.Append("Fecha");
            sb.Append(",");
            sb.Append("Numero de cuenta");
            sb.Append(",");
            sb.Append("Paso en el flujo");
            sb.Append(",");
            sb.Append("Accion realiazada por ");
            sb.Append(",");
            sb.Append("Detalles");
            sb.AppendLine();
            foreach (var registro in registros)
            {
                sb.Append(registro.IdNotification);
                sb.Append(",");
                sb.Append(registro.NotificationDate);
                sb.Append(",");
                sb.Append(registro.UserAccountNumber);
                sb.Append(",");
                sb.Append(registro.FlowStep);
                sb.Append(",");
                sb.Append(registro.Username);
                sb.Append(",");
                sb.Append(registro.NotificationDetails);
                sb.AppendLine();
            }
            return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", DateTime.Now.Date.ToString("dd-MM-yyyy") + "_Registro_Notificaciones.csv");
        }
    }
}
