using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.DataBase;
using WpfClient.DataBase.Models;

namespace WpfClient.Services
{
    public class UsersService
    {
        public static User GetUserByEmail(string email)
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                return db.Users.Where(u => u.Email == email).Single();
            }
        }
    }
}
