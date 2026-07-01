using CorIncrescendo.Models;
using Plugin.Firebase.Auth;
using System.Text.Json;

namespace CorIncrescendo.Services;

public class AuthService
{
    private readonly IFirebaseAuth _auth;
    private const string CurrentUserKey = "current_user";

    public AuthService(IFirebaseAuth auth)
    {
        _auth = auth;
    }

    public async Task<(bool ok, string error)> RegistrarAsync(
        string email, string password, string nom, string cognoms)
    {
        try
        {
            var result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
            await result.UpdateProfileAsync(displayName: $"{nom}|{cognoms}");

            var user = new User
            {
                Id = result.Uid,
                Email = email,
                Nom = nom,
                Cognoms = cognoms
            };
            GuardarSessionLocal(user);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, TraduirError(ex.Message));
        }
    }

    public async Task<(bool ok, string error)> LoginAsync(string email, string password)
    {
        try
        {
            var result = await _auth.SignInWithEmailAndPasswordAsync(email, password);
            var parts = result.DisplayName?.Split('|') ?? Array.Empty<string>();

            var user = new User
            {
                Id = result.Uid,
                Email = email,
                Nom = parts.Length > 0 ? parts[0] : email.Split('@')[0],
                Cognoms = parts.Length > 1 ? parts[1] : string.Empty
            };
            GuardarSessionLocal(user);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, TraduirError(ex.Message));
        }
    }

    public void Logout()
    {
        _auth.SignOut();
        Preferences.Remove(CurrentUserKey);
    }

    public User? GetCurrentUser()
    {
        var json = Preferences.Get(CurrentUserKey, null);
        return json == null ? null : JsonSerializer.Deserialize<User>(json);
    }

    public bool IsLoggedIn() => _auth.CurrentUser != null;

    private void GuardarSessionLocal(User user)
    {
        Preferences.Set(CurrentUserKey, JsonSerializer.Serialize(user));
    }

    private string TraduirError(string message) => message switch
    {
        var m when m.Contains("email-already-in-use") => "Aquest email ja està registrat.",
        var m when m.Contains("wrong-password") => "Contrasenya incorrecta.",
        var m when m.Contains("user-not-found") => "No existeix cap compte amb aquest email.",
        var m when m.Contains("invalid-email") => "El format de l'email no és vàlid.",
        var m when m.Contains("weak-password") => "La contrasenya és massa feble.",
        var m when m.Contains("network") => "Error de connexió. Comprova la xarxa.",
        _ => "S'ha produït un error. Torna-ho a intentar."
    };
}