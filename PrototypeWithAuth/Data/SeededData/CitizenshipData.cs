using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class CitizenshipData
    {
        public static List<Citizenship> Get()
        {
            List<Citizenship> list = new List<Citizenship>();
            list.Add(new Citizenship
            {
                CitizenshipID = 1,
                Description = "Israel"
            });
            list.Add(new Citizenship
            {
                CitizenshipID = 2,
                Description = "USA"
            });         
            return list;
        }      
    }
}
