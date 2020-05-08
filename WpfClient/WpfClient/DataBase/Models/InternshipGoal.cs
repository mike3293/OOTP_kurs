using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfClient.DataBase.Models
{
    public class InternshipGoal
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DeadlineDate { get; set; }

        [Required]
        public string Name { get; set; }

        public bool? IsCompleted { get; set; }
    }
}
