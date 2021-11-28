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


            if (Session.Loggedin)
            {
                foreach (var cliente in clientes)
                {
                    cliente.PendingPayments = CustomFuctions.GetPaymentCount(DateTime.Parse(cliente.OpeningDate), DateTime.Now) - cliente.Payments;
                    await _context.SaveChangesAsync();
                }
                return View(clientes);
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
            
                ViewData["Montos"] = JsonSerializer.Serialize(new double[] { client.Amount, client.TotalAmount - client.Amount });
                ViewData["Cuotas"] = JsonSerializer.Serialize(new double[] { client.Payments, client.PendingPayments });

                if (client.PendingPayments > 3)
                {
                    var morosidad = (float)((client.MonthlyPay * client.PendingPayments) / client.TotalAmount)*100;
                    ViewData["Morosidad"] = JsonSerializer.Serialize(new float[] { morosidad });

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
        public async Task<IActionResult> Edit(string id, [Bind("Identification,Status,Currency,AccountSubType,Nickname,OpeningDate,Amount,Name,Lastname,Email,PhoneNumber,TotalAmount,Payments,PendingPayments,MonthlyPay")] Cliente client)
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
          await  Account.RefreshAccountsAsync(_context, _context.Client);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> NotifyClientEmail(string id)
        {
            var cliente = await _context.Client.FirstOrDefaultAsync(m => m.Identification == id);

             Email.SendEmail(cliente.Identification,cliente.Email,@"Tiene un total de "+ cliente.PendingPayments+" cuotas vencidas, favor pagar lo antes posible.");
            _context.Registro.Add(new Registro() {NotificationDate=DateTime.Now.ToString(),Username=Session.Username,UserAccountNumber=cliente.Identification,NotificationDetails="Notificacion enviada via correo desde el sistema" });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
