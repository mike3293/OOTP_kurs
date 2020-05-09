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
                return await db.Users.Where(u => u.Email == email).SingleOrDefaultAsync();
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

        public static bool CheckIfUserExistsByEmail(string email)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                return db.Users.Any(u => u.Email == email);
            }
        }
    }
}
