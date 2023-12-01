using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProje.Entities.Entities
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public virtual List<Product> Products { get; set; }
        public Category()
        {
            Products = new List<Product>();
        }
    }
}
