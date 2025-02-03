namespace E_knjiznica.Models.API
{
    public class OpenLibraryBook
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string FirstPublishYear { get; set; }
        public string CoverId { get; set; }

        public string CoverUrl => !string.IsNullOrEmpty(CoverId)
            ? $"https://covers.openlibrary.org/b/id/{CoverId}-L.jpg"
            : "https://via.placeholder.com/150";
    }
}
