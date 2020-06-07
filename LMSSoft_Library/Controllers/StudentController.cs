using LMSSoft_Library.App_Start;
using LMSSoft_Library.Models;
using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSSoft_Library.Controllers
{
    [ValidateSession]
    public class StudentController : BaseController
    {
        // GET: Student
        public ActionResult Index()
        {
            return View(db.StudentRegistration.ToList().OrderBy(s => s.Roll_No));
        }

        public ActionResult Create()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["entryMode"].ToString()))
                return RedirectToAction("AccessDenied", "Home");
            int? Id = RouteData.Values["Id"] == null ? 0 : Convert.ToInt32(RouteData.Values["Id"]);

            if (Id > 0)
            {
                var stud = db.StudentRegistration.Find(Id);
                if (stud == null)
                {
                    ShowMessage(MessageType.warning, "Student Not Found, Please Enter With Proper Url.");
                    return RedirectToAction("Index");
                }

                var vModel = new StudentRegistrationVModel()
                {
                    Address = stud.Address,
                    AdmissionDate = stud.AdmissionDate,
                    BirthDate = stud.BirthDate,
                    CourseId = stud.CourseId,
                    Email = stud.Email,
                    FacultyId = stud.FacultyId,
                    FatherName = stud.FatherName,
                    FullName = stud.FullName,
                    MobileNo = stud.MobileNo,
                    Roll_No = stud.Roll_No,
                    SectionId = stud.SectionId,
                    SemesterId = stud.SemesterId,
                    StudCode = stud.StudCode,
                    Year = stud.Year,
                    Password = stud.Password,
                    ImagePath = stud.ImagePath
                };
                Populate_Dropdown_List(stud.FacultyId, stud.CourseId, stud.SemesterId, stud.SectionId);
                return View(vModel);
            }
            var vModel1 = new StudentRegistrationVModel()
            {
                AdmissionDate = DateTime.Now,
            };
            Populate_Dropdown_List();
            return View(vModel1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ImagePath")]StudentRegistrationVModel vModel, HttpPostedFileBase ImagePath)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int? Id = RouteData.Values["Id"] == null ? 0 : Convert.ToInt32(RouteData.Values["Id"]);

                    if (Id > 0)
                    {
                        var stud = db.StudentRegistration.Find(Id);
                        if (stud == null)
                        {
                            ShowMessage(MessageType.warning, "Student Not Found, Please Enter With Proper Url.");
                            return RedirectToAction("Index", new { Id = "" });
                        }

                        if (ImagePath != null)
                        {
                            ImagePath.SaveAs(Server.MapPath("~/Uploads/StudImage/" + vModel.StudCode + Path.GetExtension(ImagePath.FileName)));
                            stud.ImagePath = "~/Uploads/StudImage/" + vModel.StudCode + Path.GetExtension(ImagePath.FileName);
                        }
                        stud.Address = vModel.Address;
                        stud.AdmissionDate = vModel.AdmissionDate;
                        stud.BirthDate = vModel.BirthDate;
                        stud.CourseId = vModel.CourseId;
                        stud.CreatedBy = HttpContext.Session["LoginName"].ToString();
                        stud.CreatedDateTime = DateTime.Now;
                        stud.Email = vModel.Email;
                        stud.FacultyId = vModel.FacultyId;
                        stud.FatherName = vModel.FatherName;
                        stud.FullName = vModel.FullName;
                        stud.MobileNo = vModel.MobileNo;
                        stud.Roll_No = vModel.Roll_No;
                        stud.SectionId = vModel.SectionId;
                        stud.SemesterId = vModel.SemesterId;
                        stud.StudCode = vModel.StudCode;
                        stud.Year = vModel.Year;
                        stud.Password = vModel.Password;
                        db.Entry(stud).State = EntityState.Modified;
                        db.SaveChanges();

                        ShowMessage(MessageType.success, "Student Updated Successfully.");
                        return RedirectToAction("Create", new { Id = "" });
                    }
                    else
                    {
                        StudentRegistration dbModel = new StudentRegistration();
                        if (ImagePath != null)
                        {
                            ImagePath.SaveAs(Server.MapPath("~/Uploads/StudImage/" + vModel.StudCode + Path.GetExtension(ImagePath.FileName)));
                            dbModel.ImagePath = "~/Uploads/StudImage/" + vModel.StudCode + Path.GetExtension(ImagePath.FileName);
                        }
                        dbModel.Address = vModel.Address;
                        dbModel.AdmissionDate = vModel.AdmissionDate;
                        dbModel.BirthDate = vModel.BirthDate;
                        dbModel.CourseId = vModel.CourseId;
                        dbModel.CreatedBy = HttpContext.Session["LoginName"].ToString();
                        dbModel.CreatedDateTime = DateTime.Now;
                        dbModel.Email = vModel.Email;
                        dbModel.FacultyId = vModel.FacultyId;
                        dbModel.FatherName = vModel.FatherName;
                        dbModel.FullName = vModel.FullName;
                        dbModel.MobileNo = vModel.MobileNo;
                        dbModel.Roll_No = vModel.Roll_No;
                        dbModel.SectionId = vModel.SectionId;
                        dbModel.SemesterId = vModel.SemesterId;
                        dbModel.StudCode = vModel.StudCode;
                        dbModel.Year = vModel.Year;
                        dbModel.Password = vModel.Password;
                        db.StudentRegistration.Add(dbModel);
                        db.SaveChanges();
                        ShowMessage(MessageType.success, "Student Added Successfully.");
                        return RedirectToAction("Create", new { Id = "" });
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.warning, "Oops, There was an error. Try again, and if fails contact your system administrator. Error : " + ex.Message);
            }
            Populate_Dropdown_List();
            return View();
        }

        private void Populate_Dropdown_List(object FacultyVal = null, object CourseVal = null, object SemesterVal = null, object SectionVal = null)
        {
            ViewBag.lstFaculty = new SelectList(from d in db.Faculty.AsEnumerable() select new { Value = d.Id, Text = d.FacultyDesc }, "Value", "Text", FacultyVal);
            ViewBag.lstCourse = new SelectList(from d in db.Course.AsEnumerable() select new { Value = d.Id, Text = d.CourseDesc }, "Value", "Text", CourseVal);
            ViewBag.lstSemester = new SelectList(from d in db.Semester.AsEnumerable() select new { Value = d.Id, Text = d.SemDesc }, "Value", "Text", SemesterVal);
            ViewBag.lstSection = new SelectList(from d in db.Section.AsEnumerable() select new { Value = d.Id, Text = d.SectionDesc }, "Value", "Text", SectionVal);
        }


        [HttpPost]
        public JsonResult GetDetails(int id)
        {
            try
            {
                using (DataBaseContext db = new DataBaseContext())
                {
                    var data = db.Database.SqlQuery<StudentListVModel>(@"Select StudCode,FullName,FatherName,Address,MobileNo,Email,BirthDate,AdmissionDate,ImagePath,(Select FacultyDesc from Faculty Where Id = FacultyId) as Faculty,(Select CourseDesc from Course Where Id = CourseId) as Course,(Select SemDesc from Semester Where Id = SemesterId) as Semester,Year,Roll_No,(Select SectionDesc from Section Where Id = SectionId) as Section from StudentRegistration Where Id = '" + id + "'").FirstOrDefault();
                    if (data != null)
                    {
                        return Json(new
                        {
                            success = true,
                            studcode = data.StudCode,
                            fullname = data.FullName,
                            admdate = Convert.ToDateTime(data.AdmissionDate).ToString("yyyy-MM-dd"),
                            fathername = data.FatherName,
                            address = data.Address,
                            mobileno = data.MobileNo,
                            email = data.Email,
                            dob = Convert.ToDateTime(data.BirthDate).ToString("yyyy-MM-dd"),
                            faculty = data.Faculty,
                            course = data.Course,
                            semester = data.Semester,
                            year = data.Year,
                            section = data.Section,
                            rollno = data.Roll_No,
                            imgpath = data.ImagePath == null ? "" : data.ImagePath.Substring(data.ImagePath.IndexOf("."), data.ImagePath.Length - data.ImagePath.IndexOf("."))
                        });
                    }
                    else
                        return Json(new { success = false, responseText = "Student not found." });
                };
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }

    }
}