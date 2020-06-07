using LMSSoft_Library.App_Start;
using LMSSoft_Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace LMSSoft_Library.Controllers
{
    [ValidateSession]
    public class BookController : BaseController
    {
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Issue()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["entryMode"].ToString()))
                return RedirectToAction("AccessDenied", "Home");
            var sys = db.SystemSetting.FirstOrDefault();
            var vModel = new BookIssueVModel()
            {
                Due_Days = sys.Due_Days,
                Issue_Date = DateTime.Now
            };
            PopulateIssueDropDownList();
            return View(vModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Issue(BookIssueVModel vModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (DataBaseContext db = new DataBaseContext())
                    {
                        var sys = db.SystemSetting.FirstOrDefault();
                        int IssueTOID = 0;

                        if (vModel.IssueTo.Equals("Student"))
                        {
                            var stud = db.StudentRegistration.Where(s => s.StudCode == vModel.StudCode).FirstOrDefault();
                            if (stud == null)
                            {
                                ShowMessage(MessageType.warning, "Invalid Record for Student.");
                                PopulateIssueDropDownList(vModel.IssueTo);
                                return View(vModel);
                            }
                            int DueBooks = db.Database.SqlQuery<int>("Select Count(*) as NoOfBooks from BookIssue Where IssueTo_Id = '" + stud.Id + "' And Returned = 0").FirstOrDefault();
                            if (DueBooks >= sys.Max_Book)
                            {
                                ShowMessage(MessageType.warning, "Student has already taken " + DueBooks + " Books.");
                                PopulateIssueDropDownList(vModel.IssueTo);
                                return View(vModel);
                            }

                            IssueTOID = stud.Id;
                        }
                        else
                        {
                            var staff = db.Staffs.Where(s => s.Staff_Code == vModel.StaffCode).FirstOrDefault();
                            if (staff == null)
                            {
                                ShowMessage(MessageType.warning, "Invalid Record for Staff/Teacher.");
                                PopulateIssueDropDownList(vModel.IssueTo);
                                return View(vModel);
                            }
                            IssueTOID = staff.Id;
                        }
                        var book = db.BooKRegistration.Where(b => b.AccessionNo == vModel.AccessionNo).FirstOrDefault();

                        if (book == null)
                        {
                            ShowMessage(MessageType.warning, "Invalid Record for Book.");
                            PopulateIssueDropDownList(vModel.IssueTo);
                            return View(vModel);
                        }

                        BookIssue dbmodel = new BookIssue();
                        dbmodel.Issue_Date = vModel.Issue_Date;
                        dbmodel.Due_Days = vModel.Due_Days;
                        dbmodel.Issue_To = vModel.IssueTo;
                        dbmodel.IssueTo_Id = IssueTOID;
                        dbmodel.Book_Id = book.Id;
                        dbmodel.Remarks = vModel.Remarks;
                        dbmodel.Returned = 0;
                        dbmodel.CreatedBy = HttpContext.Session["LoginName"].ToString();
                        dbmodel.CreatedDateTime = DateTime.Now;

                        db.BookIssue.Add(dbmodel);
                        db.SaveChanges();

                        ShowMessage(MessageType.success, "Book Issued Successfully.");
                        return RedirectToAction("Issue", new { Id = "" });
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(MessageType.error, ex.Message);
                }
            }
            PopulateIssueDropDownList(vModel.IssueTo);
            return View(vModel);
        }

        protected void PopulateIssueDropDownList(object IssuToVal = null)
        {
            ViewBag.lstIssuTo = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = "Student", Value = "Student"},
                new SelectListItem{Text = "Staff/Teacher", Value = "Staff"}
            }, "Value", "Text", IssuToVal);
        }

        public ActionResult Return()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["entryMode"].ToString()))
                return RedirectToAction("AccessDenied", "Home");
            var vModel = new BookReturnVModel()
            {
                Return_Date = DateTime.Now
            };
            PopulateReturnDropDownList();
            return View(vModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(BookReturnVModel vModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var book = db.BooKRegistration.Where(b => b.AccessionNo == vModel.AccessionNo).FirstOrDefault();
                    if (book == null)
                    {
                        PopulateReturnDropDownList(vModel.Type);
                        ShowMessage(MessageType.warning, "Invalid Record for Book.");
                        return View(vModel);
                    }
                    BookReturn dbmodel = new BookReturn();
                    dbmodel.Return_Date = vModel.Return_Date;
                    dbmodel.Book_Id = book.Id;
                    dbmodel.Type = vModel.Type;
                    dbmodel.FineAmt = vModel.FineAmt;
                    dbmodel.Remarks = vModel.Remarks;
                    dbmodel.CreatedBy = HttpContext.Session["LoginName"].ToString();
                    dbmodel.CreatedDateTime = DateTime.Now;

                    db.BookReturn.Add(dbmodel);
                    db.SaveChanges();

                    var bookIssues = db.BookIssue.Where(i => i.Book_Id == book.Id && i.Returned == 0).FirstOrDefault();
                    if (bookIssues != null)
                    {
                        bookIssues.Returned = 1;
                        db.Entry(bookIssues).State = EntityState.Modified;

                        if (vModel.Type.Equals("Lost"))
                        {
                            var books = db.BooKRegistration.Find(book.Id);
                            books.Status = "Lost";
                            db.Entry(books).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }

                    ShowMessage(MessageType.success, "Book Returned Successfully.");
                    return RedirectToAction("Return", new { Id = "" });
                }
                catch (Exception ex)
                {
                    ShowMessage(MessageType.error, ex.Message);
                }
            }
            PopulateReturnDropDownList(vModel.Type);
            return View(vModel);
        }

        protected void PopulateReturnDropDownList(object TypeVal = null)
        {
            ViewBag.lstTypeVal = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = "Return", Value = "Return"},
                new SelectListItem{Text = "Lost", Value = "Lost"}
            }, "Value", "Text", TypeVal);
        }


        public JsonResult GetStaffList(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select Staff_Code + '--' + Full_Name as name from Staffs WHere Staff_Code Like '" + term + "%' or Full_Name like '" + term + "%'").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStudentList(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select StudCode + '--' + FullName as name from StudentRegistration WHere StudCode Like '" + term + "%' or FullName like '" + term + "%'").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetStudentDetails(string studcode)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                string sSql = @"Select StudCode,FullName,(Select FacultyDesc from Faculty Where Id = FacultyId) as Faculty,(Select CourseDesc from Course Where Id = CourseId) as Course,(Select SemDesc from Semester Where Id = SemesterId) as Semester,(Select SectionDesc from Section Where Id = SectionId) as Section,Roll_No from StudentRegistration Where StudCode = '" + studcode + "'";
                var studData = db.Database.SqlQuery<StudentDetails>(sSql).FirstOrDefault();

                if (studData != null)
                    return Json(new { rescode = "400", code = studData.StudCode, name = studData.FullName, faculty = studData.Faculty, course = studData.Course, sem = studData.Semester, section = studData.Section, rollno = studData.Roll_No });
                else
                    return Json(new { rescode = "404" });
            }
        }
        public JsonResult GetBooksList(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select AccessionNo from BooKRegistration WHere Status = 'Available' And AccessionNo Like '" + term + "%' And Id Not in(Select Book_Id from BookIssue WHere Returned = 0) ").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetBookDetails(string accno)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                var bookData = db.Database.SqlQuery<BooKRegistration>(@"Select * from BooKRegistration WHere Status = 'Available' And AccessionNo = '" + accno + "' And Id Not in(Select Book_Id from BookIssue WHere Returned = 0) ").FirstOrDefault();

                if (bookData != null)
                    return Json(new { rescode = "400", accno = bookData.AccessionNo, title = bookData.BookTitle, isbnno = bookData.ISBN_NO, author = bookData.AuthorName });
                else
                    return Json(new { rescode = "404" });
            }
        }


        public JsonResult GetIssuedBooksList(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select AccessionNo from BooKRegistration WHere AccessionNo Like '" + term + "%' And Id in(Select Book_Id from BookIssue WHere Returned = 0) ").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetIssuedBookDetails(string accno)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                var bookData = db.Database.SqlQuery<BookStudDetal>(@"Select Isnull(Price,0.00) as Price,Issue_To,AccessionNo,ISBN_NO,BookCategory,BookTitle,Book_SubTitle,AuthorName,PublisherName,SR.FullName,SR.Roll_No,(Select CourseDesc from Course WHere Id = SR.CourseId) as Course,(Select FacultyDesc from Faculty WHere Id = SR.FacultyId) as Faculty,(Select SemDesc from Semester WHere Id = SR.SemesterId) as Sem,(Select SectionDesc from Section WHere Id = SR.SectionId) as Section,Convert(Varchar(50),BI.Issue_Date) as Issue_Date,BI.Due_Days from BooKRegistration BR, BookIssue BI,StudentRegistration SR WHere SR.Id = BI.IssueTo_Id And BI.Issue_To = 'Student' And BR.Id = BI.Book_Id And BI.Returned = 0 And BR.AccessionNo = '" + accno + "'   Union All   Select Isnull(Price,0.00) as Price,Issue_To,AccessionNo,ISBN_NO,BookCategory,BookTitle,Book_SubTitle,AuthorName,PublisherName,SR.Full_Name,0 Roll_No,'' as Course,'' Faculty,'' as Sem,'' as Section,Convert(Varchar(50),BI.Issue_Date) as Issue_Date,BI.Due_Days from BooKRegistration BR, BookIssue BI,Staffs SR WHere SR.Id = BI.IssueTo_Id And BI.Issue_To = 'Staff' And BR.Id = BI.Book_Id And BI.Returned = 0 And BR.AccessionNo = '" + accno + "' ").FirstOrDefault();
                int FineDays = 0;
                int FineAmt = 0;
                if (bookData != null)
                {
                    int DiffDays = DateTime.Today.Subtract(Convert.ToDateTime(bookData.Issue_Date)).Days;
                    if (DiffDays > bookData.Due_Days)
                    {
                        FineDays = DiffDays - 10;
                        if (bookData.Issue_To.Equals("Student"))
                            FineAmt = FineDays * 5;

                    }
                    return Json(new { rescode = "400", finedays = FineDays, fineamt = FineAmt, price = (bookData.Price * 2), issueto = bookData.Issue_To, accno = bookData.AccessionNo, isbnno = bookData.ISBN_NO, bookcategory = bookData.BookCategory, booktitle = bookData.BookTitle, booksubtitle = bookData.Book_SubTitle, author = bookData.AuthorName, publisher = bookData.PublisherName, studname = bookData.FullName, rollno = bookData.Roll_No, course = bookData.Course, faculty = bookData.Faculty, sem = bookData.Sem, sec = bookData.Section, duedays = bookData.Due_Days, issuedate = bookData.Issue_Date });
                }
                else
                    return Json(new { rescode = "404" });
            }
        }

        private class StudentDetails
        {
            public string StudCode { get; set; }
            public string FullName { get; set; }
            public string Faculty { get; set; }
            public string Course { get; set; }
            public string Semester { get; set; }
            public string Section { get; set; }
            public int? Roll_No { get; set; }
        }

        private class BookStudDetal
        {
            public decimal? Price { get; set; }
            public string Issue_To { get; set; }
            public string AccessionNo { get; set; }
            public string ISBN_NO { get; set; }
            public string BookCategory { get; set; }
            public string BookTitle { get; set; }
            public string Book_SubTitle { get; set; }
            public string AuthorName { get; set; }
            public string PublisherName { get; set; }
            public string FullName { get; set; }
            public int Roll_No { get; set; }
            public string Course { get; set; }
            public string Faculty { get; set; }
            public string Sem { get; set; }
            public string Section { get; set; }
            public string Issue_Date { get; set; }
            public int Due_Days { get; set; }
        }


    }
}