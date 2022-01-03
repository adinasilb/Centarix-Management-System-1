using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Authorization;
using PrototypeWithAuth.Models;
using Microsoft.AspNetCore.Identity;
using Abp.Threading.Extensions;
using PrototypeWithAuth.ViewModels;
using static PrototypeWithAuth.AppData.AppUtility;

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Linq.Expressions;
using PrototypeWithAuth.AppData.UtilityModels;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrototypeWithAuth.Controllers
{
    public class ApplicationUsersController : SharedController
    {
        
        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, ICompositeViewEngine viewEngine)
            : base(context, userManager, hostingEnvironment, viewEngine, httpContextAccessor)
        {
        }
        // GET: /<controller>/
        [HttpGet]

        [Authorize(Roles = "Requests, Operations")]
        public async Task<IActionResult> Index(AppUtility.PageTypeEnum PageType = AppUtility.PageTypeEnum.RequestRequest, AppUtility.MenuItems SectionType = AppUtility.MenuItems.Requests)
        {
            if (await base.IsAuthorizedAsync(SectionType))
            {

                var categoryType = 1;
                if (SectionType == AppUtility.MenuItems.Operations)
                {
                    categoryType = 2;
                }
                TempData["CategoryType"] = categoryType == 1 ? AppUtility.CategoryTypeEnum.Lab : AppUtility.CategoryTypeEnum.Operations;
                if (categoryType == 1)
                {
                    TempData[AppUtility.TempDataTypes.PageType.ToString()] = PageType;
                    TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Owner;
                    TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Requests;
                }
                if (categoryType == 2)
                {
                    TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Operations;
                    TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Owner;
                    if (PageType == AppUtility.PageTypeEnum.RequestRequest || PageType == AppUtility.PageTypeEnum.OperationsRequest)
                    {
                        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.OperationsRequest;
                    }
                    else if (PageType == AppUtility.PageTypeEnum.RequestInventory || PageType == AppUtility.PageTypeEnum.OperationsInventory)
                    {
                        TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.OperationsInventory;
                    }
                }
                return View(await _employeesProc.Read(new List<Expression<Func<Employee, bool>>> { u => !u.IsSuspended }).ToListAsync());

            }
            else
            {
                return Redirect(base.AccessDeniedPath);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> Details()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersWorkers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Details;
            EmployeeDetailsViewModel employeeDetailsViewModel = GetWorkersDetailsViewModel();

            return View(employeeDetailsViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> _Details()
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            EmployeeDetailsViewModel employeeDetailsViewModel = GetWorkersDetailsViewModel();

            return PartialView(employeeDetailsViewModel);
        }
        private EmployeeDetailsViewModel GetWorkersDetailsViewModel()
        {
            var employees =  _employeesProc.Read(new List<Expression<Func<Employee, bool>>> { u => u.EmployeeStatusID != 4, u => !u.IsSuspended },
                new List<ComplexIncludes<Employee, ModelBase>> {
                    new ComplexIncludes<Employee, ModelBase> { Include = e => e.EmployeeStatus},
                    new ComplexIncludes<Employee, ModelBase> { Include = e => e.SalariedEmployee},
                    new ComplexIncludes<Employee, ModelBase> { 
                        Include = e => e.JobSubcategoryType,
                        ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = js => ((JobSubcategoryType)js).JobCategoryType } }
                });
            var centarixIDs = _centarixIDsProc.Read();
            //var ed = employeeDetails.OrderBy(ed => ed.Employee.UserNum);
            EmployeeDetailsViewModel employeeDetailsViewModel = new EmployeeDetailsViewModel
            {
                Employees = employees.Select(u => new UserWithCentarixIDViewModel
                {
                    Employee = u,
                    CentarixID = AppUtility.GetEmployeeCentarixID( centarixIDs.Where(ci => ci.EmployeeID == u.Id).OrderBy(ci => ci.TimeStamp))
                }).OrderBy(ed => ed.Employee.UserNum),
                SalariedEmployeeCount = employees.Where(e => e.EmployeeStatusID == 1).Count(),
                FreelancerCount = employees.Where(e => e.EmployeeStatusID == 2).Count(),
                AdvisorCount = employees.Where(e => e.EmployeeStatusID == 3).Count(),
            };
            return employeeDetailsViewModel;
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> Hours(YearlyMonthlyEnum yearlyMonthlyEnum = YearlyMonthlyEnum.Monthly, int month = 0, int year = 0)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersWorkers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Hours;

            WorkersHoursViewModel viewModel = await hoursPagePopulate(yearlyMonthlyEnum, month, year);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> _Hours(YearlyMonthlyEnum yearlyMonthlyEnum = YearlyMonthlyEnum.Monthly, int month = 0, int year = 0, int amountInYear = 0)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            WorkersHoursViewModel viewModel = await hoursPagePopulate(yearlyMonthlyEnum, month, year, amountInYear);
            return PartialView(viewModel);
        }
        private async Task<WorkersHoursViewModel> hoursPagePopulate(YearlyMonthlyEnum yearlyMonthlyEnum = YearlyMonthlyEnum.Monthly, int month = 0, int year = 0, int amountInYear = 0)
        {
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }

            var employees = _employeesProc.Read(new List<Expression<Func<Employee, bool>>> { u => u.EmployeeStatusID != 4, u => !u.IsSuspended },
                new List<ComplexIncludes<Employee, ModelBase>> {
                    new ComplexIncludes<Employee, ModelBase> { Include = e => e.EmployeeStatus},
                    new ComplexIncludes<Employee, ModelBase> { Include = e => e.SalariedEmployee},
                    new ComplexIncludes<Employee, ModelBase> {
                        Include = e => e.JobSubcategoryType,
                        ThenInclude = new ComplexIncludes<ModelBase, ModelBase>{ Include = js => ((JobSubcategoryType)js).JobCategoryType } }
                }).Select(e => new
                {
                 Employee = e,
                 EmployeeHours = e.EmployeeHours.Where(eh => (yearlyMonthlyEnum == YearlyMonthlyEnum.Monthly && (eh.Date.Year == year && eh.Date.Month == month && eh.Date.Date < DateTime.Now.Date))
                 || ((yearlyMonthlyEnum == YearlyMonthlyEnum.Yearly && (eh.Date.Year == year && eh.Date.Date < DateTime.Now.Date)))),
                 EmployeeHoursAwaitingApproval = e.EmployeeHours.Where(eh => (yearlyMonthlyEnum == YearlyMonthlyEnum.Monthly && (eh.Date.Year == year && eh.Date.Month == month && eh.Date.Date < DateTime.Now.Date))
                 || ((yearlyMonthlyEnum == YearlyMonthlyEnum.Yearly && (eh.Date.Year == year && eh.Date.Date < DateTime.Now.Date)))).Select(eh => eh.EmployeeHoursAwaitingApproval).Where(eha => eha != null && !eha.IsDenied)

                });



            List<WorkerHourViewModel> workerHoursViewModel = new List<WorkerHourViewModel>();
            var companyDaysOff = _companyDaysOffProc.Read().ToList();
            //if enum is yearly
            double totalWorkingDaysInMonthOrYear = amountInYear == 0 ? GetTotalWorkingDaysThisYear(new DateTime(year, 1, 1), companyDaysOff) : amountInYear;
            if (yearlyMonthlyEnum == YearlyMonthlyEnum.Monthly)
            {
                totalWorkingDaysInMonthOrYear = GetTotalWorkingDaysThisMonth(new DateTime(year, month, 1), companyDaysOff);
            }

            List<Task> listOfTasks = new List<Task>();
            await employees.ForEachAsync(employee =>
            {
                int wholeVacationDays = 0;
                double totalVacationDays = 0;
                double workingDays = 0;
                int wholeSickDays = 0;
                double totalSickDays = 0;
                double sickHours;
                double vacationSickCount = 0;
                double partialOffDayPercent = 0;
                int missingDays = 0;
                int specialDays = 0;
                int unpaidLeave = 0;
                double vacationHours;
                TimeSpan hours = new TimeSpan();
                //IEnumerable<EmployeeHours> employeeHoursOfMonthOrYear;


                var workingDaysDates = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == null).Where(eh => (eh.Exit1 != null || eh.TotalHours != null)).Select(eh => eh.Date.Date).ToList();
                workingDays = workingDaysDates.Count();

                if (employee.Employee.EmployeeStatusID == 1)
                {
                    var awaitingApprovalCount = employee.EmployeeHoursAwaitingApproval.Where(ehaa => !workingDaysDates.Contains(ehaa.Date.Date)).Count();

                    missingDays = employee.EmployeeHours.Where(eh => (eh.Entry1 == null && eh.OffDayTypeID == null && eh.TotalHours == null && eh.CompanyDayOffID == null)
                    || (eh.Entry1 != null && eh.Exit1 == null)).Count() - awaitingApprovalCount;

                    wholeSickDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 1/*&& eh.IsBonus == false*/).Count();
                    sickHours = employee.EmployeeHours.Where(eh => eh.PartialOffDayTypeID == 1/*&& eh.IsBonus == false*/).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                    totalSickDays = Math.Round(wholeSickDays + (sickHours / employee.Employee.SalariedEmployee?.HoursPerDay ?? 1), 2);

                    wholeVacationDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 2/* && eh.IsBonus == false*/).Count();
                    vacationHours = employee.EmployeeHours.Where(eh => eh.PartialOffDayTypeID == 2 /*&& eh.IsBonus == false*/).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                    totalVacationDays = Math.Round(wholeVacationDays + (vacationHours / employee.Employee.SalariedEmployee?.HoursPerDay ?? 1), 2);

                    partialOffDayPercent = Math.Round((sickHours + vacationHours) / employee.Employee.SalariedEmployee?.HoursPerDay ?? 1, 2);

                    hours = new TimeSpan(employee.EmployeeHours.Select(eh => new { TimeSpan = eh.TotalHours?.Ticks ?? 0 }).Sum(a => a.TimeSpan));
                    specialDays = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 4).Count();
                    unpaidLeave = employee.EmployeeHours.Where(eh => eh.OffDayTypeID == 5).Count();
                    vacationSickCount = totalSickDays + totalVacationDays + specialDays + unpaidLeave;

                    workingDays = workingDays - partialOffDayPercent;
                }
                WorkerHourViewModel worker = new WorkerHourViewModel
                {
                    Employee = employee.Employee,
                    SickDays = totalSickDays,
                    UnpaidLeave = unpaidLeave,
                    VacationDays = totalVacationDays,
                    WorkingDays = workingDays,
                    Hours = hours,
                    VacationSickCount = vacationSickCount,
                    MissingDays = missingDays
                };
                workerHoursViewModel.Add(worker);
            });
            WorkersHoursViewModel viewModel = new WorkersHoursViewModel
            {
                Year = year,
                Month = month,
                YearlyMonthlyEnum = yearlyMonthlyEnum,
                Employees = workerHoursViewModel,
                Months = Enumerable.Range(1, 12).ToList(),
                Years = Enumerable.Range(AppUtility.YearStartedTimeKeeper, DateTime.Today.Year - (AppUtility.YearStartedTimeKeeper - 1)).ToList(),
                TotalWorkingDaysInMonthOrYear = (int)totalWorkingDaysInMonthOrYear
            };
            return viewModel;
        }


        [Authorize(Roles = "Users")]
        public async Task<IActionResult> UserHours(string userId, int month, int year)
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersWorkers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Hours;

            Employee user = await _employeesProc.ReadOneAsync(new List<Expression<Func<Employee, bool>>> { e => e.Id == userId },
                new List<ComplexIncludes<Employee, ModelBase>> {
                    new ComplexIncludes<Employee, ModelBase> { Include = e => e.SalariedEmployee},
                });

            SummaryHoursViewModel viewModel = await base.SummaryHoursFunctionAsync(month, year, user);
            viewModel.PageType = PageTypeEnum.UsersWorkers;
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> Salary()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersWorkers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.Salary;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> AwaitingApproval()
        {
            TempData[AppUtility.TempDataTypes.PageType.ToString()] = AppUtility.PageTypeEnum.UsersWorkers;
            TempData[AppUtility.TempDataTypes.MenuType.ToString()] = AppUtility.MenuItems.Users;
            TempData[AppUtility.TempDataTypes.SidebarType.ToString()] = AppUtility.SidebarEnum.AwaitingApproval;

            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = GetAwaitingApprovalModel();
            return View(awaitingApproval);
        }
        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> _AwaitingApproval(string? ErrorMessage = null)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = GetAwaitingApprovalModel();
            if (awaitingApproval.Count > 0)
            {
                awaitingApproval[0].ErrorMessage = ErrorMessage;
            }

            return PartialView(awaitingApproval);
        }
        private List<EmployeeHoursAwaitingApprovalViewModel> GetAwaitingApprovalModel()
        {
            var employeeHoursAwaitingApproval = _employeeHoursAwaitingApprovalProc.Read(new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { ehwa => !ehwa.IsDenied },
                new List<ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase>> {
                    new ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase> { Include = ehwa => ehwa.Employee},
                    new ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase> { Include = ehwa => ehwa.EmployeeHours},
                    new ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase> { Include = ehwa => ehwa.EmployeeHoursStatusEntry1},
                    new ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase> { Include = ehwa => ehwa.EmployeeHoursStatusEntry2},
                    new ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase> { Include = ehwa => ehwa.PartialOffDayType},
                });
            List<EmployeeHoursAwaitingApprovalViewModel> awaitingApproval = new List<EmployeeHoursAwaitingApprovalViewModel>();
            foreach (EmployeeHoursAwaitingApproval ehaa in employeeHoursAwaitingApproval)
            {
                bool entry1 = false;
                bool entry2 = false;
                bool exit1 = false;
                bool exit2 = false;
                bool totalHours = false;
                bool partialHours = false;
                if (ehaa.EmployeeHours?.Entry1?.ZeroSeconds().TimeOfDay != ehaa.Entry1?.TimeOfDay)
                {
                    entry1 = true;
                }
                if (ehaa.EmployeeHours?.Entry2?.ZeroSeconds().TimeOfDay != ehaa.Entry2?.TimeOfDay)
                {
                    entry2 = true;
                }
                if (ehaa.EmployeeHours?.Exit1?.ZeroSeconds().TimeOfDay != ehaa.Exit1?.TimeOfDay)
                {
                    exit1 = true;
                }
                if (ehaa.EmployeeHours?.Exit2?.ZeroSeconds().TimeOfDay != ehaa.Exit2?.TimeOfDay)
                {
                    exit2 = true;
                }
                if (ehaa.EmployeeHours?.TotalHours != ehaa.TotalHours)
                {
                    totalHours = true;
                }
                if (ehaa.EmployeeHours?.PartialOffDayHours != ehaa.PartialOffDayHours)
                {
                    partialHours = true;
                }
                EmployeeHoursAwaitingApprovalViewModel viewModel = new EmployeeHoursAwaitingApprovalViewModel
                {
                    Entry1 = entry1,
                    Entry2 = entry2,
                    Exit1 = exit1,
                    Exit2 = exit2,
                    TotalHours = totalHours,
                    PartialHours = partialHours,
                    EmployeeHoursAwaitingApproval = ehaa
                };
                awaitingApproval.Add(viewModel);
            }

            return awaitingApproval;
        }

        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> ApproveHours(int id)
        {
            var hoursApproved = await _employeeHoursProc.UpdateApprovedHoursAsync(id);
            return RedirectToAction("_AwaitingApproval", new { ErrorMessage = hoursApproved.String });
            
        }


        [HttpGet]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> DenyApprovalRequestModal(int ehaaID)
        {
            if (!AppUtility.IsAjaxRequest(Request))
            {
                return PartialView("InvalidLinkPage");
            }
            var ehaa = await _employeeHoursAwaitingApprovalProc.ReadOneAsync(new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { ehaa => ehaa.EmployeeHoursAwaitingApprovalID == ehaaID },
                new List<ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase>> {
                    new ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase> { Include = ehwa => ehwa.EmployeeHours}
                });
            return PartialView(ehaa);
        }

        [HttpPost]
        [Authorize(Roles = "Users")]
        public async Task<IActionResult> DenyApprovalRequestModal(EmployeeHoursAwaitingApproval employeeHoursAwaitingApproval)
        {
            var deniedHours = await _employeeHoursAwaitingApprovalProc.DenyHoursAsync(employeeHoursAwaitingApproval.EmployeeHoursAwaitingApprovalID);
            return RedirectToAction("_AwaitingApproval", new { ErrorMessage = deniedHours.String });
        }


    }

}