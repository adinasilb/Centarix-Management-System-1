using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;

namespace PrototypeWithAuth.AppData
{
    public static class AppUtility
    {

        public enum RequestPageTypeEnum { None, Request, Inventory, Cart, Search, Location }
        public enum PaymentPageTypeEnum { None, Notifications, General, Expenses, Suppliers } //these are all going to the ParentRequestIndex
        public enum RequestSidebarEnum { None, Type, Vendor, Owner, Location, Cart }

        public static int GetCountOfRequestsByRequestStatusIDVendorIDSubcategoryIDApplicationUserID(IQueryable<Request> RequestsList, int RequestStatusID, int VendorID = 0, int? SubcategoryID = 0, string ApplicationUserID = null)
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
            else if (ApplicationUserID != null)
            {
                ReturnList = RequestsList
                    .Where(r => r.RequestStatusID == RequestStatusID)
                    .Where(r => r.ParentRequest.ApplicationUserID == ApplicationUserID)
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
            IQueryable<Request> ReturnList = Enumerable.Empty<Request>().AsQueryable();
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
                ReturnList = RequestListToCheck1.Concat(RequestListToCheck2).OrderByDescending(r => r.ParentRequest.OrderDate);
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

        struct PaymentNotificationValuePairs
        {
            public string NotificationListName;
            public int Amount;
        }
        public static void CreateCsFile(string sNamespace, string currentLocation, string previousLocation, string futureLocation, IHostingEnvironment _hostingEnvironment, string filePath)
        {
            CodeNamespace nameSpace = new CodeNamespace(sNamespace);

            nameSpace.Imports.Add(new CodeNamespaceImport("System"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Linq"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Text"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            nameSpace.Imports.Add(new CodeNamespaceImport("System.ComponentModel.DataAnnotations"));

            CodeTypeDeclaration cls = new CodeTypeDeclaration();
            cls.Name = currentLocation;
            cls.IsClass = true;
            cls.Attributes = MemberAttributes.Public;

            ////check if this works
            //var attribute = new CodeAttributeDeclaration(new
            //    CodeTypeReference(typeof(System.ComponentModel.DataAnnotations.KeyAttribute)));
            //cls.Members.Add(attribute);

            //CodeMemberField dataAnnotationsKey = new CodeMemberField();
            //dataAnnotationsKey.Name = "[Key] //";
            //cls.Members.Add("[Key]");

            CodeMemberField primaryKeyProperty = new CodeMemberField();
            primaryKeyProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            primaryKeyProperty.Name = currentLocation + "ID";
            primaryKeyProperty.Type = new CodeTypeReference(typeof(System.Int32));
            primaryKeyProperty.Name += " { get; set; } //";


            // Create the attribute declaration for the property.
            var attr = new CodeAttributeDeclaration("Key"/*new CodeTypeReference(typeof(System.ComponentModel.DataAnnotations.KeyAttribute))*/);
            primaryKeyProperty.CustomAttributes.Add(attr);
            //var attr = new CodeAttributeDeclaration(new CodeTypeReference(typeof(System.ComponentModel.DataAnnotations.RangeAttribute)));
            //attr.Arguments.Add(new CodeAttributeArgument(new CodeTypeOfExpression(typeof(System.ComponentModel.DataAnnotations.KeyAttribute))));
            //cls.CustomAttributes.Add(attr);
            cls.Members.Add(primaryKeyProperty);

            //foreign key to parent class
            if (previousLocation != "")
            {
                //CodeMemberField constant = new CodeMemberField(new CodeTypeReference(typeof(System.String)), "[Required]");
                //constant.Attributes = MemberAttributes.Const;
                //constant.InitExpression = new CodePrimitiveExpression(0x0000AE77);
                //cls.Members.Add(constant);

                //CodeSnippetStatement dataAnnotationsReq = new CodeSnippetStatement();
                //dataAnnotationsReq.Value = "[Required] //";
                //cls.Members.Add(dataAnnotationsReq);

                //Foreign key id
                CodeMemberField propertyID = new CodeMemberField();
                propertyID.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                propertyID.Name = previousLocation + "ID";
                propertyID.Type = new CodeTypeReference(typeof(System.Int32));
                propertyID.Name += " { get; set; } //";


                // Create the attribute declaration for the property.
                var attr2 = new CodeAttributeDeclaration("Required"/*new CodeTypeReference(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute))*/);
                propertyID.CustomAttributes.Add(attr2);
                cls.Members.Add(propertyID);


                //Foreign key property
                CodeMemberField propPrevLocation = new CodeMemberField();
                propPrevLocation.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                propPrevLocation.Name = previousLocation;
                propPrevLocation.Type = new CodeTypeReference(previousLocation);
                propPrevLocation.Name += " { get; set; } //";
                cls.Members.Add(propPrevLocation);
            }

            //property (which refers to the nested folder) inside of current class
            if (futureLocation != "")
            {
                CodeMemberField property = new CodeMemberField();
                property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                property.Name = futureLocation;
                property.Type = new CodeTypeReference("IEnumerable<" + futureLocation + ">");
                property.Name += " { get; set; } //";
                cls.Members.Add(property);
            }

            nameSpace.Types.Add(cls);

            CodeCompileUnit compileUnit = new CodeCompileUnit();
            compileUnit.Namespaces.Add(nameSpace);


            CSharpCodeProvider provider = new CSharpCodeProvider();

            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");
                provider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
                tw.Close();
            }
        }

    }
}
