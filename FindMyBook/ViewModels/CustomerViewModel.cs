using System;

namespace FindMyBook.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public string ProfileImage { get; set; }
    }
}