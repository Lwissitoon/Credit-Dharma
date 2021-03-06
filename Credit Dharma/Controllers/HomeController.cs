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
          
          // morosidad += (float)((cliente.MonthlyPay * cliente.PendingPayments) / (cliente.TotalAmount - (cliente.Payments * cliente.MonthlyPay))) * 100;
            try
            {
             
                ViewData["MorosidadGeneral"] = JsonSerializer.Serialize(new double[] { CustomFuctions.GetMorosidadGeneral(clientes) });
            }
            catch(ArgumentException)
            {

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
