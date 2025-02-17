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

        public int Age { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

       


        public Customer(int id, string name, int age, string phone, string email)
        {
            Id = id;
            Name = name;
            Age = age;
            Phone = phone;
            Email = email;
            
        }



       public static void GetAllCustomers()
       {

            Console.Clear();
            Customer[] allCustomers = 
            {                    new Customer(1, "Lasse", 51, "070-1234567", "lasse.karlsson@gmail.com"),
                                 new Customer(2, "Rosa", 25, "070-3020101", "rosa.lagerblad@gmail.se"),
                                 new Customer(3, "Nils", 29, "072-3322110", "nils.bergholm@yahoo.se"),
                                 new Customer(4, "Linda", 31, "071-0102022", "linda.andersson@hotmail.com"),
                                 new Customer(5, "Morgan", 59, "072-3334445", "morgan.johansson@yahoo.se"),
                                 new Customer(6, "Göran", 72, "072-4043031", "göran.granqvist@yahoo.com"),
                                 new Customer(7, "Jenny", 47, "070-2041304", "jenny.eriksson@gmail.com"),
                                 new Customer(8, "Saga", 20, "072-0349012", "saga.larsson@hotmail.se"),
                                 new Customer(9, "Johan", 37, "070-3054102", "johan.olsson@hotmail.com"),
                                 new Customer(10, "Kjell", 56, "070-3031024", "kjell.svensson@yahoo.se"), 
            };
            foreach(Customer customer in allCustomers)
            {
                Console.Write($"Kund-Id: {customer.Id} | Namn: {customer.Name.PadRight(8)} | Age: {customer.Age} | Telefon: {customer.Phone} | Email: {customer.Email}");
                Console.WriteLine();
            }

            Console.WriteLine("Tryck på valfritagent för att komma tillbaka");
            Console.ReadKey();
       }




    }
}
