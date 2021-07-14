using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
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

        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
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
                        AjaxLink = "open-experiment",
                        Link = "Experiment",
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

        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
        public IActionResult Experiment(int ID)
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.HumanTrials;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.BiomarkersExperiments;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Biomarkers;

            ExperimentViewModel experimentViewModel = new ExperimentViewModel()
            {
                Experiment = _context.Experiments.Where(e => e.ExperimentID == ID).FirstOrDefault(),
                _ParticipantsViewModel = GetParticipantsViewModel(ID)
            };

            return View(experimentViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
        public IActionResult _ParticipantsViewModel(int ID)
        {
            _ParticipantsViewModel participantsViewModel = GetParticipantsViewModel(ID);
            return PartialView(participantsViewModel);
        }


        [Authorize(Roles = "Biomarkers")]
        public _ParticipantsViewModel GetParticipantsViewModel(int ID)
        {
            _ParticipantsViewModel participantsViewModel = new _ParticipantsViewModel()
            {
                Experiment = _context.Experiments.Where(e => e.ExperimentID == ID).FirstOrDefault()
            };
            participantsViewModel.Headers = new List<TDViewModel>()
            {
                new TDViewModel()
                {
                    Value = "Serial Number"
                },
                new TDViewModel()
                {
                    Value = "DOB"
                },
                new TDViewModel()
                {
                    Value = "Gender"
                },
                new TDViewModel()
                {
                    Value = "Timepoint"
                },
                new TDViewModel()
                {
                    Value = "Visit"
                },
                new TDViewModel()
                {
                    Value = "Status"
                }
            };
            participantsViewModel.Rows = new List<List<TDViewModel>>();
            if (participantsViewModel.Experiment.Participants != null)
            {
                participantsViewModel.Experiment.Participants.ToList().ForEach(p =>
                participantsViewModel.Rows.Add(
                    new List<TDViewModel>()
                    {
                        new TDViewModel()
                        {
                            Value = p.CentarixID
                        },
                        new TDViewModel()
                        {
                            Value = AppUtility.FormatDate(p.DOB)
                        },
                        new TDViewModel()
                        {
                            Value = p.Gender.Description
                        },
                        new TDViewModel()
                        {
                            Value = "0"
                        },
                        new TDViewModel()
                        {
                            Value = "0"
                        },
                        new TDViewModel()
                        {
                            Value = p.ParticipantStatus.Description
                        }
                    }
                    )
                );
            }

            return participantsViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
        public ActionResult AddParticipantModal()
        {
            return PartialView(new Participant());
        }

        [HttpPost]
        [Authorize(Roles = "Biomarkers")]
        public ActionResult AddParticipantModal(Participant participant)
        {
            return PartialView();
        }
    }
}
