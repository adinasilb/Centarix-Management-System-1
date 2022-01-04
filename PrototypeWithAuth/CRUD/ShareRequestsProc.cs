using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class ShareRequestsProc : ShareBasesProc<ShareRequest>
    {
        public ShareRequestsProc(ApplicationDbContext context, bool FromBase = false) : base(context, FromBase)
        {
            if (!FromBase)
            {
                this.InstantiateProcs();
            }
        }

        public override async Task<StringWithBool> DeleteAsync(int objectID, string userID)
        {
            return await base.DeleteAsync(objectID, userID);
        }

        public async Task<StringWithBool> UpdateAsync(int objectID, string currentUserID, List<string> userIDs)
        {
            return await base.UpdateAsync(objectID, currentUserID, userIDs, new ShareRequest());
        }
    }
}
