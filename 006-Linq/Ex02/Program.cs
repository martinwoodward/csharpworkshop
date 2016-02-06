using System;
using System.Linq;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string[] strings =
             {
                "A penny saved is a penny earned.",
                "The early bird catches the worm.",
                "The pen is mightier than the sword."
            };

            // Get the words containing an E
            var eWords =
                from sentence in strings
                let words = sentence.Split(' ')
                from word in words
                let w = word.ToLower()
                where w.IndexOf('e') >= 0
                select word;

			// Sort by word length
			eWords = eWords.OrderBy(w => w.Length);

            // Display them.
            foreach (var w in eWords)
            {
                Console.WriteLine($"\"{w}\"");
            }
        }
    }
}
