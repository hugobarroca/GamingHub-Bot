using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GamingHubBot.Infrastructure.Gateways.Models;
using GamingHubBot.Application.Interfaces;

namespace GamingHubBot.Infrastructure.Gateways

{
    public class CatFactGateway : CatFactAPI
    {
        public async Task<CatFact> GetCatFact()
        {
            var apiClient = new HttpClient();
            CatFact fact = null;
            HttpResponseMessage response = await apiClient.GetAsync("https://catfact.ninja/fact");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var pirate = JsonConvert.DeserializeObject<CatFact>(jsonString);
            }
            return fact;
        }
    }
}
