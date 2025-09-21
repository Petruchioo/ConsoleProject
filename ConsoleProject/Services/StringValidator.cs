using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleProject.Services
{
    public class StringValidator
    {
        

        public void ValidateString(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{paramName} cannot be empty", paramName);
        }
    }
}
