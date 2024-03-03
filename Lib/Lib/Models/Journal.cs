using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    internal class Journal : BaseItem
    {
        public Journal(string name,string desc ,string publisher, DateTime publish_date, float price, float discount, Genre genre)
            : base(name,desc,publisher, publish_date, price,discount, genre)
        {
        }
    }
}
