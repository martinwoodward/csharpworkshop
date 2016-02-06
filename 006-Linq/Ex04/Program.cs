using System;
using System.Collections.Generic;
using System.Linq;

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

            foreach(var person in people.OrderBy(p => p.Age))
            {
                Console.WriteLine(person);
            }

			Console.WriteLine("Oldest: {0}", people.Max(p => p.Age));
            
        }
    }
}
