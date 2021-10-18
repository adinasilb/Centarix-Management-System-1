using LinqToExcel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class UploadInvoiceExcelModel
    {

        [ExcelColumn("PO #")]
        public string OrderNumber { get; set; }
        [ExcelColumn("invoice number")]
        public string InvoiceNumber {get; set;}
        [ExcelColumn("invoice date")]
        public DateTime InvoiceDate { get; set; }
        [ExcelColumn("document number")]
        public int DocumentNumber { get; set; }
        [ExcelColumn("Catalog Num")]
        public string CatalogNumber { get; set; }

    }
}
