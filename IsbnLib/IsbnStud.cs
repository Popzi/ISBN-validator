using System;
using System.Linq;
using System.Text.RegularExpressions;

// ******************************************************************************
// ISBN References:                                                             *
// http://en.wikipedia.org/wiki/International_Standard_Book_Number              *
// https://en.wikipedia.org/wiki/Category:Articles_with_invalid_ISBNs           *
// https://en.wikipedia.org/wiki/Wikipedia:ISBN                                 *
// http://www.hahnlibrary.net/libraries/isbncalc.html                           *
// ISBN generator: https://generate.plus/en/number/isbn                         *
// ******************************************************************************

/*
ISBN (International Standard Book Number) is a unique number assigned to each book.
ISBN-10:
    • The number has 9 information digits and ends with 1 check digit. 
    • Assuming the digits are "abcdefghi-j" where j is the check digit. Then the check digit is computed by the following formula: 
j = ( [a b c d e f g h i] * [1 2 3 4 5 6 7 8 9] ) mod 11

ISBN-13:
    • The number has 12 information digits and ends with 1 check digit.
    • Assuming the digits are "abcdefghijkl-m" where m is the check digit. Then the check digit is computed by the following formula: 
m = ( [a b c d e f g h i j k l] * [1 3 1 3 1 3 1 3 1 3 1 3] ) mod 10 
*/

namespace IsbnLib
{
    /// <summary>
    /// ISBN Library with common methods
    /// </summary>
    public class IsbnStud
    {

        public const string err_null_string = "ISBN is null";
        public const string err_empty_string = "ISBN is empty";
        public const string err_nonvalid_string = "ISBN is not valid";

