using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class EmployeeHoursAwaitingApprovalProc : ApplicationDbContextProc<EmployeeHoursAwaitingApproval>
    {
        public EmployeeHoursAwaitingApprovalProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }


        public async Task DeleteAsync(int ID)
        {
            try
            {
                var ehaa = await ReadOneAsync( new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { ehaa => ehaa.EmployeeHoursAwaitingApprovalID == ID });
                if (ehaa != null)
                {
                    _context.Remove(ehaa);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Delete of employee hours awaiting approval did not go through");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Delete hours awaiting approval- "+AppUtility.GetExceptionMessage(ex));
            }
        }

        public async Task DeleteByEHIDAsync(int ID)
        {
            try
            {
                var ehaa = await ReadOneAsync(new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { ehaa => ehaa.EmployeeHoursID == ID });
                if (ehaa != null)
                {
                    _context.Remove(ehaa);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Delete of employee hours awaiting approval did not go through");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Delete hours awaiting approval- "+AppUtility.GetExceptionMessage(ex));
            }
        }

        public async Task<StringWithBool> DenyHoursAsync(int ID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    EmployeeHoursAwaitingApproval employeeHoursBeingApproved = await _employeeHoursAwaitingApprovalProc.ReadOneAsync(
                        new List<Expression<Func<EmployeeHoursAwaitingApproval, bool>>> { ehaa => ehaa.EmployeeHoursAwaitingApprovalID == ID },
                        new List<ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase>> {
                            new ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase> { Include = ehwa => ehwa.EmployeeHours},
                            new ComplexIncludes<EmployeeHoursAwaitingApproval, ModelBase> { Include = ehwa => ehwa.Employee}
                        });

                    employeeHoursBeingApproved.IsDenied = true;
                    _context.Update(employeeHoursBeingApproved);
                    await _context.SaveChangesAsync();

                    TimekeeperNotification notification = new TimekeeperNotification()
                    {
                        EmployeeHoursID = employeeHoursBeingApproved.EmployeeHoursID,
                        IsRead = false,
                        ApplicationUserID = employeeHoursBeingApproved.EmployeeID,
                        Description = "update hours request denied for " + employeeHoursBeingApproved.Date.GetElixirDateFormat(),
                        NotificationStatusID = 5,
                        TimeStamp = DateTime.Now,
                        Controller = "Timekeeper",
                        Action = "SummaryHours"
                    };
                    await _timekeeperNotificationsProc.CreateWithoutTransactionAsync(notification);

                    await transaction.CommitAsync();
                    ReturnVal.SetStringAndBool(true, null);
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    ReturnVal.SetStringAndBool(false, "Failed to deny hours- "+ AppUtility.GetExceptionMessage(ex));
                }
            }

            return ReturnVal;
        }
    }
}
