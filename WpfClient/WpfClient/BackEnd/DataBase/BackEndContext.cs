﻿using System.Data.Entity;

namespace WpfClient.BackEnd.DataBase
{
    internal class BackEndContext : DbContext
    {
        public BackEndContext() : base("Company")
        { }

        public DbSet<Internship> Internships { get; set; }
        public DbSet<InternshipGoal> InternshipGoals { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
    }
}
