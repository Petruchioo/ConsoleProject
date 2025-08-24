using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Models
{
    public class User
    {
        public string UserName { get; set; }
        public int UserId { get; set; }

        public List<Note> Notes { get; set; } = new List<Note>();

    }
}
