using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    [Flags]
    public enum Genre
    {
        Novel = 0b000001,
        Comedy = 0b000010,
        Romantic = 0b000100,
        Horror = 0b001000,
        ScienceFiction = 0b010000,
        Youth = 0b100000,
    };
}
