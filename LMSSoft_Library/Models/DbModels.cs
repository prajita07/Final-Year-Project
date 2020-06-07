using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMSSoft_Library.Models
{
    [Table("SystemSetting")]
    public class SystemSetting
    {
        public int Id { get; set; }
        public int Due_Days { get; set; }
        public int Max_Book { get; set; }
        public int FinePer_Day { get; set; }
    }

    [Table("Users")]
    public class UserLoginModel
    {
        public int ID { get; set; }
        public string Full_Name { get; set; }
        public string Mobile_No { get; set; }
        public string Email { get; set; }
        public string Login_Name { get; set; }
        public string Password { get; set; }
        public string User_Type { get; set; }
        public string User_Status { get; set; }
        public string Ent_User { get; set; }
        public DateTime? Ent_DateTime { get; set; }
    }
    [Table("Course")]
    public class Course
    {
        public int Id { get; set; }
        public string CourseDesc { get; set; }
    }
    [Table("Faculty")]
    public class Faculty
    {
        public int Id { get; set; }
        public string FacultyDesc { get; set; }
    }
    [Table("Section")]
    public class Section
    {
        public int Id { get; set; }
        public string SectionDesc { get; set; }
    }
    [Table("Semester")]
    public class Semester
    {
        public int Id { get; set; }
        public string SemDesc { get; set; }
    }
    [Table("StudentRegistration")]
    public class StudentRegistration
    {
        public int Id { get; set; }
        public string StudCode { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public int? FacultyId { get; set; }
        public int? CourseId { get; set; }
        public int? SemesterId { get; set; }
        public int? Year { get; set; }
        public int? SectionId { get; set; }
        public int? Roll_No { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string ImagePath { get; set; }
    }

    [Serializable()]
    [Table("BooKRegistration")]
    public class BooKRegistration
    {
        public int Id { get; set; }
        public string AccessionNo { get; set; }
        public string ISBN_NO { get; set; }
        public DateTime? EntryDate { get; set; }
        public string BookCategory { get; set; }
        public string BookTitle { get; set; }
        public string Book_SubTitle { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
        public int? PublishedYear { get; set; }
        public string Edition { get; set; }
        public int NoOfPage { get; set; }
        public decimal? Price { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string ImagePath { get; set; }

    }

    [Table("BookIssue")]
    public class BookIssue
    {
        public int Id { get; set; }
        public DateTime? Issue_Date { get; set; }
        public int Due_Days { get; set; }
        public string Issue_To { get; set;  }
        public int IssueTo_Id { get; set; }
        public int Book_Id { get; set; }
        public string Remarks { get; set; }
        public int Returned { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }

    [Table("BookReturn")]
    public class BookReturn
    {
        public int Id { get; set; }
        public DateTime? Return_Date { get; set; }
        public int Book_Id { get; set; }
        public string Remarks { get; set; }
        public string Type { get; set; }
        public decimal? FineAmt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
    
    [Table("Staffs")]
    public class Staffs
    {
        public int Id { get; set; }
        public DateTime? Join_Date { get; set; }
        public string Staff_Code { get; set; }
        public string Full_Name { get; set; }
        public string Father_Name { get; set; }
        public string Address { get; set; }
        public string Mobile_No { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public string Faculty { get; set; }
        public string Designation { get; set; }
        public string Image_Path { get; set; }
        public string Password { get; set; }
        public string Ent_User { get; set; }
        public DateTime? Ent_DateTime { get; set; }
    }

    public class Dashboard
    {
        public int TotalBooks { get; set; }
        public int TotalIssue { get; set; }
        public int TotalStudent { get; set; }
        public int TotalStaff { get; set; }
        public int TotalBooksCategory { get; set; }
    }
}