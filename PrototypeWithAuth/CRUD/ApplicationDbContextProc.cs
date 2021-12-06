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

        protected ApplicationUsersProc _applicationUsersProc;
        protected CategoryTypesProc _categoryTypesProc;
        protected CountriesProc _countriesProc;
        protected EmployeeHoursProc _employeeHoursProc;
        protected EmployeesProc _employeesProc;
        protected TimekeeperNotificationsProc _timekeeperNotificationsProc;
        protected VendorCommentsProc _vendorCommentsProc;
        protected VendorContactsProc _vendorContactsProc;
        protected VendorsProc _vendorsProc;
        protected VendorCategoryTypesProc _vendorCategoryTypesProc;
        protected EmployeeHoursAwaitingApprovalProc _employeeHoursAwaitingApprovalProc;
        protected EmployeeHoursStatuesProc _employeeHoursStatuesProc;
        protected CompanyDaysOffProc _companyDaysOffProc;
        protected OffDayTypesProc _offDayTypesProc;

        public ApplicationDbContextProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public void InstantiateProcs()
        {
            _applicationUsersProc = new ApplicationUsersProc(_context, _userManager, true);
            _categoryTypesProc = new CategoryTypesProc(_context, _userManager, true);
            _countriesProc = new CountriesProc(_context, _userManager, true);
            _employeeHoursProc = new EmployeeHoursProc(_context, _userManager, true);
            _employeesProc = new EmployeesProc(_context, _userManager, true);
            _timekeeperNotificationsProc = new TimekeeperNotificationsProc(_context, _userManager, true); 
            _vendorCommentsProc = new VendorCommentsProc(_context, _userManager, true); 
            _vendorContactsProc = new VendorContactsProc(_context, _userManager, true); 
            _vendorsProc = new VendorsProc(_context, _userManager, true);
            _vendorCategoryTypesProc = new VendorCategoryTypesProc(_context, _userManager, true); 
            _employeeHoursAwaitingApprovalProc = new EmployeeHoursAwaitingApprovalProc(_context, _userManager, true); 
            _employeeHoursStatuesProc = new EmployeeHoursStatuesProc(_context, _userManager, true); 
            _companyDaysOffProc = new CompanyDaysOffProc(_context, _userManager, true); 
            _offDayTypesProc = new OffDayTypesProc(_context, _userManager, true); 
        }
    }
}
