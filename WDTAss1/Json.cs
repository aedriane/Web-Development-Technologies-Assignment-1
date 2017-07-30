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
    class Json
    {
        //=================== Writing to json files =====================//
        public void WriteStoreStock(List<StoreInv> storeInv, Store store)
        {
            string st = store.StoreName + "_inventory.json";
            File.WriteAllText(st, JsonConvert.SerializeObject(storeInv, Formatting.Indented));
        }
        public void WriteRequests(List<StockRequest> reqs)
        {
            File.WriteAllText("stockrequest.json", JsonConvert.SerializeObject(reqs, Formatting.Indented));
        }
        public void WriteOwnerStock(List<OwnerStock> reqs)
        {
            File.WriteAllText("owners_inventory.json", JsonConvert.SerializeObject(reqs, Formatting.Indented));
        }
        public void WriteCustomerCart(List<CustomerCart> custCart)
        {
            File.WriteAllText("customer.json", JsonConvert.SerializeObject(custCart, Formatting.Indented));
        }
        public void AddCustomerWorkshop(List<CustomerWorkshop> workshop)
        {
            File.WriteAllText("customerWorkshop.json", JsonConvert.SerializeObject(workshop, Formatting.Indented));
        }

        //=================== Reading from json files =====================//
        public List<Store> LoadStoreList()
        {
            return JsonConvert.DeserializeObject<List<Store>>(File.ReadAllText("store.json"));
        }
        public List<StoreInv> LoadStoreStock(Store store)
        {
            string st = store.StoreName + "_inventory.json";
            return JsonConvert.DeserializeObject<List<StoreInv>>(File.ReadAllText(st));
        }
        public List<StockRequest> LoadStockRequests()
        {
            return JsonConvert.DeserializeObject<List<StockRequest>>(File.ReadAllText("stockrequest.json"));
        }
        public List<OwnerStock> LoadOwnerStock()
        {
            return JsonConvert.DeserializeObject<List<OwnerStock>>(File.ReadAllText("owners_inventory.json"));
        }
        public List<CustomerCart> LoadCustomerCart()
        {
            return JsonConvert.DeserializeObject<List<CustomerCart>>(File.ReadAllText("customer.json"));
        }
        public List<Workshop> LoadWorkshop(Store store)
        {
            return JsonConvert.DeserializeObject<List<Workshop>>(File.ReadAllText("workshop.json"));
        }
        public List<CustomerWorkshop> LoadCustomerWorkshop(Store store)
        {
            return JsonConvert.DeserializeObject<List<CustomerWorkshop>>(File.ReadAllText("customerWorkshop.json"));
        }
    }
}
