namespace AutoDoc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Web.Mvc;
    
    public partial class AutoDocContext : DbContext
    {
        public AutoDocContext()
            : base("name=DefaultConnection")
        {
        }
       
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ControlForm> ControlForms { get; set; }
        public DbSet<Mark> Marks { get; set; }
    }
}