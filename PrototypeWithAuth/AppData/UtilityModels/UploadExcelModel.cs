using LinqToExcel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class UploadExcelModel
    {
        [ExcelColumn("Item Name")]
        public string ItemName { get; set; }
        [ExcelColumn("Requested By")]
        public string RequstedBy { get; set; }
        [ExcelColumn("Vendor")]
        public string VendorName { get; set; }
        [ExcelColumn("Catalog #")]
        public string CatalogNumber { get; set; }
        [ExcelColumn("ParentCategory")]
        public string ParentCategoryName { get; set; }
        [ExcelColumn("SubCategory")]
        public string ProductSubCategoryName { get; set; }
        [ExcelColumn("Qty")]
        public uint Unit { get; set; }
        [ExcelColumn("URL")]
        public string Url { get; set; }
        [ExcelColumn("Total Price")]
        public decimal TotalPrice { get; set; }
        [ExcelColumn("PO #")]
        public string OrderNumber { get; set; }
        [ExcelColumn("Date Requested")]
        public DateTime DateRequested { get; set; }
        [ExcelColumn("Date Ordered")]
        public DateTime DateOrdered { get; set; }
        [ExcelColumn("Ordered By")]
        public string OrderedBy { get; set; }
        [ExcelColumn("Date Received")]
        public DateTime DateReceived { get; set; }
        [ExcelColumn("Received By")]
        public string ReceivedBy { get; set; }
      

    }
}
