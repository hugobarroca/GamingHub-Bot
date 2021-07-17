using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiCalls
{
    public class AnimeApi
    {
        private readonly string _baseURL = "https://animechan.vercel.app/api/";

        public async Task<AnimeQuoteModel> GetRandomAnimeQuote()
        {

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(_baseURL + "random"))
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
