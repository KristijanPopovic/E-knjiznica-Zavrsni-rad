using E_knjiznica.Models.API;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace E_knjiznica.Services
{
    public class OpenLibraryService
    {
        private readonly HttpClient _httpClient;

        public OpenLibraryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OpenLibraryBook>> SearchBooksAsync(string query)
        {
            var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new List<OpenLibraryBook>();

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var docs = json["docs"];

            var books = docs?.Select(doc => new OpenLibraryBook
            {
                Title = doc["title"]?.ToString(),
                AuthorName = doc["author_name"]?.FirstOrDefault()?.ToString(),
                FirstPublishYear = doc["first_publish_year"]?.ToString(),
                CoverId = doc["cover_i"]?.ToString()
            }).ToList();

            return books ?? new List<OpenLibraryBook>();
        }
        public async Task<string> GetRawSearchResultsAsync(string query)
        {
            var response = await _httpClient.GetAsync($"https://openlibrary.org/search.json?q={query}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();

        }
    }
}
