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

        public enum RequestPageTypeEnum { None, Request, Inventory }

        //public static Request CheckRequestForNullsAndReplace(Request request)
        //{
        //    request.LocationID = ReplaceIntValueIfNull(request.LocationID);
        //    request.AmountWithInLocation = ReplaceIntValueIfNull(request.AmountWithInLocation);
        //    request.AmountWithOutLocation = ReplaceIntValueIfNull(request.AmountWithOutLocation);
        //    ParentRequest.OrderNumber = ReplaceIntValueIfNull(request.OrderNumber);
        //    request.Quantity = ReplaceIntValueIfNull(request.Quantity);
        //    request.InvoiceNumber = ReplaceStringValueIfNull(request.InvoiceNumber);
        //    request.CatalogNumber = ReplaceStringValueIfNull(request.CatalogNumber);
        //    request.SerialNumber = ReplaceStringValueIfNull(request.SerialNumber);
        //    request.URL = ReplaceStringValueIfNull(request.URL);
        //    return request;
        //}

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

        //this checks if a list is empty
        //right now used in the requestscontroller -> index
        public static Boolean IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true; // or throw an exception
            return !source.Any();
        }

        //combines two lists first checking if one is empty so it doesn't get an error
        public static IQueryable<Request> CombineTwoRequestsLists(IQueryable<Request> RequestListToCheck, IQueryable<Request> FullRequestList)
        {
            IQueryable<Request> ReturnList = null;
            if (!RequestListToCheck.IsEmpty())
            {
                ReturnList = RequestListToCheck.Concat(FullRequestList);
            }
            else
            {
                ReturnList = FullRequestList;
            }
            return ReturnList;
        }
    }
}
