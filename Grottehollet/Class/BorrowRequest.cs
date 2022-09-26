using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Grottehollet.Class
{
    public class BorrowRequest
    {
        public string Type { get; set; }
        public string Titel { get; set; }
        public string TagsOrAuthor { get; set; }
        public bool Borrowing { get; set; }
        public BitmapImage Image { get; set; }
        public string Member { get; set; }

        public BorrowRequest()
        {

        }
        public BorrowRequest(string type, string titel, string tagsorauthor, string member, bool borrowing)
        {
            if (type == "Brætspil")
            {
                Type = type;
                Image = new BitmapImage(new Uri("pack://application:,,,/Grottehollet;component/Resources/controller.png"));
            }
            else if (type == "Bog") 
            {
                Type = type;
            }

            TagsOrAuthor = tagsorauthor;
            Titel = titel;
            Member = member;
            Borrowing = borrowing;
        }
        public override string ToString()
        {
            return $"{Type},{Titel},{TagsOrAuthor},{Member}";
        }

    }
}
