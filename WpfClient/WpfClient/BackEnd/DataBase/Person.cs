namespace WpfClient.BackEnd.DataBase
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public enum Role { Manager, Mentor, Intern }

    public partial class Person
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
