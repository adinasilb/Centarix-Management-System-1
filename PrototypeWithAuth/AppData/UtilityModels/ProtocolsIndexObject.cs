using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData
{
    public class ProtocolsIndexObject
    {
        public string ErrorMessage { get; set; }
        private int _PageNumber;
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
    }
}
