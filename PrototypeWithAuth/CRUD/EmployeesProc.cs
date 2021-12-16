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
      
     
        public async Task<double> GetOffDaysByYear(Employee User, int OffDayTypeID, int thisYear)
        {
            double offDaysLeft = 0;
            var year = AppUtility.YearStartedTimeKeeper;
         
            while (year <= thisYear)
            {
                double vacationDays = 0;
                double sickDays = 0;
                double offDaysTaken = _employeeHoursProc.ReadOffDaysByYear(year, OffDayTypeID, User.Id).Count();
                if (User.EmployeeStatusID == 1 && OffDayTypeID == 2)
                {
                    var vacationHours =await _employeeHoursProc.ReadPartialOffDayHoursByYearAsync(year, 2, User.Id);
                    offDaysTaken = Math.Round(offDaysTaken + (vacationHours / User.SalariedEmployee.HoursPerDay), 2);
                }
                if (year == AppUtility.YearStartedTimeKeeper && year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = (User.VacationDaysPerMonth * month) + User.RollOverVacationDays;
                    sickDays = (User.SickDaysPerMonth * month) + User.RollOverSickDays;
                }
                else if (year == AppUtility.YearStartedTimeKeeper)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = User.VacationDays + User.RollOverVacationDays;
                    sickDays = User.SickDays + User.RollOverSickDays;
                }
                else if (year == User.StartedWorking.Year)
                {
                    int month = 12 - User.StartedWorking.Month + 1;
                    vacationDays = User.VacationDaysPerMonth * month;
                    sickDays = User.SickDays * month;
                }
                else if (year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = (User.VacationDaysPerMonth * month);
                    sickDays = (User.SickDaysPerMonth * month);
                }
                else
                {
                    vacationDays = User.VacationDays;
                    sickDays = User.SickDays;
                }
                if (OffDayTypeID == 2)
                {
                    offDaysLeft += vacationDays - offDaysTaken;
                }
                else
                {
                    offDaysLeft += sickDays - offDaysTaken;
                }
                year = year + 1;
            }

            return offDaysLeft;
        }

        public async Task<StringWithBool> UpdateAsync(Employee employee)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public async Task<StringWithBool> CreateUser(RegisterUserViewModel registerUserViewModel, IHostingEnvironment _hostingEnvironment, IUrlHelper Url, HttpRequest Request)
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
                        SendConfimationEmail(user, Url, Request);
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

        
        private async void SendConfimationEmail(ApplicationUser user, IUrlHelper Url, HttpRequest Request)
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

        public static string GeneratePassword(bool includeLowercase = true, bool includeUppercase = true, bool includeNumeric = true, bool includeSpecial = true, bool includeSpaces = false, int lengthOfPassword = 12)
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
    }
}
