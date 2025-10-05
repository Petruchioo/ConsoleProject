using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleProject.Interface;
using ConsoleProject.Models;
using ConsoleProject.Exceptions;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace ConsoleProject.Services
{
    public class NoteService : INote
    {
        public List<Note> notes = new List<Note>();
        private string _noteFileName = "Notes.json";
        private readonly IUser _userService;
        private readonly IValidator _stringValidator;


        public NoteService(IValidator stringValidator, IUser userService)
        {
            _stringValidator = stringValidator ?? throw new ArgumentNullException(nameof(stringValidator));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public IEnumerable<Note> GetAllNotes(int userId)
        {
            if (_userService.GetByUserId(userId) == null)
                throw new ArgumentNullException(nameof(userId), $"User with id {userId} not found");

            notes = MyDeserialize(_noteFileName);

            return notes.Where(n => n.NoteUserId == userId).ToList();
        }

        public Note GetById(int id)
        {
            notes = MyDeserialize(_noteFileName);

            var note = notes.FirstOrDefault(n => n.Id == id);
            if (note is null)
            {
                throw new KeyNotFoundException($"Note {id} not found Exception");
            }

            return note;
        }

        public int GetIDNoteByTitle(string title)
        {
            _stringValidator.ValidateString(title, nameof(title));
            notes = MyDeserialize(_noteFileName);

            var note = notes.FirstOrDefault(n => n.Title == title);
            if (note is null) { throw new KeyNotFoundException($"Note {title} not found Exception"); }

            return note.Id;
        }

        public Note AddNote(string title, string noteDescription, int noteUserId)
        {
            _stringValidator.ValidateString(title, nameof(title));
            notes = MyDeserialize(_noteFileName);

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

            notes.Add(newNote);
            SaveNote();
            return newNote;
        }

        public void DeleteNote(int noteId)
        {
            notes = MyDeserialize(_noteFileName);

            var note = GetById(noteId);
            if (note is null) throw new NoteNotFoundExceptions(noteId);

            notes.Remove(note);
            SaveNote();

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
            _stringValidator.ValidateString(title, nameof(title));

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

        //public void ShowNote(int noteId)
        //{
        //    var note = GetById(noteId);

        //    try
        //    {
        //        if (!File.Exists(_noteFileName))
        //        {
        //            throw new IOException($"File {_noteFileName} not found");
        //        }

        //        var json = File.ReadAllText(_noteFileName);

        //        if (string.IsNullOrEmpty(json))
        //        {
        //            throw new KeyNotFoundException($"Note for ID = {noteId} not found");
        //        }

        //        notes = JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();

        //        Console.WriteLine(json);
        //    }
        //    catch (IOException ex)
        //    {
        //        Console.WriteLine($"File access error {_noteFileName}: {ex.Message}");
        //    }
        //    catch (System.Text.Json.JsonException ex)
        //    {
        //        Console.WriteLine($"Deserialization JSON error: {ex.Message}");
        //    }
        //}

        public List<Note> MyDeserialize(string FileName)
        {
            var json = File.ReadAllText(_noteFileName);
            var jsonListNotes = new List<Note>();
            jsonListNotes = JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();

            return jsonListNotes;
        }
    }
}
