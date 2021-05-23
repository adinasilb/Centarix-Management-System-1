using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public class RequestIndexObject
    {
        public string ErrorMessage { get; set; }
        private int _PageNumber;
        private int _RequestStatusID;
        private List<string> _SelectedPriceSort;
        private AppUtility.PageTypeEnum _PageType;
        public int PageNumber {
            get
            {
                if (_PageNumber == 0)
                {
                    return 1;
                }
                else
                {
                    return _PageNumber;
                }
            }
            set { _PageNumber = value; }
        } 
        public int RequestStatusID
        {
            get
            {
                if (_RequestStatusID == 0 && (PageType == AppUtility.PageTypeEnum.RequestSummary || PageType == AppUtility.PageTypeEnum.OperationsInventory))
                {
                    return 3;
                }
                else if (_RequestStatusID == 0 && PageType == AppUtility.PageTypeEnum.OperationsRequest)
                {
                    return 2;
                }
                else if (_RequestStatusID == 0)
                {
                    return 6;
                }
                else
                {
                    return _RequestStatusID;
                }
            }
            set { _RequestStatusID = value; }
        }
        private AppUtility.SidebarEnum _SidebarType;
        public AppUtility.SidebarEnum SidebarType
        {
            get
            {
                if (_SidebarType == AppUtility.SidebarEnum.None)
                {
                    return AppUtility.SidebarEnum.List;
                }
                else
                {
                    return _SidebarType;
                }
            }
            set { _SidebarType = value; }
        }
        public String SidebarFilterID { get; set; }
        public AppUtility.PageTypeEnum PageType 
        {
            get
            {
                if (_PageType == AppUtility.PageTypeEnum.None)
                {
                    return AppUtility.PageTypeEnum.RequestRequest;
                }
                else
                {
                    return _PageType;
                }
            }
            set { _PageType = value; }
        }
        public AppUtility.MenuItems SectionType { get; set; }
        public List<String> SelectedPriceSort
        {
            get
            {
                if (_SelectedPriceSort == null)
                {
                    return new List<string>() { AppUtility.PriceSortEnum.TotalVat.ToString()};
                }
                else
                {
                    if(_SelectedPriceSort[0]==null)
                    {
                        return new List<string>() { AppUtility.PriceSortEnum.TotalVat.ToString() };
                    }
                    return _SelectedPriceSort;
                }
            }
            set { _SelectedPriceSort = value; }
        }
        public AppUtility.CurrencyEnum SelectedCurrency { get; set; }
        public AppUtility.OrderTypeEnum OrderType { get; set; }
        public bool CategorySelected { get; set; }
        public bool SubcategorySelected { get; set; }
        public bool IsReorder { get; set; }
         //ExpensesFilter = null, List<int> CategoryTypeIDs = null, List<int> Months = null, List<int> Years = null
         public bool IsReorder { get; set; }
    }
}
