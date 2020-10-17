using System.Collections.Generic;

namespace IsbnLib
{

    public class LibraryDbStud : ILibrary<string>
    {
        public bool LendBook(string isbn)
        {
            throw new System.NotImplementedException();
        }

        public bool ReturnBook(string isbn)
        {
            throw new System.NotImplementedException();
        }

        public List<string> SearchBooks(string sql)
        {
            throw new System.NotImplementedException();
        }
    }

}
