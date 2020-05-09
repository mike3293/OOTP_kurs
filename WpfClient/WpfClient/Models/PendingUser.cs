namespace WpfClient.Models
{
    public class PendingUser
    {
        public PendingUser(int id,int detailsId, string email, string firstName, string lastName)
        {
            Id = id;
            DetailsId = detailsId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public int Id { get; set; }
        public int DetailsId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
