using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class JobCategoryTypeData
    {
        public static List<JobCategoryType> Get()
        {
            List<JobCategoryType> list = new List<JobCategoryType>();
            list.Add(new JobCategoryType
            {
                JobCategoryTypeID = 1,
                Description = "Executive"
            });
            list.Add(new JobCategoryType
            {
                JobCategoryTypeID = 2,
                Description = "Rejuvenation"
            });
            list.Add(new JobCategoryType
            {
                JobCategoryTypeID = 3,
                Description = "Biomarker"
            });
            list.Add(new JobCategoryType
            {
                JobCategoryTypeID = 4,
                Description = "Delivery Systems"
            });
            list.Add(new JobCategoryType
            {
                JobCategoryTypeID = 5,
                Description = "Clinical Trials"
            });
            list.Add(new JobCategoryType
            {
                JobCategoryTypeID = 6,
                Description = "Business Development"
            });
            list.Add(new JobCategoryType
            {
                JobCategoryTypeID = 7,
                Description = "Software Development"
            });
            list.Add(new JobCategoryType
            {
                JobCategoryTypeID = 8,
                Description = "General"
            });
            list.Add(new JobCategoryType
            {
                JobCategoryTypeID = 9,
                Description = "Lab"
            });
            list.Add(new JobCategoryType
            {
                JobCategoryTypeID = 10,
                Description = "Bioinformatics"
            });
            return list;
        }
    }
}
