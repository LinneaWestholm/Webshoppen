using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop
{
    internal class MyDbContext : DbContext
    {
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.ShoppingCart> ShoppingCarts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Webshoppen;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        //optionsBuilder.UseSqlServer("Server = tcp:linneasdatabas.database.windows.net, 1433; Initial Catalog = linneasdatabas1; Persist Security Info = False; User ID = lwadmin; Password = admin123!; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
    }


}
