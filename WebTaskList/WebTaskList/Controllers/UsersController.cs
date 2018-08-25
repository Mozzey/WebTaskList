using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTaskList.Data;
using WebTaskList.Domain.Models;

namespace WebTaskList.Controllers
{
    public class UsersController : Controller
    {
        private WebTaskListContext db = new WebTaskListContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Password")] User user)
        {
            HttpCookie passwordCookie;
            if (Request.Cookies["passwordCookie"] == null)
            {
                passwordCookie = new HttpCookie("passwordCookie");
                passwordCookie.Value = user.Email.ToString();
                passwordCookie.Expires = DateTime.UtcNow.AddYears(1);
            }
            else
            {
                passwordCookie = Request.Cookies["passwordCookie"];
            }

            passwordCookie.Value = user.Password;
            Response.Cookies.Add(passwordCookie);
            if (ModelState.IsValid)
            {
                if (db.Users.Any(x => x.Password == passwordCookie.Value))
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Users");
                }
                
            }
            return View(user);
        }

        public ActionResult Login()
        {
            
            return View();
        }

        //[HttpPost]
        //public ActionResult Login(User user)
        //{
        //    HttpCookie idCookie;
        //    if (Request.Cookies["idCookie"] == null)
        //    {
        //        idCookie = new HttpCookie("idCookie");
        //        idCookie.Value = user.Id.ToString();
        //        idCookie.Expires = DateTime.UtcNow.AddYears(1);
        //    }
        //    else
        //    {
        //        idCookie = Request.Cookies["idCookie"];
        //    }

        //    idCookie.Value = user.Id.ToString();
        //    Response.Cookies.Add(idCookie);
        //    return View();
        //}

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
