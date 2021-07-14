using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class BiomarkersController : SharedController
    {
        private ApplicationDbContext _context;
        public BiomarkersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
              : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
            _context = context;
        }

        public IActionResult HumanTrialsList()
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.HumanTrials;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.BiomarkersExperiments;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Biomarkers;

            ExperimentListViewModel experimentListViewModel = new ExperimentListViewModel();
            experimentListViewModel.Headers = new List<TDViewModel>()
            {
                new TDViewModel(){ Value="Experiment Name" },
                new TDViewModel(){ Value="Experiment Code"},
                new TDViewModel(){ Value="Participant"},
                new TDViewModel(){ Value="Age Range"},
                new TDViewModel(){ Value="Timepoints"},
                new TDViewModel(){ Value="Visits"},
                new TDViewModel(){ Value="Start Date"},
                new TDViewModel(){ Value="End Date" }
            };
            experimentListViewModel.ValuesPerRow = new List<List<TDViewModel>>();

            _context.Experiments.ToList().ForEach(e =>
            experimentListViewModel.ValuesPerRow.Add(
                new List<TDViewModel>()
                {
                    new TDViewModel()
                    {
                        Value = e.Description,
                        Link = "open-experiment",
                        ID = e.ExperimentID
                    },
                    new TDViewModel()
                    {
                        Value = e.ExperimentCode
                    },
                    new TDViewModel()
                    {
                        Value = e.NumberOfParticipants.ToString()
                    },
                    new TDViewModel()
                    {
                        Value = e.MinimumAge.ToString() + " - " + e.MaximumAge.ToString()
                    },
                    new TDViewModel()
                    {
                        Value = e.Timepoints == null ? "0" : e.Timepoints.Count().ToString()
                    },
                    new TDViewModel()
                    {
                        Value = e.Timepoints == null ? "0" : e.Timepoints.Select(t => t.AmountOfVisits).Sum().ToString()
                    },
                    new TDViewModel()
                    {
                        Value = AppUtility.FormatDate(e.StartDateTime)
                    },
                    new TDViewModel()
                    {
                        Value = AppUtility.FormatDate(e.EndDateTime)
                    }
                }
                )
            );

            return View(experimentListViewModel);
        }
        public IActionResult Experiment(int ID)
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.HumanTrials;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.BiomarkersExperiments;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Biomarkers;

            ExperimentViewModel experimentViewModel = new ExperimentViewModel()
            {
                Experiment = _context.Experiments.Where(e => e.ExperimentID == ID).FirstOrDefault()
            };

            return PartialView(experimentViewModel);
        }
    }
}
