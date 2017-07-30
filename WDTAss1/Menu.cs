using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//----------------- Group 22 -----------------//
//--------- s3494336 - Jackson Lloyd  ---------//
//--------- s3541804 - Aedriane Hernan ---------//

namespace WDTAss1
{
    // Menus Abstract + Interface
    interface IMenu
    {
        void PrintMenu();
    }
    abstract class Menu : IMenu
    {
        public string Title;

        public List<String> Options = new List<String>();

        public abstract void PrintMenu();
    } 
}
