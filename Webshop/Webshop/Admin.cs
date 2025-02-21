using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Models;
using System.ComponentModel.Design;


namespace Webshop
{
    internal class Admin
    {
        public static void AdminPage()
        {
            // ADMIN
            bool isRunning = true;

            while (isRunning)
            {
                List<string> adminMenu = new List<string> { "1. Lägg till ny produkt", "2. Uppdatera produkt", "3. Ta bort produkt", "4. Lägg till ny kategori", "5. Uppdatera kategori", "6. Ta bort kategori"};
                var adminWindow = new Window("Admin", 2, 1, adminMenu);
                adminWindow.Draw();
                List<string> exit = new List<string> { "Avsluta [x]" };
                var adminWindow1 = new Window("", 4, 4, exit);

                Console.Write("Välj mellan 1-6, [x] för att avsluta: ");
                var choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (choice)
                {
                    case '1':
                        AddNewProduct();
                        break;
                    case '2':
                        UpdateProduct();
                        break;
                    case '3':
                        RemoveProduct();
                        break;
                    case '4':
                        AddNewCategory();
                        break;
                    case '5':
                        UpdateCategory();
                        break;
                    case '6':
                        RemoveCategory();
                        break;
                    case 'x':
                        isRunning = false;
                        Console.WriteLine("..Hejdå!");
                        return;
                    default:
                        Console.WriteLine("Ogiltligt val. Välj ett nummer mellan 1-4");
                        break;
                }
                Console.ReadLine();
            }
        }

        public static void AddNewProduct()
        {
            bool goBack = false;
            while (!goBack)
            {
                using (var myDb = new MyDbContext())
                {
                    // Lägg till ny produkt
                    Console.WriteLine("------------------------");
                    Console.WriteLine("Lägg till ny produkt: ");
                    Console.WriteLine("------------------------");
                    foreach (var category in myDb.Categories)           // Visar kategorier
                    {
                        Console.WriteLine(category.Id + "." + category.Name);
                    }
                    Console.WriteLine("Ange kategori id: ");  // Välj vilken kategori produkten ska befinna sig i genom att mata in kategori-id
                    var productCategory = Console.ReadLine();

                    // Fyll i poroduktinfo
                    Console.WriteLine("Ange namn på produkt: ");
                    var productName = Console.ReadLine();

                    Console.WriteLine("Ange färg på produkt: ");
                    var productColour = Console.ReadLine();

                    Console.WriteLine("Lägg till en kort beskrivning: ");
                    var productDescription = Console.ReadLine();

                    Console.WriteLine("Ange pris: ");
                    var productPrice = Console.ReadLine();

                    Console.WriteLine("Visa på startsida (Ja/Nej): ");
                    var showOnFrontPage = Console.ReadLine().ToLower();

                    bool showProductOnFrontPage = showOnFrontPage == "ja" || showOnFrontPage == "j";

                    // Skapar en ny produkt och sparar den till databasen
                    var newProduct = new Product
                    {
                        CategoryId = int.Parse(productCategory),
                        Name = productName,
                        Colour = productColour,
                        Description = productDescription,
                        Price = double.Parse(productPrice),
                        ShowOnFrontPage = showProductOnFrontPage
                    };
                    myDb.Add(newProduct);
                    myDb.SaveChanges();
                    Console.WriteLine("Ny produkt har lagts till.");
                }
                Console.ReadLine();
                Console.Clear();
                goBack = true;
            }
        }

        public static void UpdateProduct()
        {
            bool goBack = false;
            while (!goBack)
            {
                using (var myDb = new MyDbContext())
                {
                    Console.WriteLine("-------------");
                    Console.WriteLine("Ändra produkt");
                    Console.WriteLine("--------------");
                    foreach (var product in myDb.Products) // Visar produkter
                    {
                        Console.WriteLine(product.Id + "." + product.Name);
                    }
                    Console.Write("Ange produkt id: "); // Ange produkt-Id på produkten som ska ändras
                    int productId = int.Parse(Console.ReadLine());

                    var updateProduct = myDb.Products.Where(p => p.Id == productId).FirstOrDefault();
                 
                    // Uppdatera produkter
                    if (updateProduct != null)
                    {
                        // Ny info läggs till, lämmna fältet tomt om man inte vill ändra
                        Console.Write("Ange nytt produktnamn(annars-lämna tomt): ");
                        var newName = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newName)) 
                        {
                            updateProduct.Name = newName;
                        }

                        Console.Write("Ange ny färg (annars-lämna tomt): ");
                        var newColour = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newColour))
                        {
                            updateProduct.Colour = newColour;
                        }

