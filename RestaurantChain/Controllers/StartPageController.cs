using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantChain.Controllers
{
    public class StartPageController : Controller
    {
        // GET: StartPage
        public ActionResult Index()
        {
            return View();
        }
    }
}