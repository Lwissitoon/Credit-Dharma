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

            /*
             * $.Data..Account..Account..Identification
             * $.Data..Account..Nickname
             * $.Data..Balance..Amount..Amount
             * $.Data..AccountSubType
             * $.Data..Balance..Currency
             * $.Data..Status
             */

            return a.SelectToken("$.Data..Account").ToString();
        }


        public static void RefreshAccounts()
        {

        }
    }
}
