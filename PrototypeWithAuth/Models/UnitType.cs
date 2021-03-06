using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Models
{
    public class UnitType : ModelBase
    {
        [Key]
        public int UnitTypeID { get; set; }

        public string UnitTypeDescription { get; set; }
        public int UnitParentTypeID { get; set; }
        public UnitParentType UnitParentType { get; set; }
        public ListImplementsModelBase<UnitTypeParentCategory> UnitTypeParentCategory { get; set; }

    }
}
