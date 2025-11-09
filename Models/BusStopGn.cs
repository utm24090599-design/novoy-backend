using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class BusStopGn
    {
        public int PeopleIn { get; set; }

        public int? StudentsCantity {get; set;}
        public int? ChildrenCantity { get; set; }
        public int? OldCantity { get; set; }
        public int? AnyPersonCantity { get; set; }
    }
}
