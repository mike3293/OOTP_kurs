using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfClient.BackEnd.DataBase
{
    internal class Assessment
    {
        public Guid Id { get; set; }

        [Required]
        public Internship Internship { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public string Location { get; set; }

        public string Topic { get; set; }
    }
}
