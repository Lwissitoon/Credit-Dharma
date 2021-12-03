using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credit_Dharma.Helper;
using Credit_Dharma.Models;
using Microsoft.EntityFrameworkCore;

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

            var Id = a.SelectTokens("$.Data..Account..Account..Identification");
            return Id.First().ToString();

        }


        public static async Task RefreshAccountsAsync(DbContext context, DbSet<Cliente> entities, string provider = "AB4WRD")
        {
            var client = ServiceCore.client;
            var request = new RestRequest("/manage-accounts/api/2.0/accounts?provider=" + provider, Method.GET);
            var token = Token.GetToken();
            var consent = Token.GetUserConsent();
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("token-id", consent);

            var response = client.Execute(request);
            //var a = System.Text.Json.JsonSerializer.Deserialize<object>(response.Content);

            //Response.WriteAsync(Convert.ToString(a));

            var a = JObject.Parse(response.Content);


            /*
             * $.Data..Account..Account..Identification
             * $.Data..Account..Nickname
             * $.Data..Balance..Amount..Amount
             * $.Data..AccountSubType
             * $.Data..Balance..Currency
             * $.Data..Status
             */
            var id = a.SelectTokens("$.Data..Account..Account..Identification");
            var status = a.SelectTokens("$.Data..Status");
            var currency = a.SelectTokens("$.Data..Balance..Currency");
            var accountSubType = a.SelectTokens("$.Data..AccountSubType");
            var nickname = a.SelectTokens("$.Data..Account..Nickname");
            var amount = a.SelectTokens("$.Data..Balance..Amount..Amount");

            List<Cliente> clientes = new List<Cliente>();
      
            for (int i = 0; i < id.ToList().Count; i++)
            {

                clientes.Add(new Cliente()
                {
                    Identification = (string)id.ToList().ElementAt(i),
                    OpeningDate= (string)nickname.ToList().ElementAt(i),
                    Status = (string)status.ToList().ElementAt(i),
                    Currency = (string)currency.ToList().ElementAt(i),
                    AccountSubType = (string)accountSubType.ToList().ElementAt(i),
                    Nickname = (string)nickname.ToList().ElementAt(i),
                    Amount = (double)amount.ToList().ElementAt(i),
                    Payments = Transaction.GetTransactionsAccount((string)id.ToList().ElementAt(i).ToString()).Count,

                    
                }) ; 
            }


            foreach (var cliente in clientes)
            {
                try
                {

                    //entities.Attach(cliente);
                    //context.Entry(cliente).Property(x => x.TotalAmount).IsModified = false;
                    //context.Entry(cliente).Property(x => x.Nickname).IsModified = true;
                    //context.Entry(cliente).Property(x => x.Payments).IsModified = true;

                    var updateClient = entities.FirstOrDefault(x => x.Identification == cliente.Identification);
                    if (updateClient== null)
                    {
                        entities.Add(cliente);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        updateClient.Nickname = cliente.Nickname;
                        updateClient.Amount = cliente.Amount;
                        updateClient.Payments = cliente.Payments;
                        updateClient.OpeningDate = cliente.OpeningDate;
                        
                        await context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException e)
                {
                    entities.Add(cliente);
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    entities.Add(cliente);
                    await context.SaveChangesAsync();
                }
            }
            
    }





    }
}
