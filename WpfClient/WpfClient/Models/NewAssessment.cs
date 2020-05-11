using System;

namespace WpfClient.Models
{
    public class NewAssessment
    {
        public DateTime Date { get; set; }

        public DateTime Time { get; set; }

        public string Location { get; set; }

        public string Topic { get; set; }
    }
}
