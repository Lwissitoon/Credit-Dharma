using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Credit_Dharma.Services.Fihogar
{
    static public class Token
    {

        public static string GetToken()
        {
            var client = ServiceCore.client;
            var request = new RestRequest("/token", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");
            request.AddHeader("Authorization", "Bearer " + ServiceCore.ServiceToken);
            var response = client.Execute(request);
            //var a = System.Text.Json.JsonSerializer.Deserialize<object>(response.Content);

            //Response.WriteAsync(Convert.ToString(a));

            var a = JObject.Parse(response.Content);
            return a["access_token"].ToString();
        }

        public static string GetUserConsent(string provider = "AB4WRD")
        {
            var client =new  RestClient(@"https://api.uat.4wrd.tech:8243/authorize/2.0/token?provider="+provider);

            var request = new RestRequest(Method.POST);
             //request.AddParameter("provider", "AB4WRD");

            //request.AddHeader("Authorization", "Bearer " + "0d437428-3f9b-33b3-80f2-ef5daf923fb6");
            //var p = "grant_type=password&username=Dharma_1&password=Dharma_1";
            //request.AddBody(p);

            //request.AddBody(new
            //{
            //    grant_type = "password",
            //    password = "Dharma_1",
            //    userName = "Dharma_1"
            //});
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //request.AddHeader("Cache-Control", "no-cache");
            //request.AddHeader("Authorization", "Bearer " + "3dc35f22-dd24-405a-ba27-dd7237191fe8");
            //request.AddOrUpdateParameter("grant_type", "password", ParameterType.RequestBody);
            //request.AddOrUpdateParameter("username", "Dharma_1", ParameterType.RequestBody);
            //request.AddOrUpdateParameter("password", "Dharma_1", ParameterType.RequestBody);


            request.AddHeader("Authorization", "Bearer " + GetToken());

            request.AddParameter("grant_type", "password");
            request.AddParameter("username", "Dharma_3");
            request.AddParameter("password", "Dharma_3");


            var response = client.Execute(request);
            //var a = System.Text.Json.JsonSerializer.Deserialize<object>(response.Content);

            //Response.WriteAsync(Convert.ToString(a));

            var a = JObject.Parse(response.Content);
            return a["access_token"].ToString();
        }

        public static string Sample ()
        {
            var url = "https://api.4wrd.tech:8243/authorize/2.0/token?provider=AB4WRD";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Headers["Authorization"] = "Bearer 8d79fb1c-e94a-4ce6-951d-13ff7ea102fd";
            httpRequest.ContentType = "application/x-www-form-urlencoded";
            httpRequest.Headers["cache-control"] = "no-cache";

            var data = "grant_type=password&username=Dharma_1=&password=Dharma_1";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }
            var result="";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                 result = streamReader.ReadToEnd();
            }

            Console.WriteLine(httpResponse.StatusCode);
            return result.ToString();
        }
    }
}
