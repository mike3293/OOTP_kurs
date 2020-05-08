using System.ComponentModel.DataAnnotations;

namespace WpfClient.DataBase.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public virtual byte[] Image { get; set; }
    }
}
