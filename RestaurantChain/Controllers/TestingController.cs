using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantChain.Models;
namespace RestaurantChain.Controllers
{
    public class TestingController : Controller
    {
        [NonAction]
        public ActionResult Index()
        {
            var items = from i in Values()
                        orderby i.Id
                        select i;
            
            return View(items);
        }

        public ActionResult CartTest()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Carts);
        }
        public ActionResult CategoryTest()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Categories);
        }
        public List<TestClass> Values()
        {
            return new List<TestClass>
            {
                new TestClass
            {
                Id=93, Item= "apple", Quantity=12, SubTotal= 76, Total=86
            },
                new TestClass
            {
                Id=4, Item= "orange", Quantity=2, SubTotal= 6, Total=82
                },
                new TestClass
            {
                Id=6, Item= "grapes", Quantity=12, SubTotal= 66, Total=83
            }
            };
         } 
    }


}
