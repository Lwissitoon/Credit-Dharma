using Credit_Dharma.Models;
using Credit_Dharma.Services.Fihogar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //  Response.WriteAsync(Token.GetToken()+" "+Token.GetUserConsent());
            //  Services.Email.SendEmail("100", "mariahenriquezdelgado@gmail.com", "Funciona");
          //    Response.WriteAsync(Transaction.GetTransactionsAccount("35081814").Count.ToString());
          //    Response.WriteAsync(Account.GetAccounts().ToString());


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
