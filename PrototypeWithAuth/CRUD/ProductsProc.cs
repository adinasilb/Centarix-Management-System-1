using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.CRUD
{
    public class ProductsProc : ApplicationDbContextProc<Product>
    {
        public ProductsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> DeleteAsync(Product product)
        {

            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var productRequests = _requestsProc.Read( new List<Expression<Func<Request, bool>>> { r => r.ProductID == product.ProductID }).ToList();
                if (productRequests.Count() == 0)
                {
                    await _productCommentsProc.DeleteAsync(product.ProductID);
                    product.IsDeleted = true;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }


    }
}
