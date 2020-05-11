using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.DataBase;
using WpfClient.DataBase.Models;

namespace WpfClient.Services
{
    public class AssessmentsService
    {
        public static async Task<List<Assessment>> GetAssessmentsByInternIdAsync(int internId)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                return await db.Assessments.Where(a => a.Internship.Intern.Id == internId).ToListAsync();
            }
        }

        public static async Task<bool> AddAssessmentAsync(Assessment assessment)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                Internship internship = await db.Internships.FindAsync(assessment.Internship.Id);
                assessment.Internship = internship;

                db.Assessments.Add(assessment);
                int rowsUpdated = await db.SaveChangesAsync();
                return rowsUpdated > 0;
            }
        }

        public static async Task<bool> DeleteAssessmentAsync(int assessmentId)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                Assessment assessment = await db.Assessments.FindAsync(assessmentId);

                db.Assessments.Remove(assessment);
                int rowsUpdated = await db.SaveChangesAsync();

                return rowsUpdated > 0;
            }
        }
    }
}
