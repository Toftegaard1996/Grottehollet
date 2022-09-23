using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Grottehollet.Class
{
    public class DBConnection
    {
        public string ConnectionString = "Data Source=DESKTOP-GE1IOSV;Initial Catalog=GrotteholletDB;Integrated Security=True";
        public string Name;
        public SqlConnection cnn;
        public SqlDataReader reader;
        public SqlCommand cmd;
        public List<BoardGame> boardGames;

        public DBConnection()
        {
            cnn = new SqlConnection(ConnectionString);
        }

        public void SeeBookList() 
        {
            List<Book> books = new List<Book>();
            cnn.Open();
            cmd = new SqlCommand("SELECT * FROM Bog_view", cnn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                books.Add(new Book("Bog", reader["Title"].ToString()));
            }
            cnn.Close();
        }

        public List<BoardGame> SeeBoardGameList()
        {
            //The og of all time
            boardGames = new List<BoardGame>();
            cnn.Open();
            cmd = new SqlCommand("SELECT * FROM Spil_tags_view", cnn);
            reader = cmd.ExecuteReader();
            string GameGenre = "";
            while (reader.Read())
            {
                boardGames.Add(new BoardGame("Brætspil", reader["Brætspil"].ToString(), reader["Genre"].ToString() + ", "));
            }




            List<BoardGame> DineNyeOgShinyListeDuVedTingDuLigeHarLAvetDerIKKEVarDerFør = new List<BoardGame>();

            for (int i = 0; i < boardGames.Count -1; i++)
            {
                for (int j = i+1; j < boardGames.Count; j++)
                {
                    
                    if (boardGames[i].Titel == boardGames[j].Titel)
                    {
                        GameGenre += boardGames[i].Tags;
                        GameGenre += boardGames[j].Tags;
                        DineNyeOgShinyListeDuVedTingDuLigeHarLAvetDerIKKEVarDerFør.Add(new BoardGame("Brætspil", "Titel: " +  boardGames[i].Titel, " | Genre: " + GameGenre));
                        GameGenre = "";
                    }
                    else
                    {
                        if (!DineNyeOgShinyListeDuVedTingDuLigeHarLAvetDerIKKEVarDerFør.Any(x=>x.Titel.Contains(boardGames[i].Titel)))
                        {
                            DineNyeOgShinyListeDuVedTingDuLigeHarLAvetDerIKKEVarDerFør.Add(new BoardGame("Brætspil", "Titel: " + boardGames[i].Titel, " | Genre: " + boardGames[i].Tags));
                        }
                       
                    }
                }
            }
            if (!DineNyeOgShinyListeDuVedTingDuLigeHarLAvetDerIKKEVarDerFør.Any(x => x.Titel.Contains(boardGames[boardGames.Count-1].Titel)))
            {
                DineNyeOgShinyListeDuVedTingDuLigeHarLAvetDerIKKEVarDerFør.Add(new BoardGame("Brætspil", "Titel: " + boardGames[boardGames.Count - 1].Titel, " | Genre: " + boardGames[boardGames.Count - 1].Tags));

            }

            cnn.Close();
            return DineNyeOgShinyListeDuVedTingDuLigeHarLAvetDerIKKEVarDerFør;
        }

    }
}
