using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Dharma.Helper
{
    public static class CustomFuctions
    {
        public static int GetPaymentCount(DateTime start, DateTime end)
        {
            var count = (end.Month + end.Year * 12) - (start.Month + start.Year * 12);
            return count;
        }
    }
}
