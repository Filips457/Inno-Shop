using System.ComponentModel.DataAnnotations;

namespace ProductApplication.DTOs;

public class ProductRequestDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }


    public int UserId { get; set; }
}