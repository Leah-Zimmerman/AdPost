using AdPost.Data;
using AdPost.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AdPost.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog = AdPost; Integrated Security=true;";
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult NewAd()
        {
            return View();
        }
        public IActionResult SubmitAd(string phonenumber, string description, int userId)
        {
            var am = new AdManager(_connectionString);
            am.AddAd(phonenumber, description, userId);
            
            return RedirectToAction("home");
        }
    }
}