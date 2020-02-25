using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public static class AppUtility
    {
        public static int ReplaceIntValueIfNull(int? value)
        {
            int iReturn = 0;
            if (value != null)
            {
                iReturn = (int)value;
            }
            return iReturn;
        }

        public static string ReplaceStringValueIfNull(string value)
        {
            string sReturn = "";
            if (value != null)
            {
                sReturn = value;
            }
            return sReturn;
        }
    }
}
