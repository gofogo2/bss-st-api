using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bss_st_api.Helpers
{
    public static class UtilHelper
    {
        public static bool diff(System.Drawing.Color b, System.Drawing.Color a)
        {
            if (b.R == a.R && b.G == a.G && b.B == a.B)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
