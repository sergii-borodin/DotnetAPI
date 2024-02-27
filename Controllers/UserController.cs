using Microsoft.AspNetCore.Mvc;

namespace dotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public UserController()
    {

    }

    [HttpGet("test")]
    // public IActionResult Test()
    public string[] Test()
    {
        string[] responseArray = new string[]{
            "test1",
            "test2"
    };
        return responseArray;
    }
}