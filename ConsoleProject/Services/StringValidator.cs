using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsoleProject.Interface;

namespace ConsoleProject.Services
{
    public class StringValidator : IValidator
    {
        public void ValidateString(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{paramName} cannot be empty", paramName);
        }
    }
}
