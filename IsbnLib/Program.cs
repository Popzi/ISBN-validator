using System;

namespace IsbnLib
{
    class Program
    {
        static void Main(string[] args)
        {
            // valid isbn but may not point to an actual book
            string[] ValidIsbns = new string[] {
                "1617290890",
                "1590593006",
                "9783878313793",
                "978-3-16-148410-0",
                "0-8436-1072-7",
                "15-84885-40-8",
                "0-8436-1072-7",
                "316148410X",
                "978-158488-540-5",
                "15 8488 5408",
            };

            string[] NonValidIsbns = new string[] {
                null,
                "",
                " ",
                "2.5",
                "ttt",
                //"0-8436-1072-7", VALID
                "123-45-567-8-9",
                "3878313798",
                "0-14-020652-4",
                "970-3-16-148410-0",
                "978-3-16-148410-3",
                "abcdefghix",
                "abcdefghixabc",
                "abcdefghi1",
                "abcdefghixab1",
                "0 - 14 - 020652 - 4",
            };


            foreach (var isbn in ValidIsbns)
            {
                try
                {
                    Console.WriteLine($"{isbn}: IsValid ISBN-10/13? {IsbnStud.TryValidate(isbn)}");
                    Console.WriteLine($"BookUrl: {IsbnStud.CreateBookUrl("", isbn)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }


            Console.WriteLine(Environment.NewLine);


            foreach (var isbn in NonValidIsbns)
            {
                try
                {
                    Console.WriteLine($"{isbn}: IsValid ISBN-10/13? {IsbnStud.TryValidate(isbn)}");
                    Console.WriteLine($"BookUrl: {IsbnStud.CreateBookUrl("", isbn)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }


            Console.WriteLine(Environment.NewLine);


            foreach (var isbn in ValidIsbns)
            {
                try
                {
                    Console.WriteLine($"Convert valid ISBN-10/13: {isbn} To ISBN-13/10: " +
                        $"{IsbnStud.TryValidate(IsbnStud.TryConvert(isbn))} " + Environment.NewLine);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }

            foreach (var isbn in NonValidIsbns)
            {
                try
                {
                    Console.WriteLine($"Convert nonvalid ISBN-10/13: {isbn}: To ISBN-13/10: " +
                        $"{IsbnStud.TryValidate(IsbnStud.TryConvert(isbn))} " + Environment.NewLine);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e}");
                }
            }


            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey(true);
        }
    }
}