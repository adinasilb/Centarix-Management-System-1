using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public static class AppUtility
    {

        public static Request CheckRequestForNullsAndReplace(Request request)
        {
            request.LocationID = ReplaceIntValueIfNull(request.LocationID);
            request.AmountWithInLocation = ReplaceIntValueIfNull(request.AmountWithInLocation);
            request.AmountWithOutLocation = ReplaceIntValueIfNull(request.AmountWithOutLocation);
            request.OrderNumber = ReplaceIntValueIfNull(request.OrderNumber);
            request.Quantity = ReplaceIntValueIfNull(request.Quantity);
            request.InvoiceNumber = ReplaceStringValueIfNull(request.InvoiceNumber);
            request.CatalogNumber = ReplaceStringValueIfNull(request.CatalogNumber);
            request.SerialNumber = ReplaceStringValueIfNull(request.SerialNumber);
            request.URL = ReplaceStringValueIfNull(request.URL);
            return request;
        }

        public static Product CheckProductForNullsAndReplace(Product product)
        {
            product.ProductName = ReplaceStringValueIfNull(product.ProductName);
            product.LocationID = ReplaceIntValueIfNull(product.LocationID);
            product.Handeling = ReplaceStringValueIfNull(product.Handeling);
            product.QuantityPerUnit = ReplaceIntValueIfNull(product.QuantityPerUnit);
            product.UnitsInStock = ReplaceIntValueIfNull(product.UnitsInStock);
            product.UnitsInOrder = ReplaceIntValueIfNull(product.UnitsInOrder);
            product.ReorderLevel = ReplaceIntValueIfNull(product.ReorderLevel);
            product.ProductComment = ReplaceStringValueIfNull(product.ProductComment);
            product.ProductMedia = ReplaceStringValueIfNull(product.ProductMedia);
            return product;
        }

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
