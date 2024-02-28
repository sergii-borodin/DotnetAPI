using System.Security.Cryptography.X509Certificates;
using HelloWorld.Data;
using Microsoft.AspNetCore.Mvc;

namespace dotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }
    [HttpGet("TestConnection")]

    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers/{testValue}")]
    // public IActionResult Test()
    public string[] GetUsers(string testValue)
    {
        string[] responseArray = new string[]{
            "User 1",
            "User 2",
            testValue
    };
        return responseArray;
    }
}