using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfClient.DataBase.Models
{
    public class Internship
    {
        public int Id { get; set; }

        [Required]
        public Person Intern { get; set; }

        public virtual Person Manager { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public bool? IsCompleted { get; set; }

        public virtual List<InternshipGoal> InternshipGoals { get; set; }
    }
}
