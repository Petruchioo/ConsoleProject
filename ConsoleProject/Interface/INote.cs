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
        int GetIDNoteByTitle(string title);
        Note AddNote(string title, string noteDescription, int noteUserId);
        void CompletedNote (int id);
        void ChangeNote (int id, string title, string noteDescription);
        void DeleteNote (int id);
        //void ShowNote(int id);
    }
}
