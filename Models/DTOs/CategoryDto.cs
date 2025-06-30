namespace MyEcommAPI.Models.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

public class CreateCategoryDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(300)]
    public string Description { get; set; }
}
            