using System;

namespace ConsoleApplication
{
    public class Person
    {
        public Person(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth;
        }

        public override string ToString()
        {
            return $"{FullName} ({UserId})";
        }
        
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public int Age
        {
            get
            {
                return ( (Int32.Parse(DateTime.Today.ToString("yyyyMMdd")) - 
                          Int32.Parse(DateOfBirth.ToString("yyyyMMdd"))) / 10000);
            }
        }
        
        public bool Adult => Age >= 18;
        
        private string userId;
        public string UserId
        {
            get
            {
                if (String.IsNullOrWhiteSpace(userId))
                {
                  userId = (FirstName.Substring(0, 1) 
                    + LastName + DateOfBirth.ToString("yy")).ToLowerInvariant();
                }
                return userId;
            }
            set
            {
                userId = value;
            }
        }

    }
}
