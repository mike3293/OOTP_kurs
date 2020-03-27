namespace WpfClient.BackEnd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Internship
    {
        public Guid Id { get; set; }

        public Person Intern { get; set; }

        public Person Manager { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public virtual List<InternshipGoal> InternshipGoals { get; set; }
    }
}
