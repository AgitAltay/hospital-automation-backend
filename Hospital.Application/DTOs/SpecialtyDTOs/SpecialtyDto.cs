namespace Hospital.Application.DTOs.SpecialtyDTOs
{
    public class SpecialtyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}