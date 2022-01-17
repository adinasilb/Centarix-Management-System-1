﻿using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UploadQuoteViewModel : ViewModelBase
    {
        public ParentQuote ParentQuote { get; set; }
        public List<string> FileStrings { get; set; }
        public AppUtility.OrderMethod OrderMethodEnum { get; set; }
        //public RequestIndexObject RequestIndexObject { get; set; }
        public TempRequestListViewModel TempRequestListViewModel { get; set; }

        [Display(Name = "Expected Supply Days")]
        [Range(0, 2147483647, ErrorMessage = "Field must be a positive number")]
        public byte? ExpectedSupplyDays { get; set; }
        //public decimal Discount { get; set; }

        //public bool IsReorder { get; set; }
    }
}
