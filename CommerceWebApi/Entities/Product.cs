namespace CommerceWebApi.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }
}