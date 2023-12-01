using StokKontrolProje.Entities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace StokKontrolProje.Entities
{
    public class Product:BaseEntity
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short? Stock { get; set; }
        public DateTime? ExpireDate { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }
        public Product()
        {
            OrderDetails = new List<OrderDetails>();
        }
    }
}