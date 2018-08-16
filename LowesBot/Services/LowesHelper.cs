using System;
using System.Text.RegularExpressions;

namespace LowesBot.Dialogs
{
    public static class LowesHelper
    {
        public static bool IsValidOrderId(string number) => Regex.IsMatch(number, "\\d{,9}");
        public static bool IsValidPurchaseOrder(string number) => Regex.IsMatch(number, "\\d{,8}");
        public static bool IsValidInvoiceId(string number) => Regex.IsMatch(number, "\\d{,5}");

        internal static bool IsValidOrderId(object id)
        {
            throw new NotImplementedException();
        }
    }
}