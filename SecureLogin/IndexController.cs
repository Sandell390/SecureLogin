using Microsoft.AspNetCore.Mvc;

namespace SecureLogin;

[ApiController]
[Route("/[controller]/[action]")]
public class IndexController : Controller
{
    Database db = new Database();

    [HttpPost]
    public string Login([FromBody] User user)
    {
        string userPassword = db.GetPassHash(user.Username);
        
        bool result = Hashing.VerifyPassword(userPassword,db.GetSalt(user.Username), user.Password);

        string newSalt = Hashing.GenerateSalt();
        string password = userPassword.Substring(0, 64);
        
        
        db.UpdateSalt(user.Username, newSalt);
        db.UpdatePassHash(user.Username, password + newSalt);

        return result == true ? "success" : "failed";
    }

    [HttpPost]
    public string RequestSalt([FromBody] User user)
    {
        if (db.DoesUserExist(user.Username))
        {
            return db.GetSalt(user.Username);
        }

        return "Username does not exist";
    }
}
