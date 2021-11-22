using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credit_Dharma.Helper;
namespace Credit_Dharma.Services.Fihogar
{
    public static class Account
    {

        public static string GetAccounts(string provider = "AB4WRD")
        {
            var client = ServiceCore.client;
            var request = new RestRequest("/manage-accounts/api/2.0/accounts?provider="+provider, Method.GET);
            var token = Token.GetToken();
            var consent = Token.GetUserConsent();
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("token-id",consent);

            var response = client.Execute(request);
            //var a = System.Text.Json.JsonSerializer.Deserialize<object>(response.Content);

            //Response.WriteAsync(Convert.ToString(a));

            var a = JObject.Parse(response.Content);
            a.SelectToken("Amount");
            return a.SelectToken("$.Data.Account..Balance..Amount.Amount").ToString();
        }


        public static decimal Sumaproductos()
        {
            var client = new RestClient("https://dummyjson.com/products");
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            var a = JObject.Parse(response.Content);
            return a.SelectToken("$.products").Sum(m => (decimal)m.SelectToken("..price"));
        }
    }
}
