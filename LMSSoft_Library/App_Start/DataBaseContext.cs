using LMSSoft_Library.Models;
using System.Data.Entity;

namespace LMSSoft_Library.App_Start
{
    public class DataBaseContext : DbContext
    {
        public DbSet<SystemSetting> SystemSetting { get; set; }
        public DbSet<UserLoginModel> UserLoginModel { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Semester> Semester { get; set; }
        public DbSet<StudentRegistration> StudentRegistration { get; set; }

        public DbSet<Staffs> Staffs { get; set; }

        public DbSet<BooKRegistration> BooKRegistration { get; set; }

        public DbSet<BookIssue> BookIssue { get; set; }
        public DbSet<BookReturn> BookReturn { get; set; }
        

    }
}