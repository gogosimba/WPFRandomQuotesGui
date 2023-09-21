using ErrorReportsGui.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ErrorReportsGui.Services
{
    class RandomQuoteGenerator
    {
        public static async Task<RandomQuoteModel> GetRandomQuote()
        {
            var client = new HttpClient();
            string apikey = "e71244bcfdmsh5ea67ddbc2309f2p18c1dbjsn63a9cb586054";
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://quotes15.p.rapidapi.com/quotes/random/"),
                    Headers =
                {
                    { "x-rapidapi-key", apikey },
                    { "x-rapidapi-host", "quotes15.p.rapidapi.com" },
                },
                };
                Console.WriteLine($"Sending API request to: {request.RequestUri}");


                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    RandomQuoteModel randomquote = JsonConvert.DeserializeObject<RandomQuoteModel>(body)!;
                    return randomquote;



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpRequestException("Error getting quote");
            }

        }
    }
}
