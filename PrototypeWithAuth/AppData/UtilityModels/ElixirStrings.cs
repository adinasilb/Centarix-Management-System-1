using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public static class ElixirStrings
    {
        public const String ServerSideError = "Cannot complete this action.";
        public const String DifferentCurrencyErrorMessage = "The items selected have different currency types.";
        public const String DifferentVendorErrorMessage = "The items selected have different Vendors.";
        public const String ExistingInvoiceNumberVendorErrorMessage = "This invoice number already exists for the chosen vendor";
        public const String MissingFileErrorMessage = "Requred file was not uploaded";
        public const String ServerDifferentCurrencyErrorMessage = ServerSideError + DifferentCurrencyErrorMessage;
        public const String ServerDifferentVendorErrorMessage = ServerSideError + DifferentVendorErrorMessage;
        public const String ServerExistingInvoiceNumberVendorErrorMessage = ServerSideError + ExistingInvoiceNumberVendorErrorMessage;
        public const String ServerMissingFile = ServerSideError + MissingFileErrorMessage;
    }
}
