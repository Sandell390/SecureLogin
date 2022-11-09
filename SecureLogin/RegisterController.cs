using Microsoft.AspNetCore.Mvc;

namespace SecureLogin;

[ApiController]
[Route("/[controller]/[action]")]
public class RegisterController : Controller
{
    Database db = new Database();
    
    [HttpPost]
    public string RegisterUser([FromBody] User user)
    {
        
        if (db.DoesUserExist(user.Username))
        {
            Console.WriteLine("User already exists");
            return "Username already exists";
        }

        string passwordHash = user.Password.Substring(0, 64);
        string passwordSalt = user.Password.Substring(64, 44);
        
        db.Register(user.Username, passwordHash + passwordSalt);
        db.RegisterSalt(user.Username, passwordSalt);
        
        Console.WriteLine("User registered");
        return "Success";
    }
    
    [HttpPost]
    public string RequestSalt()
    {
        return Hashing.GenerateSalt();
    }
    
}