using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;


namespace PrototypeWithAuth.Models
{
    public class LocationsTier1Model
    {
        [Key]
        public int LocationsTier1ModelID { get; set; }
        [Required]
        public string LocationsTier1ModelDescription { get; set; }
        
        [Required]
        //[Unique(ErrorMessage = "This already exist !!")]
        public string LocationsTier1ModelAbbreviation { get; set; }
        public IEnumerable<LocationsTier2Model> LocationsTier2Models { get; set; }
    }
}
