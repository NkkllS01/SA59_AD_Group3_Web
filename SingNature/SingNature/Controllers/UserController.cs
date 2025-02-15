using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SingNature.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true  // Disable SSL validation
            };
            _httpClient = new HttpClient(handler);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("Profile")]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost("Register")]
        private async Task<UserResponse> RegisterUser(RegisterRequest request)
        {
            var apiUrl = "https://167.172.73.161/api/auth/register";
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiUrl, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserResponse>(responseData);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Register API request failed: {ex.Message}");
                return null;
            }
        }

        private async Task<UserResponse> AuthenticateUser(LoginRequest request)
        {
            var apiUrl = "https://167.172.73.161/api/auth/login";
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiUrl, jsonContent);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserResponse>(responseData);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Login API request failed: {ex.Message}");
                return null;
            }
        }
    }

    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public bool Warning { get; set; }
        public bool Newsletter { get; set; }
    }
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool Warning { get; set; }
        public bool Newsletter { get; set; }
    }
}




