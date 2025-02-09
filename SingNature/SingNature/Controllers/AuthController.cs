using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using authorization.Data;
using authorization.Models;

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

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _userDao.GetUserByUsername(request.Username);
            if (user == null || user.Password != request.Password)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            return Ok(new 
            { 
                message = "Login successful",
                userId = user.Id,
                username = user.Username,
                email = user.Email,
                phone = user.Phone,
                subscribeWarning = user.SubscribeWarning,
                subscribeNewsletter = user.SubscribeNewsletter
                });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out successfully" });
        }

 
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            var existingUser = _userDao.GetUserByUsername(request.Username);
            if (existingUser != null)
            {
                return Conflict(new { message = "Username already exists" });
            }

            var newUser = new User
            {
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
                Phone = request.Phone,
                SubscribeWarning = request.SubscribeWarning,
                SubscribeNewsletter = request.SubscribeNewsletter
            };

            _userDao.CreateUser(newUser);
            return Ok(new { message = "User registered successfully" });
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
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone
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
            user.Phone = request.Phone ?? user.Phone;
            user.SubscribeWarning = request.SubscribeWarning;
            user.SubscribeNewsletter = request.SubscribeNewsletter;
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

            _userDao.UpdateSubscription((int)userId, request.SubscribeWarning, request.SubscribeNewsletter);
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
                SubscribeWarning = user.SubscribeWarning,
                SubscribeNewsletter = user.SubscribeNewsletter
            });
        }
    }


    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool SubscribeWarning { get; set; } = false;
        public bool SubscribeNewsletter { get; set; } = false;
    }

    public class UpdateProfileRequest
{
    public int UserId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool SubscribeWarning { get; set; }
    public bool SubscribeNewsletter { get; set; }
}



        public class SubscriptionRequest
    {
        public bool SubscribeWarning { get; set; }
        public bool SubscribeNewsletter { get; set; }
    }
}
