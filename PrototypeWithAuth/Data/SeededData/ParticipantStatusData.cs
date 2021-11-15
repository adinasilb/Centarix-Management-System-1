using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public static class ParticipantStatusData
    {
        public static List<ParticipantStatus> Get()
        {
            List<ParticipantStatus> list = new List<ParticipantStatus>();
            list.Add(new ParticipantStatus
            {
                ParticipantStatusID = 1,
                Description = "Active"
            });
            list.Add(new ParticipantStatus
            {
                ParticipantStatusID = 2,
                Description = "Dropout"
            });
            return list;
        }

    }
}
