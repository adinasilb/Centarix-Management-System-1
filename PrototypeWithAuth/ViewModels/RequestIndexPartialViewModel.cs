﻿using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexPartialViewModel
    {
        public IPagedList<RequestIndexPartialRowViewModel> PagedList { get; set; }
        public AppUtility.PageTypeEnum PageType { get; set; }
        public List<PriceSortViewModel> PriceSortEnums { get; set; }
        public List<String> PriceSortEnumsList { get; set; }
        public int? PageNumber { get; set; }
        public int RequestStatusID { get; set; }
    }
}
