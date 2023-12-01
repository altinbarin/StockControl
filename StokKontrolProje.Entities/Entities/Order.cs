using StokKontrolProje.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProje.Entities.Entities
{
    public class Order:BaseEntity
    {
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User? User { get; set; }
        public Status Status { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }
        public Order()
        {
            OrderDetails = new List<OrderDetails>();
        }
    }
}
