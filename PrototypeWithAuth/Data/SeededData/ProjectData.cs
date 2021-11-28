using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class ProjectData
    {
        public static List<Project> Get()
        {
            List<Project> list = new List<Project>();
            list.Add(new Project
            {
                ProjectID = 1,
                ProjectDescription = "Rejuvenation"
            });
            list.Add(new Project
            {
                ProjectID = 2,
                ProjectDescription = "Delivery Systems"
            });
            list.Add(new Project
            {
                ProjectID = 3,
                ProjectDescription = "Biomarkers"
            });
            list.Add(new Project
            {
                ProjectID = 4,
                ProjectDescription = "Clinical Trials"
            });
            list.Add(new Project
            {
                ProjectID = 5,
                ProjectDescription = "General"
            });
            return list;
        }
    }
}
