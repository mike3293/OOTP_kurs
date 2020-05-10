using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.DataBase;
using WpfClient.DataBase.Models;

namespace WpfClient.Services
{
    public class PeopleService
    {
        public static async Task<bool> UpdatePersonAsync(Person person)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                Person personToUpdate = db.People.Find(person.Id);
                personToUpdate.FirstName = person.FirstName;
                personToUpdate.LastName = person.LastName;

                int rowsUpdated = await db.SaveChangesAsync();

                return rowsUpdated > 0;
            }
        }

        public static async Task<bool> UpdatePersonImageAsync(int personId, byte[] img)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                Person personToUpdate = db.People.Find(personId);
                personToUpdate.Image = img;

                int rowsUpdated = await db.SaveChangesAsync();

                return rowsUpdated > 0;
            }
        }
    }
}
