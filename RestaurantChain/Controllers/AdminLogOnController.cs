using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantChain.Models;
using System.Web.Security;

namespace RestaurantChain.Controllers
{

    public class AdminLogOnController : Controller
    {
        // GET: AdminLogOn
        readonly RestaurantsEntities db = new RestaurantsEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.AdminLogin model)
        {

            bool isValid = db.AdminLogins.Any(x => x.UserName == model.UserName && x.Password == model.Password);
            if (isValid)
            {
                Session["Type"] = "Admin";
                Session["Username"] = model.UserName;
                return RedirectToAction("AdminIndex", "AdminHome");
            }

            ModelState.AddModelError("", "Invalid username and password");
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index","StartPage"); 
        }
    }
}