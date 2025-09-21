using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Exceptions
{
    internal class UserNotFoundException : Exception
    {
        public UserNotFoundException(int id) : base($"User ID = {id} is not found") { }
    }
}
