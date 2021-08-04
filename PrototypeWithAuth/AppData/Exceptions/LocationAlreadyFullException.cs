using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.Exceptions
{
    public class LocationAlreadyFullException : Exception
    {
        public LocationAlreadyFullException()
        {
        }

        public LocationAlreadyFullException(string message) : base(message)
        {
        }

        public LocationAlreadyFullException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
