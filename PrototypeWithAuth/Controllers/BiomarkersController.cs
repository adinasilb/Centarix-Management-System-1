using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.Controllers
{
    public class BiomarkersController : SharedController
    {
        public BiomarkersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
              : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
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

            _experimentsProc.Read().ToList().ForEach(e =>
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
        public new void DocumentsModal(DocumentsModalViewModel documentsModalViewModel)
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
                Experiment = await _experimentsProc.ReadOneAsync(new List<Expression<Func<Experiment, bool>>> { e => e.ExperimentID == ID }),
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
                Experiment = await _experimentsProc.ReadOneAsync(new List<Expression<Func<Experiment, bool>>> { e => e.ExperimentID == ID },
                new List<ComplexIncludes<Experiment, ModelBase>>
                {
                    new ComplexIncludes<Experiment, ModelBase>
                    {
                        Include = e => e.Participants ,
                        ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{Include = p => ((Participant)p).Gender }},
                    new ComplexIncludes<Experiment, ModelBase>{
                        Include = e => e.Participants ,
                        ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{Include = p => ((Participant)p).ParticipantStatus }},
                })
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
                BioRows = await GetParticipantsRows(_participantsProc.Read(new List<Expression<Func<Participant, bool>>> { p => p.ExperimentID == ExperimentID },
                    new List<ComplexIncludes<Participant, ModelBase>>
                    {
                        new ComplexIncludes<Participant, ModelBase>{Include = p => p.Gender},
                        new ComplexIncludes<Participant, ModelBase>{Include = p => p.ParticipantStatus}
                    }));
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
            Participants.OrderByDescending(p => p.DateCreated).ToList();
            foreach (var p in Participants)
            {
                var row = new List<TDViewModel>();
                var vm1 = new TDViewModel()
                {
                    Value = p.CentarixID,
                    AjaxLink = "open-participant-entries",
                    ID = p.ParticipantID
                };
                row.Add(vm1);
                var vm2 = new TDViewModel()
                {
                    Value = p.DOB.GetElixirDateFormat()
                };
                row.Add(vm2);
                var vm3 = new TDViewModel()
                {
                    Value = p.Gender.Description
                };
                row.Add(vm3);
                //new TDViewModel()
                //{
                //    Value = "0"
                //},
                //new TDViewModel()
                //{
                //    Value = "0"
                //},
                var vm4 = new TDViewModel()
                {
                    Value = p.ParticipantStatus.Description
                };
                row.Add(vm4);

                rows.Add(row);
            }
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
                Genders = _gendersProc.Read().ToList()

            };

            return PartialView(addParticipantViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Biomarkers")]
        public async Task<ActionResult> AddParticipantModal(AddParticipantViewModel addParticipant)
        {
            var success = await _participantsProc.CreateAsync(addParticipant.Participant);
            return RedirectToAction("_BiomarkersRows", new { ExperimentID = addParticipant.Participant.ExperimentID });
        }

        [HttpGet]
        [Authorize(Roles = "Biomarkers")]
        public async Task<ActionResult> EditParticipantModal(int ParticipantID, bool IsTestPage)
        {
            AddParticipantViewModel addParticipantViewModel = new AddParticipantViewModel()
            {
                Participant = await _participantsProc.ReadOneAsync(new List<Expression<Func<Participant, bool>>> { p => p.ParticipantID == ParticipantID },
                    new List<ComplexIncludes<Participant, ModelBase>> {
                        new ComplexIncludes<Participant, ModelBase>{Include = p => p.Gender},
                        new ComplexIncludes<Participant, ModelBase>{Include = p => p.ParticipantStatus}
                }),
                Genders = _gendersProc.Read().ToList(),
                ParticipantStatuses = _participantStatusesProc.Read().ToList(),
                DisableFields = true,
                IsTestPage = IsTestPage
            };

            return PartialView(addParticipantViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Biomarkers")]
        public async Task<ActionResult> EditParticipantModal(AddParticipantViewModel editParticipant)
        {
            await _participantsProc.UpdateAsync(editParticipant.Participant);
            if (editParticipant.IsTestPage)
            {
                Participant participant = editParticipant.Participant;
                participant.Gender = await _gendersProc.ReadOneAsync(new List<Expression<Func<Gender, bool>>>
                     { g => g.GenderID == participant.GenderID });
                participant.ParticipantStatus =
                    await _participantStatusesProc.ReadOneAsync(new List<Expression<Func<ParticipantStatus, bool>>>
                    { ps => ps.ParticipantStatusID == participant.ParticipantStatusID });
                return PartialView("_ParticipantsHeader", participant);
            }
            else
            {
                return RedirectToAction("_Entries", new { ParticipantID = editParticipant.Participant.ParticipantID });
            }
        }

        public async Task<ActionResult> _ParticipantsHeader(int ParticipantID)
        {
            return PartialView(await _participantsProc.ReadOneAsync(new List<Expression<Func<Participant, bool>>> { p => p.ParticipantID == ParticipantID }));
        }

        public async Task<int> GetParticipantsCount(int ExperimentID)
        {
            return (_participantsProc.Read(new List<Expression<Func<Participant, bool>>> { p => p.ExperimentID == ExperimentID }).Count());
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
                Participant = await _participantsProc.ReadOneAsync(new List<Expression<Func<Participant, bool>>> { p => p.ParticipantID == ParticipantID },
                new List<ComplexIncludes<Participant, ModelBase>> {
                    new ComplexIncludes<Participant, ModelBase>{Include = p => p.ParticipantStatus },
                    new ComplexIncludes<Participant, ModelBase>{Include = p => p.Gender }
                     }),

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
            var experimentEntries = _experimentEntriesProc.Read(new List<Expression<Func<ExperimentEntry, bool>>> { ee => ee.ParticipantID == ParticipantID },
                new List<ComplexIncludes<ExperimentEntry, ModelBase>> {
                    new ComplexIncludes<ExperimentEntry, ModelBase> { Include = ee => ee.Site },
                    new ComplexIncludes<ExperimentEntry, ModelBase> { Include = ee => ee.ApplicationUser }
                });
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
            //var visits =
            var visits = _participantsProc.Read(new List<Expression<Func<Participant, bool>>> { p => p.ParticipantID == ID }).Select(p => p.Experiment.AmountOfVisits).FirstOrDefault();
            visits = visits == 0 ? 10 : visits;
            //var lastVisitNum = 0;
            var prevVisitNums = new List<int>();
            if (_participantsProc.Read(new List<Expression<Func<Participant, bool>>> { p => p.ParticipantID == ID }).Select(p => p.Experiment).Any())
            {
                prevVisitNums = _participantsProc.Read(new List<Expression<Func<Participant, bool>>> { p => p.ParticipantID == ID }).Select(p => p.ExperimentEntries.Select(ee => ee.VisitNumber)).FirstOrDefault().ToList();
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
                Sites = _sitesProc.Read(),
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
            await _experimentEntriesProc.CreateAsync(ee);

            return RedirectToAction("_BiomarkersRows", new { ParticipantID = newEntryViewModel.ParticipantID });
        }

        [HttpGet]
        [HttpPost]
        public async Task<ActionResult> Test(int ID)
        {
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.HumanTrials;
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.BiomarkersExperiments;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Biomarkers;

            var ee = await _experimentEntriesProc.ReadOneAsync(new List<Expression<Func<ExperimentEntry, bool>>> { e => e.ExperimentEntryID == ID },
                new List<ComplexIncludes<ExperimentEntry, ModelBase>>
                {
                    new ComplexIncludes<ExperimentEntry, ModelBase> {Include = ee => ee.Site},
                    new ComplexIncludes<ExperimentEntry, ModelBase> {Include = ee => ee.Participant, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = p => ((Participant)p).Gender } },
                    new ComplexIncludes<ExperimentEntry, ModelBase> {Include = ee => ee.Participant, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = p => ((Participant)p).ParticipantStatus} }
                });
            var tests = _testsProc.Read(new List<Expression<Func<Test, bool>>> { t => t.SiteID == ee.SiteID, t => t.ExperimentTests.Select(et => et.ExperimentID).Contains(ee.Participant.ExperimentID) },
                new List<ComplexIncludes<Test, ModelBase>> {
                    new ComplexIncludes<Test, ModelBase>{Include = t => t.TestOuterGroups, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>
                        { Include = tog => ((TestOuterGroup)tog).TestGroups, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = tg => ((TestGroup)tg).TestHeaders } } }
                }).ToList();
            var testValues = _testValuesProc.Read(new List<Expression<Func<TestValue, bool>>> { tv => tv.ExperimentEntryID == ID && tv.TestHeader.TestGroup.TestOuterGroup.TestID == tests.FirstOrDefault().TestID },
                new List<ComplexIncludes<TestValue, ModelBase>> { new ComplexIncludes<TestValue, ModelBase> { Include = tv => tv.TestHeader } }).ToList();
            var areFilesFilled = new List<BoolIntViewModel>();
            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, AppUtility.ParentFolderName.ExperimentEntries.ToString());

            var value1 = _testValuesProc.Read(new List<Expression<Func<TestValue, bool>>> { tv => tv.ExperimentEntryID == ID }).Count();
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
                ExperimentEntries = _experimentEntriesProc.Read(new List<Expression<Func<ExperimentEntry, bool>>> { ee2 => ee2.ParticipantID == ee.ParticipantID })
                                  .Select(
                                      e => new SelectListItem
                                      {
                                          Text = "Entry " + e.VisitNumber + " - " + e.Site.Name,
                                          Value = e.ExperimentEntryID.ToString()
                                      }
                                  ).ToList(),
                FilesPrevFilled = filesPrevFilled
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

            var save = await _testValuesProc.SaveAsync(testViewModel);
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
            return _testValuesProc.Read(new List<Expression<Func<TestValue, bool>>> { tv => tv.ExperimentEntryID == ExperimentEntryID && tv.TestHeader.TestGroup.TestOuterGroup.TestID == TestID },
                new List<ComplexIncludes<TestValue, ModelBase>> { new ComplexIncludes<TestValue, ModelBase> { Include = tv => tv.TestHeader } }).ToList();
        }

        public async Task<ActionResult> CancelTestChanges(Guid CurrentGuid, int TestID, int ListNumber, int SiteID, int ExperimentID, int ExperimentEntryID)
        {
            var testValues = GetTestValuesFromTestIDAndExperimentEntryID(TestID, ExperimentEntryID);
            DeleteFiles(CurrentGuid, ExperimentID, testValues);
            return RedirectToAction("_TestValues", new { TestId = TestID, ListNumber = ListNumber, SiteID = SiteID, ExperimentID = ExperimentID, ExperimentEntryID = ExperimentEntryID });
        }

        public async Task<ActionResult> _TestValues(int TestID, int ListNumber, int SiteID, int ExperimentID, int ExperimentEntryID)
        {
            var test = await _testsProc.ReadOneAsync(new List<Expression<Func<Test, bool>>> { t => t.TestID == TestID },
                new List<ComplexIncludes<Test, ModelBase>> {
                    new ComplexIncludes<Test, ModelBase>
                    {
                        Include = t => t.TestOuterGroups, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>
                        {
                            Include = tog => ((TestOuterGroup)tog).TestGroups, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>
                            {
                                Include = tg => ((TestGroup)tg).TestHeaders
                            }
                        }
                    }
                });
            var tests = _testsProc.Read(new List<Expression<Func<Test, bool>>> { t => t.SiteID == SiteID && t.ExperimentTests.Select(et => et.ExperimentID).Contains(ExperimentID) },
                new List<ComplexIncludes<Test, ModelBase>> {
                    new ComplexIncludes<Test, ModelBase>
                    {
                        Include = t => t.TestOuterGroups, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>
                        {
                            Include = tog => ((TestOuterGroup)tog).TestGroups, ThenInclude = new ComplexIncludes<ModelBase, ModelBase>
                            {
                                Include = tg => ((TestGroup)tg).TestHeaders
                            }
                        }
                    }
                }).ToList();
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
        public async Task<ActionResult> RunScriptsAsync()
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {



                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return RedirectToAction("HumanTrialsList");
        }


        public bool CheckUniqueCentarixID(string CentarixID, int? ParticipantID)
        {
            var boolCheck = true;
            //validation for create

            if (CentarixID != null && ParticipantID == null && _participantsProc.Read(new List<Expression<Func<Participant, bool>>> { p => p.CentarixID == CentarixID }).Any())
            {
                boolCheck = false;
            }
            //validation for edit
            else if (CentarixID != null && ParticipantID != null &&
                _participantsProc.Read(new List<Expression<Func<Participant, bool>>> { p => p.CentarixID == CentarixID && p.ParticipantID != ParticipantID }).Any())
            {
                boolCheck = false;
            }
            return boolCheck;
        }

        [HttpPost]
        public string UploadFile(DocumentsModalViewModel documentsModalViewModel)
        {
            return base.UploadFile(documentsModalViewModel);
        }

    }
}