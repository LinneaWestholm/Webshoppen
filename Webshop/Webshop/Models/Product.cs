using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Models
{
    internal class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Colour {  get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public virtual Category? Category { get; set; }
        public int? CategoryId { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
        public bool? ShowOnFrontPage { get; set; } = true;

    }
}
