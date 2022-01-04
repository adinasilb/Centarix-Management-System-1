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
                ID = 1,
                Description = "Rejuvenation",
                IsMain = true,
                IsResourceType = false,
                ImageUrl = "rejuvenation_image.svg",
                IsReportsCategory = true
            });
            list.Add(new ResourceCategory
            {
                ID = 2,
                Description = "Biomarkers",
                IsMain = true,
                IsResourceType = false,
                ImageUrl = "biomarkers_image.svg",
                IsReportsCategory = true
            });
            list.Add(new ResourceCategory
            {
                ID = 3,
                Description = "Delivery Systems",
                IsMain = true,
                IsResourceType = false,
                ImageUrl = "delivery_systems_image.svg",
                IsReportsCategory = true
            });
            list.Add(new ResourceCategory
            {
                ID = 4,
                Description = "Clinical Trials",
                IsMain = true,
                IsResourceType = false,
                ImageUrl = "clinical_trials_image.svg"
            });
            list.Add(new ResourceCategory
            {
                ID = 5,
                Description = "AAV",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ID = 6,
                Description = "Telomere Rejuvenation",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ID = 7,
                Description = "Telomere Measurement",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ID = 8,
                Description = "Methylation Biomarker",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ID = 9,
                Description = "Transcriptome",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ID = 10,
                Description = "Serum Rejuvenation",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ID = 11,
                Description = "Reprogramming",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ID = 12,
                Description = "Methylation Rejuvenation",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ID = 13,
                Description = "New Methods",
                IsMain = false,
                IsResourceType = false
            });
            list.Add(new ResourceCategory
            {
                ID = 14,
                Description = "Software",
                IsMain = false,
                IsResourceType = true,
                ImageUrl = "software_image.svg"
            });
            list.Add(new ResourceCategory
            {
                ID = 15,
                Description = "Learning",
                IsMain = false,
                IsResourceType = true,
                ImageUrl = "learning_image.svg"
            });
            list.Add(new ResourceCategory
            {
                ID = 16,
                Description = "Companies",
                IsMain = false,
                IsResourceType = true,
                ImageUrl = "companies_image.svg"
            });
            list.Add(new ResourceCategory
            {
                ID = 17,
                Description = "News",
                IsMain = false,
                IsResourceType = true,
                ImageUrl = "news_image.svg"
            });
            return list;
        }
    }
}