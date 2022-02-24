namespace WebApiBooks.Entidades
{
    public class Genre
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubGenre { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
