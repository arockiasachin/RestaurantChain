using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantChain.Models;
using RestaurantChain.ViewModels;

namespace RestaurantChain.Controllers
{
    public class MenusController : Controller
    {
        private readonly RestaurantsEntities db = new RestaurantsEntities();

        // GET: Menus
        public ActionResult Index(string Category = null)
        {
            var menus = db.Menus.Include(m => m.Hotel);
            var result = menus.ToList();
            List<Menu> r = new List<Menu>();
            var categories = db.Categories.ToList();

            if (Category == null)
            {
                r = result;
            }
            else
            {
                r = result.Where(p => p.Category == Category).ToList();
            }



            Filters model = new Filters
            {
                Items = r,
                Categories = categories
            };
            return View(model);
        }


        public ActionResult Create()
        {
            ViewBag.RestaurantId = new SelectList(db.Hotels, "RestaurantId", "RestaurantName");
            SelectList list = new SelectList(GetCategory(), "CategoryName", "CategoryName");
            ViewBag.Category = list;

            return View();
        }

        //[HttpPost]
        //public ActionResult Create(Menu menu)
        //{

        //        if (menu.MenuImage != null)
        //    {
        //        var FileName = UploadImage(menu);
        //        menu.MenuImage.SaveAs(FileName);
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        db.Menus.Add(menu);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.RestaurantId = new SelectList(db.Hotels, "RestaurantId", "RestaurantName", menu.RestaurantId);
        //    ModelState.Clear();
        //    return View(menu);
        //}
        //public string UploadImage(Menu image)
        //{
        //    string fileName = Path.GetFileName(image.ImageFile.FileName);
        //    //string extension = Path.GetExtension(image.ImageFile.FileName);
        //    fileName = "Menu" + fileName;
        //    image.MenuImage = fileName;
        //    fileName = Path.Combine(Server.MapPath("//Content//Images//"), fileName);
        //    return fileName;

        //}

        [HttpPost]
        public ActionResult Create(Menu menu, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null)
            {
                menu.MenuImage = UploadImage(ImageFile);
            }

            if (ModelState.IsValid)
            {
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RestaurantId = new SelectList(db.Hotels, "RestaurantId", "RestaurantName", menu.RestaurantId);
            ModelState.Clear();
            return View(menu);
        }

        public string UploadImage(HttpPostedFileBase imageFile)
        {
            string fileName = Path.GetFileName(imageFile.FileName);
            fileName = "Menu_" + fileName; // Prefix filename to avoid collisions
            string filePath = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
            imageFile.SaveAs(filePath); // Save the uploaded file
            return fileName; // Return the file path
        }



        public IEnumerable<Category> GetCategory()
        {
            var items = db.Categories.ToList();
            var result = items.GroupBy(x => x.CategoryName).Select(g => g.First());
            return result;
        }

        public ActionResult EditPhoto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.RestaurantId = new SelectList(db.Hotels, "RestaurantId", "RestaurantName", menu.RestaurantId);
            ViewBag.RestaurantId = new SelectList(db.Hotels, "RestaurantId", "RestaurantName");
            SelectList list = new SelectList(GetCategory(), "CategoryName", "CategoryName");
            ViewBag.Category = list;
            ModelState.Clear();
            return View(menu);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditPhoto(Menu menu)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var FileName = UploadImage(menu);
        //        menu.MenuImage.SaveAs(FileName);
        //        if (menu.MenuImage == null)
        //        {
        //            menu.MenuImage = (from i in db.Menus
        //                              where (i.ItemId == menu.ItemId)
        //                              select i.MenuImage).FirstOrDefault();
        //        }
        //        db.Entry(menu).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.RestaurantId = new SelectList(db.Hotels, "RestaurantId", "RestaurantName", menu.RestaurantId);
        //    //}
        //    return View(menu);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhoto(Menu menu, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    menu.MenuImage = UploadImage(ImageFile); // Update MenuImage property
                }

                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RestaurantId = new SelectList(db.Hotels, "RestaurantId", "RestaurantName", menu.RestaurantId);
            return View(menu);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.RestaurantId = new SelectList(db.Hotels, "RestaurantId", "RestaurantName", menu.RestaurantId);
            ViewBag.RestaurantId = new SelectList(db.Hotels, "RestaurantId", "RestaurantName");
            SelectList list = new SelectList(GetCategory(), "CategoryName", "CategoryName");
            ViewBag.Category = list;
            ModelState.Clear();
            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                if (menu.MenuImage == null)
                {
                    menu.MenuImage = (from i in db.Menus
                                      where (i.ItemId == menu.ItemId)
                                      select i.MenuImage).FirstOrDefault();
                }
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RestaurantId = new SelectList(db.Hotels, "RestaurantId", "RestaurantName", menu.RestaurantId);
            //}
            return View(menu);
        }
        public ActionResult Delete(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            db.SaveChanges();
            return RedirectToAction("Index");
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
