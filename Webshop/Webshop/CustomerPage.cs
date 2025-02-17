using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Models;
using Dapper;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Webshop
{
    internal class CustomerPage
    {
        public static void CustomerMenu()
        {
            Console.WriteLine();
            List<string> frontPageText = new List<string> { " # Wear & Tear # ", "Clothes made to wear" };
            var windowTop = new Window("", 2, 1, frontPageText);
            windowTop.Draw();

            List<string> customerMenu = new List<string> { "1. Startsida", "2. Shoppen", "3. Varukorgen" };
            var customerWindow = new Window("Kund", 2, 5, customerMenu);
            customerWindow.Draw();
            Console.Write("Välj mellan 1-3: ");
            int choice = int.Parse(Console.ReadLine());
          

            while (true)
            {
                switch (choice)
                {
                    case 1:
                        ShowFrontPage();
                        break;
                    case 2:
                        ShowCategories();
                        break;
                    case 3:
                        ShowCart();
                        break;
                    default:
                        Console.WriteLine("Ogiltligt val. Välj ett nummer mellan 1-3");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
                
               


            }
        }







        public static void ShowFrontPage()
        {
            
            while (true)
            {
                
                using (var myDb = new MyDbContext())
                {
                    var frontPageProducts = myDb.Products
                                         .Where(p => p.ShowOnFrontPage == true)
                                         .Take(3)
                                         .ToArray();

                    if (frontPageProducts.Any())
                    {

                        Console.WriteLine("----------------------------");
                        Console.WriteLine("Utvalda produkter");
                        Console.WriteLine("----------------------------");

                      
                        foreach (var product in frontPageProducts)    
                        {

                          Console.WriteLine($"{product.Name}, Pris: {product.Price}kr ");


                        }
                        
                        
                        //var productWindow = new Window("Erbjudande ", 2, 8, frontPage);
                        //productWindow.Draw();

                        //List<string> productText2 = frontPageProducts.ToList(); /*new List<string> { "Byxor", "Lagom långa byxor", "Pris: 299 kr", "Tryck B för att köpa" };*/
                        //var windowTop3 = new Window("Erbjudande 2", 28, 6, );
                        //windowTop3.Draw();

                        //List<string> topText4 = new List<string> { "Läderskor", "Extra flotta", "Pris: 450 kr", "Tryck C för att köpa" };
                        //var windowTop4 = new Window("Erbjudande 3", 56, 6, topText4);
                        //windowTop4.Draw();
                    }
                    else
                    {
                        Console.WriteLine("Produkt visas inte");
                    }
                    Console.ReadLine();
                    
                }

          
             
                
       
            }
        }

       
        public static void ShowChoosenProducts()
        {
            

        }


        

        public static void ShowCategories()
        {
            using (var myDb = new MyDbContext())
            {
                var categories = myDb.Categories;

                Console.WriteLine($"---------------------------------------------");
                Console.WriteLine("Kategorier");
                Console.WriteLine($"---------------------------------------------");

                foreach (var category in categories)
                {
                    Console.WriteLine(category.Id + "." + category.Name);
                }
                Console.Write("Välj en kategori: ");
                var categoryId = int.Parse(Console.ReadLine());

                var selectedCategory = categories.Where(c => c.Id == categoryId).FirstOrDefault();

                if (selectedCategory != null)
                {
                    // Visa produkter i vald kategori
                    var productsInCategory = myDb.Products.Where(p => p.CategoryId == selectedCategory.Id).ToList();

                    Console.WriteLine($"---------------------------------------------");
                    Console.WriteLine($"Produkter i kategorin: {selectedCategory.Name}");
                    Console.WriteLine($"---------------------------------------------");


                    if (productsInCategory.Any())
                    {
                        // Visa produkter
                        for (int i = 0; i < productsInCategory.Count; i++)
                        {
                            var product = productsInCategory[i];
                            Console.WriteLine($"{i + 1}. {product.Name}, {product.Price}kr");
                        }

                        // Välja produkt
                        Console.Write("Välj en produkt genom att ange numret: ");
                        var productIndex = int.Parse(Console.ReadLine());

                        var selectedProduct = myDb.Products.Where(p=>p.Id == productIndex).ToList();

                        // Produkt-selektion
                        if(selectedProduct != null)
                        {
                            var productSelected = productsInCategory[productIndex];
                            Console.WriteLine($"Du har valt produkten: {productSelected.Name}, {productSelected.Price}kr");
                        }
                        else
                        {
                            Console.WriteLine("Ogiltligt produktval.");
                        }
                        Console.WriteLine("Välj alternativ:");
                        Console.WriteLine("[+] Lägg till i kundvagn \n [x] Avsluta");
                        ConsoleKeyInfo key = Console.ReadKey();
                        Console.Clear();

                        switch (key.KeyChar)
                        {
                            case '+':
                                var newCart = new ShoppingCart()
                                {
                                    



                                };
                                myDb.ShoppingCarts.Add(newCart);
                                myDb.SaveChanges();
                                Console.WriteLine("Vald produkt las till i varukorgen");
                                break;

                            case 'x':
                                Console.WriteLine("Avslutar..");
                                return;
                        }
                        






                    }
                    else
                    {
                        Console.WriteLine("Det finns inga produkter i denna kategori.");
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt kategori-ID.");
                }
                Console.ReadLine();
            }

        }


        public static void ShowCart()
        {

            using(var myDb = new MyDbContext())
            {
                Console.WriteLine("---------------");
                Console.WriteLine($"Din Varukorg: ");

                foreach (var cart in myDb.ShoppingCarts.Include(p => p.Products))
                {
                    Console.WriteLine(cart.Id);
                    foreach(var product in cart.Products)
                    {
                        Console.WriteLine("\t" + product);
                    }
                   
                  
                   
                }
                Console.ReadLine();


               
              

                //var cartText = myDb.ShoppingCarts.ToList();// new /*List<*/string> { "1 st Blå byxor", "2 st Grön tröja", "1 st Röd skjorta", "Tryck X för att checka ut" };
                //var windowCart = new Window("Din varukorg", 30, 1, cartText);
                //windowCart.Draw();

                Console.ReadLine();
               
            }




        }

    }

}
            
    



