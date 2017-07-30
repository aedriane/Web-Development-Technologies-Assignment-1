using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections;

//----------------- Group 22 -----------------//
//--------- s3494336 - Jackson Lloyd  ---------//
//--------- s3541804 - Aedriane Hernan ---------//

namespace WDTAss1
{
    class CustomerMenu : Menu
    {
        Json json = new Json();

        public CustomerMenu()
        {
            Options.Add("Display Products");
            Options.Add("Display Workshops");
            Options.Add("Return to Main Menu");
            Options.Add("Quit");
            Title = "Customer Menu";
        }

        public override void PrintMenu()
        {
            MainMenu main = new MainMenu();
            Store store;
            store = main.RequestStore();
            bool loop = true;
            Console.Clear();

            while (loop)
            {
                Console.WriteLine("Welcome");
                Console.WriteLine(Title + " " + store.StoreName + "\n----------------------------");

                for (int i = 0, j = 1; i < Options.Count; j++, i++)
                {
                    Console.WriteLine("\t" + j + ". " + Options[i] + "\n");
                }

                Console.Write("Enter Option: ");

                string menu = Console.ReadLine();

                int iSel;

                try
                {
                    iSel = int.Parse(menu);
                }

                catch (FormatException)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Input\n");
                    continue;
                }

                switch (iSel)
                {
                    case 0:
                        loop = true;
                        break;

                    case 1:
                        Console.Clear();
                        DisplayStoreStock(store);
                        break;

                    case 2:
                        Console.Clear();
                        DisplayWorkshops(store);
                        break;

                    case 3:
                        //Returns to the main menu
                        Console.Clear();
                        return;

                    case 4:
                        Console.WriteLine("Goodbye!\n");
                        Environment.Exit(0);
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input, try again...\n");
                        break;
                }
            }
        }
        //=================== initalising json files =====================//
        // Display All Store Inventory - OPTION 1
        private void DisplayStoreStock(Store store)
        {
            List<StoreInv> storeStock = json.LoadStoreStock(store);
            List<CustomerCart> custCart = json.LoadCustomerCart();
            List<Workshop> work = json.LoadWorkshop(store);
            List<CustomerWorkshop> cw = json.LoadCustomerWorkshop(store);
            PrintStoreStock(cw, work, storeStock, custCart, store);
        }

        private void DisplayWorkshops(Store store)
        {
            List<Workshop> workshop = json.LoadWorkshop(store);
            List<CustomerWorkshop> workshop2 = json.LoadCustomerWorkshop(store);
            PrintWorkshops(workshop2, workshop, store);
        }

