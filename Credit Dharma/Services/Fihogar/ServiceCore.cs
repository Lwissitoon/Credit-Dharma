using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Services.Fihogar
{
    static public class ServiceCore
    {
        public static RestClient client = new RestClient("https://api.uat.4wrd.tech:8243");
        public static string ServiceToken= "dzBMbkVNOUpYeWhNYmlBMEg4Nk9lM3FwVjVzYTpyRlRqanFUdkxOY25ZbTltSndHX0FNbVp6b2dh";
    }
}
