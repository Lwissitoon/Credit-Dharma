using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Credit_Dharma.Data;
using Credit_Dharma.Models;
using Credit_Dharma.Services.Fihogar;
using Credit_Dharma.Helper;
using System.Text.Json;
using Credit_Dharma.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Credit_Dharma.Views
{
    public class ClientsController : Controller
    {
        private readonly Credit_DharmaContext _context;
        public ClientsController(Credit_DharmaContext context)
        {
            
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {


         var clientes = await _context.Client.ToListAsync();

            clientes = clientes.FindAll(c => c.AccountSubType.ToUpper().Trim() == "Loan".ToUpper().Trim());
            // Response.WriteAsync(CustomFuctions.GetPaymentCount(DateTime.Parse( clientes.First().OpeningDate),DateTime.Now).ToString());
            var usuariosLista = _context.Usuario.ToList();
            ViewBag.usuarios = usuariosLista;

            if (Session.Loggedin)
            {
              
                foreach (var cliente in clientes)
                {
                    try
                    {
                        cliente.PendingPayments = CustomFuctions.GetPaymentCount(DateTime.Parse(cliente.OpeningDate), DateTime.Now) - cliente.Payments;

                    }
                    catch(ArgumentNullException)
                    {
                        cliente.PendingPayments = 0;
                    }
                    await _context.SaveChangesAsync();
                }
                if (Session.Admin)
                {
                    clientes = clientes.FindAll(c => c.AccountSubType.ToUpper().Trim() == "Loan".ToUpper().Trim());
                    return View(clientes);
                }
                else
                {


                 return View( _context.Client.FromSqlRaw("SELECT * FROM[dbo].[Client] Where[Assigned] = '"+Session.Username+"'"));
                        
                    
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (Session.Loggedin)
            {

                var usuariosLista = _context.Usuario.ToList();
                ViewBag.usuarios = usuariosLista;

                if (id == null)
                {
                    return NotFound();
                }

                
                var client = await _context.Client
                    .FirstOrDefaultAsync(m => m.Identification == id);


                if (client == null)
                {
                    return NotFound();
                }

                try
                {
                    ViewData["Montos"] = JsonSerializer.Serialize(new double[] { client.Amount, client.TotalAmount - client.Amount });
                    ViewData["Cuotas"] = JsonSerializer.Serialize(new double[] { client.Payments, client.PendingPayments });
                }
                catch(ArgumentNullException)
                { 
                    ViewData["Montos"] = JsonSerializer.Serialize(new double[] {0});
                    ViewData["Cuotas"] = JsonSerializer.Serialize(new double[] { 0});
                }





                if (client.PendingPayments > 3)
                {
                   
                    ViewData["Morosidad"] = JsonSerializer.Serialize(new double[] {CustomFuctions.GetMorosidad(_context.Client.ToList(),client) });

                    // Response.WriteAsync(JsonSerializer.Serialize(new float[] { morosidad}).ToString());
                }





                else
                {
                    ViewData["Morosidad"] = 0;
                }
                return View(client);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            if (Session.Admin)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Identification,Status,Currency,AccountSubType,Nickname,OpeningDate,Amount,Name,Lastname,Email,PhoneNumber")] Cliente client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (Session.Admin)
            {
                var usuariosLista = (from Usuario in _context.Usuario
                                   select new SelectListItem()
                                   {
                                       Text = Usuario.Username,
                                       Value = Usuario.Username
                                   }).ToList();




                ViewBag.usuarios = usuariosLista;

                if (id == null)
                {
                    return NotFound();
                }

                var client = await _context.Client.FindAsync(id);
                if (client == null)
                {
                    return NotFound();
                }
                return View(client);
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Identification,Status,Currency,AccountSubType,Nickname,OpeningDate,Amount,Name,Lastname,Email,PhoneNumber,TotalAmount,Payments,PendingPayments,MonthlyPay,Assigned")] Cliente client)
        {
            if (id != client.Identification)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Identification))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (Session.Admin)
            {

            
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Identification == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = await _context.Client.FindAsync(id);
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
            return _context.Client.Any(e => e.Identification == id);
        }

        public async Task<ActionResult> RefreshAsync()
        {
            if (Session.Admin)
            {
                await Account.RefreshAccountsAsync(_context, _context.Client);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> NotifyClientEmail(string id)
        {
            var cliente = await _context.Client.FirstOrDefaultAsync(m => m.Identification == id);

            try
            {
                Email.SendEmail(cliente.Identification, cliente.Email, @"Tiene un total de " + cliente.PendingPayments + " cuotas vencidas, favor pagar lo antes posible.");
                _context.Registro.Add(new Registro() { NotificationDate = DateTime.Now.ToString(), Username = Session.Username, UserAccountNumber = cliente.Identification, NotificationDetails = "Notificacion enviada via correo desde el sistema",FlowStep="Recordatorio Rapido Por Correo" });
                await _context.SaveChangesAsync();
            }
            catch (ArgumentNullException)
            {

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        public ActionResult ExportToCsv()
        {
            List<Cliente> clientes;

            if (Session.Admin)
            {
       
             clientes = _context.Client.ToList();
            clientes = clientes.FindAll(c => c.AccountSubType.ToUpper().Trim() == "Loan".ToUpper().Trim());
              }
            else
            {
                clientes = _context.Client.FromSqlRaw("SELECT * FROM[dbo].[Client] Where[Assigned] = '" + Session.Username + "'").ToList();
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Cuenta");
            sb.Append(",");
            sb.Append("Estatus");
            sb.Append(",");
            sb.Append("Moneda");
            sb.Append(",");
            sb.Append("Descripcion");
            sb.Append(",");
            sb.Append("Fecha Apertura");
            sb.Append(",");
            sb.Append("Saldo");
            sb.Append(",");
            sb.Append("Saldo Pendiente");
            sb.Append(",");
            sb.Append("Cuota Mensual");
            sb.Append(",");
            sb.Append("Cuotas Pendientes");
            sb.Append(",");
            sb.Append("Cuotas Pagadas");
            sb.Append(",");
            sb.Append("Cuotas Generadas");
            sb.Append(",");
            sb.Append("Calificacion");
            sb.AppendLine();
            foreach (var cliente in clientes)
            {
                sb.Append(cliente.Identification);
                sb.Append(",");
                sb.Append(cliente.Status);
                sb.Append(",");
                sb.Append(cliente.Currency);
                sb.Append(",");
                sb.Append(cliente.Nickname);
                sb.Append(",");
                sb.Append(cliente.OpeningDate);
                sb.Append(",");
                sb.Append(cliente.TotalAmount-cliente.Amount);
                sb.Append(",");
                sb.Append(cliente.PendingPayments*cliente.MonthlyPay);
                sb.Append(",");
                sb.Append(cliente.MonthlyPay);
                sb.Append(",");
                sb.Append(cliente.PendingPayments);
                sb.Append(",");
                sb.Append(cliente.Payments);
                sb.Append(",");
                sb.Append(cliente.Payments+cliente.PendingPayments);
                sb.Append(",");
                sb.Append(CustomFuctions.CalificarCliente( cliente.PendingPayments));
                sb.AppendLine();
            }
            return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", DateTime.Now.Date.ToString("dd-MM-yyyy")+"_clientes.csv");
        }


        public async Task<ActionResult> AssignToAsync(string id,string user)
        {
            var cliente = await _context.Client.FirstOrDefaultAsync(x => x.Identification == id);
                cliente.Assigned = user ;
            _context.SaveChanges();

            var usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Username == user);
            try
            {
                Email.SendEmailInternalNotify(cliente.Identification,usuario.Email,"El usuario "+Session.Username+" te acaba de asignar un nuevo cliente con el numero de cuenta "+cliente.Identification+". Esta disponible en tu dashboard para seguimiento.");



             }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Index");
             }

            var url = Request.Headers["Referer"].ToString();
            //Response.WriteAsync(user+id);
            return Redirect(url);

        }
    }
}
