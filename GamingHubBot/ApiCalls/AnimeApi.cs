using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiCalls
{
    public class AnimeApi
    {
        public async Task<AnimeQuoteModel> GetRandomAnimeQuote()
        {
            string url = "https://animechan.vercel.app/api/random";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var quote = await response.Content.ReadAsAsync<AnimeQuoteModel>();
                    return quote;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
    }
}
