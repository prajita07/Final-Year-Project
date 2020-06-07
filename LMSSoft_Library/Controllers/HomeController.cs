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
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                var dashboard = db.Database.SqlQuery<Dashboard>(@"Select (Select Count(DIstinct BookCategory) from BooKRegistration) as TotalBooksCategory,(Select Count(*) from BooKRegistration) as TotalBooks,(Select Count(*) from BookIssue Where Returned = 0) as TotalIssue,(Select Count(*) from StudentRegistration) as TotalStudent,(Select Count(*) from Staffs) as TotalStaff").FirstOrDefault();
                ViewBag.lstData = db.Database.SqlQuery<DueBooksList>(@"Select Issue_Date,Due_Days,Issue_To,Book_Id,(Select ISBN_NO From BooKRegistration WHere Id = Book_Id) as IsbnNo,(Select BookTitle From BooKRegistration WHere Id = Book_Id) as BookTitle,(Select AccessionNo From BooKRegistration WHere Id = Book_Id) as AccessionNo,IssueTo_Id,Case when Issue_To = 'Student' Then (Select FullName From StudentRegistration Where Id = IssueTo_Id) Else (Select Full_Name from Staffs Where Id = IssueTo_Id) End as IssuedToName from BookIssue Where Returned = 0 And Issue_To = '" + Session["UserType"].ToString() + "' and  IssueTo_Id = '" + Session["UserId"].ToString() + "' Order By Issue_Date").ToList();

                return View(dashboard);
            }
        }

        public static List<string> GetNotificationsList(string usertype, string userid)
        {
            List<string> list = new List<string>();
            using (DataBaseContext db = new DataBaseContext())
            {
                var lstdata = db.Database.SqlQuery<DueBooksList>(@"Select Issue_Date,Due_Days,Issue_To,Book_Id,(Select ISBN_NO From BooKRegistration WHere Id = Book_Id) as IsbnNo,(Select BookTitle From BooKRegistration WHere Id = Book_Id) as BookTitle,(Select AccessionNo From BooKRegistration WHere Id = Book_Id) as AccessionNo,IssueTo_Id,Case when Issue_To = 'Student' Then (Select FullName From StudentRegistration Where Id = IssueTo_Id) Else (Select Full_Name from Staffs Where Id = IssueTo_Id) End as IssuedToName, Case When Dateadd(Day,Due_Days,Issue_Date) = GETDATE() Then 'You have to return book for ISBN No. ' + (Select ISBN_NO From BooKRegistration WHere Id = Book_Id) + ' Today, Please return on time to avoid Fine.'  Else 'You have exceeded due date for ISBN No. ' + (Select ISBN_NO From BooKRegistration WHere Id = Book_Id) + ' , Please return soon.' end as Remarks from BookIssue Where Returned = 0 And Issue_To = '" + usertype + "' and  IssueTo_Id = '" + userid + "' and Dateadd(Day,Due_Days,Issue_Date) <= GETDATE() Order By Issue_Date").ToList();
                foreach (var item in lstdata)
                {
                    list.Add(item.Remarks);
                }
            }
            return list;
        }

        public ActionResult SearchBook()
        {
            ViewBag.lstBooks = new List<BookRegistrationVModel>();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchBook(string SearchText)
        {
            ViewBag.lstBooks = db.Database.SqlQuery<BookRegistrationVModel>("Select AccessionNo,ISBN_NO,EntryDate,BookCategory,BookTitle,Book_SubTitle,AuthorName,PublisherName,PublishedYear,Edition,NoOfPage,Price,PurchaseDate, case when (Select 1 from BookIssue WHere Returned = 0 and Book_Id = B.Id) = 1 then 'Issued' Else 'Available' end as Status from BooKRegistration B WHere AccessionNo Like '" + SearchText + "%' Or ISBN_NO Like '" + SearchText + "%' or BookCategory Like '" + SearchText + "%' Or BookTitle Like '" + SearchText + "%' Or Book_SubTitle Like '" + SearchText + "%' Or AuthorName Like '" + SearchText + "%' Or PublisherName Like '" + SearchText + "%' Order By BookTitle").ToList();
            return View();
        }


        public ActionResult SignOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "User");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}