using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grottehollet.Class
{
    public class Book
    {
        public string BookType;
        public string Titel;
        public string Author;
        public bool Borrowed;

        public Book()
        {

        }

        public Book(string booktype, string titel)
        {
            BookType = booktype;
            Titel = titel;
        }
    }
}
