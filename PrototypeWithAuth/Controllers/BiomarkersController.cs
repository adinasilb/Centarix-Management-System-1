﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public ActionResult RunScripts()
        {
            var testheaders = _context.TestHeaders;
            foreach (var th in testheaders)
            {
                _context.Remove(th);
            }
            _context.SaveChanges();
            var testgroups = _context.TestGroups;
            foreach (var tg in testgroups)
            {
                _context.Remove(tg);
            }
            _context.SaveChanges();
            var testoutergroups = _context.TestOuterGroups;
            foreach (var tog in testoutergroups)
            {
                _context.Remove(tog);
            }
            _context.SaveChanges();
            var experimentTests = _context.Experiments.SelectMany(e => e.ExperimentTests);
            foreach (var et in experimentTests)
            {
                _context.Remove(et);
            }
            _context.SaveChanges();
            var tests = _context.Tests;
            foreach (var t in tests)
            {
                _context.Remove(t);
            }
            _context.SaveChanges();
            var experimentEntries = _context.ExperimentEntries;
            foreach (var ee in experimentEntries)
            {
                _context.Remove(ee);
            }
            _context.SaveChanges();
            var sites = _context.Sites;
            foreach (var s in sites)
            {
                _context.Remove(s);
            }
            _context.SaveChanges();
            var particpants = _context.Participants;
            foreach (var p in particpants)
            {
                _context.Remove(p);
            }
            _context.SaveChanges();
            var experiments = _context.Experiments;
            foreach (var e in experiments)
            {
                _context.Remove(e);
            }

            _1InsertExeriments();
            _2InsertSites();
            _3Add02TestsBloodPressure();
            _4InsertECGTest();
            return RedirectToAction("HumanTrialsList");
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
            ExperimentTest et1 = new ExperimentTest()
            {
                ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault(),
                TestID = _context.Tests.Where(t => t.Name == "Blood Pressure").Select(t => t.TestID).FirstOrDefault()
            };
            ExperimentTest et2 = new ExperimentTest()
            {
                ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault(),
                TestID = _context.Tests.Where(t => t.Name == "Blood Pressure").Select(t => t.TestID).FirstOrDefault()
            };
            _context.Add(et1);
            _context.Add(et2);
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

        public void _4InsertECGTest()
        {
            var Test = new Test()
            {
                Name = "ECG",
                SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
            };
            _context.Add(Test);
            _context.SaveChanges();
            var testId = _context.Tests.Where(t => t.Name == "ECG").Select(t => t.TestID).FirstOrDefault();
            var experimentTest = new ExperimentTest()
            {
                TestID = testId,
                ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex1").Select(e => e.ExperimentID).FirstOrDefault()
            };
            var experimentTest2 = new ExperimentTest()
            {
                TestID = testId,
                ExperimentID = _context.Experiments.Where(e => e.ExperimentCode == "ex2").Select(e => e.ExperimentID).FirstOrDefault()
            };
            _context.Add(experimentTest);
            _context.Add(experimentTest2);
            _context.SaveChanges();
            var testoutergroup = new TestOuterGroup()
            {
                IsNone = true,
                TestID = testId,
                SequencePosition = 1
            };
            _context.Add(testoutergroup);
            _context.SaveChanges();
            var testgroup = new TestGroup()
            {
                IsNone = true,
                TestOuterGroupID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                    .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID,
                SequencePosition = 1
            };
            _context.Add(testgroup);
            _context.SaveChanges();
            var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
            var avgHr = new TestHeader()
            {
                Name = "Average Hr",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 1,
                TestGroupID = tgId,
            };
            var sdnn = new TestHeader()
            {
                Name = "SDNN",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 1,
                TestGroupID = tgId,
            };
            var rnsdd = new TestHeader()
            {
                Name = "RNSDD",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 1,
                TestGroupID = tgId,
            };
            var sdsd = new TestHeader()
            {
                Name = "SDSD",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 1,
                TestGroupID = tgId,
            };
            var file = new TestHeader()
            {
                Name = "File",
                Type = AppUtility.DataTypeEnum.File.ToString(),
                SequencePosition = 1,
                TestGroupID = tgId,
            };
            _context.Add(avgHr);
            _context.Add(sdnn);
            _context.Add(rnsdd);
            _context.Add(sdsd);
            _context.Add(file);
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

        public async Task<ActionResult> _BiomarkersRows(int? ExperimentID, int? ParticipantID)
        {
            List<List<TDViewModel>> BioRows = new List<List<TDViewModel>>();
            if (ExperimentID != null)
            {
                BioRows = await GetParticipantsRows(_context.Participants.Include(p => p.Gender).Include(p => p.ParticipantStatus).Where(p => p.ExperimentID == ExperimentID));
            }
            else if (ParticipantID != null)
            {
                BioRows = await GetEntriesRows(Convert.ToInt32(ParticipantID));
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

        public async Task<ActionResult> Entries(int ParticipantID)
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.HumanTrials;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.BiomarkersExperiments;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Biomarkers;

            return View(await GetEntriesViewModelAsync(ParticipantID));
        }

        public async Task<ActionResult> _Entries(int ParticipantID)
        {
            return PartialView(await GetEntriesViewModelAsync(ParticipantID));
        }

        public async Task<EntriesViewModel> GetEntriesViewModelAsync(int ParticipantID)
        {
            var entriesViewModel = new EntriesViewModel()
            {
                Participant = _context.Participants.Include(p => p.Gender).Include(p => p.ParticipantStatus)
                    .Where(p => p.ParticipantID == ParticipantID).FirstOrDefault(),
                EntryHeaders = await GetEntriesHeaders(),
                EntryRows = await GetEntriesRows(ParticipantID)
            };
            return entriesViewModel;
        }

        public async Task<List<TDViewModel>> GetEntriesHeaders()
        {
            var headersList = new List<TDViewModel>()
            {
                new TDViewModel()
                {
                    Value = "Entry No."
                },
                new TDViewModel()
                {
                    Value = "Created By"
                },
                new TDViewModel()
                {
                    Value = "Date"
                },
                new TDViewModel()
                {
                    Value = "Site"
                },
                new TDViewModel()
                {
                    Value = "Visit No."
                }
            };
            return headersList;
        }

        public async Task<List<List<TDViewModel>>> GetEntriesRows(int ParticipantID)
        {
            List<List<TDViewModel>> rows = new List<List<TDViewModel>>();
            var experimentEntries = _context.ExperimentEntries
                .Include(ee => ee.ApplicationUser).Include(ee => ee.Site)
                .Where(ee => ee.ParticipantID == ParticipantID);
            foreach (var ee in experimentEntries)
            {
                rows.Add(
                    new List<TDViewModel>()
                    {
                        new TDViewModel()
                        {
                             Value = ee.VisitNumber.ToString(),
                             Link = "Test",
                             ID = ee.ExperimentEntryID
                        },
                        new TDViewModel()
                        {
                             Value = ee.ApplicationUser.FirstName
                        },
                        new TDViewModel()
                        {
                             Value = ee.DateTime.GetElixirDateFormat()
                        },
                        new TDViewModel()
                        {
                             Value = ee.Site.Name.ToString()
                        },
                        new TDViewModel()
                        {
                             Value = ee.VisitNumber.ToString()
                        }
                    }
                    );
            }
            return rows;
        }

        public async Task<ActionResult> _NewEntry(int ID)
        {
            var visits = _context.Participants.Where(p => p.ParticipantID == ID).Select(p => p.Experiment.AmountOfVisits).FirstOrDefault();
            visits = visits == 0 || visits == null ? 10 : visits;
            var visitList = new List<SelectListItem>();
            for (int v = 1; v <= visits; v++)
            {
                visitList.Add(new SelectListItem()
                {
                    Text = v.ToString(),
                    Value = v.ToString()
                });
            };
            NewEntryViewModel nevm = new NewEntryViewModel()
            {
                Sites = _context.Sites,
                ParticipantID = ID,
                VisitNumbers = visitList,
                Date = DateTime.Now
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
                ApplicationUserID = _userManager.GetUserId(User),
                SiteID = newEntryViewModel.SiteID
            };

            _context.Add(ee);
            await _context.SaveChangesAsync();

            return RedirectToAction("_BiomarkersRows", newEntryViewModel.ParticipantID);
        }

        [HttpGet]
        public async Task<ActionResult> Test(int ID)
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.HumanTrials;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.BiomarkersExperiments;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Biomarkers;

            var ee = _context.ExperimentEntries.Where(e => e.ExperimentEntryID == ID).Include(ee => ee.Site)
                    .Include(ee => ee.Participant).ThenInclude(p => p.Gender).Include(ee => ee.Participant.ParticipantStatus)
                    .FirstOrDefault();
            var tests = _context.Tests.Where(t => t.SiteID == ee.SiteID)
                    .Include(t => t.TestOuterGroups).ThenInclude(tog => tog.TestGroups).ThenInclude(tg => tg.TestHeaders)
                    .Where(t => t.ExperimentTests.Select(et => et.ExperimentID).Contains(ee.Participant.ExperimentID))
                    .ToList();
            TestViewModel testViewModel = new TestViewModel()
            {
                ExperimentEntry = ee,
                Tests = tests,
                TestValues = _context.TestValues.Include(tv => tv.TestHeader).Where(tv => tv.ExperimentEntryID == ID 
                                && tv.TestHeader.TestGroup.TestOuterGroup.TestID == tests.FirstOrDefault().TestID).ToList()
            };
            return View(testViewModel);
        }

        public async Task<ActionResult> SaveTestModal()
        {
            return PartialView();
        }

        public async Task SaveTests(TestViewModel testViewModel, TestValuesViewModel testValuesViewModel, List<FieldViewModel> fieldViewModels)
        {
            var e = 1;
        }
    }
}
