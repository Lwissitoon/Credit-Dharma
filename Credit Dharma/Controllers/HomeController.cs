using Credit_Dharma.Data;
using Credit_Dharma.Helper;
using Credit_Dharma.Models;
using Credit_Dharma.Services.Fihogar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Credit_Dharma.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Credit_DharmaContext _context;

        public HomeController(ILogger<HomeController> logger, Credit_DharmaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            
          var clientes = await _context.Client.ToListAsync();
            clientes = clientes.FindAll(c => c.AccountSubType.ToUpper().Trim() == "Loan".ToUpper().Trim());
            var morosidad = 0.0;
            double amount=0, totalAmount = 0;
            foreach (var cliente in clientes)
            {
                try
                {
                    cliente.PendingPayments = CustomFuctions.GetPaymentCount(DateTime.Parse(cliente.OpeningDate), DateTime.Now) - cliente.Payments;
                }
                catch (ArgumentNullException)
                {
                    cliente.PendingPayments = 0;
                }
                if (cliente.PendingPayments > 3)
                {
                    morosidad += (float)((cliente.MonthlyPay * cliente.PendingPayments) / (cliente.TotalAmount - (cliente.Payments * cliente.MonthlyPay))) * 100;
                }
                amount += cliente.Amount;
                totalAmount += cliente.TotalAmount;
            }
            try
            {
                ViewData["MontosGenerales"] = JsonSerializer.Serialize(new double[] { amount, totalAmount - amount });
                ViewData["MorosidadGeneral"] = JsonSerializer.Serialize(new double[] { morosidad / clientes.ToList().Count });
            }
            catch(ArgumentException)
            {
                ViewData["MontosGenerales"] = JsonSerializer.Serialize(new double[] {0});
                ViewData["MorosidadGeneral"] = JsonSerializer.Serialize(new double[] { 0});
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("404")]
        public IActionResult Error404()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
