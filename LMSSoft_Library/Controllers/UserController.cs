using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSSoft_Library.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            var user = (from n in db.UserLoginModel where n.Login_Name.Equals(username) select n).FirstOrDefault();
            if (user == null)
            {
                var stud = db.StudentRegistration.Where(s => s.StudCode == username).FirstOrDefault();
                if (stud == null)
                {
                    var staff = db.Staffs.Where(s => s.Staff_Code == username).FirstOrDefault();
                    if (staff == null)
                    {
                        ShowMessage(MessageType.warning, "Invalid Username.");
                        return View();
                    }
                    else
                    {
                        if (staff.Password.Equals(password))
                        {
                            HttpContext.Session["UserId"] = staff.Id;
                            HttpContext.Session["FullName"] = staff.Full_Name;
                            HttpContext.Session["LoginName"] = staff.Staff_Code;
                            HttpContext.Session["UserType"] = "Staff";

                            return RedirectToAction("Index", new { Controller = "Home" });
                        }
                        else
                        {
                            ShowMessage(MessageType.warning, "Invalid Password.");
                            return View();
                        }
                    }
                }
                else
                {
                    if (stud.Password.Equals(password))
                    {
                        HttpContext.Session["UserId"] = stud.Id;
                        HttpContext.Session["FullName"] = stud.FullName;
                        HttpContext.Session["LoginName"] = stud.StudCode;
                        HttpContext.Session["UserType"] = "Student";

                        return RedirectToAction("Index", new { Controller = "Home" });
                    }
                    else
                    {
                        ShowMessage(MessageType.warning, "Invalid Password.");
                        return View();
                    }
                }
            }
            if (user.User_Status == "Active")
            {
                if (user.Password.Equals(password))
                {
                    HttpContext.Session["UserId"] = user.ID;
                    HttpContext.Session["FullName"] = user.Full_Name;
                    HttpContext.Session["LoginName"] = user.Login_Name;
                    HttpContext.Session["UserType"] = user.User_Type;

                    return RedirectToAction("Index", new { Controller = "Home" });
                }
                else
                    ShowMessage(MessageType.warning, "Invalid Password.");
            }
            else
                ShowMessage(MessageType.warning, "Inactive User.");
            return View();
        }


        public ActionResult SignOut()
        {
            HttpContext.Session.Abandon();
            return RedirectToAction("Index");
        }
    }

    public class ValidateSession : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["LoginName"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                {
                    { "Controller","User" },
                    { "Action","Index"},
                    { "Area","" }
                });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}