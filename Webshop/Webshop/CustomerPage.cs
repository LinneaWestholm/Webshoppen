﻿using Microsoft.EntityFrameworkCore;
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
                
            }
           
        }

        public static void ShowFrontPage()
        {
            
            while (true)
            {
                
                using (var myDb = new MyDbContext())
                {
                    var frontPageProducts = myDb.Products.Where(p => p.ShowOnFrontPage == true).ToArray();
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Utvalda produkter");
                    Console.WriteLine("----------------------------");

            
                    foreach (var product in frontPageProducts)
                    {
                        Console.WriteLine($"{product.Name}");
                        Console.WriteLine($"{product.Colour}");
                        Console.WriteLine($"{product.Description}");
                        Console.WriteLine($"Pris: {product.Price}kr");
                        Console.WriteLine($"Tryck {product.Id} för att köpa");
                        Console.WriteLine("----------------------------");
                    }

                 
                    
                    Console.ReadLine();
                }
                Console.Clear();
            }
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

                        // Produkt-selektion
                        if(selectedProduct != null)
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
                                    var newCart = new ShoppingCart();
                                    newCart.Products.Add(selectedProduct);

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
                Console.ReadLine();
            }
            Console.ReadLine();

        }


        public static void ShowCart()
        { 
            using(var myDb = new MyDbContext())
            {
                Console.WriteLine("Mata in ditt ID: ");
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
                Console.WriteLine("[x] Check out");
                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.KeyChar)
                {
                    case '+':
                        Console.WriteLine("------------------");
                        Console.WriteLine("Produkter");
                        foreach(var product in myDb.Products)
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
                        Console.WriteLine("Produkter");
                        foreach (var product in myDb.Products)
                        {
                            Console.WriteLine($"{product.Id}.{product.Name}, Pris:{product.Price}");
                        }

                        Console.Write("Ange produkt ID: ");
                        var productIdRemove = int.Parse(Console.ReadLine());

                        var removeProduct = myDb.ShoppingCarts.Where(c => c.Products.Any (p => p.Id == productIdRemove)).FirstOrDefault();
                        if (removeProduct != null)
                        {
                            var removeFromCart = removeProduct.Products.FirstOrDefault(p => p.Id == productIdRemove);

                            if(removeFromCart != null)
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
                        else
                        {
                            Console.WriteLine("Produkten blev inte borttagen");
                        }

                        break;



                    case 'x':
                        Console.WriteLine("============================");
                        Console.WriteLine("======== CHECK OUT =========");
                        Console.WriteLine("============================");
                        Console.WriteLine();
                        Console.WriteLine("Fyll i din information nedan");
                        Console.Write("Namn: ");
                        var customerName = Console.ReadLine();



                        break;

                    default:
                        Console.WriteLine("Ogiltligt val.. Försök igen");
                        break;

                   
                }

            }




        }

    }

}
            
    



