using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class DegreeData
    {
        public static List<Degree> Get()
        {
            List<Degree> list = new List<Degree>();
            list.Add(new Degree
            {
                DegreeID = 1,
                Description = "B.Sc"
            });
            list.Add(new Degree
            {
                DegreeID = 2,
                Description = "M.Sc"
            });
            list.Add(new Degree
            {
                DegreeID = 3,
                Description = "P.hd"
            });
            list.Add(new Degree
            {
                DegreeID = 4,
                Description = "Post P.hd"
            });
            list.Add(new Degree
            {
                DegreeID = 5,
                Description = "No Degree"
            });
            list.Add(new Degree
            {
                DegreeID = 6,
                Description = "Certificate"
            });           
            return list;
        }
    }
}
