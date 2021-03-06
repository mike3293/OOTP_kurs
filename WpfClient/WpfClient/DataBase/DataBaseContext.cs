﻿using System.Data.Entity;
using WpfClient.DataBase.Models;

namespace WpfClient.DataBase
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Internship> Internships { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
    }
}
