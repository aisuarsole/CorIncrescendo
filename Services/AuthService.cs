using CorIncrescendo.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CorIncrescendo.Services
{
    public class AuthService
    {
        private const string UsersKey = "users";
        private const string CurrentUserKey = "current_user";

        public async Task<(bool ok, string error)> RegistrarAsync(string email, string password)
        {
            var users = GetUsers();
            if (users.Any(u => u.Email.ToLower() == email.ToLower()))
                return (false, "Aquest email ja està registrat.");

            var user = new User
            {
                Email = email,
                PasswordHash = HashPassword(password)
            };
            users.Add(user);
            SaveUsers(users);
            await GuardarSessionAsync(user);
            return (true, string.Empty);
        }

        public async Task<(bool ok, string error)> LoginAsync(string email, string password)
        {
            var users = GetUsers();
            var user = users.FirstOrDefault(u =>
                u.Email.ToLower() == email.ToLower() &&
                u.PasswordHash == HashPassword(password));

            if (user == null)
                return (false, "Email o contrasenya incorrectes.");

            await GuardarSessionAsync(user);
            return (true, string.Empty);
        }

        public void Logout()
        {
            Preferences.Remove(CurrentUserKey);
        }

        public User? GetCurrentUser()
        {
            var json = Preferences.Get(CurrentUserKey, null);
            return json == null ? null : JsonSerializer.Deserialize<User>(json);
        }

        public bool IsLoggedIn() => GetCurrentUser() != null;

        private Task GuardarSessionAsync(User user)
        {
            Preferences.Set(CurrentUserKey, JsonSerializer.Serialize(user));
            return Task.CompletedTask;
        }

        private List<User> GetUsers()
        {
            var json = Preferences.Get(UsersKey, "[]");
            return JsonSerializer.Deserialize<List<User>>(json) ?? new();
        }

        private void SaveUsers(List<User> users)
        {
            Preferences.Set(UsersKey, JsonSerializer.Serialize(users));
        }

        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password + "CorIncrescendo_Salt"));
            return Convert.ToBase64String(bytes);
        }
    }
}


