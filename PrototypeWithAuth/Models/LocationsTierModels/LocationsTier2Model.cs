using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class LocationsTier2Model
    {
        [Key]
        public int LocationsTier2ModelID { get; set; }

        public int LocationsTier1ModelID { get; set; }
        public LocationsTier1Model LocationsTier1Model { get; set; }

        [Required]
        public string LocationsTier2ModelDescription { get; set; }

        [Required]
        //[Unique(ErrorMessage = "This already exist !!")]
        public string LocationsTier2ModelAbbreviation { get; set; }

        public IEnumerable<LocationsTier3Model> LocationsTier3Models { get; set; }

    }
}
