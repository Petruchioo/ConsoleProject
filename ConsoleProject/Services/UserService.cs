using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleProject.Interface;
using ConsoleProject.Models;

namespace ConsoleProject.Services
{
    public class UserService : IUser
    {
        public List<User> _users = new List<User>();
        private User _currentUser;
        private string _userFileName = "Users.json";
        static readonly Service _service = new Service();

        public User GetByUserId(int userId)
        {
            var user = _users.FirstOrDefault(x => x.UserId == userId);
            if (user == null) { throw new KeyNotFoundException($"User with id {userId} not found"); }
            return user;
        }

        public User Registration(string username)
        {
            _service.ValidateString(username, nameof(username));
            _service.ValidateStringLatin(username, nameof(username));

            if (_users.Any(u => u.UserName == username))
            {
                throw new ArgumentException($"User with user name: {username} already exists", nameof(username));
            }

            var newUser = new User
            {
                UserName = username,
                UserId = _users.Count + 1,
                Notes = new List<Note>()
            };

            try
            {
                _users.Add(newUser);
            }
            catch (Exception ex) { Console.WriteLine($"Error from add User: {username}", ex); }

            SaveUser();

            return newUser;
        }

        public User Login(string username)
        {
            _service.ValidateString(username, nameof(username));
            _service.ValidateStringLatin(username, nameof(username));

            if (_users.Any(u => u.UserName != username))
            {
                throw new ArgumentException($"User with user name: {username} does not exists", nameof(username));
            }

            var _currentUser = _users.FirstOrDefault(x => x.UserName == username);
            return _currentUser;
        }

        public void SaveUser()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_users, options);
                File.WriteAllText(_userFileName, jsonString);
            }
            catch (Exception ex) { throw new InvalidOperationException("Error from Save User", ex); }
        }

        public void ShowUser(int userId)
        {
            var user = GetByUserId(userId);

            try
            {
                if (!File.Exists(_userFileName))
                {
                    throw new IOException($"File {_userFileName} not found");
                }

                var json = File.ReadAllText(_userFileName);
                if (string.IsNullOrEmpty(json))
                {
                    throw new KeyNotFoundException($"Note for ID = {userId} not found");
                }

                _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

                Console.WriteLine(user);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File access error {_userFileName}: {ex.Message}");
            }
            catch (System.Text.Json.JsonException ex)
            {
                Console.WriteLine($"Deserialization JSON error: {ex.Message}");
            }
        }
    }
}
