using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SecureLogin.Pages;

public class Register : PageModel
{
    Database db = new Database();
    
    public void OnGet()
    {
        
    }
    
    public void OnPost()
    {
        string username = Request.Form["uname"];
        string password = Request.Form["password1"];

        if (db.DoesUserExist(username))
        {
            Console.WriteLine("User already exists");
            return;
        }

        string passwordHash = Hashing.HashPassword(password, out string salt);
        
        db.Register(username, passwordHash);
        db.RegisterSalt(username, salt);
        
    }
}