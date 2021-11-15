using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class SubProjectData
    {
        public static List<SubProject> Get()
        {
            List<SubProject> list = new List<SubProject>();
            list.Add(new SubProject
            {
                SubProjectID = 101,
                ProjectID = 1,
                SubProjectDescription = "General"
            });
            list.Add(new SubProject
            {
                SubProjectID = 102,
                ProjectID = 1,
                SubProjectDescription = "Epigenetic Rejuvenation"
            });
            list.Add(new SubProject
            {
                SubProjectID = 103,
                ProjectID = 1,
                SubProjectDescription = "Plasma Rejuvenation"
            });
            list.Add(new SubProject
            {
                SubProjectID = 201,
                ProjectID = 2,
                SubProjectDescription = "General"
            });
            list.Add(new SubProject
            {
                SubProjectID = 202,
                ProjectID = 2,
                SubProjectDescription = "AAV"
            });
            list.Add(new SubProject
            {
                SubProjectID = 301,
                ProjectID = 3,
                SubProjectDescription = "General"
            });
            list.Add(new SubProject
            {
                SubProjectID = 302,
                ProjectID = 3,
                SubProjectDescription = "Epigenetic Clock"
            });
            list.Add(new SubProject
            {
                SubProjectID = 303,
                ProjectID = 3,
                SubProjectDescription = "Telomere Measurement"
            });
            list.Add(new SubProject
            {
                SubProjectID = 401,
                ProjectID = 4,
                SubProjectDescription = "General"
            });
            list.Add(new SubProject
            {
                SubProjectID = 402,
                ProjectID = 4,
                SubProjectDescription = "Biomarker Trial"
            });
            list.Add(new SubProject
            {
                SubProjectID = 501,
                ProjectID = 5,
                SubProjectDescription = "General"
            });
            return list;
        }
    }
}
