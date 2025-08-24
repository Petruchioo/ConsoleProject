using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleProject.Models;
using ConsoleProject.Services;

namespace ConsoleProject
{
    internal class Program
    {
        private static readonly Service _service = new Service();
        static void Main(string[] args)
        {
            _service.Menu();
        }
    }
}