        private void PrintStoreStock(List <CustomerWorkshop> cw, List<Workshop> work, List<StoreInv> storeStock, List<CustomerCart> custCart, Store store)
        {
            int i;
            int stockCount = storeStock.Count();
            int stop = stockCount - 1;
            bool lastPage = false;
            bool exit = false;
            Console.WriteLine(store.StoreName + " Store");
            Console.WriteLine("ID\tProduct\t\tCurrent Stock");
            Console.WriteLine("__________________________________________________________");

            for ( i = 0; i< stockCount; i++ )
            {
                if(exit==true)
                {
                    break;
                }
                Console.WriteLine("{0}\t{1}\t\t{2}", storeStock[i].ID, storeStock[i].Product, storeStock[i].Quantity);
                
                if ( i == stop)
                {
                    lastPage = true;
                }
                if ( i == 4 || i == 9 || i == 14 || i == 19 || lastPage == true)
                {
                    bool flag = true;
                    while (flag)
                    {
                        Console.WriteLine("\n[Legend: 'P' Next Page | 'R' Return to Menu | 'C' Complete Transaction]\n");
                        Console.Write("Enter Item number to purchase or Function: ");

                        string UserInput = Console.ReadLine();
                        int number;
                        bool result = Int32.TryParse(UserInput, out number);

                        if (result == true)
                        {
                            foreach (StoreInv storeInv in storeStock)
                            {
                                if (number == storeInv.ID && storeInv.Quantity > 0)
                                {
                                    Console.Write("Enter quantity: ");
                                    string Quantity = Console.ReadLine();
                                    int quan;
                                    bool q = Int32.TryParse(Quantity, out quan);
                                    if (q == true)
                                    {
                                        custCart.Add(new CustomerCart() { ID = storeInv.ID, Product = storeInv.Product, Quantity = quan });
                                        json.WriteCustomerCart(custCart);

                                        foreach (CustomerCart cart in custCart)
                                        {
                                            int dif = storeInv.Quantity - cart.Quantity;

                                            if (dif >= 0)
                                            {
                                                storeInv.Quantity = dif;
                                                exit = true;
                                                break;
                                            }
                                        }
                                        json.WriteStoreStock(storeStock, store);

                                        Console.WriteLine("Would you like to purchase another item? (y/n)");
                                        Console.Write("Enter choice: ");
                                        string AnotherOne = Console.ReadLine();
                                        Console.WriteLine();

                                        if (AnotherOne.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            Console.Clear();
                                            DisplayStoreStock(store);
                                            flag = false;
                                            exit = true;
                                        }

                                        else if (AnotherOne.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            Console.Clear();
                                            flag = false;
                                            exit = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Invalid Input\n");
                                        DisplayStoreStock(store);
                                        exit = true;
                                        flag = false;
                                    }
                                }

                                else if (number == storeInv.ID && storeInv.Quantity == 0)
                                {
                                    Console.WriteLine("There is no stock available for this product\n");
                                }
                            }

                            if (number > storeStock.Count)
                            {
                                Console.WriteLine("Product does not exist!\n");
                            }
                        }

                        else if (UserInput.Equals("p", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (!lastPage)
                            {
                                Console.Clear();
                                Console.WriteLine(store.StoreName + " Store");
                                Console.WriteLine("ID\tProduct\t\tCurrent Stock");
                                Console.WriteLine("__________________________________________________________");
                                flag = false;
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Invalid - This is the Last Page");
                            }
                        }

                        else if (UserInput.Equals("r", StringComparison.InvariantCultureIgnoreCase))
                        {
                            Console.Clear();
                            flag = false;
                            exit = true;
                            break;
                        }

                        else if (UserInput.Equals("c", StringComparison.InvariantCultureIgnoreCase))
                        {
                            Console.WriteLine("You have purchased the following items: ");
                            Console.WriteLine("ID\t\tProduct\t\t\tQuantity");
                            Console.WriteLine("__________________________________________________________");

                            bool prodCart = false;
                            bool workCart = false;

                            foreach (CustomerCart c in custCart)
                            {
                                if (custCart.Count > 0)
                                {
                                    Console.WriteLine("{0}\t\t{1}\t\t\t{2}", c.ID, c.Product, c.Quantity);
                                    prodCart = true;
                                }
                            }

                            Console.WriteLine();
                            Console.WriteLine("You have booked in to:");
                            Console.WriteLine("Book ID\t\tName\t\t\tDate");
                            Console.WriteLine("__________________________________________________________");

                            foreach (CustomerWorkshop c in cw)
                            {

                                if (cw.Count > 0)
                                {

                                    Console.WriteLine("{0}\t\t{1}\t{2}", c.BookID, c.Name, c.Date);
                                    Console.WriteLine("\nBooking Ref No.: " + GenerateRandomNo() + "\n");
                                    workCart = true;

                                    {
                                        if (custCart.Count > 0)
                                        {
                                            Console.WriteLine("*****You have a 10% discount on purchased products*****");
                                        }
                                    }
                                }
                            }
                            // checks if product or workshop cart is empty
                            if (prodCart == false && workCart == false)
                            {
                                // if empty, error message and returned to menu
                                Console.Clear();
                                Console.WriteLine("Your Cart is Empty!\n");
                            }

                            // user input to complete transaction
                            Console.WriteLine();
                            Console.WriteLine("Press g to continue");
                            string key = Console.ReadLine();

                            if (key.Equals("g", StringComparison.InvariantCultureIgnoreCase))
                            {
                                flag = false;
                                // emptying customer cart
                                custCart = new List<CustomerCart>();
                                cw = new List<CustomerWorkshop>();

                                json.WriteCustomerCart(custCart);
                                json.AddCustomerWorkshop(cw);

                                Console.Clear();
                                break;
                            }
                        }
                        else
                        {
                            Console.Clear();
                            DisplayStoreStock(store);
                            exit = true;
                        }
                    }
                }
             }
        }
        //++++++++++++++++++++++++++++++++++++++++++++++ Adding Workshops - OPTION 3 ++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void PrintWorkshops(List<CustomerWorkshop> custWorkshop, List<Workshop> workshop, Store store)
        {
            Console.WriteLine("Workshops to book");
            Console.WriteLine("Book ID\tWorkshop Name\t\t\tDate");
            Console.WriteLine("__________________________________________________________");

            foreach (Workshop w in workshop)
            {
                Console.WriteLine("{0}\t{1}\t\t\t{2}", w.BookID, w.Name, w.Date);

            }
            Console.WriteLine();

            bool loop = false;
            while (!loop)
            {
                // create temporary customer workshop list to alter
                List<CustomerWorkshop> tempCustWorkshop = custWorkshop;

                // user input //
                Console.WriteLine("Enter a Book ID to book or x to exit.");
                Console.Write("Input: ");
                string UserInput = Console.ReadLine();
                int number;
                //int parse checking
                bool result = Int32.TryParse(UserInput, out number);
                if (result == true)
                {
                    bool added = false;
                    bool found = false;
                    foreach (Workshop w in workshop)
                    {
                        //search though workshop to find user inputed
                        if (number == w.BookID)
                        {
                            found = true;
                            bool alreadyAdded = false;
                            foreach (CustomerWorkshop cw in custWorkshop)
                            {
                                if (w.BookID == cw.BookID)
                                {
                                    alreadyAdded = true;
                                    break;
                                }
                            }
                            if (!alreadyAdded)
                            {
                                // user input
                                Console.WriteLine("you are about to book into this workshop, continue? y/n");
                                Console.Write("Enter choice: ");
                                string userInput = Console.ReadLine();

                                // user conformation to add workshop
                                if (userInput.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    tempCustWorkshop.Add(new CustomerWorkshop() { BookID = w.BookID, Name = w.Name, Date = w.Date });
                                    added = true;
                                    loop = true;
                                    break;
                                }
                                else if (userInput.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    Console.Clear();
                                    Console.WriteLine("Item was NOT added!\n");
                                    loop = true;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Workshop already in cart!\n");
                            }
                        }
                    }
                    if (!found)
                    {
                        Console.WriteLine("Invalid Input!\n");
                    }
                    if (added == true)
                    {
                        json.AddCustomerWorkshop(tempCustWorkshop);
                        Console.Clear();
                        Console.WriteLine("Item was added!\n");
                    }

                }
                else if (UserInput.Equals("x", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.Clear();
                    loop = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Input\n");
                }
            }
        }

        //This function is for generating the unique booking reference number for the workshop.
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }//end of class

    public class CustomerCart
    {
        public int ID { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
    }

    public class CustomerWorkshop
    {
        public int BookID { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
    }
}


