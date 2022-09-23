using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Grottehollet.Class
{
    public class BoardGame
    {
        public string BoardGameType { get; set; }
        public string Titel { get; set; }
        public string Tags { get; set; }
        public bool Borrowed { get; set; }
        public BitmapImage Image { get; set; } = new BitmapImage(new Uri("pack://application:,,,/Grottehollet;component/Resources/controller.png"));

        public BoardGame()
        {

        }

        public BoardGame(string boardgametype, string titel, string tags)
        {
            BoardGameType = boardgametype;
            Titel = titel;
            Tags = tags;
        }

        public override string ToString()
        {
            return $"{BoardGameType},{Titel},{Tags}";
        }
    }
}
