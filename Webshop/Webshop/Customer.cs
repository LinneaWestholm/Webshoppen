using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Webshop.Models;

namespace Webshop
{
    internal class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

     
        public Customer(int id, string name)
        {
            Id = id;
            Name = name;
        }



    }
}
