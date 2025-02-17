using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Models
{
    internal class ShoppingCart
    {
        public int Id { get; set; }
        public bool? IsPaid { get; set; } = true;
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();


        public ShoppingCart()
        {
            Products = new List<Product>();
        }

   
     
    }
}
