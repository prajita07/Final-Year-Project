using LMSSoft_Library.App_Start;
using LMSSoft_Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSSoft_Library.Controllers
{
    [ValidateSession]
    public class StaffController : BaseController
    {
        // GET: Staff
        public ActionResult Index()
        {
            return View(db.Staffs.ToList());
        }

        public ActionResult Create()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["entryMode"].ToString()))
                return RedirectToAction("AccessDenied", "Home");

            int? Id = RouteData.Values["Id"] == null ? 0 : Convert.ToInt32(RouteData.Values["Id"]);

            if (Id > 0)
            {
                var staff = db.Staffs.Find(Id);
                if (staff == null)
                {
                    ShowMessage(MessageType.warning, "Staff/Teacher Not Found, Please Enter With Proper Url.");
                    return RedirectToAction("Index");
                }

                var vModel = new StaffRegistrationVModel()
                {
                    Address = staff.Address,
                    Designation = staff.Designation,
                    DOB = staff.DOB,
                    Email = staff.Email,
                    Faculty = staff.Faculty,
                    Father_Name = staff.Father_Name,
                    Full_Name = staff.Full_Name,
                    Image_Path = staff.Image_Path,
                    Join_Date = staff.Join_Date,
                    Mobile_No = staff.Mobile_No,
                    Staff_Code = staff.Staff_Code,
                    Password = staff.Password
                };
                return View(vModel);
            }
            var vModel1 = new StaffRegistrationVModel()
            {
                Join_Date = DateTime.Now,
            };
            return View(vModel1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Image_Path")]StaffRegistrationVModel vModel, HttpPostedFileBase Image_Path)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int? Id = RouteData.Values["Id"] == null ? 0 : Convert.ToInt32(RouteData.Values["Id"]);

                    if (Id > 0)
                    {
                        var staff = db.Staffs.Find(Id);
                        if (staff == null)
                        {
                            ShowMessage(MessageType.warning, "Staff/Teacher Not Found, Please Enter With Proper Url.");
                            return RedirectToAction("Index", new { Id = "" });
                        }

                        if (Image_Path != null)
                        {
                            Image_Path.SaveAs(Server.MapPath("~/Uploads/StaffImage/" + vModel.Staff_Code + Path.GetExtension(Image_Path.FileName)));
                            staff.Image_Path = "~/Uploads/StaffImage/" + vModel.Staff_Code + Path.GetExtension(Image_Path.FileName);
                        }
                        staff.Address = vModel.Address;
                        staff.Designation = vModel.Designation;
                        staff.DOB = vModel.DOB;
                        staff.Email = vModel.Email;
                        staff.Faculty = vModel.Faculty;
                        staff.Father_Name = vModel.Father_Name;
                        staff.Full_Name = vModel.Full_Name;
                        staff.Join_Date = vModel.Join_Date;
                        staff.Mobile_No = vModel.Mobile_No;
                        staff.Staff_Code = vModel.Staff_Code;
                        staff.Password = vModel.Password;
                        db.Entry(staff).State = EntityState.Modified;                        
                        db.SaveChanges();

                        ShowMessage(MessageType.success, "Staff/Teacher Updated Successfully.");
                        return RedirectToAction("Create", new { Id = "" });
                    }
                    else
                    {
                        Staffs dbModel = new Staffs();
                        if (Image_Path != null)
                        {
                            Image_Path.SaveAs(Server.MapPath("~/Uploads/StaffImage/" + vModel.Staff_Code + Path.GetExtension(Image_Path.FileName)));
                            dbModel.Image_Path = "~/Uploads/StaffImage/" + vModel.Staff_Code + Path.GetExtension(Image_Path.FileName);
                        }

                        dbModel.Address = vModel.Address;
                        dbModel.Designation = vModel.Designation;
                        dbModel.DOB = vModel.DOB;
                        dbModel.Email = vModel.Email;
                        dbModel.Faculty = vModel.Faculty;
                        dbModel.Father_Name = vModel.Father_Name;
                        dbModel.Full_Name = vModel.Full_Name;
                        dbModel.Join_Date = vModel.Join_Date;
                        dbModel.Mobile_No = vModel.Mobile_No;
                        dbModel.Staff_Code = vModel.Staff_Code;
                        dbModel.Password = vModel.Password;
                        dbModel.Ent_User = HttpContext.Session["LoginName"].ToString();
                        dbModel.Ent_DateTime = DateTime.Now;

                        db.Staffs.Add(dbModel);
                        
                        db.SaveChanges();
                        ShowMessage(MessageType.success, "Staff/Teacher Added Successfully.");
                        return RedirectToAction("Create", new { Id = "" });
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.warning, "Oops, There was an error. Try again, and if fails contact your system administrator. Error : " + ex.Message);
            }
            return View();
        }


        public JsonResult GetFaculty(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select Distinct Faculty from Staffs WHere Faculty Like '" + term + "%' Order by Faculty").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDesignation(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select Distinct Designation from Staffs WHere Designation Like '" + term + "%' Order by Designation").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetDetails(int id)
        {
            try
            {
                var staffs = db.Staffs.Find(id);
                if (staffs != null)
                    return Json(new
                    {
                        success = true,
                        staffcode = staffs.Staff_Code,
                        fullname = staffs.Full_Name,
                        joindate = Convert.ToDateTime(staffs.Join_Date).ToString("yyyy-MM-dd"),
                        fathername = staffs.Father_Name,
                        address = staffs.Address,
                        mobileno = staffs.Mobile_No,
                        email = staffs.Email,
                        dob = Convert.ToDateTime(staffs.DOB).ToString("yyyy-MM-dd"),
                        faculty = staffs.Faculty,
                        designation = staffs.Designation,                        
                        imgpath = staffs.Image_Path == null ? "" : staffs.Image_Path.Substring(staffs.Image_Path.IndexOf("."), staffs.Image_Path.Length - staffs.Image_Path.IndexOf("."))
                    });
                else
                    return Json(new { success = false, responseText = "Staff not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }
    }
}