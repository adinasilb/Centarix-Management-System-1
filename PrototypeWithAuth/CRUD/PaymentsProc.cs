using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class PaymentsProc : ApplicationDbContextProc<Payment>
    {
        public PaymentsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }

        public StringWithBool CreateWithoutSaveChanges(Payment payment)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                _context.Entry(payment).State = EntityState.Added;
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }

        public async Task<StringWithBool> DeleteAsync(int requestID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var payments = Read(new List<Expression<Func<Payment, bool>>> { p => p.RequestID == requestID }).ToList();
                foreach (var payment in payments)
                {
                    payment.IsDeleted = true;
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;           
        }

        public async Task<StringWithBool> UpdateAsync(PaymentsPayModalViewModel paymentsPayModalViewModel)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    if (paymentsPayModalViewModel.ShippingToPay != null)
                    {
                        try
                        {
                            await  _parentRequestsProc.UpdateShippingPaidAsync(paymentsPayModalViewModel);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Something went wrong while updating shipping paid - "+AppUtility.GetExceptionMessage(ex));
                        }
                    }
                    //var paymentsList =Read(new List<Expression<Func<Payment, bool>>> { p => p.IsPaid == false }).AsEnumerable();
                    foreach (Request request in paymentsPayModalViewModel.Requests)
                    {
                        //var requestToUpdate = await _requestsProc.ReadOneAsync( new List<Expression<Func<Request, bool>>> { r => r.RequestID == request.RequestID });
                        Payment payment = await _paymentsProc.ReadOneAsync( new List<Expression<Func<Payment, bool>>> { p => p.RequestID == request.RequestID });
                        //if (requestToUpdate.PaymentStatusID == 7)
                        //{
                        //    payment = paymentsList.Where(p => p.RequestID == requestToUpdate.RequestID).FirstOrDefault();
                        //    _context.Add(new Payment() { PaymentDate = payment.PaymentDate.AddMonths(1), RequestID = requestToUpdate.RequestID });
                        //}
                        //else if (requestToUpdate.PaymentStatusID == 5)
                        //{
                        //    var payments = paymentsList.Where(p => p.RequestID == requestToUpdate.RequestID);
                        //    var count = payments.Count();

                        //    payment = payments.OrderBy(p => p.PaymentDate).FirstOrDefault();
                        //    if (count <= 1)
                        //    {
                        //        payment.Sum = requestToUpdate.Cost ?? 0;
                        //    }
                        //}
                        //else
                        //{
                        payment.Sum = request.Cost ?? 0;
                        payment.PaymentDate = DateTime.Now.Date;
                        payment.RequestID = request.RequestID;
                        //}
                        payment.Reference = paymentsPayModalViewModel.Payment.Reference;
                        payment.CompanyAccountID = paymentsPayModalViewModel.Payment.CompanyAccountID;
                        payment.PaymentReferenceDate = paymentsPayModalViewModel.Payment.PaymentReferenceDate;
                        payment.PaymentTypeID = paymentsPayModalViewModel.Payment.PaymentTypeID;
                        payment.CreditCardID = paymentsPayModalViewModel.Payment.CreditCardID;
                        payment.CheckNumber = paymentsPayModalViewModel.Payment.CheckNumber;
                        payment.IsPaid = true;


                        _context.Update(payment);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public async Task UpdateWithoutTransactionAsync(PaymentsInvoiceViewModel paymentsInvoiceViewModel)
        {
            var payment = await ReadOneAsync( new List<Expression<Func<Payment, bool>>> { p => p.PaymentID == paymentsInvoiceViewModel.Payment.PaymentID });
            payment.Reference = paymentsInvoiceViewModel.Payment.Reference;
            payment.CompanyAccountID = paymentsInvoiceViewModel.Payment.CompanyAccountID;
            payment.PaymentReferenceDate = paymentsInvoiceViewModel.Payment.PaymentReferenceDate;
            payment.PaymentTypeID = paymentsInvoiceViewModel.Payment.PaymentTypeID;
            payment.CreditCardID = paymentsInvoiceViewModel.Payment.CreditCardID;
            payment.CheckNumber = paymentsInvoiceViewModel.Payment.CheckNumber;
            payment.IsPaid = true;
            payment.HasInvoice = true;
            payment.Invoice = paymentsInvoiceViewModel.Invoice;
            payment.Sum = paymentsInvoiceViewModel.Requests.FirstOrDefault().Payments.FirstOrDefault().Sum;
            _context.Update(payment);
            await _context.SaveChangesAsync();
        }

    }
}
