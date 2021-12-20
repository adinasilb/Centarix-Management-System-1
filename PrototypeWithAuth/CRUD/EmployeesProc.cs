using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;

namespace PrototypeWithAuth.CRUD
{
    public class EmployeesProc :ApplicationDbContextProc<Employee>
    {
        public object TempData { get; private set; }

        public EmployeesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> AddSpecialDays (string UserID, int SpecialDays)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var employee = this.Read(new List<Expression<Func<Employee, bool>>> { u => u.Id == UserID }).FirstOrDefault();
                employee.SpecialDays += SpecialDays;
                _context.Update(employee);
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch(Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        //public async Task<StringWithBool> UpdateAsync(Employee employee)
        //{
        //    StringWithBool ReturnVal = new StringWithBool();
        //    try
        //    {
        //        _context.Update(employee);
        //        await _context.SaveChangesAsync();
        //        ReturnVal.SetStringAndBool(true, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
        //    }
        //    return ReturnVal;
        //}

        public async Task<StringWithBool> UpdateUser(RegisterUserViewModel registerUserViewModel, IHostingEnvironment _hostingEnvironment, IUrlHelper Url, HttpRequest Request, UserManager<ApplicationUser> _userManager)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        int selectedStatusID = registerUserViewModel.NewEmployee.EmployeeStatusID;
                        Employee employeeEditted = await _context.Employees.Where(e => e.Id == registerUserViewModel.ApplicationUserID).FirstOrDefaultAsync();
                        int oldSelectedStatus = employeeEditted.EmployeeStatusID;
                        bool changedEmployeeStatus = false;
                        if (selectedStatusID != oldSelectedStatus)
                        {
                            changedEmployeeStatus = true;
                        }
                        if (selectedStatusID == 4)
                        {
                            //never was an employee only was a user and wants to update info                 
                            employeeEditted.UserName = registerUserViewModel.Email;
                            employeeEditted.FirstName = registerUserViewModel.FirstName;
                            employeeEditted.LastName = registerUserViewModel.LastName;
                            employeeEditted.Email = registerUserViewModel.Email;
                            employeeEditted.NormalizedEmail = registerUserViewModel.Email.ToUpper();
                            employeeEditted.PhoneNumber = registerUserViewModel.PhoneNumber;
                            //are users allowed to update their password
                            if (registerUserViewModel.SecureAppPass != null)
                            {
                                employeeEditted.SecureAppPass = registerUserViewModel.SecureAppPass;
                            }
                            employeeEditted.LabMonthlyLimit = registerUserViewModel.LabMonthlyLimit;
                            employeeEditted.LabUnitLimit = registerUserViewModel.LabUnitLimit;
                            employeeEditted.LabOrderLimit = registerUserViewModel.LabOrderLimit;
                            employeeEditted.OperationMonthlyLimit = registerUserViewModel.OperationMonthlyLimit;
                            employeeEditted.OperationUnitLimit = registerUserViewModel.OperationUnitLimit;
                            employeeEditted.OperationOrderLimit = registerUserViewModel.OperaitonOrderLimit;
                            employeeEditted.EmployeeStatusID = registerUserViewModel.NewEmployee.EmployeeStatusID;
                            _context.Update(employeeEditted);
                            await _context.SaveChangesAsync();

                            if (changedEmployeeStatus)
                            {
                                await _centarixIDsProc.AddNewCentarixID(employeeEditted.Id, 4);// AddNewCentarixID(employeeEditted.Id, 4);
                            }
                        }
                        else
                        {
                            // still wants to remain an employee
                            employeeEditted.UserName = registerUserViewModel.Email;
                            employeeEditted.FirstName = registerUserViewModel.FirstName;
                            employeeEditted.LastName = registerUserViewModel.LastName;
                            employeeEditted.Email = registerUserViewModel.Email;
                            employeeEditted.NormalizedEmail = registerUserViewModel.Email.ToUpper();
                            employeeEditted.PhoneNumber = registerUserViewModel.PhoneNumber;
                            //are users allowed to update their password
                            if (registerUserViewModel.SecureAppPass != null)
                            {
                                employeeEditted.SecureAppPass = registerUserViewModel.SecureAppPass;
                            }
                            employeeEditted.LabMonthlyLimit = registerUserViewModel.LabMonthlyLimit;
                            employeeEditted.LabUnitLimit = registerUserViewModel.LabUnitLimit;
                            employeeEditted.LabOrderLimit = registerUserViewModel.LabOrderLimit;
                            employeeEditted.OperationMonthlyLimit = registerUserViewModel.OperationMonthlyLimit;
                            employeeEditted.OperationUnitLimit = registerUserViewModel.OperationUnitLimit;
                            employeeEditted.OperationOrderLimit = registerUserViewModel.OperaitonOrderLimit;
                            employeeEditted.StartedWorking = registerUserViewModel.NewEmployee.StartedWorking;
                            employeeEditted.DOB = registerUserViewModel.NewEmployee.DOB;
                            employeeEditted.GrossSalary = registerUserViewModel.NewEmployee.GrossSalary;
                            employeeEditted.EmployerTax = registerUserViewModel.NewEmployee.EmployerTax;
                            employeeEditted.IncomeTax = registerUserViewModel.NewEmployee.IncomeTax;
                            employeeEditted.TaxCredits = registerUserViewModel.NewEmployee.TaxCredits;
                            employeeEditted.VacationDays = registerUserViewModel.NewEmployee.VacationDays;
                            employeeEditted.JobSubcategoryTypeID = registerUserViewModel.NewEmployee.JobSubcategoryTypeID;
                            employeeEditted.DegreeID = registerUserViewModel.NewEmployee.DegreeID;
                            employeeEditted.IDNumber = registerUserViewModel.NewEmployee.IDNumber;
                            employeeEditted.MaritalStatusID = registerUserViewModel.NewEmployee.MaritalStatusID;
                            employeeEditted.CitizenshipID = registerUserViewModel.NewEmployee.CitizenshipID;
                            employeeEditted.EmployeeStatusID = selectedStatusID;
                            employeeEditted.RollOverSickDays = registerUserViewModel.NewEmployee.RollOverSickDays;
                            employeeEditted.RollOverVacationDays = registerUserViewModel.NewEmployee.RollOverVacationDays;
                            //employeeEditted.BonusSickDays = registerUserViewModel.NewEmployee.BonusSickDays;
                            //employeeEditted.BonusVacationDays = registerUserViewModel.NewEmployee.BonusVacationDays;
                            employeeEditted.SpecialDays = registerUserViewModel.NewEmployee.SpecialDays;
                            //employeeEditted.JobSubategoryTypeID = registerUserViewModel.NewEmployee.JobSubcategoryTypeID;

                            _context.Update(employeeEditted);


                            switch (selectedStatusID)
                            {
                                case 1: /*Salaried Employee*/
                                    var salariedEmployee = _context.SalariedEmployees.Where(x => x.EmployeeId == employeeEditted.Id).FirstOrDefault();
                                    if (salariedEmployee == null)
                                    {
                                        salariedEmployee = new SalariedEmployee();
                                    }
                                    if (changedEmployeeStatus)
                                    {
                                        await _centarixIDsProc.AddNewCentarixID(employeeEditted.Id, 1);// AddNewCentarixID(employeeEditted.Id, 1);
                                    }
                                    salariedEmployee.EmployeeId = employeeEditted.Id;
                                    salariedEmployee.HoursPerDay = registerUserViewModel.NewEmployee.SalariedEmployee.HoursPerDay;
                                    employeeEditted.SalariedEmployee = salariedEmployee;
                                    break;
                                case 2: /*Freelancer*/
                                    Freelancer freelancer = _context.Freelancers.Where(x => x.EmployeeId == employeeEditted.Id).FirstOrDefault();
                                    if (freelancer == null)
                                    {
                                        freelancer = new Freelancer();
                                    }
                                    if (changedEmployeeStatus)
                                    {
                                        await _centarixIDsProc.AddNewCentarixID(employeeEditted.Id, 2);// await AddNewCentarixID(employeeEditted.Id, 2);
                                    }
                                    freelancer.EmployeeId = employeeEditted.Id;
                                    employeeEditted.Freelancer = freelancer;
                                    break;
                                case 3: /*Advisor*/
                                    Advisor advisor = _context.Advisors.Where(a => a.EmployeeID == employeeEditted.Id).FirstOrDefault();
                                    if (advisor == null)
                                    {
                                        advisor = new Advisor();
                                    }
                                    if (changedEmployeeStatus)
                                    {
                                        await _centarixIDsProc.AddNewCentarixID(employeeEditted.Id, 3);// AddNewCentarixID(employeeEditted.Id, 3);
                                    }
                                    advisor.EmployeeID = employeeEditted.Id;
                                    employeeEditted.Advisor = advisor;
                                    break;
                            }
                            await _context.SaveChangesAsync();
                        }
                        //add new centarixID

                        if (!String.IsNullOrEmpty(registerUserViewModel.Password))
                        {
                            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(employeeEditted);
                            IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(employeeEditted, resetToken, registerUserViewModel.Password);
                            if (passwordChangeResult.Succeeded)
                            {
                                employeeEditted.NeedsToResetPassword = true;
                                await _userManager.ResetAuthenticatorKeyAsync(employeeEditted);
                                await _userManager.UpdateSecurityStampAsync(employeeEditted);
                                employeeEditted.LockoutEnabled = true;
                                employeeEditted.LockoutEnd = new DateTime(2999, 01, 01);
                                _context.Update(employeeEditted);
                                await _context.SaveChangesAsync();

                                if (!registerUserViewModel.NewEmployee.IsUser)
                                {
                                    employeeEditted.IsUser = true;
                                    _context.Update(employeeEditted);
                                    await _context.SaveChangesAsync();

                                    SendConfimationEmail(employeeEditted, Url, Request, _userManager);
                                }
                            }
                            else
                            {
                                //TODO: alert the user that it didn't succeed!!!
                            }
                        }

                        //if password isn't blank - reset the password):
                        //if (registerUserViewModel.Password != null)
                        //{
                        //    ApplicationUser cUser = await _userManager.FindByIdAsync(registerUserViewModel.ApplicationUserID);
                        //    string hashpassword = _userManager.PasswordHasher.HashPassword(cUser, registerUserViewModel.Password);
                        //    cUser.PasswordHash = hashpassword;
                        //    await _userManager.UpdateAsync(cUser);
                        //}



                        List<string> rolesList = new List<string>(await _userManager.GetRolesAsync(employeeEditted).ConfigureAwait(false));

                        foreach (var role in registerUserViewModel.OrderRoles)
                        {
                            await CheckRoleAsync(rolesList, employeeEditted, role.StringWithName.StringDefinition, role.Selected, _userManager);
                        }
                        foreach (var role in registerUserViewModel.OperationRoles)
                        {
                            await CheckRoleAsync(rolesList, employeeEditted, role.StringWithName.StringDefinition, role.Selected, _userManager);
                        }
                        foreach (var role in registerUserViewModel.ProtocolRoles)
                        {
                            await CheckRoleAsync(rolesList, employeeEditted, role.StringWithName.StringDefinition, role.Selected, _userManager);
                        }
                        await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.Biomarkers.ToString(), registerUserViewModel.BiomarkerRoles[0].Selected, _userManager);
                        await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.TimeKeeper.ToString(), registerUserViewModel.TimekeeperRoles[0].Selected, _userManager);
                        await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.LabManagement.ToString(), registerUserViewModel.LabManagementRoles[0].Selected, _userManager);
                        await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.Accounting.ToString(), registerUserViewModel.AccountingRoles[0].Selected, _userManager);
                        await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.Reports.ToString(), registerUserViewModel.ExpenseesRoles[0].Selected, _userManager);
                        await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.Income.ToString(), registerUserViewModel.IncomeRoles[0].Selected, _userManager);
                        await CheckRoleAsync(rolesList, employeeEditted, AppUtility.MenuItems.Users.ToString(), registerUserViewModel.UserRoles[0].Selected, _userManager);

                        if (registerUserViewModel.UserImageSaved == "true")
                        {
                            //delete old photo
                            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                            DirectoryInfo dir1 = new DirectoryInfo(uploadFolder1);
                            FileInfo[] files1 = dir1.GetFiles(registerUserViewModel.UserNum + ".*");
                            if (files1.Length > 0)
                            {
                                foreach (FileInfo file in files1)
                                {
                                    System.IO.File.Delete(file.FullName);
                                }
                            }

                            //add new photo
                            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                            DirectoryInfo dir = new DirectoryInfo(uploadFolder);
                            FileInfo[] files = dir.GetFiles("TempUserImage" + ".*");
                            if (files.Length > 0)
                            {
                                //File exists
                                foreach (FileInfo file in files)
                                {
                                    //System.IO.File.Move(file., user.UserNum.ToString());
                                    file.MoveTo(Path.Combine(uploadFolder, registerUserViewModel.UserNum.ToString() + file.Extension));
                                    employeeEditted.UserImage = file.FullName;
                                }
                                _context.Update(employeeEditted);
                                await _context.SaveChangesAsync();
                            }

                            //should we move the delete here and test for the extension just in case it breaks over there

                        }
                        //throw new Exception();
                        await transaction.CommitAsync();
                        ReturnVal.SetStringAndBool(true, null);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public async Task<StringWithBool> CreateUser(RegisterUserViewModel registerUserViewModel, IHostingEnvironment _hostingEnvironment, IUrlHelper Url, HttpRequest Request, UserManager<ApplicationUser> _userManager)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    int userid = 0;
                    int usernum = 1;
                    if (_employeesProc.Read().Any())
                    {
                        usernum = _employeesProc.Read().OrderByDescending(u => u.UserNum).FirstOrDefault().UserNum + 1;
                    }
                    int UserType = registerUserViewModel.NewEmployee.EmployeeStatusID;

                    Employee user = new Employee()
                    {
                        /*User*/
                        UserName = registerUserViewModel.Email,
                        Email = registerUserViewModel.Email,
                        FirstName = registerUserViewModel.FirstName,
                        LastName = registerUserViewModel.LastName,
                        SecureAppPass = registerUserViewModel.SecureAppPass,
                        PhoneNumber = registerUserViewModel.PhoneNumber,
                        PhoneNumber2 = registerUserViewModel.PhoneNumber2,
                        UserNum = usernum,
                        LabMonthlyLimit = registerUserViewModel.LabMonthlyLimit,
                        LabUnitLimit = registerUserViewModel.LabUnitLimit,
                        LabOrderLimit = registerUserViewModel.LabOrderLimit,
                        OperationMonthlyLimit = registerUserViewModel.OperationMonthlyLimit,
                        OperationUnitLimit = registerUserViewModel.OperationUnitLimit,
                        OperationOrderLimit = registerUserViewModel.OperaitonOrderLimit,
                        DateCreated = DateTime.Now,
                        EmployeeStatusID = registerUserViewModel.NewEmployee.EmployeeStatusID,
                        JobSubcategoryTypeID = registerUserViewModel.NewEmployee.JobSubcategoryTypeID,
                        IsUser = true,
                        NeedsToResetPassword = true,
                        TwoFactorEnabled = true

                    };
                    if (UserType == 4)
                    {

                    }
                    else
                    {
                        /*User*/
                        /*Employee*/
                        user.IsUser = true;
                        user.StartedWorking = registerUserViewModel.NewEmployee.StartedWorking;
                        if (user.StartedWorking > AppUtility.DateSoftwareLaunched)
                        {
                            user.LastLogin = user.StartedWorking;
                        }
                        else
                        {
                            user.LastLogin = AppUtility.DateSoftwareLaunched;
                        }
                        user.DOB = registerUserViewModel.NewEmployee.DOB;
                        user.GrossSalary = registerUserViewModel.NewEmployee.GrossSalary;
                        user.EmployerTax = registerUserViewModel.NewEmployee.EmployerTax;
                        user.IncomeTax = registerUserViewModel.NewEmployee.IncomeTax;
                        user.TaxCredits = registerUserViewModel.NewEmployee.TaxCredits;
                        user.VacationDays = registerUserViewModel.NewEmployee.VacationDays;
                        //user.JobSubcategoryTypeID = registerUserViewModel.NewEmployee.JobSubategoryTypeID;
                        user.DegreeID = registerUserViewModel.NewEmployee.DegreeID;
                        user.IDNumber = registerUserViewModel.NewEmployee.IDNumber;
                        user.MaritalStatusID = registerUserViewModel.NewEmployee.MaritalStatusID;
                        user.CitizenshipID = registerUserViewModel.NewEmployee.CitizenshipID;
                        user.JobSubcategoryTypeID = registerUserViewModel.NewEmployee.JobSubcategoryTypeID;
                        /*Salaried Employee*/
                    }

                    bool IsUser = true;
                    if (registerUserViewModel.Password == "" || registerUserViewModel.Password == null)
                    {
                        IsUser = false;
                        string newPassword = GeneratePassword(true, true, true, true, false, 10);
                        registerUserViewModel.Password = newPassword;
                    }
                    IdentityResult result = await _userManager.CreateAsync(user, registerUserViewModel.Password);
                    await _userManager.ResetAuthenticatorKeyAsync(user);
                    await _userManager.UpdateSecurityStampAsync(user);
                    if (result.Succeeded)
                    {
                        if (!IsUser)
                        {
                            user.LockoutEnabled = true;
                            user.LockoutEnd = new DateTime(2999, 01, 01);
                            user.IsUser = false;
                            _context.Update(user);
                            await _context.SaveChangesAsync();
                        }

                        if (user.NeedsToResetPassword)
                        {
                            user.LockoutEnabled = true;
                            user.LockoutEnd = new DateTime(2999, 01, 01);
                            _context.Update(user);
                            await _context.SaveChangesAsync();
                        }

                        //add in CentarixID
                        var employeeStatus = _employeeStatusesProc.ReadOne(new List<Expression<Func<EmployeeStatus, bool>>> { es => es.EmployeeStatusID == UserType }).FirstOrDefault();
                        var currentNum = employeeStatus.LastCentarixID + 1;
                        var abbrev = employeeStatus.Abbreviation;
                        if (abbrev[1] == ' ')
                        {
                            abbrev = abbrev.Substring(0, 1);
                        }
                        var cID = abbrev + currentNum.ToString();
                        CentarixID centarixID = new CentarixID()
                        {
                            EmployeeID = user.Id,
                            CentarixIDNumber = cID,
                            IsCurrent = true,
                            TimeStamp = DateTime.Now,
                            Employee = this.Read(new List<Expression<Func<Employee, bool>>> { e => e.Id == user.Id }).FirstOrDefault()
                        };
                        _centarixIDsProc.CreateWithoutSaving(centarixID);
                        await _context.SaveChangesAsync();

                        //update last ID
                        employeeStatus.LastCentarixID = currentNum;
                        employeeStatus.LastCentarixIDTimeStamp = DateTime.Now;
                        _employeeStatusesProc.CreateWithoutSaving(employeeStatus);
                        await _context.SaveChangesAsync();

                        switch (UserType)
                        {
                            case 1: /*Salaried Employee*/
                                SalariedEmployee salariedEmployee = new SalariedEmployee()
                                {
                                    EmployeeId = user.Id,
                                    HoursPerDay = registerUserViewModel.NewEmployee.SalariedEmployee.HoursPerDay
                                };
                                _context.Add(salariedEmployee);
                                break;
                            case 2: /*Freelancer*/
                                Freelancer freelancer = new Freelancer()
                                {
                                    EmployeeId = user.Id
                                };
                                _context.Add(freelancer);
                                break;
                            case 3: /*Advisor*/
                                Advisor advisor = new Advisor()
                                {
                                    EmployeeID = user.Id
                                };
                                _context.Add(advisor);
                                break;
                        }
                        await _context.SaveChangesAsync();

                        try
                        {

                            foreach (UserRoleViewModel orderRole in registerUserViewModel.OrderRoles)
                            {
                                if (orderRole.Selected)
                                {
                                    await _userManager.AddToRoleAsync(user, orderRole.StringWithName.StringDefinition);
                                }
                            }
                            foreach (UserRoleViewModel protcolRole in registerUserViewModel.ProtocolRoles)
                            {
                                if (protcolRole.Selected)
                                {
                                    await _userManager.AddToRoleAsync(user, protcolRole.StringWithName.StringDefinition);
                                }
                            }
                            foreach (UserRoleViewModel operationRole in registerUserViewModel.OperationRoles)
                            {
                                if (operationRole.Selected)
                                {
                                    await _userManager.AddToRoleAsync(user, operationRole.StringWithName.StringDefinition);
                                }
                            }
                            foreach (UserRoleViewModel biomarkerRole in registerUserViewModel.BiomarkerRoles)
                            {
                                if (biomarkerRole.Selected)
                                {
                                    await _userManager.AddToRoleAsync(user, biomarkerRole.StringWithName.StringDefinition);
                                }
                            }
                            foreach (UserRoleViewModel timekeeperRole in registerUserViewModel.TimekeeperRoles)
                            {
                                if (timekeeperRole.Selected)
                                {
                                    await _userManager.AddToRoleAsync(user, timekeeperRole.StringWithName.StringDefinition);
                                }
                            }
                            foreach (UserRoleViewModel labmanagementRole in registerUserViewModel.LabManagementRoles)
                            {
                                if (labmanagementRole.Selected)
                                {
                                    await _userManager.AddToRoleAsync(user, labmanagementRole.StringWithName.StringDefinition);
                                }
                            }
                            foreach (UserRoleViewModel accountingRole in registerUserViewModel.AccountingRoles)
                            {
                                if (accountingRole.Selected)
                                {
                                    await _userManager.AddToRoleAsync(user, accountingRole.StringWithName.StringDefinition);
                                }
                            }
                            foreach (UserRoleViewModel expensesRole in registerUserViewModel.ExpenseesRoles)
                            {
                                if (expensesRole.Selected)
                                {
                                    await _userManager.AddToRoleAsync(user, expensesRole.StringWithName.StringDefinition);
                                }
                            }
                            foreach (UserRoleViewModel incomeRole in registerUserViewModel.IncomeRoles)
                            {
                                if (incomeRole.Selected)
                                {
                                    await _userManager.AddToRoleAsync(user, incomeRole.StringWithName.StringDefinition);
                                }
                            }
                            foreach (UserRoleViewModel usersRole in registerUserViewModel.UserRoles)
                            {
                                if (usersRole.Selected)
                                {
                                    await _userManager.AddToRoleAsync(user, usersRole.StringWithName.StringDefinition);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            ReturnVal.SetStringAndBool(true, "User saved successful but something went wrong while tring to add the roles. Please retry adding roles to the newly create user");                       
                        }

                        if (registerUserViewModel.UserImageSaved == "true")
                        {
                            //delete old photo
                            string uploadFolder1 = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                            DirectoryInfo dir1 = new DirectoryInfo(uploadFolder1);
                            FileInfo[] files1 = dir1.GetFiles(user.UserNum + ".*");
                            if (files1.Length > 0)
                            {
                                foreach (FileInfo file in files1)
                                {
                                    System.IO.File.Delete(file.FullName);
                                }
                            }

                            //add new photo
                            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UserImages");
                            DirectoryInfo dir = new DirectoryInfo(uploadFolder);
                            FileInfo[] files = dir.GetFiles("TempUserImage" + ".*");
                            if (files.Length > 0)
                            {
                                //File exists
                                foreach (FileInfo file in files)
                                {
                                    //System.IO.File.Move(file., user.UserNum.ToString());
                                    file.MoveTo(Path.Combine(uploadFolder, user.UserNum.ToString() + file.Extension));
                                    user.UserImage = file.FullName;
                                }
                                _context.Update(user);
                                await _context.SaveChangesAsync();
                            }
                            //should we move the delete here and test for the extension just in case it breaks over there
                        }


                        //}
                        //else
                        //{
                        //    foreach (var error in result.Errors)
                        //    {
                        //        ModelState.AddModelError("", error.Description);
                        //    }
                        //}

                    }
                    else
                    {
                        foreach (IdentityError e in result.Errors)
                        {
                            ReturnVal.String += "User Failed to add. Please try again. " + e.Code.ToString() + " " + e.Description.ToString();
                        }
                        //refill Model to view errors
                    }
                    //throw new Exception();

                    if (IsUser)
                    {
                        SendConfimationEmail(user, Url, Request, _userManager);
                    }
                    await transaction.CommitAsync();
                    ReturnVal.Bool = true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ReturnVal.String = AppUtility.GetExceptionMessage(ex);
                }
            }
            return ReturnVal;
        }

        
        private async void SendConfimationEmail(ApplicationUser user, IUrlHelper Url, HttpRequest Request, UserManager<ApplicationUser> _userManager)
        {
            string userId = await _userManager.GetUserIdAsync(user);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string confirmationLink = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);


            MimeMessage message = new MimeMessage();

            //instantiate the body builder
            BodyBuilder builder = new BodyBuilder();

            //add a "From" Email
            message.From.Add(new MailboxAddress("Elixir", "elixir@centarix.com"));

            // add a "To" Email
            message.To.Add(new MailboxAddress(user.FirstName, user.Email));

            //subject
            message.Subject = "Confirm centarix sign-up Link";

            //body
            builder.TextBody = confirmationLink;

            message.Body = builder.ToMessageBody();

            using (SmtpClient client = new SmtpClient())
            {

                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("elixir@centarix.com", "cdbmhjidnzoghqvt");
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                }

                client.Disconnect(true);
            }
        }

        public string GeneratePassword(bool includeLowercase = true, bool includeUppercase = true, bool includeNumeric = true, bool includeSpecial = true, bool includeSpaces = false, int lengthOfPassword = 12)
        {
            const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
            const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
            const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMERIC_CHARACTERS = "0123456789";
            const string SPECIAL_CHARACTERS = @"!#$%&*@\";
            const string SPACE_CHARACTER = " ";
            const int PASSWORD_LENGTH_MIN = 8;
            const int PASSWORD_LENGTH_MAX = 128;

            if (lengthOfPassword < PASSWORD_LENGTH_MIN || lengthOfPassword > PASSWORD_LENGTH_MAX)
            {
                return "Password length must be between 8 and 128.";
            }

            string characterSet = "";

            if (includeLowercase)
            {
                characterSet += LOWERCASE_CHARACTERS;
            }

            if (includeUppercase)
            {
                characterSet += UPPERCASE_CHARACTERS;
            }

            if (includeNumeric)
            {
                characterSet += NUMERIC_CHARACTERS;
            }

            if (includeSpecial)
            {
                characterSet += SPECIAL_CHARACTERS;
            }

            if (includeSpaces)
            {
                characterSet += SPACE_CHARACTER;
            }

            char[] password = new char[lengthOfPassword];
            int characterSetLength = characterSet.Length;

            while (!PasswordIsValid(string.Join(null, password)))
            {
                System.Random random = new System.Random();
                for (int characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
                {
                    password[characterPosition] = characterSet[random.Next(characterSetLength - 1)];

                    bool moreThanTwoIdenticalInARow =
                        characterPosition > MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS
                        && password[characterPosition] == password[characterPosition - 1]
                        && password[characterPosition - 1] == password[characterPosition - 2];

                    if (moreThanTwoIdenticalInARow)
                    {
                        characterPosition--;
                    }
                }
            }


            return string.Join(null, password);
        }

        private static bool PasswordIsValid(string password, bool includeLowercase = true, bool includeUppercase = true, bool includeNumeric = true, bool includeSpecial = true, bool includeSpaces = false)
        {
            const string REGEX_LOWERCASE = @"[a-z]";
            const string REGEX_UPPERCASE = @"[A-Z]";
            const string REGEX_NUMERIC = @"[\d]";
            const string REGEX_SPECIAL = @"([!#$%&*@\\])+";
            const string REGEX_SPACE = @"([ ])+";

            bool lowerCaseIsValid = !includeLowercase || (includeLowercase && Regex.IsMatch(password, REGEX_LOWERCASE));
            bool upperCaseIsValid = !includeUppercase || (includeUppercase && Regex.IsMatch(password, REGEX_UPPERCASE));
            bool numericIsValid = !includeNumeric || (includeNumeric && Regex.IsMatch(password, REGEX_NUMERIC));
            bool symbolsAreValid = !includeSpecial || (includeSpecial && Regex.IsMatch(password, REGEX_SPECIAL));
            bool spacesAreValid = !includeSpaces || (includeSpaces && Regex.IsMatch(password, REGEX_SPACE));

            return lowerCaseIsValid && upperCaseIsValid && numericIsValid && symbolsAreValid && spacesAreValid;
        }

        public async Task CheckRoleAsync(IList<string> roleslist, Employee employee, string roleName, bool selected, UserManager<ApplicationUser> _userManager)
        {
            if (!roleslist.Contains(roleName) && selected)
            {
                var rolesResult = await _userManager.AddToRoleAsync(employee, roleName);
            }
            else if ((roleslist.Contains(roleName)) && !selected)
            {
                var rolesResult = await _userManager.RemoveFromRoleAsync(employee, roleName);
            }

        }

        public async Task<StringWithBool> SuspendUser(ApplicationUser applicationUser)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                if (applicationUser.LockoutEnabled == true && (applicationUser.LockoutEnd > DateTime.Now))
                {
                    applicationUser.IsSuspended = false;
                    applicationUser.LockoutEnabled = false;
                    applicationUser.LockoutEnd = DateTime.Now;
                }
                else
                {
                    applicationUser.IsSuspended = true;
                    applicationUser.LockoutEnabled = true;
                    applicationUser.LockoutEnd = new DateTime(2999, 01, 01);
                }
                _context.Update(applicationUser);
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch(Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

    }
}
