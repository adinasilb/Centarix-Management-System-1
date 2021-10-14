using LinqToExcel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class UploadInvoiceExcelModel
    {

        [ExcelColumn("F")]
        public string OrderNumber { get; set; }

        private string _InvoiceNumber;
        [ExcelColumn("invoice number")]
        public string InvoiceNumber
        {

            get { return _InvoiceNumber?.Replace("|", ""); }
            set { _InvoiceNumber = value; }
        }
        [ExcelColumn("invoice date")]
        public DateTime InvoiceDate { get; set; }
        [ExcelColumn("document number")]
        public int DocumentNumber { get; set; }
        [ExcelColumn("Catalog Num")]
        public string CatalogNumber { get; set; }

    }
}