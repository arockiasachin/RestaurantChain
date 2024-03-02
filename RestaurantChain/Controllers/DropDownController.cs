using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantChain.Models;
namespace RestaurantChain.Controllers
{
    public class DropDownController : Controller
    {
        readonly RestaurantsEntities db = new RestaurantsEntities();

        // GET: DropDown
        public ActionResult LocationIndex()
        {
            var location = db.Locations;
            return View(location.ToList());
        }
        public ActionResult CategoryIndex()
        {
            var category = db.Categories;
            return View(category.ToList());
        }
        public ActionResult LocationCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocationCreate([Bind(Include = "LocationId,City,RestaurantBranch")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("LocationIndex");
            }

            return View(location);
        }
        public ActionResult CategoryCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryCreate([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("CategoryIndex");
            }
            return View(category);
        }
        public ActionResult LocationEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocationEdit([Bind(Include = "LocationId,City,RestaurantBranch")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LocationIndex");
            }
            return View(location);
        }
        public ActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryEdit([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CategoryIndex");
            }
            return View(category);
        }

        public ActionResult LocationDelete(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
            db.SaveChanges();
            return RedirectToAction("LocationIndex");
        }
        public ActionResult CategoryDelete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("CategoryIndex");
        }
    }
}