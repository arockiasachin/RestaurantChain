using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantChain.Models;
namespace RestaurantChain.Controllers
{
    public class CustomerLogOnController : Controller
    {
        private readonly RestaurantsEntities db = new RestaurantsEntities();
        public ActionResult Create()
        {

            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(CustomerLogin customerLogin)
        //{

        //    if (customerLogin.CustomerImage != null)
        //    {
        //        var FileName = UploadImage(customerLogin);
        //        customerLogin.CustomerImage.SaveAs(FileName);
        //    }


        //    if (ModelState.IsValid)
        //    {
        //        db.CustomerLogins.Add(customerLogin);
        //        db.SaveChanges();
        //        return RedirectToAction("Login");
        //    }

        //    return View(customerLogin);
        //}
        //public string UploadImage(CustomerLogin image)
        //{

        //    string fileName = Path.GetFileName(image.CustomerImage.FileName);
        //    //string extension = Path.GetExtension(image.ImageFile.FileName);
        //    fileName = "C-" + fileName;
        //    image.CustomerImage = fileName;
        //    fileName = Path.Combine(Server.MapPath("//Content//Images//"), fileName);
        //    return fileName;
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerLogin customerLogin, HttpPostedFileBase CustomerImageFile)
        {
            if (CustomerImageFile != null)
            {
                customerLogin.CustomerImage = UploadImage(CustomerImageFile); // Update CustomerImage property
            }

            if (ModelState.IsValid)
            {
                db.CustomerLogins.Add(customerLogin);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(customerLogin);
        }

        public string UploadImage(HttpPostedFileBase imageFile)
        {
            string fileName = Path.GetFileName(imageFile.FileName);
            fileName = "C-" + fileName; // Prefix filename to avoid collisions
            string filePath = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
            imageFile.SaveAs(filePath); // Save the uploaded file
            return filePath; // Return the file path
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.CustomerLogin model)
        {

            bool isValid = db.CustomerLogins.Any(x => x.UserName == model.UserName && x.Password == model.Password);
            if (isValid)
            {

                Session["Username"] = model.UserName;
                string UserName = model.UserName;
                var result = (from i in db.CustomerLogins
                              where i.UserName == UserName
                              select i.CustomerId).FirstOrDefault();
                ViewBag.CustomerId = result;
                Session["Id"] = result;
                Session["Type"] = "Customer";
                HttpCookie CustomerCookie = new HttpCookie("user", model.UserName);
                Response.Cookies.Add(CustomerCookie);
                CustomerCookie.Expires.AddYears(1);
                return RedirectToAction("RestaurantIndex", "CustomerViews");
            }

            ModelState.AddModelError("", "Invalid username and password");
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "StartPage");
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