using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var people = new List<Person>
            {
                new Person("Alice", "Wonderland", "alice@contoso.com", new DateTime(1979, 11, 19)),
                new Person("Bob", "Mycroserft", "bob@contoso.com", new DateTime(1995, 03, 10)),
                new Person("Carol", "Reho", "carol@contoso.com", new DateTime(1986, 05, 26))
            };
            foreach(var person in people)
            {
                Console.WriteLine(person);
            }
            
            Func<List<Person>, int> calculateAverageAge =
			(List<Person> list) =>
			{   int totalAges = 0;
				foreach(var p in list)
				{
					totalAges += p.Age;
				}
				int averageAge = totalAges / list.Count;
				return averageAge;
			};

			Console.WriteLine("Average Age: {0}", calculateAverageAge(people));
            
        }
    }
}
