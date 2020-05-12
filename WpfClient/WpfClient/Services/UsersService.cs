using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WpfClient.DataBase;
using WpfClient.DataBase.Models;

namespace WpfClient.Services
{
    public class UsersService
    {
        public static async Task<User> GetUserByEmailAsync(string email)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                return await db.Users.Where(u => u.Email == email).Include(u => u.UserDetails).SingleOrDefaultAsync();
            }
        }

        public static async Task<User> GetUserByPersonIdAsync(int personId)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                return await db.Users.Where(u => u.UserDetails.Id == personId).SingleOrDefaultAsync();
            }
        }

        public static async Task<bool> AddUserAsync(User user)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                db.Users.Add(user);
                int rowsUpdated = await db.SaveChangesAsync();
                return rowsUpdated > 0;
            }
        }

        public static async Task<bool> CheckIfUserExistsByEmailAsync(string email)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                return await db.Users.AnyAsync(u => u.Email == email);
            }
        }

        public static async Task<List<User>> GetUsersWithoutRolesAsync()
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                return await db.Users.Where(u => u.Role == null).Include(u => u.UserDetails).ToListAsync();
            }
        }

        public static async Task<bool> UpdateUserAsync(User user)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                User userToUpdate = db.Users.Find(user.Id);
                userToUpdate.Role = user.Role;

                Person personToUpdate = db.People.Find(user.UserDetails.Id);
                personToUpdate.FirstName = user.UserDetails.FirstName;
                personToUpdate.LastName = user.UserDetails.LastName;

                int rowsUpdated = await db.SaveChangesAsync();

                return rowsUpdated > 0;
            }
        }

        public static async Task<bool> DeleteUserAsync(User user)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                Person personToDelete= db.People.Find(user.UserDetails.Id);
                db.People.Remove(personToDelete);

                User userToDelete = db.Users.Find(user.Id);
                db.Users.Remove(userToDelete);

                int rowsUpdated = await db.SaveChangesAsync();

                return rowsUpdated >= 2;
            }
        }
    }
}
