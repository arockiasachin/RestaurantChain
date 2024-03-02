using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantChain.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: AdminHome
        public ActionResult AdminIndex()
        {
            return View();
        }
    }
}