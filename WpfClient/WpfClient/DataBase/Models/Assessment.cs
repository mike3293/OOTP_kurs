using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfClient.DataBase.Models
{
    public class Assessment
    {
        public int Id { get; set; }

        [Required]
        public Internship Internship { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        public string Location { get; set; }

        public string Topic { get; set; }
    }
}
