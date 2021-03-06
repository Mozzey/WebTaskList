﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTaskList.Data;
using WebTaskList.Domain.Models;
using WebTaskList.Models;
using WebTaskList.Utility;

namespace WebTaskList.Controllers
{
    public class UserTasksController : Controller
    {
        private WebTaskListContext db = new WebTaskListContext();

        int ID = 0;
        public ActionResult StoreLogin(LogInModel login)
        {
            var user = db.Users
                    .Where(p => p.Email == login.Email)
                    .Select(p => new { p.Id, p.Password, p.Email });

            foreach (var column in user)
            {
                if (login.Password == column.Password && login.Email == column.Email)
                {
                    HttpCookie userIdCookie;
                    if (Request.Cookies[Cookies.IdCookie] == null)
                    {
                        userIdCookie = new HttpCookie(Cookies.IdCookie);
                        userIdCookie.Value = "0";
                        userIdCookie.Expires = DateTime.UtcNow.AddYears(1);
                    }
                    else
                    {
                        userIdCookie = Request.Cookies[Cookies.IdCookie];
                    }

                    ID = column.Id;
                    userIdCookie.Value = ID.ToString();
                    Response.Cookies.Add(userIdCookie);

                    HttpCookie emailCookie;
                    if (Request.Cookies[Cookies.EmailCookie] == null)
                    {
                        emailCookie = new HttpCookie(Cookies.EmailCookie);
                        emailCookie.Value = "";
                        emailCookie.Expires = DateTime.UtcNow.AddYears(1);
                    }
                    else
                    {
                        emailCookie = Request.Cookies[Cookies.EmailCookie];
                    }

                    emailCookie.Value = column.Email;
                    Response.Cookies.Add(emailCookie);

                    return RedirectToAction("Index", "UserTasks");
                }
            }
            //string alert = "The username or password is incorrect. Please try again!";
            return RedirectToAction("Login", "Users");
        }


        public ActionResult Index(string searchBy, string search)
        {
            var email = HttpContext.Request.Cookies[Cookies.EmailCookie].Value;
            ViewBag.UserName = email;
            if (searchBy == "Id")
            {
                return View(db.UserTasks.Where(x => x.Id.ToString() == search || search == null).ToList());
            }
            else if (searchBy == "Desc")
            {
                return View(db.UserTasks.Where(x => x.Description.Contains(search)).ToList());
            }
            else
            {
                return View(db.UserTasks.Where(x => x.User.Email == email).ToList());
            }

        }
        [HttpPost]
        public ActionResult Index(User user)
        {
            HttpCookie emailCookie;
            if (Request.Cookies[Cookies.EmailCookie] == null)
            {
                emailCookie = new HttpCookie(Cookies.EmailCookie);
                emailCookie.Value = user.Email.ToString();
                emailCookie.Expires = DateTime.UtcNow.AddYears(1);
            }
            else
            {
                emailCookie = Request.Cookies[Cookies.EmailCookie];
            }

            emailCookie.Value = user.Email;
            Response.Cookies.Add(emailCookie);
            HttpCookie userIdCookie;
            if (Request.Cookies["UserIdCookie"] == null)
            {
                userIdCookie = new HttpCookie("UserIdCookie");
                userIdCookie.Value = user.Id.ToString();
                userIdCookie.Expires = DateTime.UtcNow.AddYears(1);
            }
            else
            {
                userIdCookie = Request.Cookies["UserIdCookie"];
            }

            userIdCookie.Value = user.Id.ToString();
            Response.Cookies.Add(userIdCookie);
            var username = emailCookie.Value;
            //var specificUser = db.Users.Where(x => x.Email == username).Include(x => x.Tasks);
            return View(db.Users.FirstOrDefault()?.Tasks.Where(x => x.User.Email == username).ToList());
        }

        // GET: UserTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.UserTasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            return View(userTask);
        }

        // GET: UserTasks/Create
        public ActionResult Create()
        {
            var userName = HttpContext.Request.Cookies[Cookies.EmailCookie].Value;
            var userId = HttpContext.Request.Cookies[Cookies.IdCookie].Value;
            ViewBag.UserId = Convert.ToInt32(userId);
            ViewBag.UserName = userName;
            return View();
        }

        // POST: UserTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,DueDate,Complete,UserId")] UserTask userTask)
        {
            
            if (ModelState.IsValid)
            {
                db.UserTasks.Add(userTask);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userTask.UserId);
            return View(userTask);
        }

        // GET: UserTasks/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.UserTasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            var userId = HttpContext.Request.Cookies[Cookies.IdCookie].Value;
            ViewBag.UserId = Convert.ToInt32(userId);
            return View(userTask);
        }

        // POST: UserTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,DueDate,Complete,UserId")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userTask.UserId);
            return View(userTask);
        }

        // GET: UserTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.UserTasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            return View(userTask);
        }

        // POST: UserTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserTask userTask = db.UserTasks.Find(id);
            db.UserTasks.Remove(userTask);
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
