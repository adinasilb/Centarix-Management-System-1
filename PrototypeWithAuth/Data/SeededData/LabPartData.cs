using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class LabPartData
    {
        public static List<LabPart> Get()
        {
            List<LabPart> list = new List<LabPart>();
            list.Add(new LabPart
            {
                LabPartID = 1,
                LabPartName = "Closet",
                LabPartNameAbbrev = "C",
                HasShelves = true
            });
            list.Add(new LabPart
            {
                LabPartID = 2,
                LabPartName = "Glass Closet",
                LabPartNameAbbrev = "G",
                HasShelves = true
            });
            list.Add(new LabPart
            {
                LabPartID = 3,
                LabPartNameAbbrev = "T",
                LabPartName = "Table",
            });
            list.Add(new LabPart
            {
                LabPartID = 4,
                LabPartNameAbbrev = "D",
                LabPartName = "Drawer",
                HasShelves = true
            });
            list.Add(new LabPart
            {
                LabPartID = 5,
                LabPartNameAbbrev = "S",
                LabPartName = "Shelf"
            });
            list.Add(new LabPart
            {
                LabPartID = 6,
                LabPartNameAbbrev = "B",
                LabPartName = "Bench"
            });
            return list;
        }
    }
}
