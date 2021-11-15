using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class ParentCategoryData
    {
        public static List<ParentCategory> Get()
        {
            List<ParentCategory> list = new List<ParentCategory>();
            list.Add(new ParentCategory
            {
                ParentCategoryID = 1,
                ParentCategoryDescription = "Consumables",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 2,
                ParentCategoryDescription = "Reagents And Chemicals",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 3,
                ParentCategoryDescription = "Biological",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 4,
                ParentCategoryDescription = "Reusable",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 5,
                ParentCategoryDescription = "Safety",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 6,
                ParentCategoryDescription = "General",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 7,
                ParentCategoryDescription = "Clinical",
                CategoryTypeID = 1
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 14,
                ParentCategoryDescription = "Samples",
                CategoryTypeID = 1,
                IsProprietary = true
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 8,
                ParentCategoryDescription = "IT",
                CategoryTypeID = 2
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 9,
                ParentCategoryDescription = "Day To Day",
                CategoryTypeID = 2
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 10,
                ParentCategoryDescription = "Travel",
                CategoryTypeID = 2
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 11,
                ParentCategoryDescription = "Advice",
                CategoryTypeID = 2
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 12,
                ParentCategoryDescription = "Regulations",
                CategoryTypeID = 2
            });
            list.Add(new ParentCategory
            {
                ParentCategoryID = 13,
                ParentCategoryDescription = "Government",
                CategoryTypeID = 2
            });
            return list;
        }
    }
}
