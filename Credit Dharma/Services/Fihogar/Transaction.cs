using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Services.Fihogar
{
    public static class Transaction
    {

        public static string GetTransactionsAccount(string account,string provider= "AB4WRD")
        {
            var client = ServiceCore.client;
            var request = new RestRequest("/manage-accounts/api/2.0/accounts/"+account+"/transactions?provider=" + provider, Method.GET);
            var token = Token.GetToken();
            var consent = Token.GetUserConsent();
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("token-id", consent);

            var response = client.Execute(request);
            //var a = System.Text.Json.JsonSerializer.Deserialize<object>(response.Content);

            //Response.WriteAsync(Convert.ToString(a));

            var a = JObject.Parse(response.Content);
           // a.SelectToken("$.Data.Account..Balance..Amount.Amount").ToString();
            return response.Content;
        }


        public static decimal GetTransactionsTotalAmountAccount(string account, string provider = "AB4WRD")
        {
            var client = ServiceCore.client;
            var request = new RestRequest("/manage-accounts/api/2.0/accounts/" + account + "/transactions?provider=" + provider, Method.GET);
            var token = Token.GetToken();
            var consent = Token.GetUserConsent();
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("token-id", consent);

            var response = client.Execute(request);
            //var a = System.Text.Json.JsonSerializer.Deserialize<object>(response.Content);

            //Response.WriteAsync(Convert.ToString(a));

            var a = JObject.Parse(response.Content);
            // a.SelectToken("$.Data.Account..Balance..Amount.Amount").ToString();
            return a.SelectToken("$.Data..Transaction").Sum(m => (decimal)m.SelectToken("..Amount.Amount"));
        }
    }
}
