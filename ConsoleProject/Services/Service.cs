using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsoleProject.Interface;
using ConsoleProject.Models;
using ConsoleProject.Services;

namespace ConsoleProject.Services
{
    public class Service
    {
        private readonly IUser _userService;
        private readonly INote _noteService;
        private User _currentUser;
        private bool isRunning = true;

        private readonly Dictionary<string, Action> _commands;

        public Service(IUser userService, INote noteService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));

            _commands = new Dictionary<string, Action>
            {
                { "1", Registration },
                { "2", Login },
                { "3", GetAllNotes },
                { "4", AddNote },
                { "5", CompletedNote },
                { "6", ChangeNote },
                { "7", DeleteNote },
                { "8", GetAllCommands },
                { "9", Exit }
            };
        }

        public void RunMenu()
        {

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine(File.ReadAllText("AllComands.txt"));
                Console.WriteLine("\nEnter command number");
                string input = Console.ReadLine();

                if (_commands.TryGetValue(input, out Action action))
                {

                    action.Invoke();
                    if (input == "9")
                        isRunning = false;

                }
                else
                {
                    Console.WriteLine("Invalid input\nTry again");
                    Console.ReadKey();
                }
            }

        }

        public void Registration()
        {
            Console.Clear();
            Console.WriteLine(File.ReadAllText("RegistrationWindow.txt"));

            string input = Console.ReadLine();

            _currentUser = _userService.Registration(input);
            Console.WriteLine($"Welcome {input}");


            Console.ReadKey();
        }

        public void Login()
        {
            try
            {

                Console.Clear();
                Console.WriteLine(File.ReadAllText("LoginWindow.txt"));

                string input = Console.ReadLine();

                _currentUser = _userService.Login(input);

                Console.WriteLine($"Welcome {_currentUser.UserName}");



                Console.ReadKey();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка регистрации: {ex.Message}");
            }

        }

        public void GetAllNotes()
        {
            Console.Clear();
            Console.WriteLine($"All notes by {_currentUser.UserName}");

            var notes = _noteService.GetAllNotes(_currentUser.UserId);

            if (!notes.Any())
            {
                Console.WriteLine("You have no notes");
            }
            else
            {
                foreach (var note in notes)
                {
                    Console.WriteLine($"ID: {note.Id}");
                    Console.WriteLine($"Title: {note.Title}");
                    Console.WriteLine($"Description: {note.NoteDescription}");
                    Console.WriteLine($"Status: {(note.NoteIsCompleted ? "Completed" : "No Compled")}");
                    Console.WriteLine("-------------------");
                }
            }
            Console.ReadKey();

        }



        public void AddNote()
        {
            Console.Clear();
            Console.Write($"New Note\nTitle:");
            string inputTitle = Console.ReadLine().Trim();
            Console.WriteLine("Description:");
            string inputDescription = Console.ReadLine().Trim();

            _noteService.AddNote(inputTitle, inputDescription, _currentUser.UserId);

            Console.ReadKey();
        }

        public void CompletedNote()
        {

            GetAllNotes();

            Console.WriteLine("\nSelect the note you want to complete.\nAnd enter its Title");


            string input = Console.ReadLine();

            int noteId = _noteService.GetIDNoteByTitle(input);

            _noteService.CompletedNote(noteId);


            Console.ReadKey();
        }

        public void ChangeNote()
        {
            GetAllNotes();

            
            Console.WriteLine("Select the note you want to change.\nAnd enter its Title");


            string input = Console.ReadLine();

            int noteId = _noteService.GetIDNoteByTitle(input);

            Console.WriteLine($"New description note {input}");
            string inputDescription = Console.ReadLine();

            _noteService.ChangeNote(noteId, input, inputDescription);

            Console.ReadKey();
        }

        public void DeleteNote()
        {
            GetAllNotes();
            
            Console.WriteLine("\nSelect the note you want to delete.\nAnd enter its Title");


            string input = Console.ReadLine();

            int noteId = _noteService.GetIDNoteByTitle(input);


            _noteService.DeleteNote(noteId);

            Console.ReadKey();
        }

        public void GetAllCommands()
        {
            Console.Clear();


            Console.WriteLine(File.ReadAllText("AllComands.txt"));


            Console.ReadKey();
        }

        public void Exit()
        {
            Console.Clear();
            Console.WriteLine("Completion of the program");
        }

    }
}
