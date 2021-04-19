using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public UserContext db = new UserContext();

        public AccountController()
        { }

        public AccountController(UserContext context)
        {
            db = context;
        }
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                user = db.Users.FirstOrDefault(u => u.Name == model.Name && u.Password == model.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    db.Entry(user).State = EntityState.Modified;
                    user.LoginDate = DateTime.Now;
                    db.SaveChanges();

                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                user = db.Users.FirstOrDefault(u => u.Name == model.Name);
                if (user == null)
                {
                    db.Users.Add(new User { UserName = model.Name, Name = model.Name, Email = model.Email, Password = model.Password, LoginDate = DateTime.Now, RegistrationDate = DateTime.Now, Status = "Unlocked" });
                    db.SaveChanges();
                    user = db.Users.Where(u => u.Name == model.Name && u.Password == model.Password).FirstOrDefault();
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                        return RedirectToAction("Index", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
        }
        public ActionResult Delete(string[] selectedItems)
        {
            string login = HttpContext.User.Identity.Name;
            if (selectedItems == null)
            {
                return RedirectToAction("Index");
            }
            for (int i = 0; i < selectedItems.Length; i++)
            {
                User user = db.Users.Find(selectedItems[i]);
                db.Users.Remove(user);
                db.SaveChanges();
                if (login == user.Name)
                {
                    Logoff();
                }
            }
            return Json(new { redirectToUrl = Url.Action("Index", "Account") });
        }
        public ActionResult Block(string[] selectedItems)
        {
            string login = HttpContext.User.Identity.Name;
            if (selectedItems == null)
            {
                return RedirectToAction("Index");
            }
            for (int i = 0; i < selectedItems.Length; i++)
            {
                User user = db.Users.Find(selectedItems[i]);
                db.Entry(user).State = EntityState.Modified;
                user.Status = "Blocked";
                db.SaveChanges();
                if (login == user.Name)
                {
                    Logoff();
                }
            }
            return Json(new { redirectToUrl = Url.Action("Index", "Account") });
        }
        public ActionResult Unlock(string[] selectedItems)
        {
            string login = HttpContext.User.Identity.Name;
            if (selectedItems == null)
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Account") });
            }
            for (int i = 0; i < selectedItems.Length; i++)
            {
                User user = db.Users.Find(selectedItems[i]);
                db.Entry(user).State = EntityState.Modified;
                user.Status = "Unlocked";
                db.SaveChanges();
            }
            return Json(new { redirectToUrl = Url.Action("Index") });
        }
    }
}