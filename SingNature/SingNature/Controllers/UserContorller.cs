using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
    public class UserController : Controller
    {
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



}