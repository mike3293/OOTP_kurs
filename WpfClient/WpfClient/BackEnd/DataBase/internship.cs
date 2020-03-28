using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfClient.BackEnd.DataBase
{
    public partial class Internship
    {
        public Guid Id { get; set; }

        [Required]
        public Person Intern { get; set; }

        public Person Manager { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public virtual List<InternshipGoal> InternshipGoals { get; set; }
    }
}
