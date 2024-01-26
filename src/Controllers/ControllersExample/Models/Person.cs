namespace ControllersExample.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }

        public Person()
        {
            
        }

        public Person(Guid id, string firstName, string lastName, int age)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName; 
            Age = age;
        }
    }
}
