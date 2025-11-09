using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Price
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("children")]
        public double Children { get; set; }
        [Column("old")]
        public double Old { get; set; }
        [Column("student")]
        public double Student { get; set; }
        [Column("any_person")]
        public double Any_Person { get; set; }
    }
}
