using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? NoteDescription { get; set; }
        public DateTime CreationTime { get; set; }
        public bool NoteIsCompleted { get; set; } = false;
        public int NoteUserId { get; set; }

    }
}
