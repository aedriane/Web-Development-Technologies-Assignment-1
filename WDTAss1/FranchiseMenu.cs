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
    // FranchiseMenu inheriting abstract Menu through interface
    class FranchiseMenu : Menu
    {
        // Json Utility //
        Json json = new Json();
        // OPTION 2 OF MAIN MENU - FRANCHISE MENU
        
        // Set options and title for Franchise Menu
        public FranchiseMenu()
        {
            Options.Add("Display Inventory");
            Options.Add("Display Inventory (Threshold)");
            Options.Add("Add New Inventory Item");
            Options.Add("Return to Main Menu");
            Options.Add("Quit");
            Title = "Franchise Menu";
        }
        
        // Printing and Running Menu //
        public override void PrintMenu()
        {
            MainMenu main = new MainMenu();

            // STORE SELECTION //
            Store store;
            store = main.RequestStore();

            bool loop = true;
            while (loop)
            {
                // Printing Menu
                Console.WriteLine("Welcome");
                Console.WriteLine(Title + " " + store.StoreName + "\n----------------------------");

                for (int i = 0, j = 1; i < Options.Count; j++, i++)
                {
                    Console.WriteLine("\t" + j + ". " + Options[i] + "\n");
                }

                // Functionality of Franchise Menu
                // Enter Menu Option
                Console.Write("Enter Option: ");
                string menu = Console.ReadLine();

                // int checking
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

                // Switch/Case Franchise Menu
                switch (iSel)
                {
                    case 1:
                        // OPTION 1 - Display all store inventory stock
                        Console.Clear();
                        displayAllStoreInv(store);
                        break;

                    case 2:
                        // OPTION 2 - Display store inventory stock filtered
                        Console.Clear();
                        displayThreshold(store);
                        break;

                    case 3:
                        // OPTION 3 - Add New Item to Store Inventory
                        Console.Clear();
                        addNewItem(store);
                        break;

                    case 4:
                        // OPTION 4 - Returns to the main menu
                        Console.Clear();
                        return;

                    case 5:
                        // OPTION 5 - Quit program
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
        private void displayAllStoreInv(Store store)
        {
            List<StockRequest> stockRequest = json.LoadStockRequests();
            List<StoreInv> storeInv = json.LoadStoreStock(store);
            PrintAllStoreInv(stockRequest, storeInv, store);
        }
        // Display Filtered Inventory Items, if Products needed - OPTION 2
        private void displayThreshold(Store store)
        {
            List<StockRequest> stockRequest = json.LoadStockRequests();
            List<StoreInv> storeInv = json.LoadStoreStock(store);
            GetThreshold(stockRequest, storeInv, store);
        }
        // New product to store_inventory - OPTION 3
        private void addNewItem(Store store)
        {
            List<OwnerStock> ownerInv = json.LoadOwnerStock();
            List<StoreInv> storeInv = json.LoadStoreStock(store);
            List<StockRequest> stockRequest = json.LoadStockRequests();
            PrintItems(ownerInv, storeInv, store, stockRequest);
        }



        //=================== Menu Functionality/Methods =====================//
        //+++++++++++++++++++++++++++++++++++++ Show All Store Stock - OPTION 1 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        public void PrintAllStoreInv(List<StockRequest> stockRequest, List<StoreInv> storeInv, Store store)
        {
            bool flag = true;
            while (flag)
            {
                // user input //
                Console.Write("Enter Threshold Number: ");
                string thresh = Console.ReadLine();
                int threshNum;
                // in parse error handling //
                bool result = Int32.TryParse(thresh, out threshNum);
                if (!result)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Input!\n");
                }
                else
                {
                    // Threshold Number must be above 0 or else why have it //
                    if (threshNum > 0)
                    {
                        flag = false;
                        Console.Clear();
                        // Print the stock
                        PrintStoreStock(storeInv, stockRequest, store, threshNum);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid Input!\n");
                    }
                }
            }
        }
        //+++++++++++++++++++++++++++++++++++++ Print Store Stock - OPTION 1 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void PrintStoreStock(List<StoreInv> storeStock, List<StockRequest> stockRequest, Store store, int threshNum)
        {
            bool reStock;
            Console.WriteLine("Threshold Number: {0}\n", threshNum);
            Console.WriteLine("Stock Requests");
            Console.WriteLine("ID\tProduct\t\tCurrent Stock\tRe-Stock");
            Console.WriteLine("__________________________________________________________");

            foreach (StoreInv storeInv in storeStock)
            {
                if (threshNum >= storeInv.Quantity)
                    reStock = true;
                else
                    reStock = false;
                Console.WriteLine("{0}\t{1}\t\t{2}\t\t{3}", storeInv.ID, storeInv.Product, storeInv.Quantity, reStock);
            }
            // user choosing which product to request more stock
            RequestSend(stockRequest, storeStock, store, threshNum);
        }


        //+++++++++++++++++++++++++++++++++++++ Get Threshold Number & Filter if High or Low - OPTION 2 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void GetThreshold(List<StockRequest> stockRequest, List<StoreInv> storeInv, Store store)
        {

            bool flag = true;
            while (flag)
            {
                Console.Write("Enter Threshold Number: ");
                string thresh = Console.ReadLine();
                int threshNum;
                // int error handling //
                bool result = Int32.TryParse(thresh, out threshNum);
                if (!result)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Input\n");
                }
                else
                {
                    if (threshNum > 0)
                    {
                        Console.Clear();
                        while (flag)
                        {
                            // high and low threshold
                            Console.Write("Enter High or Low: ");
                            string select = Console.ReadLine();
                            if (select.Equals("high", StringComparison.InvariantCultureIgnoreCase) || select.Equals("h", StringComparison.InvariantCultureIgnoreCase) || select.Equals("low", StringComparison.InvariantCultureIgnoreCase) || select.Equals("l", StringComparison.InvariantCultureIgnoreCase))
                            {
                                bool low;
                                if (select.Equals("high", StringComparison.InvariantCultureIgnoreCase) || select.Equals("h", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    low = false;
                                }
                                else
                                {
                                    low = true;
                                }
                                flag = false;
                                Console.Clear();
                                PrintThreshold(storeInv, stockRequest, store, threshNum, low);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Invalid Input\n");
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid Input!\n");
                    }
                }
            }
        }

        //+++++++++++++++++++++++++++++++++++++ Printing Threshold Based on Number & Filter Input - OPTION 2 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void PrintThreshold(List<StoreInv> storeStock, List<StockRequest> stockRequest, Store store, int threshNum, bool low)
        {
            bool reStock;
            Console.WriteLine("Threshold Number: {0}\n", threshNum);
            Console.WriteLine("Stock Requests");
            Console.WriteLine("ID\tProduct\t\tCurrent Stock\tRe-Stock");
            Console.WriteLine("__________________________________________________________");

            foreach (StoreInv storeInv in storeStock)
            {
                // setting if restock is true or false based on threshold number input
                if (threshNum > storeInv.Quantity)
                    reStock = true;
                else
                    reStock = false;

                // printing products based on is user selected High or Low
                if (reStock == low)
                    Console.WriteLine("{0}\t{1}\t\t{2}\t\t{3}", storeInv.ID, storeInv.Product, storeInv.Quantity, reStock);
            }
            // user choosing which product to request more stock
            RequestSend(stockRequest, storeStock, store, threshNum);
        }


        //+++++++++++++++++++++++++++++++++++++ Adding New Product to Store Inventory - OPTION 3 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void PrintItems(List<OwnerStock> ownerInv, List<StoreInv> storeInv, Store store, List<StockRequest> stockRequest)
        {
            Console.WriteLine();
            Console.WriteLine("ID\tProduct\t\tCurrent Stock");
            Console.WriteLine("_____________________________________");
            List<OwnerStock> tempList = new List<OwnerStock>();
               
            foreach (OwnerStock ownerStock in ownerInv)
            {
                bool found = false;
                foreach (StoreInv storeStock in storeInv)
                {
                    if (ownerStock.Product == storeStock.Product)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("{0}\t{1}\t\t{2}", ownerStock.ID, ownerStock.Product, ownerStock.StockLevel);
                }
            }
            
            
            Console.WriteLine();
            bool flag = true;
            while (flag)
            {
                // user input //
                Console.WriteLine("Enter Product ID to Add Product to Store or 'quit' to Return to Store Menu: ");
                string sID = Console.ReadLine();
                if (sID.Equals("quit", StringComparison.InvariantCultureIgnoreCase) || sID.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                {
                    flag = false;
                    Console.Clear();
                }
                else
                {
                    int id;
                    // error handle int converstion
                    bool result = Int32.TryParse(sID, out id);
                    if (!result)
                    {
                        Console.WriteLine("Invalid Input!\n");
                    }
                    else
                    {
                        int count;
                        bool idMatch = false;
                        bool notFound = false;
                        // temporary lists which alter json files
                        List<StockRequest> stockReqs = stockRequest;
                        List<StoreInv> tempStoreInv = storeInv;

                        foreach (OwnerStock ownerStock in ownerInv)
                        {
                            // checking user input ID matches owner_inventory IDs
                            if (id == ownerStock.ID)
                            {
                                idMatch = true;
                                // checking user selected owner_inventory product is not in chosen store_inventory
                                foreach (StoreInv storeStock in storeInv)
                                {
                                    if (storeStock.Product == ownerStock.Product)
                                    {
                                        notFound = true;
                                        break;
                                    }
                                }
                                // if product is not in store_inventory
                                if (!notFound)
                                {
                                    // add product to store_inventory with no Quantity
                                    tempStoreInv.Add(new StoreInv() { ID = ownerStock.ID, Product = ownerStock.Product, Quantity = 0 });
                                    json.WriteStoreStock(tempStoreInv, store);

                                    // gets last request's ID and increments to give new stock_request ID
                                    StockRequest lastRequest = stockRequest[stockRequest.Count() - 1];
                                    count = lastRequest.ID;
                                    count++;

                                    // add request to give Quantity of 3 of newly added product to store_inventory
                                    // to be approved by Owner
                                    stockReqs.Add(new StockRequest() { ID = count, StoreName = store.StoreName, Product = ownerStock.Product, Quantity = 3 });
                                    json.WriteRequests(stockReqs);

                                    // return to Franchise menu with success message
                                    flag = false;
                                    Console.Clear();
                                    Console.WriteLine("Item Added\n");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Input\n");
                                    break;
                                }
                            }
                        }
                        if (!idMatch)
                        {
                            Console.WriteLine("Invalid Input!\n");
                        }
                    }
                }
            }
        }


        //+++++++++++++++++++++++++++++++++++++ Stock Request Checking - OPTION 2 & 3 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void RequestSend( List<StockRequest> stockRequest, List<StoreInv> storeStock, Store store, int threshNum)
        {
            Console.WriteLine();
            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Enter ID to Add to Store Inventory or 'quit' to Return to Store Menu: ");
                string sID = Console.ReadLine();
                //quit functioning
                if (sID.Equals("quit", StringComparison.InvariantCultureIgnoreCase) || sID.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                {
                    loop = false;
                    Console.Clear();
                }
                else
                {
                    int id;
                    //int checking
                    bool result = Int32.TryParse(sID, out id); ;
                    if (!result)
                    {
                        Console.WriteLine("Invalid Input\n");
                    }
                    else
                    {
                        // gets last request's ID and increments to give new stock_request ID
                        int idcount;
                        StockRequest lastRequest = stockRequest[stockRequest.Count() - 1];
                        idcount = lastRequest.ID;
                        idcount++;

                        bool idFound = false;
                        foreach (StoreInv storeInv in storeStock)
                        {
                            //check user ID matches a store_inventory ID
                            if (id == storeInv.ID)
                            {
                                idFound = true;
                                //checks if enough stock, if so warn user
                                if (threshNum <= storeInv.Quantity)
                                {
                                    while (loop)
                                    {
                                        Console.WriteLine("WARNING: store has enough stock, do you wish to continue? (yes/no)");
                                        string cont = Console.ReadLine();

                                        //user wishes to continue even with enough stock available
                                        if (cont.Equals("yes", StringComparison.InvariantCultureIgnoreCase) || cont.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            while (loop)
                                            {
                                                //stock request will sent - functionality
                                                loop = SentRequest(stockRequest, storeInv, store, idcount);
                                            }
                                        }
                                        else if (cont.Equals("no", StringComparison.InvariantCultureIgnoreCase) || cont.Equals("n", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            loop = false;
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid Input\n");
                                        }
                                    }
                                }
                                else
                                {
                                    //stock request will sent - functionality
                                    loop = SentRequest(stockRequest, storeInv, store, idcount);
                                }
                            }
                        }
                        if (!idFound)
                        {
                            Console.WriteLine("Invalid Input!\n");
                        }
                    }
                }
            }
        }

        //+++++++++++++++++++++++++++++++++++++ Stock Request Sending - OPTION 2 & 3 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private bool SentRequest(List<StockRequest> stockRequest, StoreInv storeInv, Store store, int idcount)
        {
            //user input//
            Console.WriteLine("Enter quantity: ");
            string reqQuan = Console.ReadLine();

            //int checking//
            int qu;
            bool intQuantity = Int32.TryParse(reqQuan, out qu);
            if (!intQuantity)
            {
                Console.WriteLine("Invalid Input\n");
                return true;
            }
            else
            {
                // Quantity must be above 0 //
                if (qu > 0)
                {
                    // Stock request adjusting json file
                    stockRequest.Add(new StockRequest() { ID = idcount, StoreName = store.StoreName, Product = storeInv.Product, Quantity = qu });
                    json.WriteRequests(stockRequest);
                    Console.Clear();
                    Console.WriteLine("Item Requested!\n");
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid Input!\n");
                    return true;
                }
            }
        }
    }//class ends here
 

    // SEPERATE CLASS - Store Inventory Properties //
    public class StoreInv
    {
        public int ID { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
    }
}


