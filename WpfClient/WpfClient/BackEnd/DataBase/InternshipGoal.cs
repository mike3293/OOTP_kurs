namespace WpfClient.BackEnd
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InternshipGoal
    {
        public Guid Id { get; set; }

        [Required]
        public Internship Internship { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DeadlineDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public bool? IsCompleted { get; set; }
    }
}
