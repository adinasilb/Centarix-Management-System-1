using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.Models.LocationsTierInstantiation
{
    public class LocationsTier6Instance
    {
        [Key]
        public int LocationsTier6InstanceID { get; set; }

        //get foreign key to type tier 2
        public int LocationsTier6ModelID { get; set; }
        public LocationsTier6Model LocationsTier6Model { get; set; }

        //get foreign key to instance type 1
        public int LocationsTier5InstanceID { get; set; }
        public LocationsTier5Instance LocationsTier5Instance { get; set; }

        public string LocationsTier5InstanceAbrv { get; set; }
    }
}
