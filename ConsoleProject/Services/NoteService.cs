using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleProject.Interface;
using ConsoleProject.Models;
using ConsoleProject.Exceptions;
using System.Text.Json;

namespace ConsoleProject.Services
{
    public class NoteService : INote
    {
        public List<Note> notes = new List<Note>();
        private string _noteFileName = "Notes.json";
        private readonly IUser _userService;
        //static readonly UserService _userService = new UserService();

        //private readonly IValidator _stringValidator;тест

        //static readonly Service _service = new Service();
        public NoteService(IValidator stringValidator, IUser userService)
        {
            //_stringValidator = stringValidator;
            _userService = userService;
        }

        public NoteService()
        {
        }

        public IEnumerable<Note> GetAllNotes(int userId)
        {
            if (_userService.GetByUserId(userId) == null)
                throw new ArgumentNullException(nameof(userId), $"User with id {userId} not found");

            //return _notes.Where(n => n.Equals(userId)).ToList();
            return notes.Where(n => n.NoteUserId == userId).ToList();
        }

        public Note GetById(int id)
        {
            var note = notes.FirstOrDefault(n => n.Id == id);
            if (note is null)
            {
                throw new KeyNotFoundException($"Note {id} not found Exception");
            }

            return note;
        }

        public int GetIDNoteByTitle(string title)
        {
            //_stringValidator.ValidateString(title, nameof(title));

            var note = notes.FirstOrDefault(n => n.Title == title);
            if (note is null) { throw new KeyNotFoundException($"Note {title} not found Exception"); }

            return note.Id;
        }

        public Note AddNote(string title, string noteDescription, int noteUserId)
        {
           //_stringValidator.ValidateString(title, nameof(title));

            if (notes.Any(n => n.Title == title))
            {
                throw new ArgumentException($"Note with title: {title} already exists", nameof(title));
            }

            var newNote = new Note
            {
                Id = notes.Count + 1,
                Title = title,
                NoteDescription = noteDescription,
                CreationTime = DateTime.Now,
                NoteUserId = noteUserId
            };

            try
            {
                notes.Add(newNote);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error from add Note", ex);
            }
            SaveNote();
            return newNote;
        }

        public void DeleteNote(int noteId)
        {
            var note = GetById(noteId);
            if (note is null) throw new NoteNotFoundExceptions(noteId);

            try
            {
                notes.Remove(note);
            }
            catch (Exception ex) { Console.WriteLine("Error from Delete Note", ex); }

        }

        public void CompletedNote(int noteId)
        {
            var note = GetById(noteId);
            if (note is null) throw new NoteNotFoundExceptions(noteId);

            note.NoteIsCompleted = true;
            SaveNote();
        }

        public void ChangeNote(int noteId, string title, string noteDescription)
        {
            //_stringValidator.ValidateString(title, nameof(title));

            var note = GetById(noteId);

            if (noteDescription == null)
                note.NoteDescription = "";
            note.NoteDescription = noteDescription;
            SaveNote();
        }
        public void SaveNote()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(notes, options);
                File.WriteAllText(_noteFileName, jsonString);
            }
            catch (Exception ex) { throw new InvalidOperationException("Error from Save Note", ex); }
        }

        public void ShowNote(int noteId)
        {
            var note = GetById(noteId);

            try
            {
                if (!File.Exists(_noteFileName))
                {
                    throw new IOException($"File {_noteFileName} not found");
                }

                var json = File.ReadAllText(_noteFileName);
                if (string.IsNullOrEmpty(json))
                {
                    throw new KeyNotFoundException($"Note for ID = {noteId} not found");
                }

                notes = JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();

                Console.WriteLine(note);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File access error {_noteFileName}: {ex.Message}");
            }
            catch (System.Text.Json.JsonException ex)
            {
                Console.WriteLine($"Deserialization JSON error: {ex.Message}");
            }
        }


    }
}
