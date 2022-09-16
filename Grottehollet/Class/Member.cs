using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Grottehollet.Class
{
    public class Member
    {
        public DBConnection DB;
        public Member(DBConnection db)
        {
            DB = db;
        }
        public string Name;
        public string Adress;
        public string City;
        public string EmergencyContact;
        public string Nickname;
        private string Code;

        public void CreateMember(string name, string adress, string city) 
        {
            Random rnd = new Random();
            StringBuilder stringBuilder = new StringBuilder();
            string pool = "abcdefghijklmnopqrstuvwxyz1234567890";
            for (int i = 0; i < 9; i++)
            {
                var c = pool[rnd.Next(0, pool.Length)];
                stringBuilder.Append(c);
            }
            Code = stringBuilder.ToString();
            DB.cnn.Open();
            DB.cmd = new SqlCommand("SELECT * FROM Medlemmer WHERE Medlemskode=@Medlemskode", DB.cnn);
            DB.cmd.Parameters.AddWithValue("Medlemskode", stringBuilder.ToString());
            DB.reader = DB.cmd.ExecuteReader();

            if (DB.reader.Read())
            {
                while (stringBuilder.ToString() == DB.reader["Medlemskode"].ToString())
                {
                    
                    for (int i = 0; i < 8; i++)
                    {
                        var c = pool[rnd.Next(0, pool.Length)];
                        stringBuilder.Append(c);
                    }
                    Code = stringBuilder.ToString();
                    DB.cmd = new SqlCommand("SELECT * FROM Medlemmer WHERE Medlemskode=@Medlemskode", DB.cnn);
                    DB.cmd.Parameters.AddWithValue("Medlemskode", Code);
                    DB.reader = DB.cmd.ExecuteReader();
                }
                DB.cmd = new SqlCommand("INSERT INTO Medlemmer(Navn, Adresse, City, Medlemskode) VALUES(Navn=@Name, Adresse=@Adress, City=@City, Medlemskode=@Code)", DB.cnn);
                DB.cmd.Parameters.AddWithValue("Name", name);
                DB.cmd.Parameters.AddWithValue("Adress", adress);
                DB.cmd.Parameters.AddWithValue("City", city);
                DB.cmd.Parameters.AddWithValue("Code", Code);
            }
            
            DB.cnn.Close();
            return;
        }

        public void SeeProfile() { }

        public void UpdateProfile() { }
    }
}
