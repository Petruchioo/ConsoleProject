using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Exceptions
{
    internal class ArgumentNotFoundExceptions : Exception
    {
        public ArgumentNotFoundExceptions(int id) : base($"ID = {id} is not found") { }
    }
}
