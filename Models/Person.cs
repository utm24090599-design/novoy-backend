using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Person
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("age")]
        public int Age { get; set; }
        [Column("gender")]
        public required string Gender { get; set; }
        
        //public string? Category { get; set; } // Ej: "Estudiante", "Niño", "Viejo", "Otro"
    }
}
