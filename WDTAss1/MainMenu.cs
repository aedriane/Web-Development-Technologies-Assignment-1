using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

//----------------- Group 22 -----------------//
//--------- s3494336 - Jackson Lloyd  ---------//
//--------- s3541804 - Aedriane Heran ---------//

namespace WDTAss1
{
    class MainMenu : Menu
    {
        Json json = new Json();
        OwnerMenu owner = new OwnerMenu();
        FranchiseMenu franchise = new FranchiseMenu();
        CustomerMenu customer = new CustomerMenu();

        // Set options and title for Main Menu
        public MainMenu()
        {
            Options.Add("Owner");
            Options.Add("Franchise Owner");
            Options.Add("Customer");
            Options.Add("Quit");
            Title = "Main menu";
        }

        public override void PrintMenu()
        {
            bool loop = true;
            while (loop)
            {
                // Display Menu
                Console.WriteLine("Welcome");
                Console.WriteLine(Title + "\n----------------------------");

                for (int i = 0, j = 1; i < Options.Count; j++, i++)
                {
                    Console.WriteLine("\t" + j + ". " + Options[i] + "\n");
                }

                // user input for Main Menu
                Console.Write("Enter Option: ");
                string menu = Console.ReadLine();

                // int parse error handling
                int iSel;
                try
                {
                    iSel = int.Parse(menu);
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Input!\n");
                    continue;
                }

                switch (iSel)
                {
                    case 1:
                        // OPTION 1 - Display Owner Menu
                        Console.Clear();
                        owner.PrintMenu();
                        break;

                    case 2:
                        // OPTION 2 - Display Franchise Menu
                        Console.Clear();
                        franchise.PrintMenu();
                        break;

                    case 3:
                        // OPTION 3 - Display Customer Menu
                        Console.Clear();
                        customer.PrintMenu();
                        break;

                    case 4:
                        // OPTION 4 - Quit Program
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

        // Setting store - OPTIONS 2 & 3
        public Store RequestStore()
        {
            Store store = new Store();
            Console.Clear();
            while (true)
            {
                // Display List of Stores
                Console.WriteLine("Choose a store:\n");
                List<Store> stores = json.LoadStoreList();
                for (int i = 0; i < stores.Count(); i++)
                    Console.WriteLine(i + 1 + ". " + stores[i].StoreName);

                // user input for store
                Console.WriteLine("\nEnter a Store Number: (1-5)");
                string storeNum = Console.ReadLine();

                // int parse checking
                int iStore;
                try
                {
                    iStore = Int32.Parse(storeNum);
                    // check input
                    if(iStore < 1 || iStore > 5)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid Input!\n");
                    }
                    else
                    {
                        Console.Clear();
                        store.StoreSet(iStore);
                        return store;
                    }
                }

                catch (FormatException)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Input!\n");
                    continue;
                }
            }
        }
    }
}
