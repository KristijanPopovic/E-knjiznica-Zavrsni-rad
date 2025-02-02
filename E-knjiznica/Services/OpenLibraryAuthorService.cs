using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using E_knjiznica.Models;
using Newtonsoft.Json.Linq;

namespace E_knjiznica.Services
{
    public class OpenLibraryAuthorService
    {
        private readonly HttpClient _httpClient;

        public OpenLibraryAuthorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ✅ Dohvati detalje o autoru s Open Library API-ja
        public async Task<Author> GetAuthorDetailsAsync(string openLibraryId)
        {
            var url = $"https://openlibrary.org/authors/{openLibraryId}.json";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var author = JsonSerializer.Deserialize<Author>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return author;
            }

            return null; // Ako nije uspješno, vraćamo null
        }

        // ✅ Dohvati popis djela autora
        public async Task<List<Book>> GetAuthorWorksAsync(string openLibraryId)
        {
            var url = $"https://openlibrary.org/authors/{openLibraryId}/works.json";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OpenLibraryWorksResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Pretvaranje rezultata u listu knjiga
                return result.Entries.Select(work => new Book
                {
                    Title = work.Title,
                    PublishedYear = work.FirstPublishDate ?? "Nepoznato",
                    CoverUrl = work.CoverId != null ? $"https://covers.openlibrary.org/b/id/{work.CoverId}-L.jpg" : null
                }).ToList();
            }

            return new List<Book>(); // Ako nema rezultata, vraćamo praznu listu
        }
        // ✅ Dohvaćanje detalja o autoru
        public async Task<string> GetAuthorBiographyAsync(string openLibraryId)
        {
            try
            {
                var url = $"https://openlibrary.org/authors/{openLibraryId}.json";
                var response = await _httpClient.GetStringAsync(url);

                var json = JObject.Parse(response);
                return json["bio"]?.ToString() ?? "Biografija nije dostupna.";
            }
            catch
            {
                return "Biografija nije dostupna.";
            }
        }

        // ✅ Model za parsiranje API odgovora
        public class OpenLibraryWorksResponse
        {
            public List<OpenLibraryWork> Entries { get; set; }
        }

        public class OpenLibraryWork
        {
            public string Title { get; set; }
            public string FirstPublishDate { get; set; }
            public int? CoverId { get; set; }
        }
    }
}
