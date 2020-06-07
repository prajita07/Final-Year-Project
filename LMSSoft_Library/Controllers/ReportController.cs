using LMSSoft_Library.App_Start;
using LMSSoft_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSSoft_Library.Controllers
{
    [ValidateSession]
    public class ReportController : BaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StudBookDue()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StudBookDue(string StudCode)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                var stud = db.Database.SqlQuery<StudentDueBookVModel>(@"Select StudCode,FullName,(Select FacultyDesc from Faculty Where Id = FacultyId) as Faculty,(Select CourseDesc from Course Where Id = CourseId) as Course,(Select SemDesc from Semester Where Id = SemesterId) as Semester,(Select SectionDesc from Section Where Id = SectionId) as Section,Roll_No from StudentRegistration Where StudCode = '" + StudCode + "'").FirstOrDefault();
                if (stud != null)
                    ViewBag.lstData = db.Database.SqlQuery<StudentDueBookList>("Select Issue_Date,BR.AccessionNo,br.ISBN_NO,br.BookCategory,br.BookTitle,br.AuthorName from BookIssue BI, BooKRegistration BR Where BR.Id = BI.Book_Id ANd IssueTo_Id = (Select Id from StudentRegistration Where StudCode = '" + StudCode + "') And Returned = 0 Order by Issue_Date").ToList();
                return View(stud);
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


        public ActionResult DueBookList()
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                ViewBag.lstData = db.Database.SqlQuery<DueBooksList>(@"Select Issue_Date,Due_Days,Issue_To,Book_Id,(Select ISBN_NO From BooKRegistration WHere Id = Book_Id) as IsbnNo,(Select BookTitle From BooKRegistration WHere Id = Book_Id) as BookTitle,(Select AccessionNo From BooKRegistration WHere Id = Book_Id) as AccessionNo,IssueTo_Id,Case when Issue_To = 'Student' Then (Select FullName From StudentRegistration Where Id = IssueTo_Id) Else (Select Full_Name from Staffs Where Id = IssueTo_Id) End as IssuedToName from BookIssue Where Returned = 0 Order By Issue_Date").ToList();
                return View();
            }
        }

        public ActionResult FineReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FineReport(string FDate,string TDate)
        {
            ViewBag.lstData = db.Database.SqlQuery<FineReportList>(@"Select Distinct Return_Date as FDate,Sum(FineAmt) as Amt From BookReturn where FineAmt > 0 And Return_Date between '"+ FDate +"' and '"+ TDate +"' Group By Return_Date").ToList();
            return View();
        }

        public ActionResult StudentWiseFineReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentWiseFineReport(BookIssueVModel vModel)
        {
            ViewBag.lstData = db.Database.SqlQuery<FineReportList>(@"Select Distinct Return_Date as FDate,Sum(FineAmt) as Amt From BookReturn where FineAmt > 0 And Book_Id In (Select Book_Id From BookIssue Where Issue_To = 'Student' and IssueTo_Id = (Select Id from StudentRegistration where StudCode = '"+ vModel.StudCode +"')) Group By Return_Date").ToList();
            return View(vModel);
        }


    }
}