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
        private static readonly Service _service;

        static Program()
        {
            IUser userService = new UserService();
            INote noteService = new NoteService();

            _service = new Service(userService, noteService);
        }
        static void Main(string[] args)
        {

            _service.RunMenu();
        }
    }
}