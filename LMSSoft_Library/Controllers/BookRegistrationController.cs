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
    public class BookRegistrationController : BaseController
    {
        // GET: BookRegistration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string SearchValue)
        {
            if (SearchValue.Equals(""))
                return View(db.BooKRegistration.ToList());
            else
                return View(db.BooKRegistration.Where(b => b.AccessionNo.StartsWith(SearchValue) || b.ISBN_NO.StartsWith(SearchValue) || b.BookCategory.StartsWith(SearchValue) || b.BookTitle.StartsWith(SearchValue) || b.AuthorName.StartsWith(SearchValue)).ToList());
        }

        public ActionResult Create()
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["entryMode"].ToString()))
                return RedirectToAction("AccessDenied", "Home");
            int? Id = RouteData.Values["Id"] == null ? 0 : Convert.ToInt32(RouteData.Values["Id"]);

            if (Id > 0)
            {
                var book = db.BooKRegistration.Find(Id);
                if (book == null)
                {
                    ShowMessage(MessageType.warning, "Book Not Found, Please Enter With Proper Url.");
                    return RedirectToAction("Index");
                }

                var vModel = new BookRegistrationVModel()
                {
                    AccessionNo = book.AccessionNo,
                    ISBN_NO = book.ISBN_NO,
                    EntryDate = book.EntryDate,
                    BookCategory = book.BookCategory,
                    BookTitle = book.BookTitle,
                    Book_SubTitle = book.Book_SubTitle,
                    AuthorName = book.AuthorName,
                    PublisherName = book.PublisherName,
                    PublishedYear = book.PublishedYear,
                    Edition = book.Edition,
                    NoOfPage = book.NoOfPage,
                    Price = book.Price,
                    PurchaseDate = book.PurchaseDate,
                    Status = book.Status,
                    Remarks = book.Remarks,
                    ImagePath = book.ImagePath
                };
                PopulateDropDownlist(book.Status);
                return View(vModel);
            }
            var vModel1 = new BookRegistrationVModel()
            {
                EntryDate = DateTime.Today,
            };
            PopulateDropDownlist();
            return View(vModel1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ImagePath")]BookRegistrationVModel vModel, HttpPostedFileBase ImagePath)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int? Id = RouteData.Values["Id"] == null ? 0 : Convert.ToInt32(RouteData.Values["Id"]);

                    if (Id > 0)
                    {
                        var booK = db.BooKRegistration.Find(Id);
                        if (booK == null)
                        {
                            ShowMessage(MessageType.warning, "Book Not Found, Please Enter With Proper Url.");
                            return RedirectToAction("Index", new { Id = "" });
                        }
                        if (ImagePath != null)
                        {
                            ImagePath.SaveAs(Server.MapPath("~/Uploads/BookImage/" + vModel.AccessionNo + Path.GetExtension(ImagePath.FileName)));
                            booK.ImagePath = "~/Uploads/BookImage/" + vModel.AccessionNo + Path.GetExtension(ImagePath.FileName);
                        }
                        booK.AccessionNo = vModel.AccessionNo;
                        booK.ISBN_NO = vModel.ISBN_NO;
                        booK.EntryDate = vModel.EntryDate;
                        booK.BookCategory = vModel.BookCategory;
                        booK.BookTitle = vModel.BookTitle;
                        booK.Book_SubTitle = vModel.Book_SubTitle;
                        booK.AuthorName = vModel.AuthorName;
                        booK.PublisherName = vModel.PublisherName;
                        booK.PublishedYear = vModel.PublishedYear;
                        booK.Edition = vModel.Edition;
                        booK.NoOfPage = vModel.NoOfPage;
                        booK.Price = vModel.Price;
                        booK.PurchaseDate = vModel.PurchaseDate;
                        booK.Status = vModel.Status;
                        booK.Remarks = vModel.Remarks;

                        db.Entry(booK).State = EntityState.Modified;
                        db.SaveChanges();
                        ShowMessage(MessageType.success, "Book Updated Successfully.");
                        return RedirectToAction("Create", new { Id = "" });
                    }
                    else
                    {
                        var book = db.BooKRegistration.Where(b => b.AccessionNo == vModel.AccessionNo).FirstOrDefault();
                        if (book == null)
                        {
                            BooKRegistration dbModel = new BooKRegistration();
                            if (ImagePath != null)
                            {
                                ImagePath.SaveAs(Server.MapPath("~/Uploads/BookImage/" + vModel.AccessionNo + Path.GetExtension(ImagePath.FileName)));
                                dbModel.ImagePath = "~/Uploads/BookImage/" + vModel.AccessionNo + Path.GetExtension(ImagePath.FileName);
                            }
                            dbModel.AccessionNo = vModel.AccessionNo;
                            dbModel.ISBN_NO = vModel.ISBN_NO;
                            dbModel.EntryDate = vModel.EntryDate;
                            dbModel.BookCategory = vModel.BookCategory;
                            dbModel.BookTitle = vModel.BookTitle;
                            dbModel.Book_SubTitle = vModel.Book_SubTitle;
                            dbModel.AuthorName = vModel.AuthorName;
                            dbModel.PublisherName = vModel.PublisherName;
                            dbModel.PublishedYear = vModel.PublishedYear;
                            dbModel.Edition = vModel.Edition;
                            dbModel.NoOfPage = vModel.NoOfPage;
                            dbModel.Price = vModel.Price;
                            dbModel.PurchaseDate = vModel.PurchaseDate;
                            dbModel.Status = vModel.Status;
                            dbModel.Remarks = vModel.Remarks;
                            dbModel.CreatedBy = HttpContext.Session["LoginName"].ToString();
                            dbModel.CreatedDateTime = DateTime.Now;
                            db.BooKRegistration.Add(dbModel);
                            db.SaveChanges();
                            ShowMessage(MessageType.success, "Book Added Successfully.");
                            return RedirectToAction("Create", new { Id = "" });
                        }
                        else
                            ShowMessage(MessageType.warning, "Book Accession No. Already Exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.warning, "Oops, There was an error. Try again, and if fails contact your system administrator. Error : " + ex.Message);
            }
            PopulateDropDownlist(vModel.Status);
            return View(vModel);
        }

        public JsonResult GetBookCategory(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select Distinct BookCategory from BooKRegistration WHere BookCategory Like '" + term + "%' Order by BookCategory").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetBookTitle(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select Distinct BookTitle from BooKRegistration WHere BookTitle Like '" + term + "%' Order by BookTitle").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetBook_SubTitle(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select Distinct Book_SubTitle from BooKRegistration WHere Book_SubTitle Like '" + term + "%' Order by Book_SubTitle").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAuthorName(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select Distinct AuthorName from BooKRegistration WHere AuthorName Like '" + term + "%' Order by AuthorName").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPublisherName(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select Distinct PublisherName from BooKRegistration WHere PublisherName Like '" + term + "%' Order by PublisherName").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEdition(string term)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<string> list;
                list = db.Database.SqlQuery<string>(@"Select Distinct Edition from BooKRegistration WHere Edition Like '" + term + "%' Order by Edition").ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetBookDetails(int id)
        {
            try
            {
                var books = db.BooKRegistration.Find(id);
                if (books != null)
                    return Json(new
                    {
                        success = true,
                        accno = books.AccessionNo,
                        isbn = books.ISBN_NO,
                        entDate = Convert.ToDateTime(books.EntryDate).ToString("yyyy-MM-dd"),
                        category = books.BookCategory,
                        title = books.BookTitle,
                        subtitle = books.Book_SubTitle,
                        author = books.AuthorName,
                        publisher = books.PublisherName,
                        pubyear = books.PublishedYear,
                        edition = books.Edition,
                        noofpage = books.NoOfPage,
                        price = books.Price,
                        purdate = Convert.ToDateTime(books.PurchaseDate).ToString("yyyy-MM-dd"),
                        status = books.Status,
                        remarks = books.Remarks,
                        imgpath = books.ImagePath == null ? "" : books.ImagePath.Substring(books.ImagePath.IndexOf("."), books.ImagePath.Length - books.ImagePath.IndexOf("."))
                    });
                else
                    return Json(new { success = false, responseText = "Book not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }

        protected void PopulateDropDownlist(object StatusVal = null)
        {
            ViewBag.lstStatus = new SelectList(new List<SelectListItem> {
                new SelectListItem{ Text = "Available", Value = "Available"},
                new SelectListItem{ Text = "Not Available", Value = "Not Available"},
                new SelectListItem{ Text = "Lost", Value = "Lost"}
            }, "Value", "Text", StatusVal);
        }
    }
}