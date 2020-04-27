using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class LocationsTier5Model
    {
        [Key]
        public int LocationsTier5ModelID { get; set; }

        public int LocationsTier4ModelID { get; set; }

        public LocationsTier4Model LocationsTier4Model { get; set; }

        [Required]
        public string LocationsTier5ModelDescription { get; set; }

        [Required]
        //[Unique(ErrorMessage = "This already exist !!")]
        public string LocationsTier5ModelAbbreviation { get; set; }

        public IEnumerable<LocationsTier6Model> LocationsTier6Models { get; set; }
    }
}
