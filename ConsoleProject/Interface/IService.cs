using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleProject.Models;

namespace ConsoleProject.Interface
{
    public interface IService
    {
        void Registration();
        void Login();
        void GetAllNotes();
        void AddNote();
        void CompletedNote();
        void ChangeNote();
        void DeleteNote();
        void GetAllComand();
        void Exit();

    }
}
