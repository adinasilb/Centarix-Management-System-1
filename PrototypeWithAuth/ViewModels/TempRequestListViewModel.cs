﻿using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class TempRequestListViewModel
    {
        public List<Guid> Guids { get; set; }
        public int SequencePosition { get; set; }
        public List<TempRequestViewModel> TempRequestViewModels { get; set; }
        public RequestIndexObject RequestIndexObject { get; set; }
    }
}
