using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class GenderData
    {
        public static List<Gender> Get()
        {
            List<Gender> list = new List<Gender>();
            list.Add(new Gender
            {
                GenderID = 1,
                Description = "Male"
            });
            list.Add(new Gender
            {
                GenderID = 2,
                Description = "Female"
            });
            return list;
        }
    }
}
