using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class AccountController : Controller
    {
        public UserContext db = new UserContext();
        private string keyForHmac = "281269751949953238506636101247451493";
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
                string string1 = GetHash(model.Password, keyForHmac);
                user = db.Users.FirstOrDefault(u => u.Name == model.Name && u.Password == string1);
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
                    db.Users.Add(new User { UserName = model.Name, Name = model.Name, Email = model.Email, Password = GetHash(model.Password, keyForHmac), LoginDate = DateTime.Now, RegistrationDate = DateTime.Now, Status = "Unlocked" });
                    db.SaveChanges();
                    string string1 = GetHash(model.Password, keyForHmac);
                    user = db.Users.Where(u => u.Name == model.Name && u.Password == string1).FirstOrDefault();
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
        public static String GetHash(String text, String key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);
            Byte[] hashBytes;
            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}