using AdPost.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdPost.Web.Controllers
{
    public class UserController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog = AdPost; Integrated Security=true;";
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var um = new UserManager(_connectionString);
            var user = um.GetUser(email, password);
            if (user == null)
            {
                TempData["message"] = "Invalid login";
                return Redirect("/user/login");
            }
            var claims = new List<Claim>
            {
                new Claim("user",email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

            return Redirect("/home/home");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/user/login");
        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var um = new UserManager(_connectionString);
            um.AddUser(user, password);
            return Redirect("/user/login");
        }
    }
}
