using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class ExperimentsProc : ApplicationDbContextProc<Experiment>
    {
        public ExperimentsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                this.InstantiateProcs();
            }
        }

        public IQueryable<Experiment> ReadWithParticipantsByID(int ExperimentID)
        {
            return _context.Experiments.Where(e => e.ExperimentID == ExperimentID)
                .Include(e => e.Participants).ThenInclude(p => p.Gender).Include(e => e.Participants).ThenInclude(p => p.ParticipantStatus).Take(1);
        }
    }
}
