﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestIndexPartialColumnViewModel
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public List<string> Value { get; set; }
        public List<IconColumnViewModel> Icons { get; set; }
        public int AjaxID { get; set; }
        public string Image { get; set; }
        public string AjaxLink { get; set; }
    }
}
