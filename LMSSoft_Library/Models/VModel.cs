using System;
using System.ComponentModel.DataAnnotations;

namespace LMSSoft_Library.Models
{
    public class StudentRegistrationVModel
    {
        [Display(Name = "Student Image")]
        public string ImagePath { get; set; }

        [Required]
        [Display(Name = "Student Code")]
        public string StudCode { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Display(Name = "Admission Date")]
        public DateTime? AdmissionDate { get; set; }

        [Display(Name = "Faculty")]
        public int? FacultyId { get; set; }

        [Display(Name = "Course")]
        public int? CourseId { get; set; }

        [Display(Name = "Semester")]
        public int? SemesterId { get; set; }

        public int? Year { get; set; }

        [Display(Name = "Section")]
        public int? SectionId { get; set; }

        [Required]
        [Display(Name = "Roll No.")]
        public int? Roll_No { get; set; }

        public string Password { get; set; }
    }

    public class StudentListVModel
    {
       public string StudCode { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string ImagePath { get; set; }
        public string Faculty { get; set; }
        public string Course { get; set; }
        public string Semester { get; set; }
        public int? Year { get; set; }
        public int? Roll_No { get; set; }
        public string Section { get; set; }
    }

    public class StaffRegistrationVModel
    {
        [Required]
        [Display(Name = "Join Date")]
        public DateTime? Join_Date { get; set; }

        [Required]
        [Display(Name = "Staff Code")]
        public string Staff_Code { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Full_Name { get; set; }

        [Required]
        [Display(Name = "Father Name")]
        public string Father_Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Mobile No.")]
        public string Mobile_No { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime? DOB { get; set; }

        [Required]
        public string Faculty { get; set; }

        [Required]
        public string Designation { get; set; }

        [Display(Name = "Image")]
        public string Image_Path { get; set; }

        public string Password { get; set; }
    }

    [Serializable()]
    public class BookRegistrationVModel
    {
        [Required]
        [Display(Name = "Accession No.")]
        public string AccessionNo { get; set; }

        [Required]
        [Display(Name = "ISBN No.")]
        public string ISBN_NO { get; set; }

        [Required]
        [Display(Name = "Entry Date")]
        public DateTime? EntryDate { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string BookCategory { get; set; }

        [Required]
        [Display(Name = "Book Title")]
        public string BookTitle { get; set; }

        [Display(Name = "Classification No")]
        public string Book_SubTitle { get; set; }

        [Display(Name = "Author Name")]
        public string AuthorName { get; set; }

        [Display(Name = "Publisher Name")]
        public string PublisherName { get; set; }

        [Display(Name = "Published Year")]
        public int? PublishedYear { get; set; }

        [Display(Name = "Edition")]
        public string Edition { get; set; }

        [Display(Name = "No. of Page")]
        public int NoOfPage { get; set; }

        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [Display(Name = "Purchase Date")]
        public DateTime? PurchaseDate { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }

        [Display(Name = "Book Image")]
        public string ImagePath { get; set; }
    }

    public class BookIssueVModel
    {
        [Required]
        [Display(Name = "Issue Date")]
        public DateTime? Issue_Date { get; set; }

        [Required]
        [Display(Name = "Due Days")]
        public int Due_Days { get; set; }

        [Display(Name = "Issue To")]
        public string IssueTo { get; set; }

        [Display(Name = "Staff Code")]
        public string StaffCode { get; set; }

        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }
        

    
        [Display(Name = "Student Code")]
        public string StudCode { get; set; }

        [Display(Name = "Student Name")]
        public string StudName { get; set; }

        public string Faculty { get; set; }

        public string Course { get; set; }

        public string Semester { get; set; }

        public string Section { get; set; }

        [Display(Name = "Roll No.")]
        public string RollNo { get; set; }

        [Required]
        [Display(Name = "Accession No")]
        public string AccessionNo { get; set; }
        
        [Display(Name = "ISBN No.")]
        public string ISBN_NO { get; set; }

        [Display(Name = "Book Title")]
        public string BookTitle { get; set; }

        [Display(Name = "Book Author")]
        public string BookAuthor { get; set; }

        public string Remarks { get; set; }
    }

    public class BookReturnVModel
    {
        [Required]
        [Display(Name = "Return Date")]
        public DateTime? Return_Date { get; set; }

        [Display(Name = "Issue Date")]
        public DateTime? Issue_Date { get; set; }

        [Display(Name = "Due Days")]
        public int DueDays { get; set; }

        public string Type { get; set; }

        [Display(Name = "Student Name")]
        public string StudName { get; set; }

        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }

        public string Faculty { get; set; }

        public string Course { get; set; }

        public string Semester { get; set; }

        public string Section { get; set; }

        [Display(Name = "Roll No.")]
        public string RollNo { get; set; }

        [Required]
        [Display(Name = "Accession No")]
        public string AccessionNo { get; set; }

        [Display(Name = "ISBN No.")]
        public string ISBN_NO { get; set; }

        [Display(Name = "Book Title")]
        public string BookTitle { get; set; }

        [Display(Name = "Book Author")]
        public string BookAuthor { get; set; }

        [Display(Name = "Fine Days")]
        public int FineDays { get; set; }

        [Display(Name = "Fine Amt.")]
        public int FineAmt { get; set; }

        public string Remarks { get; set; }
    }



    public class StudentDueBookVModel
    {
        [Required]
        public string StudCode { get; set; }
        
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        
        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }
        
        public string Faculty { get; set; }

        public string Course { get; set; }

        public string Semester { get; set; }

        public string Section { get; set; }

        [Display(Name = "Roll No.")]
        public int? Roll_No { get; set; }
    }
    public class StudentDueBookList
    {
        public DateTime? Issue_Date { get; set;  }
        public string AccessionNo { get; set; }
        public string ISBN_NO { get; set; }
        public string BookCategory { get; set; }
        public string BookTitle { get; set; }
        public string AuthorName { get; set; }
    }


    public class UserRegistrationVModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string Full_Name { get; set; }

        [Required]
        [Display(Name = "Mobile No.")]
        public string Mobile_No { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Login Name")]
        public string Login_Name { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password1 { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string Password2 { get; set; }

        [Display(Name = "User Type")]
        public string User_Type { get; set; }

        [Display(Name = "Status")]
        public string User_Status { get; set; }
    }

    public class ChangePasswordVModel
    {
        [Required]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string Password1 { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string Password2 { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string Password3 { get; set; }

    }

    public class DueBooksList
    {
        public DateTime? Issue_Date { get; set; }
        public int? Due_Days { get; set; }
        public string AccessionNo { get; set; }
        public string IsbnNo { get; set; }
        public string BookTitle { get; set; }
        public string Issue_To { get; set; }
        public int? Book_Id { get; set; }
        public int? IssueTo_Id { get; set; }
        public string IssuedToName { get; set; }

        public string Remarks { get; set; }
    }
    public class FineReportList
    {
        public DateTime? FDate { get; set; }
        public decimal? Amt { get; set; }
    }

    public class SemesterUpgradeVModel
    {
        public int FacultyId { get; set; }
        public int CourseId { get; set; }
        public int SemesterId { get; set; }
        public int? UpSemesterId { get; set; }
    }
    public class SemesterDataList
    {
        public int Id { get; set; }
        public string StudCode { get; set; }
        public string FullName { get; set; }
        public int? Roll_No { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }
}