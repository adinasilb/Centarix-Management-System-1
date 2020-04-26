using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;


namespace PrototypeWithAuth.Models
{
    public class LocationsTier4Model
    {
        [Key]
        public int LocationsTier4ModelID { get; set; }

        public int LocationsTier3ModelID { get; set; }
        public LocationsTier3Model LocationsTier3Model { get; set; }

        [Required]
        public string LocationsTier4ModelDescription { get; set; }

        [Required]
        //[Unique(ErrorMessage = "This already exist !!")]
        public string LocationsTier4ModelAbbreviation { get; set; }
        public IEnumerable<LocationsTier5Model> LocationsTier5Models { get; set; }
    }
}
