using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using authorization.Data;
using authorization.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class UserController : Controller
{
    private readonly HttpClient _httpClient;

    public UserController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public IActionResult login()
    {
        return View("login");
    }

    public IActionResult register()
    {
        return View("register");
    }

    public IActionResult profile()
    {
        return View("profile");
    }

        private async Task<UserResponse> RegisterUser(RegisterRequest request)
    {
        var apiUrl = "https://localhost:5076/api/auth/register";
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
        var apiUrl = "https://localhost:5076/api/auth/login";
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


