namespace InventoryApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Quantity{get;set;}
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public string Description { get; set; }
    }
}
