using System.Security.Cryptography.X509Certificates;
using DotnetAPI;
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

    [HttpGet("GetUsers")]
    // public IActionResult Test()
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT  [UserId], 
                    [FirstName], 
                    [LastName], 
                    [Email], 
                    [Gender], 
                    [Active]
            FROM  TutorialAppSchema.Users;";

        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    // public IActionResult Test()
    public User GetSingleUser(int userId)
    {
        string sql = @"
            SELECT  [UserId],
                    [FirstName],
                    [LastName],
                    [Email],
                    [Gender],
                    [Active] 
            FROM TutorialAppSchema.Users
                WHERE Users.UserId = " + userId.ToString();

        User user = _dapper.LoadDataSingle<User>(sql);
        return user;

    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Users
            SET [FirstName] = '" + user.FirstName + @"', 
                [LastName] = '" + user.LastName + @"', 
                [Email] = '" + user.Email + @"', 
                [Gender] = '" + user.Gender + @"', 
                [Active] = '" + user.Active + @"' 
            WHERE UserId = " + user.UserId;

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        else
        {
            throw new Exception("Failed to update user");
        }
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        string sql = @"
        INSERT into TutorialAppSchema.Users(
            [FirstName], 
            [LastName], 
            [Email], 
            [Gender], 
            [Active]) 
        VALUES (
            '" + user.FirstName + @"', 
            '" + user.LastName + @"', 
            '" + user.Email + @"', 
            '" + user.Gender + @"', 
            '" + user.Active + @"' 
        )";

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        else
        {
            throw new Exception("Failed to add user");
        }
    }
}