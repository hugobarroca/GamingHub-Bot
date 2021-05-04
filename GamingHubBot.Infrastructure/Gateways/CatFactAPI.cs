using GamingHubBot.Infrastructure.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GamingHubBot.Infrastructure
{
    public class CatFactImp : CatFact
    {
        public async Task<CatFact> getCatFact()
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
