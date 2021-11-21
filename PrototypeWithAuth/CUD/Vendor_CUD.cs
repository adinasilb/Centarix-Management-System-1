using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CUD
{
    public class Vendor_CUD
    {
        public async Task Create(ApplicationDbContext context, Vendor Vendor)
        {
            context.Add(Vendor);
            await context.SaveChangesAsync();
        }

        public async Task Update(ApplicationDbContext context, Vendor Vendor)
        {
            context.Update(Vendor);
            await context.SaveChangesAsync();
        }

        public async Task Delete(ApplicationDbContext context, Vendor Vendor)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                if (Vendor.Products.Any())
                {
                    return;
                }
                else
                {
                    if (Vendor.VendorCategoryTypes.Any())
                    {
                        foreach (var vct in Vendor.VendorCategoryTypes)
                        {
                            context.Remove(vct);
                        }
                    }
                    if (Vendor.VendorContacts.Any())
                    {
                        foreach (var vc in Vendor.VendorContacts)
                        {
                            context.Remove(vc);
                        }
                    }
                    if (Vendor.VendorComments.Any())
                    {
                        foreach(var vc in Vendor.VendorComments)
                        {
                            context.Remove(vc);
                        }
                    }
                }
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
    }
}
