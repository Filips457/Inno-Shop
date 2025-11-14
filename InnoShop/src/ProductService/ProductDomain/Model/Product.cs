namespace ProductDomain.Model;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTime CreationTime { get; set; }

    public int UserId { get; set; }
}