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
using Microsoft.EntityFrameworkCore;
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
                        Value = e.StartDateTime.GetElixirDateFormat()
                    },
                    new TDViewModel()
                    {
                        Value =e.EndDateTime.GetElixirDateFormat()
                    }
                }
                )
            );

            return View(experimentListViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
        public async Task<IActionResult> Experiment(int ID)
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.HumanTrials;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.BiomarkersExperiments;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Biomarkers;

            ExperimentViewModel experimentViewModel = new ExperimentViewModel()
            {
                Experiment = _context.Experiments.Where(e => e.ExperimentID == ID).FirstOrDefault(),
                _ParticipantsViewModel = await GetParticipantsViewModel(ID)
            };

            return View(experimentViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
        public async Task<IActionResult> _ParticipantsViewModel(int ID)
        {
            _ParticipantsViewModel participantsViewModel = await GetParticipantsViewModel(ID);
            return PartialView(participantsViewModel);
        }


        [Authorize(Roles = "Biomarkers")]
        public async Task<_ParticipantsViewModel> GetParticipantsViewModel(int ID)
        {
            _ParticipantsViewModel participantsViewModel = new _ParticipantsViewModel()
            {
                Experiment = _context.Experiments.Include(e => e.Participants)
                .ThenInclude(p => p.Gender).Include(e => e.Participants).ThenInclude(p => p.ParticipantStatus)
                .Where(e => e.ExperimentID == ID).FirstOrDefault()
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
                participantsViewModel.Rows = await GetParticipantsRows(participantsViewModel.Experiment.Participants);
            }

            return participantsViewModel;
        }

        public async Task<ActionResult> _BiomarkersRows(int? ExperimentID)
        {
            List<List<TDViewModel>> BioRows = new List<List<TDViewModel>>();
            if (ExperimentID != null)
            {
                BioRows = await GetParticipantsRows(_context.Participants.Include(p => p.Gender).Include(p => p.ParticipantStatus).Where(p => p.ExperimentID == ExperimentID));
            }
            return PartialView(BioRows);
        }

        public async Task<List<List<TDViewModel>>> GetParticipantsRows(IEnumerable<Participant> Participants)
        {
            List<List<TDViewModel>> rows = new List<List<TDViewModel>>();
            Participants.ToList().ForEach(p =>
                 rows.Add(
                     new List<TDViewModel>()
                     {
                        new TDViewModel()
                        {
                            Value = p.CentarixID
                        },
                        new TDViewModel()
                        {
                            Value = p.DOB.GetElixirDateFormat()
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
            return rows;
        }

        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
        public ActionResult AddParticipantModal(int ExperimentID)
        {
            AddParticipantViewModel addParticipantViewModel = new AddParticipantViewModel()
            {
                Participant = new Participant()
                {
                    ExperimentID = ExperimentID
                },
                Genders = _context.Genders.ToList()
            };

            return PartialView(addParticipantViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Biomarkers")]
        public async Task<ActionResult> AddParticipantModal(AddParticipantViewModel addParticipant)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    addParticipant.Participant.ParticipantStatusID = 1;
                    _context.Add(addParticipant.Participant);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
            return RedirectToAction("_BiomarkersRows", new { ExperimentID = addParticipant.Participant.ExperimentID });
        }

        public async Task<int> GetParticipantsCount(int ExperimentID)
        {
            return _context.Participants.Where(p => p.ExperimentID == ExperimentID).Count();
        }
    }
}
