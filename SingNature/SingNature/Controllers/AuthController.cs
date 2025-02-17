using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using authorization.Data;
using authorization.Models;
using BCrypt.Net;

namespace authorization.Controllers
{

    
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserDao _userDao;

        public AuthController(UserDao userDao)
        {
            _userDao = userDao;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            var existingUser = _userDao.GetUserByUsername(request.UserName);
            if (existingUser != null)
            {
                return Conflict(new { message = "Username already exists" });
            }

            var newUser = new User
            {
                UserName = request.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),               
                Email = request.Email,
                Mobile = request.Mobile,
                Warning = request.Warning,
                Newsletter = request.Newsletter
            };
            _userDao.CreateUser(newUser);
            var createdUser = _userDao.GetUserByUsername(request.UserName);
                if (createdUser == null)
                {
                    return StatusCode(500, new { message = "User registration failed." });
                }
                HttpContext.Session.SetInt32("UserId", createdUser.UserId);
                HttpContext.Session.SetString("Username", createdUser.UserName);
                return Ok(new
                {
                message = "User registered successfully",
                userId = createdUser.UserId,
                username = createdUser.UserName,
                email = createdUser.Email,
                mobile = createdUser.Mobile,
                warning = createdUser.Warning,
                newsletter = createdUser.Newsletter
                });
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _userDao.GetUserByUsername(request.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))           
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.UserName);
            return Ok(new 
            { 
                message = "Login successful",
                userId = user.UserId,
                username = user.UserName,
                email = user.Email,
                mobile = user.Mobile,
                warning = user.Warning,
                newsletter = user.Newsletter
                });
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return Unauthorized(new { message = "User is not logged in" });
            }
                HttpContext.Session.Clear();
                return Ok(new { message = "Logged out successfully" });
        }


 
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized(new { message = "Not logged in" });
            }

            var user = _userDao.GetUserById((int)userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new
            {
                UserId = user.UserId,
                Username = user.UserName,
                Email = user.Email,
                Mobile = user.Mobile
            });
        }
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                 return Unauthorized(new { message = "User not logged in" });
            }
            var user = _userDao.GetUserById(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
                return Ok(new
            {
                userId = user.UserId,
                username = user.UserName,
                email = user.Email,
                mobile = user.Mobile,
                warning = user.Warning,
                newsletter = user.Newsletter
            });
        }

        [HttpPut("update-profile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = request.UserId;
            var user = _userDao.GetUserById(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
                
            user.Email = request.Email ?? user.Email;
            user.Mobile = request.Mobile ?? user.Mobile;
            user.Warning = request.Warning;
            user.Newsletter = request.Newsletter;
            _userDao.UpdateUser(user);
            return Ok(new { message = "Profile updated successfully" });
        }



        [HttpPut("subscribe")]
        public IActionResult UpdateSubscription([FromBody] SubscriptionRequest request)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized(new { message = "User not logged in" });
            }

            _userDao.UpdateSubscription((int)userId, request.Warning, request.Newsletter);
            return Ok(new { message = "Subscription updated successfully" });
        }

        [HttpGet("subscription")]
        public IActionResult GetSubscriptionStatus()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized(new { message = "User not logged in" });
            }

            var user = _userDao.GetUserById((int)userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new
            {
                Warning = user.Warning,
                Newsletter = user.Newsletter
            });
        }
    }


    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }


    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public bool Warning { get; set; } = false;
        public bool Newsletter { get; set; } = false;
    }

    public class UpdateProfileRequest
{
    public int UserId { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public bool Warning { get; set; }
    public bool Newsletter { get; set; }
}



        public class SubscriptionRequest
    {
        public bool Warning { get; set; }
        public bool Newsletter { get; set; }
    }
}
