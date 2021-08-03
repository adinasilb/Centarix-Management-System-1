using System;
using System.Collections.Generic;
using System.Globalization;
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

        public void _1InsertExeriments()
        {
            DateTime startDate;
            DateTime.TryParseExact("20210106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
            DateTime endDate;
            DateTime.TryParseExact("20240106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            Experiment CrossSectionalAgingStudy = new Experiment()
            {
                Description = "Cross Sectional Aging Study",
                ExperimentCode = "ex1",
                NumberOfParticipants = 128,
                MinimumAge = 20,
                MaximumAge = 60,
                StartDateTime = startDate,
                EndDateTime = endDate
            };
            _context.Add(CrossSectionalAgingStudy);
            DateTime.TryParseExact("20210106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
            DateTime.TryParseExact("20240106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            Experiment LongitudinalAgingStudy = new Experiment()
            {
                Description = "Longitudinal Aging Study",
                ExperimentCode = "ex2",
                NumberOfParticipants = 32,
                MinimumAge = 20,
                MaximumAge = 80,
                StartDateTime = startDate,
                EndDateTime = endDate
            };
            _context.Add(LongitudinalAgingStudy);
            _context.SaveChanges();
        }

        public void _2InsertSites()
        {
            Site Centarix = new Site()
            {
                Name = "Centarix Biotech",
                Line1Address = "Hamarpe 3",
                City = "Har Hotzvim",
                Country = "Jerusalem",
                PrimaryContactID = _context.Users.Where(u => u.Email == "adina@centarix.com").FirstOrDefault().Id,
                PhoneNumber = "077-2634302"
            };
            _context.Add(Centarix);
            Site O2 = new Site()
            {
                Name = "O2",
                Line1Address = "",
                City = "Har Hazofim",
                Country = "Jerusalem",
                PrimaryContactID = _context.Users.Where(u => u.Email == "adina@centarix.com").FirstOrDefault().Id,
                PhoneNumber = "055-9876543"
            };
            _context.Add(O2);
            _context.SaveChanges();
        }

        public void _3Add02TestsBloodPressure()
        {
            Test BloodPressure = new Test()
            {
                Name = "Blood Pressure",
                SiteID = _context.Sites.Where(s => s.Name == "O2").FirstOrDefault().SiteID
            };
            _context.Add(BloodPressure);
            _context.SaveChanges();
            TestOuterGroup bptog1 = new TestOuterGroup()
            {
                IsNone = true,
                TestID = _context.Tests.Where(t => t.Name == "Blood Pressure").FirstOrDefault().TestID,
                SequencePosition = 1
            };
            _context.Add(bptog1);
            _context.SaveChanges();
            TestGroup bptg1 = new TestGroup()
            {
                Name = "First Measure",
                SequencePosition = 1,
                TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID
            };
            TestGroup bptg2 = new TestGroup()
            {
                Name = "Second Measure",
                SequencePosition = 2,
                TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID
            };
            TestGroup bptg3 = new TestGroup()
            {
                Name = "Third Measure",
                SequencePosition = 3,
                TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID
            };
            _context.Add(bptg1);
            _context.Add(bptg2);
            _context.Add(bptg3);
            _context.SaveChanges();
            TestHeader Pulse1 = new TestHeader()
            {
                Name = "Pulse",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 1).FirstOrDefault().TestGroupID,
                SequencePosition = 1,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Pulse1);
            TestHeader Pulse2 = new TestHeader()
            {
                Name = "Pulse",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 2).FirstOrDefault().TestGroupID,
                SequencePosition = 1,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Pulse2);
            TestHeader Pulse3 = new TestHeader()
            {
                Name = "Pulse",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 3).FirstOrDefault().TestGroupID,
                SequencePosition = 1,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Pulse3);
            TestHeader Systolic1 = new TestHeader()
            {
                Name = "Systolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 1).FirstOrDefault().TestGroupID,
                SequencePosition = 2,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Systolic1);
            TestHeader Systolic2 = new TestHeader()
            {
                Name = "Systolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 2).FirstOrDefault().TestGroupID,
                SequencePosition = 2,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Systolic2);
            TestHeader Systolic3 = new TestHeader()
            {
                Name = "Systolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 3).FirstOrDefault().TestGroupID,
                SequencePosition = 2,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Systolic3);
            TestHeader Diastolic1 = new TestHeader()
            {
                Name = "Diastolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 1).FirstOrDefault().TestGroupID,
                SequencePosition = 3,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Diastolic1);
            TestHeader Diastolic2 = new TestHeader()
            {
                Name = "Diastolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 2).FirstOrDefault().TestGroupID,
                SequencePosition = 3,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Diastolic2);
            TestHeader Diastolic3 = new TestHeader()
            {
                Name = "Diastolic",
                TestGroupID = _context.TestGroups.Where(tg => tg.SequencePosition == 3).FirstOrDefault().TestGroupID,
                SequencePosition = 3,
                Type = AppUtility.DataTypeEnum.Double.ToString()
            };
            _context.Add(Diastolic3);
            _context.SaveChanges();
        }

        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
        public IActionResult HumanTrialsList()
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.HumanTrials;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.BiomarkersExperiments;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Biomarkers;

            //_3Add02TestsBloodPressure();

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
                        Value = e.EndDateTime.GetElixirDateFormat()
                    }
                }
                )
            ); ;

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
                            Value = p.CentarixID,
                            AjaxLink = "open-participant-entries",
                            ID = p.ParticipantID
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

        public async Task<ActionResult> _Entries(int ParticipantID)
        {
            var entriesViewModel = new EntriesViewModel()
            {
                Participant = _context.Participants.Include(p => p.Gender).Include(p => p.ParticipantStatus)
                    .Where(p => p.ParticipantID == ParticipantID).FirstOrDefault()
            };
            return PartialView(entriesViewModel);
        }

        public async Task<ActionResult> _NewEntry(int ID)
        {
            NewEntryViewModel nevm = new NewEntryViewModel()
            {
                Sites = _context.Sites,
                ParticipantID = ID
            };
            return PartialView(nevm);
        }

        [HttpPost]
        public async Task<ActionResult> _NewEntry(NewEntryViewModel newEntryViewModel)
        {
            ExperimentEntry ee = new ExperimentEntry()
            {
                ParticipantID = newEntryViewModel.ParticipantID,
                DateTime = newEntryViewModel.Date,
                VisitNumber = newEntryViewModel.VisitNumber,
                ApplicationUserID = _userManager.GetUserId(User)
            };

            _context.Add(ee);
            await _context.SaveChangesAsync();

            return PartialView();
        }
    }
}
