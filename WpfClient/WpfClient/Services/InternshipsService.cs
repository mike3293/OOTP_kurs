using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WpfClient.DataBase;
using WpfClient.DataBase.Models;

namespace WpfClient.Services
{
    public class InternshipsService
    {
        public static async Task<List<Internship>> GetInternshipsByManagerIdAsync(int managerId, bool includeFinished = false)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                var query = db.Internships.Where(i => i.Manager.Id == managerId);
                if (!includeFinished)
                {
                    query = query.Where(i => i.IsCompleted != true);
                }

                return await query.Include(i => i.Intern).ToListAsync();
            }
        }

        public static async Task<bool> AddInternshipAsync(Internship internship)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                internship.Intern = db.People.Find(internship.Intern.Id);
                internship.Manager = db.People.Find(internship.Manager.Id);
                db.Internships.Add(internship);
                int rowsUpdated = await db.SaveChangesAsync();
                return rowsUpdated > 0;
            }
        }

        public static async Task<Internship> GetInternshipByInternIdAsync(int internId)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                return await db.Internships.Where(i => i.Intern.Id == internId).Include(i => i.Intern).SingleOrDefaultAsync();
            }
        }

        public static async Task<bool> CompleteInternshipAsync(int internshipId)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                Internship internship = await db.Internships.FindAsync(internshipId);
                internship.IsCompleted = true;

                int rowsUpdated = await db.SaveChangesAsync();

                return rowsUpdated > 0;
            }
        }

        public static async Task<bool> UpdateInternshipEndDateAsync(Internship internship)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                Internship internshipToUpdate = await db.Internships.FindAsync(internship.Id);
                internshipToUpdate.EndDate = internship.EndDate;

                int rowsUpdated = await db.SaveChangesAsync();

                return rowsUpdated > 0;
            }
        }
    }
}
