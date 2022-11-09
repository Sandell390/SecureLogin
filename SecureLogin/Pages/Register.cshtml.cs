using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SecureLogin.Pages;

public class Register : PageModel
{

    
    public void OnGet()
    {
        
    }
    
    public void OnPost()
    {
        // string username = Request.Form["uname"];
        // string password = Request.Form["password1"];
        //
 
        
        Console.WriteLine("Registering");
        
    }
}