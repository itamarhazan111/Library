using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Sale
    {
        public string Parameter { get; private set; }
        public string ParamValue { get; private set; }
        public float Discount { get; private set; }

        public Sale(string param, string paramVal, float disc)
        {
            Parameter = param;
            if (param == "Genre")
                paramVal = $"%{paramVal}%";
            ParamValue = paramVal;
            Discount = disc;
        }
    }
}
