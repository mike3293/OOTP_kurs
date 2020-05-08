using System.ComponentModel.DataAnnotations;

public enum Role { Manager, Mentor, Intern }

namespace WpfClient.DataBase.Models
{
    public class User
    {
        // TODO
        //public User(string email, string hashedPassword)
        //{
        //    Email = email;
        //    HashedPassword = hashedPassword;
        //}

        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        [Required]
        public virtual Person UserDetails { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
