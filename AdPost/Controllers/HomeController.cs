using AdPost.Data;
using AdPost.Models;
using AdPost.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AdPost.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog = AdPost; Integrated Security=true;";
        public IActionResult Home()
        {
            var am = new AdManager(_connectionString);
            var um = new UserManager(_connectionString);
            var currentUserEmail = User.Identity.Name;
            var user = new User();
            if(currentUserEmail !=null)
            {
                user = um.GetUserByEmail(currentUserEmail);
            }
            var adv = new AdViewModel
            {
                Ads = am.GetAdsWithUserName(),
                UserId = user.Id
            };
            return View(adv);
        }

        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitAd(string phonenumber, string description)
        {
            var am = new AdManager(_connectionString);
            var um = new UserManager(_connectionString);
            var currentUserEmail = User.Identity.Name;
            var user = um.GetUserByEmail(currentUserEmail);
            am.AddAd(phonenumber, description,user.Id);
            
            return RedirectToAction("home");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var am = new AdManager(_connectionString);
            am.DeleteAd(id);
            return Redirect("/home/home");
        }
        public IActionResult MyAccount()
        {
            var am = new AdManager(_connectionString);
            var um = new UserManager(_connectionString);
            var currentUserEmail = User.Identity.Name;
            var user = um.GetUserByEmail(currentUserEmail);
            var adv = new AdViewModel
            {
                Ads = am.GetAdsForMyAccount(user.Id),
                UserId = user.Id
            };
            return View(adv);
        }
    }
}