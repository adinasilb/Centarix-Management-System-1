using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class ApplicationDbContextProc
    {
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;

        protected readonly ApplicationUsersProc _applicationUsersProc;
        protected readonly CategoryTypesProc _categoryTypesProc;
        protected readonly CountriesProc _countriesProc;
        protected readonly EmployeeHoursProc _employeeHoursProc;
        protected readonly EmployeesProc _employeesProc;
        protected readonly TimekeeperNotificationsProc _timekeeperNotificationsProc;
        protected readonly VendorCommentsProc _vendorCommentsProc;
        protected readonly VendorContactsProc _vendorContactsProc;
        protected readonly VendorsProc _vendorsProc;
        protected readonly VendorCategoryTypesProc _vendorCategoryTypesProc;
        protected readonly EmployeeHoursAwaitingApprovalProc _employeeHoursAwaitingApprovalProc;
        protected readonly EmployeeHoursStatuesProc _employeeHoursStatuesProc;
        protected readonly CompanyDaysOffProc _companyDaysOffProc;
        protected readonly OffDayTypesProc _offDayTypesProc;
        public ApplicationDbContextProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

            _applicationUsersProc = new ApplicationUsersProc(context, userManager);
            _categoryTypesProc = new CategoryTypesProc(context, userManager);
            _countriesProc = new CountriesProc(context, userManager);
            _employeeHoursProc = new EmployeeHoursProc(context, userManager);
            _employeesProc = new EmployeesProc(context, userManager);
            _timekeeperNotificationsProc = new TimekeeperNotificationsProc(context, userManager);
            _vendorCommentsProc = new VendorCommentsProc(context, userManager);
            _vendorContactsProc = new VendorContactsProc(context, userManager);
            _vendorsProc = new VendorsProc(context, userManager);
            _vendorCategoryTypesProc = new VendorCategoryTypesProc(context, userManager);
            _employeeHoursAwaitingApprovalProc = new EmployeeHoursAwaitingApprovalProc(context, userManager);
            _employeeHoursStatuesProc = new EmployeeHoursStatuesProc(context, userManager);
            _companyDaysOffProc = new CompanyDaysOffProc(context, userManager);
            _offDayTypesProc = new OffDayTypesProc(context, userManager);
        }
    }
}
