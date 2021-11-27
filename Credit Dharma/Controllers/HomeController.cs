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
            //  Response.WriteAsync(Token.GetToken()+" "+Token.GetUserConsent());
            //  Services.Email.SendEmail("100", "mariahenriquezdelgado@gmail.com", "Funciona");
          //    Response.WriteAsync(Transaction.GetTransactionsAccount("35081814").Count.ToString());
          //    Response.WriteAsync(Account.GetAccounts().ToString());
          var clientes = await _context.Client.ToListAsync();
            clientes = clientes.FindAll(c => c.AccountSubType.ToUpper().Trim() == "Loan".ToUpper().Trim());
            var morosidad = 0.0;
            foreach (var cliente in clientes)
            {
                var pending = CustomFuctions.GetPaymentCount(DateTime.Parse(cliente.OpeningDate), DateTime.Now) - cliente.Payments;
                 morosidad += (float)((cliente.MonthlyPay * pending) / (CustomFuctions.GetPaymentCount(DateTime.Parse(cliente.OpeningDate), DateTime.Now) * cliente.MonthlyPay));
               
            }
            ViewData["MorosidadGeneral"] = JsonSerializer.Serialize(new double[] { morosidad/clientes.ToList().Count });
            return View();
        }

        public IActionResult Privacy()
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
