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
        static readonly UserService _userService = new UserService();
        static readonly NoteService _noteService = new NoteService();
        private static User _currentUser;
        public bool isRunnig = true;

        private static readonly Dictionary<string, Action> _comands = new Dictionary<string, Action>
        {
            {"1", Registration},
            {"2", Login},
            {"3", GetAllNotes },
            {"4", AddNote },
            {"5", CompletedNote },
            {"6", ChangeNote },
            {"7", DeleteNote },
            {"8", GetAllComand },
            {"9", Exit }
        };
        public void Menu()
        {

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine(File.ReadAllText("AllComands.txt"));
                Console.WriteLine("\nEnter team number");
                string input = Console.ReadLine();

                if (_comands.TryGetValue(input, out Action action))
                {
                    try
                    {
                        action.Invoke();
                        if (input == "9")
                            isRunnig = false;
                    }
                    catch (Exception ex) { Console.WriteLine("Error", ex); }
                }
                else
                {
                    Console.WriteLine("Invalid input\nTry again");
                    Console.ReadKey();
                }
            }

        }

        public static void Registration()
        {
            Console.Clear();
            Console.WriteLine(File.ReadAllText("RegistrationWindow.txt"));

            string input = Console.ReadLine();

            try
            {
                _currentUser = _userService.Registration(input);
                Console.WriteLine($"Welcome {input}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error registration", ex);
            }

            Console.ReadKey();
        }

        public static void Login()
        {
            Console.Clear();
            Console.WriteLine(File.ReadAllText("LoginWindow.txt"));

            string input = Console.ReadLine();

            try
            {
                _currentUser = _userService.Login(input);
                Console.WriteLine($"Welcome {input}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error registration", ex);
            }

            Console.ReadKey();
        }

        public static void GetAllNotes()
        {
            Console.Clear();
            Console.WriteLine($"All notes by {_currentUser.UserName}");

            try
            {
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
            }
            catch (Exception ex) { Console.WriteLine("Error", ex); }

            Console.ReadKey();
        }

        public static void AddNote()
        {
            Console.Clear();
            Console.Write($"New Note\nTitle:");
            string inputTitle = Console.ReadLine().Trim();
            Console.WriteLine("Description:");
            string inputDescription = Console.ReadLine().Trim();
            try
            {

                _noteService.AddNote(inputTitle, inputDescription, _currentUser);
            }
            catch (Exception ex) { Console.WriteLine("Error", ex); }

            Console.ReadKey();
        }

        public static void CompletedNote()
        {
            Console.Clear();
            Console.WriteLine("Select the note you want to complete.\nAnd enter its Title");

            GetAllNotes();

            string input = Console.ReadLine();

            int noteId = _noteService.GetIDNoteByTitle(input);

            try
            {
                _noteService.CompletedNote(noteId);
            }
            catch (Exception ex) { Console.WriteLine("Error", ex); }

            Console.ReadKey();
        }

        public static void ChangeNote()
        {
            Console.Clear();
            Console.WriteLine("Select the note you want to change.\nAnd enter its Title");

            GetAllNotes();

            string input = Console.ReadLine();

            int noteId = _noteService.GetIDNoteByTitle(input);

            Console.WriteLine($"New description note {input}");
            string inputDescription = Console.ReadLine();

            try
            {
                _noteService.ChangeNote(noteId, input, inputDescription);
            }
            catch (Exception ex) { Console.WriteLine("Error", ex); }


            Console.ReadKey();
        }

        public static void DeleteNote()
        {
            Console.Clear();
            Console.WriteLine("Select the note you want to delete.\nAnd enter its Title");

            GetAllNotes();

            string input = Console.ReadLine();

            int noteId = _noteService.GetIDNoteByTitle(input);

            try
            {
                _noteService.DeleteNote(noteId);
            }
            catch (Exception ex) { Console.WriteLine("Error", ex); }
            Console.ReadKey();
        }

        public static void GetAllComand()
        {
            Console.Clear();

            try
            {
                Console.WriteLine(File.ReadAllText("AllComands.txt"));
            }
            catch (Exception ex) { Console.WriteLine("Error", ex); }

            Console.ReadKey();
        }

        public static void Exit()
        {
            Console.Clear();
            Console.WriteLine("Completion of the program");
        }
        public void ValidateString(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{paramName} cannot be empty", paramName);
        }

        public void ValidateStringLatin(string value, string paramName)
        {
            if (!Regex.IsMatch(value, @"^[a-zA-Z]+$"))
            {
                throw new ArgumentException($"{paramName} должен содержать только латинские буквы.", paramName);
            }
        }

    }
}
