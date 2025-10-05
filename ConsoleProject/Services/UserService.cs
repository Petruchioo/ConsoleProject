using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleProject.Exceptions;
using ConsoleProject.Interface;
using ConsoleProject.Models;

namespace ConsoleProject.Services
{
    public class UserService : IUser
    {
        public List<User> _users = new List<User>();
        private string _userFileName = "Users.json";

        private readonly IValidator _validator;

        public UserService(IValidator validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public User GetByUserId(int userId)
        {
            _users = MyDeserialize(_userFileName);

            var user = _users.FirstOrDefault(x => x.UserId == userId);
            if (user == null) { throw new UserNotFoundException(userId); }
            return user;
        }

        public User Registration(string username)
        {
            _validator.ValidateString(username, nameof(username));

            _users = MyDeserialize(_userFileName);


            if (_users.Any(u => u.UserName == username))
            {
                throw new ArgumentException($"User with user name: {username} already exists", nameof(username));
            }

            var newUser = new User
            {
                UserName = username,
                UserId = _users.Count + 1
            };

            _users.Add(newUser);

            SaveUser();

            return newUser;
        }

        public User Login(string username)
        {
            _validator.ValidateString(username, nameof(username));

            MyDeserialize(_userFileName);

            if (_users.Any(u => u.UserName == username))
            {
                return _users.FirstOrDefault(x => x.UserName == username);
            }
            else { throw new ArgumentException($"User with user name: {username} does not exists", nameof(username)); }
            
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

        public List<User> MyDeserialize(string userFileName)
        {
            var json = File.ReadAllText(userFileName);
            //var jsonListUser = new List<User>();
            _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

            return _users;
        }
        //public void ShowUser(int userId)
        //{
        //    var user = GetByUserId(userId);

        //    try
        //    {
        //        if (!File.Exists(_userFileName))
        //        {
        //            throw new IOException($"File {_userFileName} not found");
        //        }

        //        var json = File.ReadAllText(_userFileName);
        //        if (string.IsNullOrEmpty(json))
        //        {
        //            throw new UserNotFoundException(userId);
        //        }

        //        _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        //        Console.WriteLine(user);
        //    }
        //    catch (IOException ex)
        //    {
        //        Console.WriteLine($"File access error {_userFileName}: {ex.Message}");
        //    }
        //    catch (System.Text.Json.JsonException ex)
        //    {
        //        Console.WriteLine($"Deserialization JSON error: {ex.Message}");
        //    }
        //}
    }
}
