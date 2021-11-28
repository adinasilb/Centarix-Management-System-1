using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class ResourceCategoryData
    {
        public static List<ResourceCategory> Get()
        {
            List<ResourceCategory> list = new List<ResourceCategory>();
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 1,
                ResourceCategoryDescription = "Rejuvenation",
                IsMain = true,
                IsResourceType = false,
                ImageUrl = "rejuvenation_image.svg",
                IsReportsCategory = true
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 2,
                ResourceCategoryDescription = "Biomarkers",
                IsMain = true,
                IsResourceType = false,
                ImageUrl = "biomarkers_image.svg",
                IsReportsCategory = true
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 3,
                ResourceCategoryDescription = "Delivery Systems",
                IsMain = true,
                IsResourceType = false,
                ImageUrl = "delivery_systems_image.svg",
                IsReportsCategory = true
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 4,
                ResourceCategoryDescription = "Clinical Trials",
                IsMain = true,
                IsResourceType = false,
                ImageUrl = "clinical_trials_image.svg"
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 5,
                ResourceCategoryDescription = "AAV",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 6,
                ResourceCategoryDescription = "Telomere Rejuvenation",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 7,
                ResourceCategoryDescription = "Telomere Measurement",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 8,
                ResourceCategoryDescription = "Methylation Biomarker",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 9,
                ResourceCategoryDescription = "Transcriptome",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 10,
                ResourceCategoryDescription = "Serum Rejuvenation",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 11,
                ResourceCategoryDescription = "Reprogramming",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 12,
                ResourceCategoryDescription = "Methylation Rejuvenation",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 13,
                ResourceCategoryDescription = "New Methods",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 14,
                ResourceCategoryDescription = "Software",
                IsMain = false,
                IsResourceType = true,
                ImageUrl = "software_image.svg"
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 15,
                ResourceCategoryDescription = "Learning",
                IsMain = false,
                IsResourceType = true,
                ImageUrl = "learning_image.svg"
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 16,
                ResourceCategoryDescription = "Companies",
                IsMain = false,
                IsResourceType = true,
                ImageUrl = "companies_image.svg"
            });
            list.Add(new ResourceCategory
            {
                ResourceCategoryID = 17,
                ResourceCategoryDescription = "News",
                IsMain = false,
                IsResourceType = true,
                ImageUrl = "news_image.svg"
            });
            return list;
        }
    }
}