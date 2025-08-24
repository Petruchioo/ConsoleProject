using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleProject.Models;

namespace ConsoleProject.Interface
{
    public interface INote
    {
        IEnumerable<Note> GetAllNotes(int userId);

        Note GetById(int id);
        Note AddNote(string title, string noteDescription, User owner);
        void CompletedNote (int id);
        void ChangeNote (int id, string title, string noteDescription);
        void DeleteNote (int id);
    }
}
