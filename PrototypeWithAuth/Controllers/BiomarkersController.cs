﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
                //new TDViewModel(){ Value="Timepoints"},
                //new TDViewModel(){ Value="Visits"},
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
                    //new TDViewModel()
                    //{
                    //    Value = e.Timepoints == null ? "0" : e.Timepoints.Count().ToString()
                    //},
                    //new TDViewModel()
                    //{
                    //    Value = e.Timepoints == null ? "0" : e.Timepoints.Select(t => t.AmountOfVisits).Sum().ToString()
                    //},
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
        public ActionResult DocumentsModal(string id, Guid Guid, String RequestFolderNameEnum, bool IsEdittable, bool showSwitch,
            AppUtility.ParentFolderName parentFolderName, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests, string CustomMainObjectID = "0")
        {
            DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
            {
                CustomFolderName = RequestFolderNameEnum,
                FolderName = AppUtility.FolderNamesEnum.Custom,
                CustomMainObjectID = CustomMainObjectID,
                IsEdittable = IsEdittable,
                ParentFolderName = parentFolderName,
                ObjectID = id == "" ? "0" : id,
                SectionType = SectionType,
                ShowSwitch = showSwitch,
                Guid = Guid
            };

            base.FillDocumentsViewModel(documentsModalViewModel);
            return PartialView(documentsModalViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Biomarkers")]
        public void DocumentsModal(DocumentsModalViewModel documentsModalViewModel)
        {
            base.DocumentsModal(documentsModalViewModel);
        }


        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
        public ActionResult _DocumentsModalData(string id, Guid Guid, string RequestFolderNameEnum, bool IsEdittable, bool showSwitch,
                                                AppUtility.MenuItems SectionType = AppUtility.MenuItems.Protocols, AppUtility.ParentFolderName parentFolderName = AppUtility.ParentFolderName.Protocols, bool dontAllowMultipleFiles = false, string CustomMainObjectID = "0")
        {
            DocumentsModalViewModel documentsModalViewModel = new DocumentsModalViewModel()
            {
                FolderName = AppUtility.FolderNamesEnum.Custom,
                CustomFolderName = RequestFolderNameEnum,
                CustomMainObjectID = CustomMainObjectID,
                ParentFolderName = parentFolderName,
                ObjectID = id == "" ? "0" : id,
                SectionType = SectionType,
                IsEdittable = IsEdittable,
                DontAllowMultiple = dontAllowMultipleFiles,
                ShowSwitch = showSwitch,
                Guid = Guid
            };

            base.FillDocumentsViewModel(documentsModalViewModel);
            return PartialView(documentsModalViewModel);
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
                //new TDViewModel()
                //{
                //    Value = "Timepoint"
                //},
                //new TDViewModel()
                //{
                //    Value = "Visit"
                //},
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
            Participants.OrderByDescending(p => p.DateCreated).ToList().ForEach(p =>
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
                        //new TDViewModel()
                        //{
                        //    Value = "0"
                        //},
                        //new TDViewModel()
                        //{
                        //    Value = "0"
                        //},
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
                    ExperimentID = ExperimentID,
                    DOB = DateTime.Now
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

        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
        public ActionResult EditParticipantModal(int ParticipantID, bool IsTestPage)
        {
            AddParticipantViewModel addParticipantViewModel = new AddParticipantViewModel()
            {
                Participant = _context.Participants.Where(p => p.ParticipantID == ParticipantID).Include(p => p.Gender).Include(p => p.ParticipantStatus).FirstOrDefault(),
                Genders = _context.Genders.ToList(),
                ParticipantStatuses = _context.ParticipantStatuses.ToList(),
                DisableFields = true,
                IsTestPage = IsTestPage
            };

            return PartialView(addParticipantViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Biomarkers")]
        public async Task<ActionResult> EditParticipantModal(AddParticipantViewModel editParticipant)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Update(editParticipant.Participant);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
            if (editParticipant.IsTestPage)
            {
                Participant participant = editParticipant.Participant;
                participant.Gender = _context.Genders.Where(g => g.GenderID == participant.GenderID).FirstOrDefault();
                participant.ParticipantStatus = _context.ParticipantStatuses.Where(g => g.ParticipantStatusID == participant.ParticipantStatusID).FirstOrDefault();
                return PartialView("_ParticipantsHeader", participant);
            }
            else
            {
                return RedirectToAction("_Entries", new { ParticipantID = editParticipant.Participant.ParticipantID });
            }
        }

        public async Task<ActionResult> _ParticipantsHeader(int ParticipantID)
        {
            return PartialView(_context.Participants.Where(p => p.ParticipantID == ParticipantID).FirstOrDefault());
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
                .Include(ee => ee.ApplicationUser).Include(ee => ee.Site).OrderByDescending(p => p.DateCreated)
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
            //var lastVisitNum = 0;
            var prevVisitNums = new List<int>();
            if (_context.Participants.Where(p => p.ParticipantID == ID).Select(p => p.ExperimentEntries).Any())
            {
                prevVisitNums = _context.Participants.Where(p => p.ParticipantID == ID).Select(p => p.ExperimentEntries.Select(ee => ee.VisitNumber)).FirstOrDefault().ToList();
                //lastVisitNum = _context.Participants.Where(p => p.ParticipantID == ID).Select(p => p.ExperimentEntries.OrderByDescending(ee => ee.VisitNumber).FirstOrDefault().VisitNumber).FirstOrDefault();
            }

            var visitList = new List<SelectListItem>();
            for (int v = 1; v <= visits; v++)
            {
                if (!prevVisitNums.Contains(v))
                {
                    visitList.Add(new SelectListItem()
                    {
                        Text = v.ToString(),
                        Value = v.ToString()
                    });
                }
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

            return RedirectToAction("_BiomarkersRows", new { ParticipantID = newEntryViewModel.ParticipantID });
        }

        [HttpGet]
        [HttpPost]
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
            var testValues = _context.TestValues.Include(tv => tv.TestHeader).Where(tv => tv.ExperimentEntryID == ID
                                && tv.TestHeader.TestGroup.TestOuterGroup.TestID == tests.FirstOrDefault().TestID).ToList();
            var areFilesFilled = new List<BoolIntViewModel>();
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.ExperimentEntries.ToString());

            var value1 = _context.TestValues.Include(tv => tv.TestHeader).Where(tv => tv.ExperimentEntryID == ID).Count();
            var value2 = tests.SelectMany(t => t.TestOuterGroups/*.Where(tog => tog.TestID == tests.FirstOrDefault().TestID)*/.SelectMany(to => to.TestGroups.SelectMany(tg => tg.TestHeaders))).Count();
            if (value1 < value2)
            {
                testValues = await CreateTestValuesIfNoneAsync(tests, testValues, ee.ExperimentEntryID);
            }
            List<BoolIntViewModel> filesPrevFilled = CheckForFiles(testValues, ee.ExperimentEntryID);
            TestViewModel testViewModel = new TestViewModel()
            {
                ExperimentEntry = ee,
                ExperimentID = ee.Participant.ExperimentID,
                Guid = Guid.NewGuid(),
                Tests = tests,
                TestValues = testValues,
                ExperimentEntries = _context.ExperimentEntries
                                  .Where(ee2 => ee2.ParticipantID == ee.ParticipantID)
                                  .Select(
                                      e => new SelectListItem
                                      {
                                          Text = "Entry " + e.VisitNumber + " - " + e.Site.Name,
                                          Value = e.ExperimentEntryID.ToString()
                                      }
                                  ).ToList(),
                FilesPrevFilled = filesPrevFilled
                //FieldViewModels = new List<FieldViewModel>()
                //{
                //    new FieldViewModel()
                //    {
                //        DataTypeEnum = AppUtility.DataTypeEnum.String,
                //        String = "Hello World",
                //        TestHeader = _context.TestHeaders.FirstOrDefault()
                //    },
                //    new FieldViewModel()
                //    {
                //        DataTypeEnum = AppUtility.DataTypeEnum.Bool,
                //        String = "False",
                //        TestHeader = _context.TestHeaders.FirstOrDefault()
                //    }
                //}
            };
            return View(testViewModel);
        }

        private List<BoolIntViewModel> CheckForFiles(List<TestValue> testValues, int eeID)
        {
            bool hasFile = false;
            List<BoolIntViewModel> FilesWithDocsSaved = new List<BoolIntViewModel>();
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.ExperimentEntries.ToString());
            foreach (var tv in testValues.Where(tv => tv.TestHeader.Type == AppUtility.DataTypeEnum.File.ToString()))
            {
                string uploadFolder2 = Path.Combine(uploadFolder1, eeID.ToString());
                if (Directory.Exists(uploadFolder2))
                {
                    string uploadFolder3 = Path.Combine(uploadFolder2, tv.TestValueID.ToString());
                    if (Directory.Exists(uploadFolder3))
                    {
                        DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolder3);
                        //searching for the partial file name in the directory
                        FileInfo[] docfilesfound = DirectoryToSearch.GetFiles("*.*");
                        if (docfilesfound.Length > 0)
                        {
                            FilesWithDocsSaved.Add(new BoolIntViewModel()
                            {
                                Bool = true,
                                Int = tv.TestHeaderID
                            });
                        }
                    }
                }
            }
            return FilesWithDocsSaved;
        }
        private async Task<List<TestValue>> CreateTestValuesIfNoneAsync(List<Test> tests, List<TestValue> testValues, int ExperimentEntryID)
        {
            var allTests = tests.SelectMany(t => t.TestOuterGroups.SelectMany(tog => tog.TestGroups.SelectMany(tg => tg.TestHeaders)));
            foreach (var testheader in tests.SelectMany(t => t.TestOuterGroups.SelectMany(tog => tog.TestGroups.SelectMany(tg => tg.TestHeaders))))
            {
                if (!testValues.Where(tv => tv.TestHeaderID == testheader.TestHeaderID).Any())
                {
                    TestValue tv = new TestValue()
                    {
                        TestHeaderID = testheader.TestHeaderID,
                        ExperimentEntryID = ExperimentEntryID
                    };
                    _context.Update(tv);

                    testValues.Add(tv);
                }
            }
            await _context.SaveChangesAsync();
            return testValues;
        }

        public async Task<ActionResult> SaveTestModal(string? ID = null, string? GuidString = null)
        {
            var String1 = ID == null ? "none" : ID;
            Guid CurrentGuid;
            Guid.TryParse(GuidString, out CurrentGuid);
            SaveTestViewModel saveTestViewModel = new SaveTestViewModel()
            {
                ID = String1,
                Guid = CurrentGuid
            };
            return PartialView("SaveTestModal", saveTestViewModel);
        }

        public async Task<ActionResult> SaveTests(TestViewModel testViewModel, TestValuesViewModel testValuesViewModel, List<FieldViewModel> fieldViewModels)
        {
            foreach (var fieldTest in testViewModel.FieldViewModels)
            {
                TestValue testValue = new TestValue()
                {
                };
                if (fieldTest.TestValueID != 0)
                {
                    testValue = _context.TestValues.Where(tv => tv.TestValueID == fieldTest.TestValueID).FirstOrDefault();
                }
                else
                {
                    testValue.ExperimentEntryID = testViewModel.ExperimentEntry.ExperimentEntryID;
                    testValue.TestHeaderID = fieldTest.TestHeader.TestHeaderID;
                }
                switch (fieldTest.DataTypeEnum)
                {
                    case AppUtility.DataTypeEnum.Double:
                        testValue.Value = fieldTest.Double.ToString();
                        break;
                    case AppUtility.DataTypeEnum.String:
                        testValue.Value = fieldTest.String;
                        break;
                    case AppUtility.DataTypeEnum.DateTime:
                        testValue.Value = fieldTest.DateTime.ToString();
                        break;
                    case AppUtility.DataTypeEnum.Bool:
                        testValue.Value = fieldTest.Bool.ToString();
                        break;
                    case AppUtility.DataTypeEnum.File:
                        //save file here and save name of file
                        if (fieldTest.File != null)
                        {
                            testValue.Value = fieldTest.File.ToString();
                        }
                        break;
                }
                _context.Update(testValue);
            }
            var entries = _context.ChangeTracker.Entries();
            await _context.SaveChangesAsync();

            SaveFiles(testViewModel.Guid, testViewModel.ExperimentEntry.ExperimentEntryID);

            return RedirectToAction("_TestValues", new { TestID = testViewModel.FieldViewModels.FirstOrDefault().TestID, ListNumber = 0, SiteID = testViewModel.ExperimentEntry.SiteID, ExperimentID = testViewModel.ExperimentID, ExperimentEntryID = testViewModel.ExperimentEntry.ExperimentEntryID });
        }

        private void DeleteFiles(Guid guid, int ExperimentID, List<TestValue> testValues)
        {
            try
            {
                string uploadFolder = Path.Combine("wwwroot", AppUtility.ParentFolderName.ExperimentEntries.ToString());
                string GuidFolder = Path.Combine(uploadFolder, guid.ToString());

                Directory.Delete(GuidFolder, true);
            }
            catch (Exception ex)
            {

            }
        }
        private void SaveFiles(Guid guid, int ExperimentEntryID)
        {
            try
            {
                string uploadFolder = Path.Combine("wwwroot", AppUtility.ParentFolderName.ExperimentEntries.ToString());
                string GuidFolder = Path.Combine(uploadFolder, guid.ToString());
                string IDFolder = Path.Combine(uploadFolder, ExperimentEntryID.ToString());

                var directories = Directory.GetDirectories(GuidFolder);

                if (directories.Count() >= 1 && !Directory.Exists(IDFolder))
                {
                    Directory.CreateDirectory(IDFolder);
                }

                foreach (var directory in directories)
                {
                    var directoryFolderName = AppUtility.GetLastFiles(directory, 1);
                    string SmallerFolder = Path.Combine(IDFolder, directoryFolderName);
                    if (!Directory.Exists(SmallerFolder))
                    {
                        Directory.CreateDirectory(SmallerFolder);
                    }
                    var files = Directory.GetFiles(directory);
                    foreach (var file in files)
                    {
                        var fileName = AppUtility.GetLastFiles(file, 1);
                        var FileToSave = Path.Combine(SmallerFolder, fileName);
                        System.IO.File.Copy(file, FileToSave);
                        System.IO.File.Delete(file);
                    }
                    Directory.Delete(directory);
                }

                Directory.Delete(GuidFolder);
            }
            catch (Exception ex)
            {

            }

        }

        private List<TestValue> GetTestValuesFromTestIDAndExperimentEntryID(int TestID, int ExperimentEntryID)
        {
            return _context.TestValues.Include(tv => tv.TestHeader).Where(tv => tv.ExperimentEntryID == ExperimentEntryID
                              && tv.TestHeader.TestGroup.TestOuterGroup.TestID == TestID).ToList();
        }

        public async Task<ActionResult> CancelTestChanges(Guid CurrentGuid, int TestID, int ListNumber, int SiteID, int ExperimentID, int ExperimentEntryID)
        {
            var testValues = GetTestValuesFromTestIDAndExperimentEntryID(TestID, ExperimentEntryID);
            DeleteFiles(CurrentGuid, ExperimentID, testValues);
            return RedirectToAction("_TestValues", new { TestId = TestID, ListNumber = ListNumber, SiteID = SiteID, ExperimentID = ExperimentID, ExperimentEntryID = ExperimentEntryID });
        }

        public async Task<ActionResult> _TestValues(int TestID, int ListNumber, int SiteID, int ExperimentID, int ExperimentEntryID)
        {
            var test = _context.Tests.Where(t => t.TestID == TestID).FirstOrDefault();
            var tests = _context.Tests.Where(t => t.SiteID == SiteID)
                     .Include(t => t.TestOuterGroups).ThenInclude(tog => tog.TestGroups).ThenInclude(tg => tg.TestHeaders)
                     .Where(t => t.ExperimentTests.Select(et => et.ExperimentID).Contains(ExperimentID))
                     .ToList();
            var testValues = GetTestValuesFromTestIDAndExperimentEntryID(TestID, ExperimentEntryID);
            List<BoolIntViewModel> filesPrevFilled = CheckForFiles(testValues, ExperimentEntryID);
            TestValuesViewModel testValuesViewModel = new TestValuesViewModel()
            {
                ListNumber = ListNumber,
                Test = test,
                TestValues = testValues,
                FilesPrevFilled = filesPrevFilled
            };

            return PartialView(testValuesViewModel);
        }

        public string GetBMI(double weight, double height)
        {
            double value = (weight / (height * height)) * 10000;
            return String.Format("{0:N2}", value);
        }

        [HttpGet]
        private async Task<ActionResult> RunScriptsAsync()
        {
            var testvalues = _context.TestValues;
            foreach (var tv in testvalues)
            {
                _context.Remove(tv);
            }
            _context.SaveChanges();
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
            _context.SaveChanges();
            await _1InsertExeriments();
            await _2InsertSites();
            await _3Add02TestsBloodPressure();
            await _4InsertECGTest();
            await _5InsertAnthropometryTest();
            await _6InsertFlexibilityTest();
            await _7InsertBalanceTest();
            await _8InsertMuscleStrengthTest();
            await _9InsertCPETandSpirometryTest();
            await _10InsertDexaTest();
            await _11InsertImmunochemistryTest();
            await _12InsertBloodChemicalsTest();
            await _13InsertBloodCountTest();
            await _14InsertUltrasoundTest();
            await _15InsertAgeReaderTest();
            await _16InsertProcedureDocTest();
            return RedirectToAction("HumanTrialsList");
        }

        private async Task _1InsertExeriments()
        {
            DateTime startDate;
            DateTime.TryParseExact("20210106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
            DateTime endDate;
            DateTime.TryParseExact("20240106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            Experiment HighFrequency = new Experiment()
            {
                Description = "High Frequency",
                ExperimentCode = "ex1",
                NumberOfParticipants = 40,
                MinimumAge = 30,
                MaximumAge = 80,
                StartDateTime = startDate,
                EndDateTime = endDate
            };
            _context.Add(HighFrequency);
            DateTime.TryParseExact("20210106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
            DateTime.TryParseExact("20240106", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            Experiment LowFrequency = new Experiment()
            {
                Description = "Low Frequency",
                ExperimentCode = "ex2",
                NumberOfParticipants = 120,
                MinimumAge = 20,
                MaximumAge = 80,
                StartDateTime = startDate,
                EndDateTime = endDate
            };
            _context.Add(LowFrequency);
            _context.SaveChanges();
        }

        private async Task _2InsertSites()
        {
            Site Centarix = new Site()
            {
                Name = "Centarix Biotech",
                Line1Address = "Hamarpe 3",
                City = "Har Hotzvim",
                Country = "Jerusalem",
                PrimaryContactID = _context.Users.Where(u => u.Email == "rachelstrauss@centarix.com").FirstOrDefault().Id,
                PhoneNumber = "077-2634302"
            };
            _context.Add(Centarix);
            Site O2 = new Site()
            {
                Name = "O2",
                Line1Address = "",
                City = "Har Hazofim",
                Country = "Jerusalem",
                PrimaryContactID = _context.Users.Where(u => u.Email == "rachelstrauss@centarix.com").FirstOrDefault().Id,
                PhoneNumber = "055-9876543"
            };
            _context.Add(O2);
            _context.SaveChanges();
        }

        private async Task _3Add02TestsBloodPressure()
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

        private async Task _4InsertECGTest()
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
                SequencePosition = 2,
                TestGroupID = tgId,
            };
            var rnsdd = new TestHeader()
            {
                Name = "RNSDD",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 3,
                TestGroupID = tgId,
            };
            var sdsd = new TestHeader()
            {
                Name = "SDSD",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 4,
                TestGroupID = tgId,
            };
            var file = new TestHeader()
            {
                Name = "File",
                Type = AppUtility.DataTypeEnum.File.ToString(),
                SequencePosition = 5,
                TestGroupID = tgId,
            };
            _context.Add(avgHr);
            _context.Add(sdnn);
            _context.Add(rnsdd);
            _context.Add(sdsd);
            _context.Add(file);
            _context.SaveChanges();
        }

        private async Task _5InsertAnthropometryTest()
        {
            try
            {

                Test test = new Test()
                {
                    Name = "Anthropometry",
                    SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Anthropometry").Select(t => t.TestID).FirstOrDefault();
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
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var weight = new TestHeader()
                {
                    Name = "Weight (kg)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                var height = new TestHeader()
                {
                    Name = "Height (cm)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                var bodyFat = new TestHeader()
                {
                    Name = "Body Fat (%)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 3,
                    TestGroupID = tgId,
                };
                var visceralFat = new TestHeader()
                {
                    Name = "Visceral Fat (%)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 4,
                    TestGroupID = tgId,
                };
                var muscleMass = new TestHeader()
                {
                    Name = "Muscle Mass (kg)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 5,
                    TestGroupID = tgId,
                };
                var skip = new TestHeader()
                {
                    IsSkip = true,
                    SequencePosition = 6,
                    TestGroupID = tgId
                };
                var waistCircumferance = new TestHeader()
                {
                    Name = "Waist Circumference (cm)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 7,
                    TestGroupID = tgId
                };
                var underBellyCircumferance = new TestHeader()
                {
                    Name = "Under Belly Circumference (cm)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 8,
                    TestGroupID = tgId
                };
                var bmi = new TestHeader()
                {
                    Name = "BMI",
                    Calculation = AppUtility.DataCalculation.BMI.ToString(),
                    SequencePosition = 9,
                    TestGroupID = tgId
                };
                _context.Update(weight);
                _context.Update(height);
                _context.Update(bodyFat);
                _context.Update(visceralFat);
                _context.Update(muscleMass);
                _context.Update(skip);
                _context.Update(waistCircumferance);
                _context.Update(underBellyCircumferance);
                _context.Update(bmi);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _6InsertFlexibilityTest()
        {
            Test test = new Test()
            {
                Name = "Flexibility",
                SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
            };
            _context.Add(test);
            _context.SaveChanges();
            var testId = _context.Tests.Where(t => t.Name == "Flexibility").Select(t => t.TestID).FirstOrDefault();
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
            var togID = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                         .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID;
            var testgroup1 = new TestGroup()
            {
                Name = "First Measure",
                TestOuterGroupID = togID,
                SequencePosition = 1
            };
            _context.Add(testgroup1);
            _context.SaveChanges();
            var testgroup2 = new TestGroup()
            {
                Name = "Second Measure",
                TestOuterGroupID = togID,
                SequencePosition = 2
            };
            _context.Add(testgroup2);
            _context.SaveChanges();
            var testgroup3 = new TestGroup()
            {
                Name = "Third Measure",
                TestOuterGroupID = togID,
                SequencePosition = 3
            };
            _context.Add(testgroup3);
            _context.SaveChanges();
            var tgId1 = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                     .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
            var tgId2 = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                     .Where(tg => tg.SequencePosition == 2).Select(tg => tg.TestGroupID).FirstOrDefault();
            var tgId3 = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                     .Where(tg => tg.SequencePosition == 3).Select(tg => tg.TestGroupID).FirstOrDefault();
            var distance1 = new TestHeader()
            {
                Name = "Distance from toes (cm)",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 1,
                TestGroupID = tgId1,
            };
            var distance2 = new TestHeader()
            {
                Name = "Distance from toes (cm)",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 2,
                TestGroupID = tgId2,
            };
            var distance3 = new TestHeader()
            {
                Name = "Distance from toes (cm)",
                Type = AppUtility.DataTypeEnum.Double.ToString(),
                SequencePosition = 3,
                TestGroupID = tgId3,
            };
            _context.Add(distance1);
            _context.Add(distance2);
            _context.Add(distance3);
            _context.SaveChanges();
        }

        private async Task _7InsertBalanceTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Balance",
                    SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Balance").Select(t => t.TestID).FirstOrDefault();
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
                var eyesOpen = new TestOuterGroup()
                {
                    Name = "Eyes Open",
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(eyesOpen);
                var eyesClosed = new TestOuterGroup()
                {
                    Name = "Eyes Closed",
                    TestID = testId,
                    SequencePosition = 2
                };
                _context.Add(eyesClosed);
                _context.SaveChanges();
                var eyesOpenId = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                             .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID;
                var eyesClosedId = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                             .Where(tog => tog.SequencePosition == 2).FirstOrDefault().TestOuterGroupID;
                var eyesopenleftleg = new TestGroup()
                {
                    Name = "Left Leg",
                    TestOuterGroupID = eyesOpenId,
                    SequencePosition = 1
                };
                var eyesopenrightleg = new TestGroup()
                {
                    Name = "Right Leg",
                    TestOuterGroupID = eyesOpenId,
                    SequencePosition = 2
                };
                _context.Add(eyesopenleftleg);
                _context.Add(eyesopenrightleg);
                _context.SaveChanges();
                var eyesclosedleftleg = new TestGroup()
                {
                    Name = "Left Leg",
                    TestOuterGroupID = eyesClosedId,
                    SequencePosition = 1
                };
                var eyesclosedrightleg = new TestGroup()
                {
                    Name = "Right Leg",
                    TestOuterGroupID = eyesClosedId,
                    SequencePosition = 2
                };
                _context.Add(eyesclosedleftleg);
                _context.Add(eyesclosedrightleg);
                _context.SaveChanges();
                var eyesopenleftlegid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 1 && tg.TestOuterGroupID == eyesOpenId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var eyesopenrightlegid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 2 && tg.TestOuterGroupID == eyesOpenId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var eyesclosedleftlegid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 1 && tg.TestOuterGroupID == eyesClosedId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var eyesclosedrightlegid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 2 && tg.TestOuterGroupID == eyesClosedId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var firstmeasure1 = new TestHeader()
                {
                    Name = "First Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = eyesopenleftlegid,
                };
                var secondmeasure1 = new TestHeader()
                {
                    Name = "Second Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = eyesopenleftlegid,
                };
                var firstmeasure2 = new TestHeader()
                {
                    Name = "First Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = eyesopenrightlegid,
                };
                var secondmeasure2 = new TestHeader()
                {
                    Name = "Second Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = eyesopenrightlegid,
                };
                var firstmeasure3 = new TestHeader()
                {
                    Name = "First Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = eyesclosedleftlegid,
                };
                var secondmeasure3 = new TestHeader()
                {
                    Name = "Second Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = eyesclosedleftlegid,
                };
                var firstmeasure4 = new TestHeader()
                {
                    Name = "First Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = eyesclosedrightlegid,
                };
                var secondmeasure4 = new TestHeader()
                {
                    Name = "Second Measure (sec)",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = eyesclosedrightlegid,
                };
                _context.Add(firstmeasure1);
                _context.Add(firstmeasure2);
                _context.Add(firstmeasure3);
                _context.Add(firstmeasure4);
                _context.Add(secondmeasure1);
                _context.Add(secondmeasure2);
                _context.Add(secondmeasure3);
                _context.Add(secondmeasure4);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _8InsertMuscleStrengthTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Muscle Strength",
                    SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Muscle Strength").Select(t => t.TestID).FirstOrDefault();
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
                var handgrip = new TestOuterGroup()
                {
                    Name = "Handgrip",
                    TestID = testId,
                    SequencePosition = 1
                };
                _context.Add(handgrip);
                var legextension = new TestOuterGroup()
                {
                    Name = "Leg Extension",
                    TestID = testId,
                    SequencePosition = 2
                };
                _context.Add(legextension);
                _context.SaveChanges();
                var handgripId = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                             .Where(tog => tog.SequencePosition == 1).FirstOrDefault().TestOuterGroupID;
                var legextesnionId = _context.TestOuterGroups.Where(tog => tog.TestID == testId)
                             .Where(tog => tog.SequencePosition == 2).FirstOrDefault().TestOuterGroupID;
                var lefthandhandgrip = new TestGroup()
                {
                    Name = "Left Hand",
                    TestOuterGroupID = handgripId,
                    SequencePosition = 1
                };
                var righthandhandgrip = new TestGroup()
                {
                    Name = "Right Hand",
                    TestOuterGroupID = handgripId,
                    SequencePosition = 2
                };
                _context.Add(lefthandhandgrip);
                _context.Add(righthandhandgrip);
                _context.SaveChanges();
                var leftleglegextension = new TestGroup()
                {
                    Name = "Left Leg",
                    TestOuterGroupID = legextesnionId,
                    SequencePosition = 1
                };
                var rightleglegextension = new TestGroup()
                {
                    Name = "Right Leg",
                    TestOuterGroupID = legextesnionId,
                    SequencePosition = 2
                };
                _context.Add(leftleglegextension);
                _context.Add(rightleglegextension);
                _context.SaveChanges();
                var lefthandhandgripid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 1 && tg.TestOuterGroupID == handgripId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var righthandhandgripid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 2 && tg.TestOuterGroupID == handgripId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var leftleglegextensionid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 1 && tg.TestOuterGroupID == legextesnionId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var rightleglegextensionid = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId).Where(tg => tg.SequencePosition == 2 && tg.TestOuterGroupID == legextesnionId).Select(tg => tg.TestGroupID).FirstOrDefault();
                var firstmeasure1 = new TestHeader()
                {
                    Name = "First Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = lefthandhandgripid,
                };
                var secondmeasure1 = new TestHeader()
                {
                    Name = "Second Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = lefthandhandgripid,
                };
                var firstmeasure2 = new TestHeader()
                {
                    Name = "First Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = righthandhandgripid,
                };
                var secondmeasure2 = new TestHeader()
                {
                    Name = "Second Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = righthandhandgripid,
                };
                var firstmeasure3 = new TestHeader()
                {
                    Name = "First Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = leftleglegextensionid,
                };
                var secondmeasure3 = new TestHeader()
                {
                    Name = "Second Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = leftleglegextensionid,
                };
                var firstmeasure4 = new TestHeader()
                {
                    Name = "First Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = rightleglegextensionid,
                };
                var secondmeasure4 = new TestHeader()
                {
                    Name = "Second Measure",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = rightleglegextensionid,
                };
                _context.Add(firstmeasure1);
                _context.Add(firstmeasure2);
                _context.Add(firstmeasure3);
                _context.Add(firstmeasure4);
                _context.Add(secondmeasure1);
                _context.Add(secondmeasure2);
                _context.Add(secondmeasure3);
                _context.Add(secondmeasure4);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _9InsertCPETandSpirometryTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "CPET and Spirometry",
                    SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                await _context.SaveChangesAsync();
                var testId = _context.Tests.Where(t => t.Name == "CPET and Spirometry").Select(t => t.TestID).FirstOrDefault();
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
                _context.SaveChanges(); var testoutergroup = new TestOuterGroup()
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
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file = new TestHeader()
                {
                    Name = "File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _10InsertDexaTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Dexa",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Dexa").Select(t => t.TestID).FirstOrDefault();
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
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file1 = new TestHeader()
                {
                    Name = "Results File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file1);
                await _context.SaveChangesAsync();
                var file2 = new TestHeader()
                {
                    Name = "Procedure File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(file2);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _11InsertImmunochemistryTest()
        {
            try
            {

                Test test = new Test()
                {
                    Name = "Cognifit",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Cognifit").Select(t => t.TestID).FirstOrDefault();
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
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file1 = new TestHeader()
                {
                    Name = "Results File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file1);
                await _context.SaveChangesAsync();
                var file2 = new TestHeader()
                {
                    Name = "Procedure File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(file2);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }


        private async Task _12InsertBloodChemicalsTest()
        {
            try
            {

                Test test = new Test()
                {
                    Name = "Blood Chemicals",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Blood Chemicals").Select(t => t.TestID).FirstOrDefault();
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
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file1 = new TestHeader()
                {
                    Name = "Results File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file1);
                await _context.SaveChangesAsync();
                var file2 = new TestHeader()
                {
                    Name = "Procedure File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(file2);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }


        private async Task _13InsertBloodCountTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Blood Count",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Blood Count").Select(t => t.TestID).FirstOrDefault();
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
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file1 = new TestHeader()
                {
                    Name = "Results File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file1);
                await _context.SaveChangesAsync();
                var file2 = new TestHeader()
                {
                    Name = "Procedure File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(file2);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public bool CheckUniqueCentarixID(string CentarixID, int? ParticipantID)
        {
            var boolCheck = true;
            //validation for create

            if (CentarixID != null && ParticipantID == null && _context.Participants.Where(r => r.CentarixID == CentarixID).Any())
            {
                boolCheck = false;
            }
            //validation for edit
            else if (CentarixID != null && ParticipantID != null && _context.Participants.Where(p => p.CentarixID == CentarixID && p.ParticipantID != ParticipantID).Any())
            {
                boolCheck = false;
            }
            return boolCheck;
        }

        private async Task _14InsertUltrasoundTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Ultrasound",
                    SiteID = _context.Sites.Where(s => s.Name == "O2").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Ultrasound").Select(t => t.TestID).FirstOrDefault();
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
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var file1 = new TestHeader()
                {
                    Name = "Echogardiagram File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(file1);
                await _context.SaveChangesAsync();
                var file2 = new TestHeader()
                {
                    Name = "cIMT File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(file2);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _15InsertAgeReaderTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Age Reader",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Age Reader").Select(t => t.TestID).FirstOrDefault();
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
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var rightarm = new TestHeader()
                {
                    Name = "Right Arm",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(rightarm);
                await _context.SaveChangesAsync();
                var leftarm = new TestHeader()
                {
                    Name = "Left Arm",
                    Type = AppUtility.DataTypeEnum.Double.ToString(),
                    SequencePosition = 2,
                    TestGroupID = tgId,
                };
                _context.Add(leftarm);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }


        private async Task _16InsertProcedureDocTest()
        {
            try
            {
                Test test = new Test()
                {
                    Name = "Procedure Document",
                    SiteID = _context.Sites.Where(s => s.Name == "Centarix Biotech").Select(s => s.SiteID).FirstOrDefault()
                };
                _context.Add(test);
                _context.SaveChanges();
                var testId = _context.Tests.Where(t => t.Name == "Procedure Document").Select(t => t.TestID).FirstOrDefault();
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
                var tgId = _context.TestGroups.Where(tg => tg.TestOuterGroup.TestID == testId)
                    .Where(tg => tg.SequencePosition == 1).Select(tg => tg.TestGroupID).FirstOrDefault();
                var procDoc = new TestHeader()
                {
                    Name = "File",
                    Type = AppUtility.DataTypeEnum.File.ToString(),
                    SequencePosition = 1,
                    TestGroupID = tgId,
                };
                _context.Add(procDoc);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

    }
}