using WpfClient.DataBase.Models;

namespace WpfClient.Helpers
{
    public static class InternshipExtension
    {
        public static string GetSearchData(this Internship internship)
        {
            if (internship.Intern != null)
            {
                Person intern = internship.Intern;
                return ($"{intern.FirstName} {intern.LastName}").ToLower();
            }

            return null;
        }
    }
    
}
