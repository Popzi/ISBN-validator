using System.Collections.Generic;

namespace IsbnLib
{
    public interface ILibrary<T>
    {
        bool LendBook(T isbn);
        bool ReturnBook(T isbn);

        List<T> SearchBooks(T sql);
    }
}
