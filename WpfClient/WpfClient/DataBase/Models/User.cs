using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WpfClient.ViewModels;

public enum Role { Manager, Intern }

namespace WpfClient.DataBase.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        [Required]
        public Person UserDetails { get; set; }

        public Role? Role { get; set; }
    }
}
