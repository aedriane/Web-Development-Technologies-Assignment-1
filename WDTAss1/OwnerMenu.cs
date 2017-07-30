using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

//----------------- Group 22 -----------------//
//--------- s3494336 - Jackson Lloyd  ---------//
//--------- s3541804 - Aedriane Hernan ---------//

namespace WDTAss1
{
    // OwnerMenu inheriting abstract Menu through interface
    class OwnerMenu : Menu
    {
        // Json Utility //
        Json json = new Json();
        // OPTION 1 OF MAIN MENU - OWNER MENU

        // Set options and title for Owner Menu
        public OwnerMenu()
        {
            Options.Add("Display All Stock Requests");
            Options.Add("Display Stock Requests (True/False)");
            Options.Add("Display All Product Lines");
            Options.Add("Return to Main Menu");
            Options.Add("Quit");
            Title = "Owner Menu";
        }

        // Printing and Running Menu //
        public override void PrintMenu()
        {
            bool loop = true;
            while (loop)
            {
                // Printing Menu
                Console.WriteLine("Welcome");
                Console.WriteLine(Title + "\n----------------------------");

                for (int i = 0, j = 1; i < Options.Count; j++, i++)
                {
                    Console.WriteLine("\t" + j + ". " + Options[i] + "\n");
                }

                // Functionality of Owner Menu
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

                // Switch/Case Owner Menu
                switch (iSel)
                {
                    case 1:
                        Console.Clear();
                        // OPTION 1 - Display All Stock Requests
                        displayStockRequests();
                        break;

                    case 2:
                        // OPTION 2 - Display Filtered Stock Requests
                        Console.Clear();
                        displayStockRequestsTF();
                        break;

                    case 3:
                        // OPTION 3 - Display all owner_inventory stock
                        Console.Clear();
                        displayAllStock();
                        break;

                    case 4:
                        // OPTION 4 - Returns to the main menu
                        Console.Clear();
                        return;

                    case 5:
                        // OPTION 5 - Exit Program
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
        // Display All Stock Requests - OPTION 1
        private void displayStockRequests()
        {
            List<StockRequest> stockRequest = json.LoadStockRequests();
            List<OwnerStock> ownerStock = json.LoadOwnerStock();
            PrintRequests(stockRequest, ownerStock);
        }
        // Display Filtered Stock Requests - OPTION 2
        private void displayStockRequestsTF()
        {
            List<StockRequest> stockRequest = json.LoadStockRequests();
            List<OwnerStock> ownerStock = json.LoadOwnerStock();
            PrintRequestsTF(stockRequest, ownerStock);
        }
        // Display All Owner Stock - OPTION 3
        private void displayAllStock()
        {
            List<OwnerStock> ownerStock = json.LoadOwnerStock();
            PrintOwnerInv(ownerStock);
        }



        //=================== Menu Functionality/Methods =====================//
        //+++++++++++++++++++++++++++++++++++++ Show All Stock Requests - OPTION 1 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void PrintRequests(List<StockRequest> stockreqs, List<OwnerStock> ownerStock)
        {
            bool available;
            Console.WriteLine("Stock Requests");
            Console.WriteLine("ID\tStore\tProduct\t\tQuantity\tCurrent Stock\tStock Availability");
            Console.WriteLine("___________________________________________________________________________________");

            foreach (StockRequest stockreq in stockreqs)
            {
                foreach (OwnerStock ownInv in ownerStock)
                {
                    if (ownInv.Product == stockreq.Product)
                    {
                        // checks enough stock if enough available to fulfill request
                        if (ownInv.StockLevel >= stockreq.Quantity)
                            available = true;
                        else
                            available = false;
                        // prints all stock requests
                        Console.WriteLine("{0}\t{1}\t{2}\t\t{3}\t\t{4}\t\t{5}", stockreq.ID, stockreq.StoreName, stockreq.Product, stockreq.Quantity, ownInv.StockLevel, available);
                    }
                }
            }
            // Deal with processing requests
            RequestProcessing(stockreqs, ownerStock);
        }

        //+++++++++++++++++++++++++++++++++++++ Sorts User Input to Ability to Complete Request - OPTION 2 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void PrintRequestsTF(List<StockRequest> stockRequest, List<OwnerStock> ownerStock)
        {
            bool ava;
            bool flag = true;
            while (flag)
            {
                // user input
                Console.Write("Enter True or False: ");
                string tf = Console.ReadLine();
               
                //check if user input is ture or false
                if (tf.Equals("true", StringComparison.InvariantCultureIgnoreCase) || tf.Equals("t", StringComparison.InvariantCultureIgnoreCase))
                {
                    ava = true;
                    // show requests that can be processed
                    PrintRequestsTF(stockRequest, ownerStock, ava);
                    flag = false;
                }
                else if (tf.Equals("false", StringComparison.InvariantCultureIgnoreCase) || tf.Equals("f", StringComparison.InvariantCultureIgnoreCase))
                {
                    ava = false;
                    // show requests that cannot be processed
                    PrintRequestsTF(stockRequest, ownerStock, ava);
                    flag = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid Input!\n");
                }
            }
        }

        //+++++++++++++++++++++++++++++++++++++ Show Stock Requests based on - OPTION 2 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void PrintRequestsTF(List<StockRequest> stockreqs, List<OwnerStock> ownerStock, bool ava)
        {
            int stockLevel;
            bool available;
            Console.WriteLine("Stock Requests");
            Console.WriteLine("ID\tStore\tProduct\t\tQuantity\tCurrent Stock\tStock Availability");
            Console.WriteLine("___________________________________________________________________________________");

            foreach (StockRequest stockreq in stockreqs)
            {
                foreach (OwnerStock ownInv in ownerStock)
                {
                    if (ownInv.Product == stockreq.Product)
                    {
                        // sorts user input
                        stockLevel = ownInv.StockLevel;
                        if (stockLevel >= stockreq.Quantity)
                            available = true;
                        else
                            available = false;

                        // prints user selected requests
                        if (ava == available)
                            Console.WriteLine("{0}\t{1}\t{2}\t\t{3}\t\t{4}\t\t{5}", stockreq.ID, stockreq.StoreName, stockreq.Product, stockreq.Quantity, stockLevel, available);
                    }
                }
            }
            // Deal with processing requests
            RequestProcessing(stockreqs, ownerStock);
        }

        //+++++++++++++++++++++++++++++++++++++ Display All Owner's Stock - OPTION 3 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void PrintOwnerInv(List<OwnerStock> ownerStock)
        {
            Console.WriteLine("Owner's Inventory");
            Console.WriteLine();
            Console.WriteLine("ID\tProduct\t\tCurrent Stock");
            Console.WriteLine("_____________________________________");
            
            // Display owner's stock
            foreach (OwnerStock ownInv in ownerStock)
            {
                Console.WriteLine("{0}\t{1}\t\t{2}", ownInv.ID, ownInv.Product, ownInv.StockLevel);
            }

            // Waits till enter is hit before Returning to Owner Menu
            Console.WriteLine("\nPress Enter to Return to Owner Menu...");
            string enter = Console.ReadLine();
            Console.Clear();
        }


        //+++++++++++++++++++++++++++++++++++++ Processing Stock Requests - OPTION 1 & 2 ++++++++++++++++++++++++++++++++++++++++++++++++++++//
        private void RequestProcessing(List<StockRequest> stockreqs, List<OwnerStock> ownerStock)
        {
            Console.WriteLine();

            bool flag = true;
            while (flag)
            {
                // Getting user input
                Console.WriteLine("Enter ID of Request to process or 'quit' to Return to Owner Menu: ");
                string sID = Console.ReadLine();

                // check if quitting
                if (sID.Equals("quit", StringComparison.InvariantCultureIgnoreCase) || sID.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                {
                    flag = false;
                    Console.Clear();
                }
                else
                {
                    int id;
                    int dif;
                    bool found = false;
                    
                    // create temport stock request list to alter
                    List<StockRequest> tempReqs = stockreqs;

                    // int error checking
                    bool result = Int32.TryParse(sID, out id);
                    if (!result)
                    {
                        Console.WriteLine("Invalid Input\n");
                    }
                    else
                    {
                        foreach (StockRequest stockreq in stockreqs)
                        {
                            // check user ID matches a stock Request ID
                            if (id == stockreq.ID)
                            {
                                found = true;
                                bool ownerChanged = false;
                                bool storeChanged = false;

                                foreach (OwnerStock ownInv in ownerStock)
                                {
                                    // check stock product matches owner product
                                    if (stockreq.Product == ownInv.Product)
                                    {
                                        dif = ownInv.StockLevel - stockreq.Quantity;
                                        // difference has to be greater than 1
                                        if (dif >= 0)
                                        {
                                            // change owner's product stock level based on request
                                            ownInv.StockLevel = dif;
                                            ownerChanged = true;
                                            break;
                                        }
                                    }
                                }

                                // get store based on stock request
                                Store store = new Store();
                                store.StoreName = stockreq.StoreName;
                                List<StoreInv> storeInv = json.LoadStoreStock(store);

                                foreach (StoreInv storeStock in storeInv)
                                {
                                    // search through store products to find stock request product
                                    if (stockreq.Product == storeStock.Product)
                                    {
                                        dif = storeStock.Quantity + stockreq.Quantity;
                                        // difference has to be greater than 1
                                        if (dif >= 0)
                                        {
                                            // change store's product stock level based on request
                                            storeStock.Quantity = dif;
                                            storeChanged = true;
                                            break;
                                        }
                                    }
                                }
                                if (ownerChanged == true && storeChanged == true)
                                {
                                    // altering json files according to previous
                                    json.WriteOwnerStock(ownerStock);
                                    json.WriteStoreStock(storeInv, store);
                                }
                            }
                            if (found == true)
                            {
                                // removes request from tempory stock request list
                                tempReqs.Remove(stockreq);
                                break;
                            }
                        }
                        if (!found)
                        {
                            Console.WriteLine("Invalid Input!\n");
                        }
                        else
                        {
                            // changes stock request json to remove processed request
                            // returns to Owner Menu with success message
                            json.WriteRequests(tempReqs);
                            flag = false;
                            Console.Clear();
                            Console.WriteLine("Request Completed!\n");
                        }
                    }
                }
            }
        }
    }//class ends here


    // SEPERATE CLASS - Store Request Properties //
    public class StockRequest
    {
        public int ID { get; set; }
        public string StoreName { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
    }//class ends here
    // SEPERATE CLASS - Owner Stock Properties //
    public class OwnerStock
    {
        public int ID { get; set; }
        public string Product { get; set; }
        public int StockLevel { get; set; }
    }//class ends here
}

