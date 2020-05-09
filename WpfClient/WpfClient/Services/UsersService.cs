using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WpfClient.DataBase;
using WpfClient.DataBase.Models;

namespace WpfClient.Services
{
    public class UsersService
    {
        public static async Task<User> GetUserByEmail(string email)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                return await db.Users.Where(u => u.Email == email).SingleOrDefaultAsync();
            }
        }
    }
}