                        Console.Write("Ange ny beskrivning (annars-lämna tomt): ");
                        var newDescription = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newDescription))
                        {
                            updateProduct.Description = newDescription;
                        }

                        Console.Write("Ange nytt produktpris (annars-lämna tomt): ");
                        var newPrice = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newPrice))
                        {
                            updateProduct.Price = double.Parse(newPrice);
                        }

                        Console.Write("Visa på startsida (Ja/Nej): ");
                        var showProduct = Console.ReadLine()?.ToLower();

                        if (!string.IsNullOrEmpty(showProduct))
                        {
                            bool showProductOnFrontPage = showProduct == "ja" || showProduct == "j";
                            updateProduct.ShowOnFrontPage = showProductOnFrontPage;
                        }
                        myDb.SaveChanges();
                        Console.WriteLine("Produkt är uppdaterad");  // Ändringarna är sparade
                    }
                }
                Console.ReadLine();
                Console.Clear();
                goBack = true;
            }
        }

        public static void RemoveProduct()
        {
            bool goBack = false;
            while (!goBack)
            {
                using (var myDb = new MyDbContext())
                {
                    Console.WriteLine("--------------");
                    Console.WriteLine("Ta bort produkt");
                    Console.WriteLine("--------------");
                    foreach (var product in myDb.Products) // Visar lista med produkter
                    {
                        Console.WriteLine(product.Id + "." + product.Name);
                    }
                    Console.Write("Ange produkt id: ");  // Välj produkt genom produkt-Id
                    int productId = int.Parse(Console.ReadLine());

                    var removeProduct = myDb.Products.Where(p => p.Id == productId).FirstOrDefault();

                    // Tar bor produkt
                    if (removeProduct != null)
                    {
                        myDb.Products.Remove(removeProduct);
                        myDb.SaveChanges();
                        Console.WriteLine("Produkt borttagen..");
                    }
                    else
                    {
                        Console.WriteLine("Produkten hittades inte");
                    }
                }
                Console.ReadLine();
                Console.Clear();
                goBack = true;
            }
        }

        public static void AddNewCategory()
        {
            bool goBack = false;
            while (!goBack)
            {
                using (var myDb = new MyDbContext())
                {
                    Console.WriteLine("----------------------");
                    Console.WriteLine("Lägg till ny kategori");
                    Console.WriteLine("----------------------");
                    foreach (var category in myDb.Categories)  // Visar kategorier
                    {
                        Console.WriteLine(category.Name);
                    }
                    Console.Write("Ange kategorinamn: ");
                    var categoryName = Console.ReadLine();

                    // Skapar ny kategori
                    var newCategory = new Category
                    {
                        Name = categoryName
                    };
                    myDb.Add(newCategory);
                    myDb.SaveChanges();
                }
                Console.ReadLine();
                Console.Clear();
                goBack = true;
            }
        }


        public static void UpdateCategory()
        {
            bool goBack = false;
            while (!goBack)
            {
                using (var myDb = new MyDbContext())
                {
                    // Uppdatera kategorier
                    var categories = myDb.Categories;
                    Console.WriteLine("------------------");
                    Console.WriteLine("Uppdatera kategori");
                    Console.WriteLine("------------------");
                    foreach (var category in categories)   // Visar kategorier
                    {
                        Console.WriteLine(category.Id + "." + category.Name);
                    }
                    Console.Write("Ange kategori id: "); 
                    var categoryId = int.Parse(Console.ReadLine());


                    var updateCategory = categories.Where(c => c.Id == categoryId).FirstOrDefault();

                    if (updateCategory != null)
                    {
                        {
                            // Nytt namn lägs till, lämmnas tomt om man inte vill ändra
                            Console.WriteLine("Ange nytt namn på kategori(annars-lämna tomt): ");
                            var newName = Console.ReadLine();
                            if (!string.IsNullOrEmpty(newName))
                            {
                                updateCategory.Name = newName;
                            }
                            myDb.SaveChanges();
                            Console.WriteLine("Kategori är uppdaterad..");
                        }
                    }
                }
                Console.ReadLine();
                Console.Clear();
                goBack = true;
            }
        }

        public static void RemoveCategory()
        {
            bool goBack = false;
            while (!goBack)
            {
                using (var myDb = new MyDbContext())
                {
                    // Ta bort kategori
                    Console.WriteLine("----------------");
                    Console.WriteLine("Ta bort kategori");
                    Console.WriteLine("-----------------");
                    foreach (var category in myDb.Categories) // Visar de kategorier som finns
                    {
                        Console.WriteLine(category.Id + "." + category.Name);
                    }
                    Console.WriteLine("Ange kategori id: ");
                    int categoryId = int.Parse(Console.ReadLine());

                    var removeCategory = myDb.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

                    // Kategorin tas bort
                    if (removeCategory != null)
                    {
                        myDb.Categories.Remove(removeCategory);
                        myDb.SaveChanges();
                        Console.WriteLine("Kategori är borttagen..");
                    }
                    else
                    {
                        Console.WriteLine("Kategori hittades inte..");
                    }
                }
            }
            Console.ReadLine();
            Console.Clear();
            goBack = true;
        }
    }
}
