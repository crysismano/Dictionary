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
    // A fordításhoz használt szolgáltatás
    public class TranslatorService
    {
        // Base Uri
        private readonly Uri serverUrl = new Uri("https://dictionary.yandex.net");
        private readonly string apiKey = "dict.1.1.20210507T230628Z.c6ae057e380147a6.beff197d0a0a4e80af9d656eddc57251f3595338";

        private async Task<T> GetAsync<T>(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                if (response.StatusCode.ToString() != "OK")
                {
                    throw new HttpRequestException(response.StatusCode.ToString());
                }
                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }
        // támogatott nyelvek lekérdezése
        public async Task<List<string>> GetSupportedLanguagesAsync()
        {
            return await GetAsync<List<string>>(new Uri(serverUrl, $"/api/v1/dicservice.json/getLangs?key={apiKey}"));
        }
        // a fordítás lekérdezése
        public async Task<Translation> GetTranslationAsync(string fromLang, string toLang, string text)
        {
            return await GetAsync<Translation>(new Uri(serverUrl, $"/api/v1/dicservice.json/lookup?key={apiKey}&lang={fromLang}-{toLang}&text={text}"));
        }
    }
}
