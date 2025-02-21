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
using Azure;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Webshop
{
    internal class CustomerPage
    {

        public static void CustomerMenu()
        {

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine();
                List<string> frontPageText = new List<string> { " # Wear & Tear # ", "Clothes made to wear" };
                var windowTop = new Window("", 2, 1, frontPageText);
                windowTop.Draw();

                List<string> customerMenu = new List<string> { "1. Startsida", "2. Shoppen", "3. Varukorgen", "4. Avsluta" };
                var customerWindow = new Window("Kund", 2, 5, customerMenu);
                customerWindow.Draw();
                Console.Write("Välj mellan 1-4: ");
                int choice = int.Parse(Console.ReadLine());
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        ShowFrontPage();
                        break; ;
                    case 2:
                        ShowCategories();
                        break;
                    case 3:
                        ShowCart();
                        break;

                    case 4:
                        isRunning = false;
                        Console.WriteLine("...Hejdå!");
                        break;

                    default:
                        Console.WriteLine("Ogiltligt val. Välj ett nummer mellan 1-3");
                        break; 
                }
                
            }
           
        }

        public static void ShowFrontPage()
        {
            bool goBack = false;
            
            while (!goBack)
            {
                
                using (var myDb = new MyDbContext())
                {
                    var frontPageProducts = myDb.Products.Where(p => p.ShowOnFrontPage == true).ToArray();
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Utvalda produkter");
                    Console.WriteLine("----------------------------");

                    // Visar utvalda produkter (ShowOnFrontPage)
                    foreach (var product in frontPageProducts)
                    {
                        Console.WriteLine($"{product.Name}");
                        Console.WriteLine($"{product.Colour}");
                        Console.WriteLine($"{product.Description}");
                        Console.WriteLine($"Pris: {product.Price}kr");
                        Console.WriteLine($"Tryck {product.Id} för att köpa");
                        Console.WriteLine("----------------------------");
                    } 
                    Console.Write("Ange vald product: ");   // Välj utvald produkt via product.Id
                    var productId = int.Parse(Console.ReadLine());

                    var selectedProduct = myDb.Products.Where(p => p.Id == productId).FirstOrDefault();

                    if (selectedProduct != null)
                    {
                        Console.Write("Mata in ditt ID: "); // Mata in ShoppingCartId
                        var cartId = int.Parse(Console.ReadLine());

                        var cart = myDb.ShoppingCarts
                            .Include(c => c.Products)
                            .FirstOrDefault(c => c.Id == cartId);

                        if(cart != null)
                        {
                            // Lägger till utvald produkt i vald varukorg
                            cart.Products.Add(selectedProduct);
                            myDb.SaveChanges();
                            Console.WriteLine("Vald produkt las till i varukorgen");
                        }
                        else
                        {
                            Console.WriteLine("Ogiltligt Id");
                        }

                       
                    }
                    else
                    {
                        
                        Console.WriteLine("Ogiltligt Id");

                    }
                 

                  
                }
                Console.ReadLine();
                Console.Clear();
                goBack = true;
               
                
            }
        }

        public static void ShowCategories()
        {
            bool goBack = false;
            while (!goBack)
            {
                using (var myDb = new MyDbContext())
                {
                    var categories = myDb.Categories;

                    Console.WriteLine($"---------------------------------------------");
                    Console.WriteLine("Kategorier");
                    Console.WriteLine($"---------------------------------------------");

                    foreach (var category in categories)         // Skriver ut kategorier
                    {
                        Console.WriteLine(category.Id + "." + category.Name);
                    }
                    Console.Write("Välj en kategori: ");   // Välj kategori via inmatning av Id
                    var categoryId = int.Parse(Console.ReadLine());

                    var selectedCategory = categories.Where(c => c.Id == categoryId).FirstOrDefault();      

                    if (selectedCategory != null)                      
                    {
                        // Visa produkter i vald kategori
                        var productsInCategory = myDb.Products.Where(p => p.CategoryId == selectedCategory.Id).ToList();

                        Console.WriteLine("---------------------------------------------");
                        Console.WriteLine($"Produkter i kategorin: {selectedCategory.Name}");
                        Console.WriteLine("---------------------------------------------");


                        if (productsInCategory.Any())
                        {
                            // Visa produkter
                            foreach (var product in productsInCategory)
                            {
                                Console.WriteLine($"{product.Name}, \nPris: {product.Price}kr \nProdukt-ID: {product.Id}");
                                Console.WriteLine("---------------------------------------------");
                            }

                            // Välja produkt
                            Console.Write("Välj en produkt genom att ange Produkt-ID: ");
                            var productId = int.Parse(Console.ReadLine());

                            var selectedProduct = myDb.Products.Where(p => p.Id == productId).FirstOrDefault();

                            if (selectedProduct != null)
                            {
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Du har valt produkten:");
                                Console.WriteLine($"{selectedProduct.Name} \n{selectedProduct.Colour} \n{selectedProduct.Description} \n{selectedProduct.Price}kr");
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine("Välj alternativ:");
                                Console.WriteLine("[+] Lägg till i kundvagn \n[x] Avsluta");
                                ConsoleKeyInfo key = Console.ReadKey();
                                Console.Clear();


                                switch (key.KeyChar)
                                {
                                    case '+':
                                        // Kollar om man redan är kund, annars skapas en ny kund=ny varukorg
                                        Console.WriteLine("1: Ny kund? ");        
                                        Console.WriteLine("2. Redan kund?");      
                                        Console.Write("Ditt val: ");
                                        var choice = Console.ReadLine();

                                        if(choice == "1")
                                        {
                                            // skapar ny varukorg
                                            var newCart = new ShoppingCart();
                                            newCart.Products.Add(selectedProduct);

                                            myDb.ShoppingCarts.Add(newCart);
                                            myDb.SaveChanges();
                                            Console.WriteLine("Ny kund skapad...");
                                            Thread.Sleep(1000);
                                            Console.WriteLine("Vald produkt las till i varukorgen");

                                        }
                                        else if (choice == "2")
                                        {
                                            // inmatning ShoppingCartId
                                            Console.Write("Mata in ditt ID: ");
                                            var cartId = int.Parse(Console.ReadLine());

                                            var cart = myDb.ShoppingCarts
                                                .Include(c => c.Products)
                                                .FirstOrDefault(c => c.Id == cartId);

                                            if(cart!= null)
                                            {
                                                // Lägger till produkt i vald varukorg
                                                cart.Products.Add(selectedProduct);
                                                myDb.SaveChanges();
                                                Console.WriteLine("Vald produkt las till i varukorgen");
                                            }

                                        }
                                        break;

                                    case 'x':
                                        Console.WriteLine("Avslutar..");
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ogiltligt produktval.");
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
                }
                Console.ReadLine();
                Console.Clear();
                goBack = true;
            }
        }


        public static void ShowCart()
        {
            bool goBack = false;

            while (!goBack)
            {
                using (var myDb = new MyDbContext())
                {
                    Console.Write("Mata in ditt ID: ");
                    var cartId = int.Parse(Console.ReadLine());

                    var cart = myDb.ShoppingCarts
                        .Include(c => c.Products)
                        .FirstOrDefault(c => c.Id == cartId);

                    if (cart != null)
                    {
                        Console.WriteLine("---------------");
                        Console.WriteLine($"Din Varukorg: {cart.Id} ");

                        foreach (var product in cart.Products)
                        {
                            Console.WriteLine($"{product.Name}, Pris: {product.Price}kr");
                        }
                    }

                    Console.ReadLine();
                    Console.WriteLine("[+] Lägg till produkt");
                    Console.WriteLine("[-] Ta bort");
                    ConsoleKeyInfo key = Console.ReadKey();

                    switch (key.KeyChar)
                    {
                        case '+':
                            Console.WriteLine("------------------");
                            Console.WriteLine("Produkter");
                            foreach (var product in myDb.Products)
                            {
                                Console.WriteLine($"{product.Id}.{product.Name}, Pris:{product.Price}");
                            }

                            Console.Write("Ange produkt-ID: ");
                            var productId = int.Parse(Console.ReadLine());

                            var selectedProduct = myDb.Products.Where(p => p.Id == productId).FirstOrDefault();

                            if (selectedProduct != null)
                            {
                                cart.Products.Add(selectedProduct);

                                myDb.SaveChanges();
                                Console.WriteLine("Vald produkt las till i varukorgen");
                            }
                            else
                            {
                                Console.WriteLine("Ogiltligt Id");
                            }
                           
                            break;

                        case '-':
                            Console.WriteLine("------------------");
                            foreach (var product in cart.Products)
                            {
                                Console.WriteLine($"{product.Id}. {product.Name}, Pris: {product.Price}kr");
                            }

                            Console.Write("Ange produkt ID: ");
                            var productIdRemove = int.Parse(Console.ReadLine());

                            var removeProduct = myDb.ShoppingCarts.Where(c => c.Products.Any(p => p.Id == productIdRemove)).FirstOrDefault();
                            if (removeProduct != null)
                            {
                                var removeFromCart = removeProduct.Products.FirstOrDefault(p => p.Id == productIdRemove);

                                if (removeFromCart != null)
                                {
                                    removeProduct.Products.Remove(removeFromCart);
                                    myDb.SaveChanges();
                                    Console.WriteLine("Produkten har tagits bort från varukorgen.");
                                    
                                }
                                else
                                {
                                    Console.WriteLine("Produkten finns inte i varukorgen");
                                }
                            }
                            break;

                        default:
                            Console.WriteLine("Ogiltligt val.. Försök igen");
                            break;


                    }
                    Console.ReadLine();
                    Console.Clear();
                    goBack = true;
                }
            }



        }

    }

}
            
    



