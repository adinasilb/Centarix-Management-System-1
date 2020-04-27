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
    public class LocationsTier4Instance
    {
        [Key]
        public int LocationsTier4InstanceID { get; set; }

        //get foreign key to type tier 2
        public int LocationsTier4ModelID { get; set; }
        public LocationsTier4Model LocationsTier4Model { get; set; }

        //get foreign key to instance type 1
        public int LocationsTier3InstanceID { get; set; }
        public LocationsTier3Instance LocationsTier3Instance { get; set; }
        public IEnumerable<LocationsTier5Instance> LocationsTier5Instances { get; set; }

        public string LocationsTier4InstanceAbrv { get; set; }
    }
}
