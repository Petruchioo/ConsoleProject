using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleProject.Models;

namespace ConsoleProject.Interface
{
    public interface IValidator
    {
        void ValidateString(string value, string paramName);
        

    }

}