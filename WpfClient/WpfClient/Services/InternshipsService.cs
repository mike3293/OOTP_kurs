using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WpfClient.DataBase;
using WpfClient.DataBase.Models;

namespace WpfClient.Services
{
    public class InternshipsService
    {
        public static async Task<List<Internship>> GetInternshipsByManagerIdAsync(int managerId)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                return await db.Internships.Where(i => i.Manager.Id == managerId).Include(i => i.Intern).ToListAsync();
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
    }
}
