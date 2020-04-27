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
    public class LocationsTier5Instance
    {
        [Key]
        public int LocationsTier5InstanceID { get; set; }

        //get foreign key to type tier 2
        public int LocationsTier5ModelID { get; set; }
        public LocationsTier5Model LocationsTier5Model { get; set; }

        //get foreign key to instance type 1
        public int LocationsTier4InstanceID { get; set; }
        public LocationsTier4Instance LocationsTier4Instance { get; set; }
        public IEnumerable<LocationsTier6Instance> LocationsTier6Instances { get; set; }

        public string LocationsTier5InstanceAbrv { get; set; }
    }
}
