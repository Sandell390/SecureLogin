using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SecureLogin;

[ApiController]
[Route("/[controller]/[action]")]
public class IndexController : Controller
{
    Database db = new Database();

    [HttpPost]
    public string Login([FromBody] User user)
    {
        Console.WriteLine("User Password: " + user.Password);
        string dbpasswordWithPepper = db.GetPassHash(user.Username);

        if (string.IsNullOrEmpty(dbpasswordWithPepper))
        {
            return "failed";
        }
        
        if (dbpasswordWithPepper == user.Password)
        {
            string newPepper = Hashing.GeneratePepper();
            db.UpdatePepper(user.Username, newPepper);
            return "success:" + newPepper;
        }
        else
        {
            return "failed";
        }
    }

    [HttpPost]
    public string RequestPepper([FromBody] User user)
    {
        if (db.DoesUserExist(user.Username))
        {
            return db.GetPepper(user.Username);
        }
        else
        {
            return "failed";
        }
    }
    
    public string UpdatePepper([FromBody] User user)
    {
        db.UpdatePassHash(user.Username, user.Password);
        return "/Privacy";
    }
}
