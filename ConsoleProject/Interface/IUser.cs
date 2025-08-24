using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleProject.Models;

namespace ConsoleProject.Interface
{
    public interface IUser
    {
        User Registration (string username);
        User Login (string username);
    }
}
