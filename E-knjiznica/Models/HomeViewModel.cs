namespace E_knjiznica.Models
{
    public class HomeViewModel
    {
        public List<Book> PopularBooks { get; set; }
        public List<Book> ClassicBooks { get; set; }
        public List<Book> FavoriteBooks { get; set; }
    }
}
