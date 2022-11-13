using Microsoft.AspNetCore.Mvc;

namespace SecureLogin;

[ApiController]
[Route("/[controller]/[action]")]
public class RegisterController : Controller
{
    Database db = new Database();
    
    static Dictionary<string,string> users = new Dictionary<string,string>();

    [HttpPost]
    public string RegisterUser([FromBody] User user)
    {
        
        if (db.DoesUserExist(user.Username))
        {
            Console.WriteLine("User already exists");
            return "Username already exists";
        }

        db.Register(user.Username, user.Password);
        db.RegisterPepper(user.Username, users[user.Username]);
        users.Remove(user.Username);
        Console.WriteLine("User registered");
        return "Success";
    }
    
    [HttpPost]
    public string RequestPepper([FromBody] User user)
    {
        Console.WriteLine("Requesting Pepper");
        string pepper = Hashing.GeneratePepper();
        users.Add(user.Username, pepper);
        return pepper;
    }
    
}