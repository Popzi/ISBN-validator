using Microsoft.VisualStudio.TestTools.UnitTesting;
using IsbnLib;
using System;

namespace isbnLib.Test
{
    [TestClass]
    public class isbnUnitTests
    {

        [TestMethod]
        public void TestConvertIsbn10ToIsbn13Int()
        {
            long isbn = 0 - 5861 - 8503 - 8;
            bool converted = IsbnStud.TryValidate(IsbnStud.TryConvert(isbn));
            Assert.IsTrue(converted);
        }

        [TestMethod]
        public void TestConvertIsbn10ToIsbn13String()
        {
            string isbn = "0 - 5861 - 8503 - 8";
            bool converted = IsbnStud.TryValidate(IsbnStud.TryConvert(isbn));
            Assert.IsTrue(converted);
        }

        [TestMethod]
        public void TestConvertIsbn13ToIsbn10Int()
        {
            long isbn = 978 - 0 - 586 - 18503 - 2;
            bool converted = IsbnStud.TryValidate(IsbnStud.TryConvert(isbn));
            Assert.IsTrue(converted);
        }

        [TestMethod]
        public void TestConvertIsbn13ToIsbn10String()
        {
            string isbn = "978 - 0 - 586 - 18503 - 2";
            bool converted = IsbnStud.TryValidate(IsbnStud.TryConvert(isbn));
            Assert.IsTrue(converted);
        }

        [TestMethod]
        public void TestCreateBookURL()
        {
            string isbn = "978-1720890713";
            isbn = IsbnStud.TryClean(isbn);
            string url = IsbnStud.CreateBookUrl("", isbn);
            Assert.AreEqual("https://amzn.com/1720890714",url);
        }

        [TestMethod]
        public void TestEmptyIsbnString()
        {
            string isbn = "";
            string url = IsbnStud.CreateBookUrl("", isbn);
            Assert.AreEqual("ISBN is empty", url);
        }

        [TestMethod]
        public void TestMalformedIsbnString()
        {
            string isbn = "KAKMONSTREN ATTACKERAR!";
            string url = IsbnStud.CreateBookUrl("", isbn);
            Assert.AreEqual("ISBN is not valid", url);
        }

        [TestMethod]
        public void TestNullIsbnString()
        {
            string isbn = null;
            string url = IsbnStud.CreateBookUrl("", isbn);
            Assert.AreEqual("ISBN is null", url);
        }

        [TestMethod]
        public void TestValidIsbn10Int()
        {
            long isbn = 0586185038;
            bool validisbn10 = IsbnStud.TryValidate(isbn);
            Assert.IsTrue(validisbn10);
        }

        [TestMethod]
        public void TestEmptyIsbn10String()
        {
            bool validisbn10 = IsbnStud.TryValidate(string.Empty);
            Assert.IsFalse(validisbn10);
        }

        [TestMethod]
        public void TestValidIsbn13Int()
        {
            long isbn = 9780586185032;
            bool validisbn13 = IsbnStud.TryValidate(isbn);
            Assert.IsTrue(validisbn13);
        }

        [TestMethod]
        public void TestEmptyIsbn13String()
        {
            bool validisbn10 = IsbnStud.TryValidate(string.Empty);
            Assert.IsFalse(validisbn10);
        }

        [TestMethod]
        public void TestLoopValidISBNStrings()
        {
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

            foreach (var isbn in ValidIsbns)
            {               
            Assert.IsTrue(IsbnStud.TryValidate(isbn));                
            }
        }

        [TestMethod]
        public void TestLoopInValidISBNStrings()
        {
            string[] NonValidIsbns = new string[] {
                null,
                "",
                " ",
                "2.5",
                "ttt",
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
            int counter = 0;
            foreach (var isbn in NonValidIsbns)
            {
#if DEBUG
                Console.WriteLine($"Sending invalid line: {isbn} - counter: {counter}");
#endif
                Assert.IsFalse(IsbnStud.TryValidate(isbn));
                counter++;
            }
        }
    }
}
