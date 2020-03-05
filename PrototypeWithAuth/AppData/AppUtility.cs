﻿using PrototypeWithAuth.Models;
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
        public enum PaymentPageTypeEnum { None, Notifications, General, Expenses} //these are all going to the ParentRequestIndex

        public static int GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryID(IQueryable<Request> RequestsList, int RequestStatusID, int VendorID = 0, int? SubcategoryID = 0)
        {
            int ReturnList = 0;
            if (VendorID > 0)
            {
                ReturnList = RequestsList
                    .Where(r => r.RequestStatusID == RequestStatusID)
                    .Where(r => r.Product.VendorID == VendorID)
                    .Count();
            }
            else if (SubcategoryID > 0)
            {
                ReturnList = RequestsList
                    .Where(r => r.RequestStatusID == RequestStatusID)
                    .Where(r => r.Product.ProductSubcategoryID == SubcategoryID)
                    .Count();
            }
            else
            {
                ReturnList = RequestsList.Where(r => r.RequestStatusID == RequestStatusID).Count();
            }
            return ReturnList;
        }

        public static IQueryable<Request> GetRequestsListFromRequestStatusID(IQueryable<Request> FullRequestList, int RequestStatusID, int AmountToTake = 0)
        {
            IQueryable<Request> ReturnList = Enumerable.Empty<Request>().AsQueryable();
            if (AmountToTake > 0)
            {
                ReturnList = FullRequestList.Where(r => r.RequestStatusID == RequestStatusID).Take(AmountToTake);
            }
            else
            {
                ReturnList = FullRequestList.Where(r => r.RequestStatusID == RequestStatusID);
            }
            return ReturnList;
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
        public static IQueryable<Request> CombineTwoRequestsLists(IQueryable<Request> RequestListToCheck1, IQueryable<Request> RequestListToCheck2)
        {
            IQueryable<Request> ReturnList = null;
            if (!RequestListToCheck1.IsEmpty() && RequestListToCheck2.IsEmpty())
            {
                ReturnList = RequestListToCheck1;
            }
            else if (RequestListToCheck1.IsEmpty() && !RequestListToCheck2.IsEmpty())
            {
                ReturnList = RequestListToCheck2;
            }
            else if (!RequestListToCheck1.IsEmpty() && !RequestListToCheck2.IsEmpty())
            {
                ReturnList = RequestListToCheck1.Concat(RequestListToCheck2);
            }
            return ReturnList;
        }


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
    }
}