        /// <summary>
        /// This method will validate an ISBN 10 or ISBN 13 code depending on length
        /// </summary>
        /// <param name="isbn">code to validate</param>
        /// <returns>true if valid, otherwise false</returns>
        public static bool TryValidate(string isbn)
        {
#if DEBUG
            Console.WriteLine($"Tryvalidate-string input: {isbn}");
#endif
            bool result = false;
            if (isbn == null) return false;
            if (isbn == string.Empty) return false;
            /*           string pattern = @"(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)";
                       if (!Regex.IsMatch(isbn, pattern)) return false; */
            isbn = CleanIsbn(isbn);

            switch (isbn.Length)
            {
                case 10:
                    if (isbn.Any(char.IsDigit) || isbn.Contains("X"))
                    {
                          result = IsValidIsbn10(isbn);
                    }
                    else
                    {
                        result = false;
                    }
                    break;
                case 13:
                    if (isbn.Any(char.IsLetter))
                    {
                        result = false;
                    }
                    else
                    {
                        result = IsValidIsbn13(isbn);
                    }
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }


        /// <summary>
        /// This method will validate an ISBN 10 or ISBN 13 code
        /// </summary>
        /// <param name="isbn">code to validate</param>
        /// <returns>true if valid, otherwise false</returns>
        public static bool TryValidate(Int64 isbn)
        {
#if DEBUG
            Console.WriteLine($"Tryvalidate-long input: {isbn}");
#endif
            if (isbn.ToString().Length < 10)
                return TryValidate(isbn.ToString().PadLeft(10, '0'));
            else
                return TryValidate(isbn.ToString());
        }


        /// <summary>
        /// Linking to Amazon - How to Create the Shortest, Simplest Links to Your Books on Amazon.com
        /// https://kindlepreneur.com/amazon-search-url-isbn-ref/
        /// If your book has a 13-digit ISBN, then Amazon will use the 10 digit version of the ISBN.
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns>Amazon url if the parameter is valid, otherwise an Exception is thrown</returns>
        public static string CreateBookUrl(string baseUrl, string isbn)
        {
            if (isbn == null) return err_null_string;
            if (isbn == string.Empty) return err_empty_string;
            if (!TryValidate(isbn)) return err_nonvalid_string;

            if (isbn.Length > 10)
            {
                isbn = TryConvert(isbn);
            }
            if (baseUrl.Length < 10)
            {
                baseUrl = "https://amzn.com/" + isbn;
            }
            else
            {
                baseUrl = baseUrl + isbn;
            }

            return baseUrl;
        }

        /// <summary>
        /// Try Strips / cleans the isbn. Removes whitespaces hyphens
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns>stripped string</returns>
        public static string TryClean(string isbn)
        {
            return CleanIsbn(isbn);
        }

        /// <summary>
        /// Strips / cleans the isbn. Removes whitespaces hyphens
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns>stripped string</returns>
        private static string CleanIsbn(string isbn)
        {
            if (isbn == null) return err_null_string;
            if (isbn == string.Empty) return err_empty_string;
            return isbn.Replace("-", "").Replace(" ", "");
        }


        /// <summary>
        /// Validates the ISBN10 code
        /// </summary>
        /// <param name="isbn10">code to validate</param>
        /// <returns>true if valid isbn10, otherwise false</returns>
        private static bool IsValidIsbn10(string isbn10)
        {
            bool result = false;
            int sum = 0;

            // Using the alternative way of calculation
            for (int i = 0; i < 9; i++)
            {
                sum += Int32.Parse(isbn10[i].ToString()) * (i + 1);
            }

            // Getting the remainder or the checkdigit
            int remainder = sum % 11;
            char checkDigit = (remainder > 9) ? 'X' : remainder.ToString()[0];
            // Check if the checkdigit is same as the last number of ISBN 10 code
            result = (checkDigit.ToString() == isbn10[9].ToString());

#if DEBUG
            Console.WriteLine($"result-> {isbn10}");
#endif
            return result;
        }


        /// <summary>
        /// Validates the ISBN13 code
        /// </summary>
        /// <param name="isbn13">code to validate</param>
        /// <returns>true if valid isbn13, otherwise false</returns>
        private static bool IsValidIsbn13(string isbn13)
        {
            bool result = false;
            int sum = 0;
            // Comment Source: Wikipedia
            // The calculation of an ISBN-13 check digit begins with the first
            // 12 digits of the thirteen-digit ISBN (thus excluding the check digit itself).
            // Each digit, from left to right, is alternately multiplied by 1 or 3,
            // then those products are summed modulo 10 to give a value ranging from 0 to 9.
            // Subtracted from 10, that leaves a result from 1 to 10. A zero (0) replaces a
            // ten (10), so, in all cases, a single check digit results.
            for (int i = 0; i < 12; i++)
            {
                sum += Int32.Parse(isbn13[i].ToString()) * (i % 2 == 1 ? 3 : 1);
            }

            int remainder = sum % 10;
            int checkDigit = 10 - remainder;
            if (checkDigit == 10)
            {
                checkDigit = 0;
            }

            result = (checkDigit == int.Parse(isbn13[12].ToString()));

#if DEBUG
            Console.WriteLine($"result-> {isbn13}");
#endif
            return result;
        }
        /*
        private static bool IsValidIsbn10(string isbn)
        {
            if (!Regex.IsMatch(isbn, @"^\d{9}(\d|X)$"))
            {
                return false;
            }

            int checkSum = 0;

            for (int i = 0; i < isbn.Length - 1; i++)
            {
                checkSum += (isbn[i] - '0') * (10 - i);
            }

            int checkDigit = (11 - checkSum) % 11;

            if (checkDigit < 0)
            {
                checkDigit += 11;
            }

            int givenCheckDigit = isbn.Last() == 'X' ? 10 : isbn.Last() - '0';
#if DEBUG
            Console.WriteLine($"IsvalidIsbn13 Checkdigit = {checkDigit} - Given checkdigit: {givenCheckDigit}");
#endif
            return givenCheckDigit == checkDigit;
        }

        private static bool IsValidIsbn13(string isbn)
        {
            if (!Regex.IsMatch(isbn, @"^\d{13}$"))
            {
                return false;
            }

            int checkSum = 0;

            for (int i = 0; i < isbn.Length - 1; i++)
            {
                checkSum += (isbn[i] - '0') * (i % 2 == 0 ? 1 : 3);
            }

            int checkDigit = (10 - checkSum) % 10;

            if (checkDigit < 0)
            {
                checkDigit += 10;
            }

            int givenCheckDigit = isbn.Last() == 'X' ? 10 : isbn.Last() - '0';
#if DEBUG
            Console.WriteLine($"IsvalidIsbn13 Checkdigit = {checkDigit} - Given checkdigit: {givenCheckDigit}");
#endif
            return givenCheckDigit == checkDigit;
        }

        */

        /// <summary>
        /// This method will Convert an ISBN 10 to ISBN 13 or vice versa depending on length
        /// after it has done validation and error control
        /// </summary>
        /// <param name="isbn">code to convert</param>
        /// <returns>empty if the parameter is invalid, otherwise the converted isbn value</returns>
        public static string TryConvert(string isbn)
        {
            string result = string.Empty;
            isbn = CleanIsbn(isbn);
            switch (isbn.Length)
            {
                case 10:
                    result = ConvertTo13(isbn);
                    break;
                case 13:
                    result = ConvertTo10(isbn);
                    break;
            }

            return result;
        }


        /// <summary>
        /// This method will Convert an ISBN 10 to ISBN 13 or vice versa
        /// after it has done validation and error control
        /// </summary>
        /// <param name="isbn">code to validate</param>
        /// <returns>empty if the parameter is invalid, otherwise the converted isbn value</returns>
        public static string TryConvert(Int64 isbn)
        {
            string isbnstring = isbn.ToString();
            isbnstring = CleanIsbn(isbnstring);
             if (isbnstring.ToString().Length < 10)
                return TryConvert(isbnstring.PadLeft(10, '0'));
            else
                return TryConvert(isbnstring);
        }

        /// <summary>
        /// Converts an ISBN13 code to an ISBN10 code
        /// </summary>
        /// <param name="isbn13">code to convert</param>
        /// <returns>empty if the parameter is invalid, otherwise the converted isbn10 value</returns>
        private static string ConvertTo10(string isbn13)
        {
            string isbn10 = string.Empty;
            Int64 temp;

            if (Int64.TryParse(isbn13, out temp))
            {
                isbn10 = isbn13.Substring(3, 9);
                int sum = 0;
                for (int i = 0; i < 9; i++)
                    sum += Int32.Parse(isbn10[i].ToString()) * (i + 1);

                int result = sum % 11;
                char checkDigit = (result > 9) ? 'X' : result.ToString()[0];
                isbn10 += checkDigit;
            }
#if DEBUG
            Console.WriteLine($"{isbn13} -> {isbn10}");
#endif
            return isbn10;
        }


        /// <summary>
        /// Converts an ISBN10 code to an ISBN13 code
        /// </summary>
        /// <param name="isbn10">code to convert</param>
        /// <returns>empty if the parameter is invalid, otherwise the converted isbn13 value</returns>
        private static string ConvertTo13(string isbn10)
        {
            string isbn13 = string.Empty;
            Int64 temp;

            if (Int64.TryParse(isbn10.Substring(0, isbn10.Length - 1), out temp))
            {
                int result = 0;
                isbn13 = "978" + isbn10.Substring(0, 9);
                for (int i = 0; i < isbn13.Length; i++)
                    result += int.Parse(isbn13[i].ToString()) * ((i % 2 == 0) ? 1 : 3);

                int checkDigit = (10 - (result % 10)) % 10;
                isbn13 += checkDigit.ToString();
            }
#if DEBUG
            Console.WriteLine($"{isbn10} -> {isbn13}");
#endif
            return isbn13;
        }
    }
}