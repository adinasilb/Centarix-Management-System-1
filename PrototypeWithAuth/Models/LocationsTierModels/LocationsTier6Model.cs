using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class LocationsTier6Model
    {
        [Key]
        public int LocationsTier6ModelID { get; set; }

        public int LocationsTier5ModelID { get; set; }
        public LocationsTier5Model LocationsTier5Model { get; set; }

        [Required]
        public string LocationsTier6ModelDescription { get; set; }

        [Required]
        //[Unique(ErrorMessage = "This already exist !!")]
        public string LocationsTier6ModelAbbreviation { get; set; }
    }
}
