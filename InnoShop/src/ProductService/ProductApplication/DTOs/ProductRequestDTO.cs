namespace ProductApplication.DTOs;

public class ProductRequestDTO
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }


    public int UserId { get; set; }
}