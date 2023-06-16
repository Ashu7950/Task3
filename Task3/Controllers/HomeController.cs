using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task3.Controllers;
using System.Data;
using System.Data.Entity;
using Task3;

namespace Task3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {
            if (ModelState.IsValid)
            {
                using (studentDBEntities db = new studentDBEntities())
                {
                    var obj = db.Users.Where(a => a.Username.Equals(objUser.Username) && a.Password.Equals(objUser.Password)&& a.Role.Equals(objUser.Role)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.UserId.ToString();
                        Session["UserName"] = obj.Username.ToString();
                        if (objUser.Role == "Teacher")
                        {
                            return RedirectToAction("DashBoard");
                        }
                        else
                        {
                            return RedirectToAction("StudentDashBoard");
                        }
                        
                    }
                }
            }
            return View(objUser);
        }

        public ActionResult DashBoard()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult StudentDashBoard()
        {
            if (Session["UserID"] != null)
            {
                object currentuserid = Session["UserID"];

                // Execute the SQL query to fetch the task data
                string query = "SELECT t.taskname, t.taskdescription, t.taskstatus " +
                               "FROM task t " +
                               "JOIN user u ON t.userid = u.userid " +
                               $"WHERE u.username = '{currentuserid}'";

                // Execute the query and fetch the results
                List<Task3.Task> tasks = new List<Task3.Task>();
                // Code to execute the query and populate the `tasks` list with the fetched data

                return View(tasks);
                
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult AddOrEdit(int id = 0)
        {
            User user = new User();
            return View(user);
        }
        [HttpPost]
        public ActionResult AddOrEdit(User user)
        {
            using (studentDBEntities db = new studentDBEntities())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successfull";

            return View("AddOrEdit",new User());
        }
    }
}