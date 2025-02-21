using Microsoft.Identity.Client;
using Webshop.Models;

namespace Webshop
{
    internal class Program
    {
        static void Main(string[] args)
        {
                bool logIn = true;

                while (logIn)
                {
                    // Logga in som Kund eller Admin, eller avluta programmet
                    using (var myDb = new MyDbContext())
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
                            CustomerPage.CustomerMenu(); // VAL 1: Kund-sidan (CustomerPage)
                        }
                        else if (choice == "2")
                        {
                            Admin.AdminPage();  // VAl 2: Admin-sidan (Admin)
                        }
                        else if (choice == "x")   // Avsluar programmet
                        {
                            Console.WriteLine("....Hejdå!");
                            logIn = false;
                        }
                    }
                }
        }

    }
}

