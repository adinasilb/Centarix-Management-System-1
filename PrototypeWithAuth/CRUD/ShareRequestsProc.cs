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


    }
}
