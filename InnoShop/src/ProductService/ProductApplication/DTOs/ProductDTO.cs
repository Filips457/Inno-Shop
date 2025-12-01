namespace ProductApplication.DTOs;

public class ProductDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }
    
    public DateTime CreationTime { get; set; }

    public bool IsActive { get; set; }


    public int UserId { get; set; }
}