using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.Exceptions
{
    public class ModelStateInvalidException : Exception
    {
        public ModelStateInvalidException()
        {
        }

        public ModelStateInvalidException(string message) : base(message)
        {
        }

        public ModelStateInvalidException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
