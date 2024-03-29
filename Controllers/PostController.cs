using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]


    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        // private readonly IConfiguration _config;

        public PostController(IConfiguration config)
        {
            // _config = config;
            _dapper = new DataContextDapper(config);

        }

        [HttpGet("Posts")]
        public IEnumerable<Post> GetPosts()
        {
            string sql = @"SELECT 
                            [PostId],
                            [UserId],
                            [PostTitle],
                            [PostContent],
                            [PostCreated],
                            [PostUpdated] 
                        From TutorialAppSchema.Posts";

            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("PostSingle/{postId}")]
        public IEnumerable<Post> GetPostSingle(int postId)
        {
            string sql = @"SELECT 
                            [PostId],
                            [UserId],
                            [PostTitle],
                            [PostContent],
                            [PostCreated],
                            [PostUpdated] 
                        From TutorialAppSchema.Posts WHERE PostId = " + postId.ToString();

            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("PostsByUser/{userId}")]
        public IEnumerable<Post> GetUserPosts(int userId)
        {
            string sql = @"SELECT 
                            [PostId],
                            [UserId],
                            [PostTitle],
                            [PostContent],
                            [PostCreated],
                            [PostUpdated] 
                        From TutorialAppSchema.Posts WHERE UserId = " + userId.ToString();

            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sql = @"SELECT 
                            [PostId],
                            [UserId],
                            [PostTitle],
                            [PostContent],
                            [PostCreated],
                            [PostUpdated] 
                        From TutorialAppSchema.Posts WHERE UserId = " + this.User.FindFirst("userId")?.Value;

            return _dapper.LoadData<Post>(sql);
        }

        [HttpPost("Post")]
        public IActionResult AddPost(PostToAddDto postToAdd)
        {
            string sql = @"INSERT INTO TutorialAppSchema.Posts (
                            [UserId],
                            [PostTitle],
                            [PostContent],
                            [PostCreated],
                            [PostUpdated])
                        VALUES (" + this.User.FindFirst("userId")?.Value
                        + ",'" + postToAdd.PostTitle
                        + "','" + postToAdd.PostContent
                        + "', GETDATE(), GETDATE() )";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to create new post!");
        }

        [HttpPut("Post")]
        public IActionResult EditPost(PostToEditDto postToEdit)
        {
            string sql = @"UPDATE TutorialAppSchema.Posts
                        SET 
                    PostTitle = '" + postToEdit.PostTitle +
                    "', PostContent = '" + postToEdit.PostContent +
                     @"', PostUpdated = GETDATE()
                        WHERE PostId = " + postToEdit.PostId.ToString() + " AND UserId = " + this.User.FindFirst("userId")?.Value;

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to edit post!");

        }

        [HttpDelete("DeletePost/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string sql = @"DELETE FROM TutorialAppSchema.Posts
            WHERE PostId = " + postId.ToString() + " AND UserId = " + this.User.FindFirst("userId")?.Value;

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to delete post!");
        }
    }
}