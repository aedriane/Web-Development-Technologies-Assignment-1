using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//----------------- Group 22 -----------------//
//--------- s3494336 - Jackson Lloyd  ---------//
//--------- s3541804 - Aedriane Heran ---------//

namespace WDTAss1
{
    class Store
    {
        public int ID { get; set; }
        public string StoreName { get; set; }

        public void StoreSet(int storeID)
        {
            if (storeID == 1)
            {
                StoreName = "cbd";
            }
            else if (storeID == 2)
            {
                StoreName = "north";
            }
            else if (storeID == 3)
            {
                StoreName = "east";
            }
            else if (storeID == 4)
            {
                StoreName = "south";
            }
            else if (storeID == 5)
            {
                StoreName = "west";
            }
        }
    }
}
