using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class UnitTypeParentCategory
    {
        [Key, Column(Order = 1)]
        public int UnitTypeID { get; set; }
        public UnitType UnitType { get; set; }
        [Key, Column(Order = 2)]
        public int ParentCategoryID { get; set; }
        public ParentCategory ParentCategory { get; set; }
    }
}
