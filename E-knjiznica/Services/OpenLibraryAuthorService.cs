using System.Net.Http;
using System.Threading.Tasks;
using E_knjiznica.Models;
using Newtonsoft.Json;

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
                var author = JsonConvert.DeserializeObject<Author>(json);
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
                var result = JsonConvert.DeserializeObject<OpenLibraryWorksResponse>(json);

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

        // ✅ Dohvaćanje biografije autora
        public async Task<string> GetAuthorBiographyAsync(string openLibraryId)
        {
            var response = await _httpClient.GetAsync($"https://openlibrary.org/authors/{openLibraryId}.json");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);
                return data?.bio?.ToString() ?? "Biografija nije dostupna.";
            }
            return "Biografija nije dostupna.";
        }

        // ✅ Dohvaćanje popisa knjiga autora
        public async Task<List<string>> GetAuthorBooksAsync(string openLibraryId)
        {
            var response = await _httpClient.GetAsync($"https://openlibrary.org/authors/{openLibraryId}/works.json");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);
                var books = new List<string>();

                foreach (var work in data.entries)
                {
                    books.Add((string)work.title);
                }
                return books;
            }
            return new List<string> { "Nema dostupnih knjiga." };
        }

        // ✅ Model za parsiranje API odgovora
        public class OpenLibraryWorksResponse
        {
            [JsonProperty("entries")]
            public List<OpenLibraryWork> Entries { get; set; }
        }

        public class OpenLibraryWork
        {
            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("first_publish_date")]
            public string FirstPublishDate { get; set; }

            [JsonProperty("cover_id")]
            public int? CoverId { get; set; }
        }
    }
}
