using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantChain.Models;
using RestaurantChain.ViewModels;
namespace RestaurantChain.Controllers
{
    public class HotelsController : Controller
    {
        readonly RestaurantsEntities db = new RestaurantsEntities();
        public ActionResult Index(string Location)
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
        public ActionResult Create()
        {
            SelectList list = new SelectList(GetCity(), "City", "City");
            ViewBag.City = list;
            return View();
        }

        // POST: CreateHotel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RestaurantId,RestaurantName,RestaurantBranch,City,Phone,Email,Address")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Hotels.Add(hotel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hotel);
        }

        public IEnumerable<Location> GetCity()
        {
            var items = db.Locations.ToList();
            var result = items.GroupBy(x => x.City).Select(g => g.First());
            return result;
        }
        public ActionResult GetBranch(string City)
        {
            List<Location> items = db.Locations.Where(x => x.City == City).ToList();
            ViewBag.Branch = new SelectList(items, "RestaurantBranch", "RestaurantBranch");
            return PartialView("DisplayBranch");

        }
        public ActionResult Edit(int? id)
        {
            SelectList list = new SelectList(GetCity(), "City", "City");
            ViewBag.City = list;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RestaurantId,RestaurantName,RestaurantBranch,City,Phone,Email,Address")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }
        public ActionResult Delete(int id)
        {

            Hotel hotel = db.Hotels.Find(id);
            db.Hotels.Remove(hotel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}