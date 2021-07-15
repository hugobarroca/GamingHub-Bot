using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ApiCalls
{
    public class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient() 
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri("https://cat-fact.herokuapp.com");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaiton/json"));
        }
    }
}
