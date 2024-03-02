using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantChain.Models;
using RestaurantChain.ViewModels;
namespace RestaurantChain.Controllers
{
    public class CustomerViewsController : Controller
    {


       
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public  RestaurantsEntities db = new RestaurantsEntities();

        public static int BillId = RandomNumber(1, 100000);
        public int bill = BillId;

       
        public ActionResult RestaurantIndex(string Location = null)
        {

            var hotels = db.Hotels;
            var result = hotels.ToList();
            List<Hotel> r = new List<Hotel>();
            var locations = db.Locations.ToList();

            if (Location == null)
            {
                r = result;
            }
            else
            {
                r = result.Where(p => p.City == Location).ToList();

            }

            Filters model = new Filters
            {
                Litems = r,
                Locations = (locations.GroupBy(x => x.City).Select(g => g.First())).ToList()
            };
            return View(model);
        }

        
        public ActionResult Index(int id = 0, string Category = null)
        {
           
            var menus = db.Menus.Include(m => m.Hotel);
            var result = menus.ToList();
            List<Menu> r = new List<Menu>();
            var categories = db.Categories.ToList();
            Filters model = new Filters();
            if (id == 0)
            {
                if (Category == null)
                {
                    r = result;
                }
                else
                {

                    r = result.Where(p => p.Category == Category).ToList();
                }


                model.Hotels = db.Hotels.ToList();
                model.Items = r;
                model.AllItems = r;
                model.Categories = categories;
                return View(model);
            }
            else
            {
                if (Category == null)
                {
                    r = result;
                }
                else
                {

                    r = result.Where(p => p.Category == Category).ToList();
                }

                //var r1 = r.Where(x => x.RestaurantId == id)..GroupBy(x => x.City).Select(g => g.First())).ToList();
                model.Items = r.Where(x => x.RestaurantId == id).ToList();
                model.AllItems = r;
                model.Hotels = db.Hotels.ToList();
                model.Categories = categories;
                return View(model);
            }
        }
        public ActionResult OldOrders()
        {
            string UserName = Request.Cookies["user"].Value;
            var Customer = (from i in db.CustomerLogins
                            where i.UserName == UserName
                            select i).FirstOrDefault();
            int id = Customer.CustomerId;
            return View(db.Orders.Where(x => x.CustomerId == id).ToList());
        }
        [HttpPost]
        public ActionResult Index(int Quantity, int Itemid, Menu menu)
        {
            menu = db.Menus.Find(Itemid);
            Cart cart = new Cart
            {
                ItemId = menu.ItemId,
                BillId = BillId,
                SubTotal = menu.Price,
                Quantity = Quantity
            };
            cart.Total = cart.Quantity * cart.SubTotal;
            cart.Item = menu.Item;
            db.Carts.Add(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Index1()
        {

            return View(db.Carts.Where(x => x.BillId == bill).ToList());
        }
        public ActionResult Payment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Payment(Order order)
        {
            
            var result = (from i in db.Carts
                          where i.BillId == bill
                          select i).FirstOrDefault();
            order.Total = result.Total;
            order.BillId = Convert.ToInt32(result.BillId);
            string UserName = Request.Cookies["user"].Value;

            var Customer = (from i in db.CustomerLogins
                            where i.UserName == UserName
                            select i).FirstOrDefault();

            order.CustomerId = Customer.CustomerId;

            order.CustomerName = Customer.Name;
            db.Orders.Add(order);
            db.SaveChanges();
            return RedirectToAction("OrderIndex", new { id = order.OrderId });
        }
        public ActionResult OrderIndex(int id)
        {
            Order orders = db.Orders.Find(id);
            BillId = RandomNumber(1, 100000);
            bill = BillId;
            return View(orders);
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BillId,Item,SubTotal,Quantity,Total,Id,ItemId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index1");
            }
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index1");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
