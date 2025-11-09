using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Bus
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("max_capacity")]
        public int Max_Capacity { get; set; }
        [Column("number_plate")]
        public required string Number_Plate {  get; set; }
        [Column("route")]
        public int Route {  get; set; }
    }
}
