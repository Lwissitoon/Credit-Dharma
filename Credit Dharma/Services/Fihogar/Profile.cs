using Credit_Dharma.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Services.Fihogar
{
    public static class Profile
    {


        public static Cliente GetProfile(string consent,string token ,string provider)
        {
            var client = ServiceCore.client;
            var request = new RestRequest("/manage-profile/api/2.0/profile/Dharma_3?provider=" + provider, Method.GET);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("token-id", consent);

            var response = client.Execute(request);
            //var a = System.Text.Json.JsonSerializer.Deserialize<object>(response.Content);

            //Response.WriteAsync(Convert.ToString(a));

            var a = JObject.Parse(response.Content);


            /*
             * $.Data.Profile..FirstName
             * $.Data.Profile..MiddleName
             * $.Data.Profile..LastName
             * $.Data..AccountSubType
             * $.Data..Balance..Currency
             * $.Data..Status
             */
            var name = a.SelectToken("$.Data.Profile..FirstName") +" "+ a.SelectToken("$.Data.Profile..MiddleName");
            var lastname = a.SelectToken("$.Data.Profile..LastName");


            return new Cliente() { Name=name,Lastname=lastname.ToString()};
        }
    }
}
