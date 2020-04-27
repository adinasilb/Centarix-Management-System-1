using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class LocationsTier3Model
    {
        [Key]
        public int LocationsTier3ModelID { get; set; }

        public int LocationsTier2ModelID { get; set; }
        public LocationsTier2Model LocationsTier2Model { get; set; }

        [Required]
        public string LocationsTier3ModelDescription { get; set; }

        [Required]
        //[Unique(ErrorMessage = "This already exist !!")]
        public string LocationsTier3ModelAbbreviation { get; set; }
        public IEnumerable<LocationsTier4Model> LocationsTier4Models { get; set; }
    }
}
