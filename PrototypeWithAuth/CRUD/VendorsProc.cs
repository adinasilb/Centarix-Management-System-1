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


        public async Task<StringWithBool> UpdateWithSaveChangesAsync(CreateSupplierViewModel createSupplierViewModel, string UserID)
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
                            Create(createSupplierViewModel.Vendor);
                        }
                        else
                        {
                            Update(createSupplierViewModel.Vendor);
                        }
                        await _context.SaveChangesAsync();
                        var vendor = await ReadOne(new List<Expression<Func<Vendor, bool>>> { v=>v.VendorID == createSupplierViewModel.Vendor.VendorID }, 
                            new List<ComplexIncludes<Vendor, ModelBase>> { new ComplexIncludes<Vendor, ModelBase> { Include= v => v.VendorCategoryTypes } } );
                        if (vendor.VendorCategoryTypes.Count() > 0)
                        {
                            foreach (var type in createSupplierViewModel.Vendor.VendorCategoryTypes)
                            {
                                _vendorCategoryTypesProc.Remove(type);
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
                                var dvc = await _vendorContactsProc.ReadOne(new List<Expression<Func<VendorContact, bool>>> { vc => vc.VendorContactID == vendorContact.VendorContact.VendorContactID });
                                _vendorContactsProc.Remove(dvc);
                            }
                            else if (!vendorContact.Delete)
                            {
                                vendorContact.VendorContact.VendorID = createSupplierViewModel.Vendor.VendorID;
                                _vendorContactsProc.Update(vendorContact.VendorContact);
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
                                        _context.Add(vendorComment);
                                    }
                                    else
                                    {    
                                        _context.Update(vendorComment);
                                    }
                                }
                                else
                                {
                                    var vendorCommentDB = _context.VendorComments.Where(c => c.CommentID == vendorComment.CommentID).FirstOrDefault();
                                    if (vendorCommentDB != null)
                                    {
                                        vendorCommentDB.IsDeleted = true;
                                        _vendorCommentsProc.Update(vendorCommentDB);
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

    }




}
