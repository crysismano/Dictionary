using Dictionary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Services
{
    // A szinonima kereséshez használt szolgáltatás
    public class SynonymService
    {
        // Base Uri
        private readonly Uri serverUrl = new Uri("http://thesaurus.altervista.org/thesaurus/v1");
        // api kulcs
        private readonly string apiKey = "OwFAz7iKs4Ba2GxReZAb";

        private async Task<T> GetAsync<T>(Uri uri)
        {
            using(var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                // ha a válasz nem OK akkor kivételt dobunk
                if(response.StatusCode.ToString() != "OK")
                {
                    throw new HttpRequestException(response.StatusCode.ToString());
                }
                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }
        // szinonimák lekérdezése
        public async Task<Synonym> GetSynonymAsync(string languageCode, string word)
        {
            return await GetAsync<Synonym>(new Uri(serverUrl, $"?word={word}&language={languageCode}&output=json&key={apiKey}"));
        }
    }
}
