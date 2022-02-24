namespace WebApiBooks.Entidades
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Genre> Genre { get; set;}
    }
}
