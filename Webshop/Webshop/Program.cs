using Microsoft.Identity.Client;
using Webshop.Models;

namespace Webshop
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using (var myDb = new MyDbContext())
            {

                bool logIn = true;

                while (logIn == true)
                {
                    Console.Clear();
                    Console.WriteLine("Välkommen! ");
                    Console.WriteLine();
                    List<string> windowtext1 = new List<string> { "1. Kund", "2. Admin" };
                    var window1 = new Window("Logga in", 2, 1, windowtext1);
                    window1.Draw();
                    Console.WriteLine("Avsluta [x]");
                    Console.Write("Ditt val: ");
                    var choice = Console.ReadLine();

                    Console.WriteLine();
                    Console.Clear();

                    if (choice == "1")
                    {
                        CustomerPage.CustomerMenu();
                    }
                    else if (choice == "2")
                    {
                        Admin.AdminPage();
                    }
                    else if (choice == "x")
                    {
                        Console.WriteLine("....Hejdå!");
                        logIn = false;
                    }

                }



            }


        }
    }
}
