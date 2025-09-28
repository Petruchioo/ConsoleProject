using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleProject.Models;
using ConsoleProject.Services;
using ConsoleProject.Interface;

namespace ConsoleProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IUser userService = new UserService();
            INote noteService = new NoteService();

            var service = new Service(userService, noteService);

            service.RunMenu();
        }
    }
}