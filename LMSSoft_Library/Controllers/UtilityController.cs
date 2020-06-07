using LMSSoft_Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSSoft_Library.Controllers
{
    [ValidateSession]
    public class UtilityController : BaseController
    {
        // GET: Utility
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Userlist()
        {
            return View(db.UserLoginModel.ToList().OrderBy(s => s.Full_Name));
        }
        public ActionResult UserReg()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["entryMode"].ToString()))
                return RedirectToAction("AccessDenied", "Home");
            int? Id = RouteData.Values["Id"] == null ? 0 : Convert.ToInt32(RouteData.Values["Id"]);

            if (Id > 0)
            {
                var user = db.UserLoginModel.Find(Id);
                if (user == null)
                {
                    ShowMessage(MessageType.warning, "User Not Found, Please Enter With Proper Url.");
                    return RedirectToAction("Userlist");
                }

                var vModel = new UserRegistrationVModel()
                {
                    Email = user.Email,
                    Full_Name = user.Full_Name,
                    Mobile_No = user.Mobile_No,
                    Login_Name = user.Login_Name,
                    User_Status = user.User_Status
                };
                Populate_DropDownList(user.User_Status);
                return View(vModel);
            }
            Populate_DropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserReg(UserRegistrationVModel vModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int? Id = RouteData.Values["Id"] == null ? 0 : Convert.ToInt32(RouteData.Values["Id"]);

                    if (Id > 0)
                    {
                        var user = db.UserLoginModel.Find(Id);
                        if (user == null)
                        {
                            ShowMessage(MessageType.warning, "User Not Found, Please Enter With Proper Url.");
                            return RedirectToAction("Userlist", new { Id = "" });
                        }
                        var userch = db.UserLoginModel.Where(u => u.Login_Name == vModel.Login_Name).FirstOrDefault();
                        if (userch == null || userch.ID == Id)
                        {
                            user.Email = vModel.Email;
                            user.User_Status = vModel.User_Status;
                            user.Login_Name = vModel.Login_Name;
                            user.Full_Name = vModel.Full_Name;
                            user.Mobile_No = vModel.Mobile_No;

                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();

                            ShowMessage(MessageType.success, "User Updated Successfully.");
                            return RedirectToAction("UserReg", new { Id = "" });
                        }
                        else
                            ShowMessage(MessageType.warning, "Login name already exists.");
                    }
                    else
                    {
                        var userch = db.UserLoginModel.Where(u => u.Login_Name == vModel.Login_Name).FirstOrDefault();
                        if (userch == null)
                        {
                            if (vModel.Password1 == vModel.Password2)
                            {
                                UserLoginModel dbModel = new UserLoginModel();

                                dbModel.Email = vModel.Email;
                                dbModel.User_Status = vModel.User_Status;
                                dbModel.Login_Name = vModel.Login_Name;
                                dbModel.Full_Name = vModel.Full_Name;
                                dbModel.Mobile_No = vModel.Mobile_No;
                                dbModel.Ent_User = HttpContext.Session["LoginName"].ToString();
                                dbModel.Ent_DateTime = DateTime.Now;
                                dbModel.Password = vModel.Password1;
                                db.UserLoginModel.Add(dbModel);
                                db.SaveChanges();
                                ShowMessage(MessageType.success, "User Added Successfully.");
                                return RedirectToAction("UserReg", new { Id = "" });
                            }
                            else
                                ShowMessage(MessageType.warning, "Confirm Password is invalid.");
                        }
                        else
                            ShowMessage(MessageType.warning, "Login name already exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.warning, "Oops, There was an error. Try again, and if fails contact your system administrator. Error : " + ex.Message);
            }
            Populate_DropDownList();
            return View(vModel);
        }


        protected void Populate_DropDownList(object StatusVal = null)
        {
            ViewBag.lstStatus = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = "Active", Value = "Active"},
                new SelectListItem{Text = "Inactive", Value = "Inactive"}
            }, "Value", "Text", StatusVal);
        }



        public ActionResult SemesterUpgrade()
        {
            PopulateSemUpgradeDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SemesterUpgrade(SemesterUpgradeVModel vModel)
        {
            ViewBag.lstData = db.Database.SqlQuery<SemesterDataList>("Select Id,StudCode,FullName,Roll_No,MobileNo,Email from StudentRegistration Where CourseId = '" + vModel.CourseId + "' And FacultyId = '" + vModel.FacultyId + "' and SemesterId = '" + vModel.SemesterId + "' Order By FullName").ToList();
            PopulateSemUpgradeDropDown(vModel.FacultyId, vModel.CourseId, vModel.SemesterId);
            return View(vModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SemesterUpgradePost(SemesterUpgradeVModel vModel)
        {
            if (vModel.SemesterId == vModel.UpSemesterId)
            {
                ShowMessage(MessageType.warning, "Same upgrade semester is not allowed.");
            }
            else
            {
                db.Database.ExecuteSqlCommand("Update StudentRegistration Set SemesterId = '" + vModel.UpSemesterId + "' Where Id In (Select Id from StudentRegistration Where CourseId = '" + vModel.CourseId + "' And FacultyId = '" + vModel.FacultyId + "' and SemesterId = '" + vModel.SemesterId + "' )");
                ShowMessage(MessageType.success, "Semester upgraded successfully.");
            }
            return RedirectToAction("SemesterUpgrade");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordVModel vModel)
        {
            if (vModel.Password2.Equals(vModel.Password3))
            {
                string Code = Session["LoginName"].ToString();
                if (Session["UserType"].ToString().Equals("admin"))
                {
                    var user = db.UserLoginModel.Where(s => s.Login_Name == Code).FirstOrDefault();
                    if (user.Password.Equals(vModel.Password1))
                    {
                        user.Password = vModel.Password2;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

                        ShowMessage(MessageType.success, "Password Changed Successfully.");
                    }
                    else
                        ShowMessage(MessageType.error, "Invalid Old Password.");
                }
                else if (Session["UserType"].ToString().Equals("Student"))
                {
                    var user = db.StudentRegistration.Where(s => s.StudCode == Code).FirstOrDefault();
                    if (user.Password.Equals(vModel.Password1))
                    {
                        user.Password = vModel.Password2;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

                        ShowMessage(MessageType.success, "Password Changed Successfully.");
                    }
                    else
                        ShowMessage(MessageType.error, "Invalid Old Password.");
                }
                else
                {
                    var user = db.Staffs.Where(s => s.Staff_Code == Code).FirstOrDefault();
                    if (user.Password.Equals(vModel.Password1))
                    {
                        user.Password = vModel.Password2;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

                        ShowMessage(MessageType.success, "Password Changed Successfully.");
                    }
                    else
                        ShowMessage(MessageType.error, "Invalid Old Password.");
                }
            }
            else
                ShowMessage(MessageType.error, "Invalid Confirm Password.");
            return View(vModel);
        }

        protected void PopulateSemUpgradeDropDown(object FacultyVal = null, object CourseVal = null, object SemesterVal = null)
        {
            ViewBag.lstFaculty = new SelectList(from d in db.Faculty.AsEnumerable() select new { Value = d.Id, Text = d.FacultyDesc }, "Value", "Text", FacultyVal);
            ViewBag.lstCourse = new SelectList(from d in db.Course.AsEnumerable() select new { Value = d.Id, Text = d.CourseDesc }, "Value", "Text", CourseVal);
            ViewBag.lstSemester = new SelectList(from d in db.Semester.AsEnumerable() select new { Value = d.Id, Text = d.SemDesc }, "Value", "Text", SemesterVal);
        }
    }
}