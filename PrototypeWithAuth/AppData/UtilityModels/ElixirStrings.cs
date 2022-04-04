using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public static class ElixirStrings
    {
        //ERRORS
        public const String ServerSideError = "Cannot complete this action.";
        public const String DifferentCurrencyErrorMessage = "The items selected have different currency types.";
        public const String DifferentVendorErrorMessage = "The items selected have different Vendors.";
        public const String ExistingInvoiceNumberVendorErrorMessage = "This invoice number already exists for the chosen vendor";
        public const String MissingFileErrorMessage = "Required file was not uploaded";
        public const String NegativePriceErrorMessage = "Price cannot be negative.";
        public const String ServerDifferentCurrencyErrorMessage = ServerSideError + DifferentCurrencyErrorMessage;
        public const String ServerNegativePriceErrorMessage = ServerSideError + NegativePriceErrorMessage;
        public const String ServerDifferentVendorErrorMessage = ServerSideError + DifferentVendorErrorMessage;
        public const String ServerExistingInvoiceNumberVendorErrorMessage = ServerSideError + ExistingInvoiceNumberVendorErrorMessage;
        public const String ServerMissingFile = ServerSideError + MissingFileErrorMessage;
        public const String DeleteModalRoute = "/DeleteModal";

        //Settings Values
        public const string IntPlaceholder = "000000";
        public const string SmallIntPlaceholder = "00";
        public const string UrlPlaceholder = "Paste Link Here";
        public const string ExpectedSupplyDate = "Date Selector";
    }
}
