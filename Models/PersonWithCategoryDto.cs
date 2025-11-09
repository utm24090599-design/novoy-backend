namespace backend.Dtos
{
    public class PersonWithCategoryDto
    {
        public long Id { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Category { get; set; } // Ej: "Estudiante", "Niño", etc.
    }
}
