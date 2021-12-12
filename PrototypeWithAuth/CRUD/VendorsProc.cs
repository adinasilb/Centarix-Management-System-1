using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.Exceptions;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
namespace PrototypeWithAuth.CRUD
{
    public class VendorsProc :  ApplicationDbContextProc<Vendor>
    {
        public VendorsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public override IQueryable<Vendor> Read(List<Expression<Func<Vendor, bool>>> wheres = null, List<ComplexIncludes<Vendor, ModelBase>> includes = null)
        {
            return base.Read(wheres, includes);
        }
        public override async Task<Vendor> ReadOne(List<Expression<Func<Vendor, bool>>> wheres = null, List<ComplexIncludes<Vendor, ModelBase>> includes = null)
        {
            return await base.ReadOne(wheres, includes);
        }

        public  async Task<StringWithBool> UpdateAsync(CreateSupplierViewModel createSupplierViewModel, ModelStateDictionary ModelState, string UserID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var context = new ValidationContext(createSupplierViewModel.Vendor, null, null);
                    var results = new List<ValidationResult>();

                    if (Validator.TryValidateObject(createSupplierViewModel.Vendor, context, results, true))
                    {
                        if (createSupplierViewModel.Vendor.VendorID==0)
                        {
                            await CreateWithSaveChangesAsync(createSupplierViewModel.Vendor);
                        }
                        else
                        {
                            await UpdateWithSaveChangesAsync(createSupplierViewModel.Vendor);
                        }
                        await _context.SaveChangesAsync();
                        var vendor = await
                            _context.Vendors.Where(v => v.VendorID == createSupplierViewModel.Vendor.VendorID).Include(v => v.VendorCategoryTypes).FirstOrDefaultAsync();
                        if (vendor.VendorCategoryTypes.Count() > 0)
                        {
                            foreach (var type in createSupplierViewModel.Vendor.VendorCategoryTypes)
                            {
                                //need proc?
                                _context.Remove(type);
                            }
                        }
                        foreach (var type in createSupplierViewModel.VendorCategoryTypes)
                        {
                            _vendorCategoryTypesProc.Create(new VendorCategoryType { VendorID  = createSupplierViewModel.Vendor.VendorID, CategoryTypeID= type });
                        }

                        foreach (var vendorContact in createSupplierViewModel.VendorContacts)
                        {
                            if (vendorContact.Delete && vendorContact.VendorContact.VendorContactID != 0)
                            {
                                var dvc = _context.VendorContacts.Where(vc => vc.VendorContactID == vendorContact.VendorContact.VendorContactID).FirstOrDefault();
                                //need proc?
                                _context.Remove(dvc);
                            }
                            else if (!vendorContact.Delete)
                            {
                                vendorContact.VendorContact.VendorID = createSupplierViewModel.Vendor.VendorID;
                                //need proc?
                                _context.Update(vendorContact.VendorContact);
                            }

                        }
                        if (createSupplierViewModel.Comments != null)
                        {
                            foreach (var vendorComment in createSupplierViewModel.Comments)
                            {
                                if (!vendorComment.IsDeleted)
                                {
                                    vendorComment.ObjectID = createSupplierViewModel.Vendor.VendorID;
                                    if (vendorComment.CommentID == 0)
                                    {
                                        vendorComment.ApplicationUserID = UserID;
                                        vendorComment.CommentTimeStamp = DateTime.Now;
                                        //need proc?
                                        _context.Add(vendorComment);
                                    }
                                    else
                                    {    //need proc?
                                        _context.Update(vendorComment);
                                    }
                                }
                                else
                                {
                                    var vendorCommentDB = _context.VendorComments.Where(c => c.CommentID == vendorComment.CommentID).FirstOrDefault();
                                    if (vendorCommentDB != null)
                                    {
                                        vendorCommentDB.IsDeleted = true;
                                        //need proc?
                                        _context.Update(vendorCommentDB);
                                    }
                                }
                            }
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        ReturnVal.Bool = true;
                    }
                    else
                    {
                        ReturnVal.Bool = false;
                        ReturnVal.String = "Model State Invalid Exception";
                        throw new ModelStateInvalidException();
                    }
                }
                catch (Exception ex)
                {
                    ReturnVal.Bool = false;
                    ReturnVal.String = AppUtility.GetExceptionMessage(ex);
                    await transaction.RollbackAsync();
                }
            }
            return ReturnVal;
        }

        public override async Task<StringWithBool> UpdateWithSaveChangesAsync(Vendor vendor)
        {
            return await base.UpdateWithSaveChangesAsync(vendor);
        }
    }




}
