using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Collections.ObjectModel;

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
        public string Number;
        public string EmergencyContact;
        public string Nickname;
        public string Code;
        public string Admin;

        public void CreateMember(string name, string adress, string city)
        {
            Random rnd = new Random();
            StringBuilder stringBuilder = new StringBuilder();
            string pool = "abcdefghijklmnopqrstuvwxyz1234567890";
            for (int i = 0; i < 8; i++)
            {
                var c = pool[rnd.Next(0, pool.Length)];
                stringBuilder.Append(c);
            }
            Code = stringBuilder.ToString();

            DB.cnn.Open();
            DB.cmd = new SqlCommand("SELECT * FROM Medlemmer WHERE Medlemskode=@Medlemskode", DB.cnn);
            DB.cmd.Parameters.AddWithValue("Medlemskode", Code);
            DB.reader = DB.cmd.ExecuteReader();

            if (DB.reader.Read())
            {
                while (Code == DB.reader["Medlemskode"].ToString())
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
            }
            DB.cnn.Close();
            DB.cnn.Open();

            DB.cmd = new SqlCommand("CreateMedlemV3", DB.cnn);
            DB.cmd.CommandType = System.Data.CommandType.StoredProcedure;

            DB.cmd.Parameters.AddWithValue("@Navn", name);
            DB.cmd.Parameters.AddWithValue("@Adresse", adress);
            DB.cmd.Parameters.AddWithValue("@City", city);
            DB.cmd.Parameters.AddWithValue("@Medlemskode", Code);

            DB.cmd.ExecuteNonQuery();
            DB.cnn.Close();
        }

        public void SeeProfile(string membercode)
        {
            DB.cnn.Close();
            DB.cnn.Open();
            DB.cmd = new SqlCommand("SELECT * FROM Medlemmer WHERE Medlemskode=@Medlemskode", DB.cnn);
            DB.cmd.Parameters.AddWithValue("Medlemskode", membercode);
            DB.reader = DB.cmd.ExecuteReader();

            if (DB.reader.Read())
            {
                Name = DB.reader["Navn"].ToString();
                Adress = DB.reader["Adresse"].ToString();
                City = DB.reader["City"].ToString();
                Number = DB.reader["Tlf"].ToString();
                EmergencyContact = DB.reader["Nødkontakt"].ToString();
                Nickname = DB.reader["Kaldenavn"].ToString();
                Code = membercode;
                Admin = DB.reader["AdminRole"].ToString();
            }
        }

        public void UpdateProfile(string membercode, string PFNickname, string PFNumber, string PFemergencyName, string PFemergencyPhone, string PFAdress, string PFCity)
        {
            if (PFAdress != "" && PFCity != "")
            {
                DB.cnn.Open();
                DB.cmd = new SqlCommand("UPDATE Medlemmer SET Kaldenavn = @Nickname, Tlf = @Number, Adresse = @Adress, City = @city WHERE Medlemskode=@Medlemskode", DB.cnn);
                DB.cmd.Parameters.AddWithValue("@Medlemskode", membercode);
                DB.cmd.Parameters.AddWithValue("@Nickname", PFNickname);
                DB.cmd.Parameters.AddWithValue("@Number", PFNumber);
                DB.cmd.Parameters.AddWithValue("@Adress", PFAdress);
                DB.cmd.Parameters.AddWithValue("@City", PFCity);

                DB.cmd.ExecuteNonQuery();
                DB.cnn.Close();
            }
        }

        public ObservableCollection<BorrowRequest> borrowRequests = new ObservableCollection<BorrowRequest>();
        public static object Lockject = new object();

        public ObservableCollection<BorrowRequest> RequestForBorrowing(string forborrowing)
        {
            string[] splitted = forborrowing.Split(',');
            BindingOperations.EnableCollectionSynchronization(borrowRequests, Lockject);

            lock (Lockject)
            {
                borrowRequests.Add(new BorrowRequest(splitted[0].ToString(), splitted[1].ToString(), splitted[2].ToString(), Name, false));
                return borrowRequests;
            }
        }

        public List<string> ConRejList = new List<string>();
        public void PlaceRequest(List<string> requestborrowing) 
        {
            foreach (var item in requestborrowing)
            {
                string[] splitted = item.Split(',');
                DB.cnn.Open();
                DB.cmd = new SqlCommand("UPDATE Brætspil SET Udlånes=@Udlånes WHERE Navn=@Titel", DB.cnn);
                DB.cmd.Parameters.AddWithValue("@Titel", splitted[1].ToString());
                DB.cmd.Parameters.AddWithValue("@Udlånes", true);
                DB.cmd.ExecuteNonQuery();
                DB.cnn.Close();

            }
            
        }
        #region Admin
        public ObservableCollection<BorrowRequest> ViewRequests() 
        {
            return borrowRequests;
        }
        public void ConfirmRequest(string titel) 
        {
            string[] splitted = titel.Split(',');
            DB.cnn.Open();
            if (splitted[0] == "Brætspil")
            {
                DB.cmd = new SqlCommand("GodkendUdlån_Brætspil", DB.cnn);
                DB.cmd.CommandType = System.Data.CommandType.StoredProcedure;
                DB.cmd.Parameters.AddWithValue("@Title", splitted[1].ToString());
            }
            else if (splitted[0] == "Bog") 
            {
                DB.cmd = new SqlCommand("GodkendUdlån_Bog", DB.cnn);
                DB.cmd.CommandType = System.Data.CommandType.StoredProcedure;
                DB.cmd.Parameters.AddWithValue("@Title", splitted[1].ToString());
            }
            DB.cmd.ExecuteNonQuery();
            DB.cnn.Close();
            DB.cnn.Open();
            if (splitted[0] == "Brætspil")
            {
                DB.cmd = new SqlCommand("CreateBrætspilUdlån", DB.cnn);
                DB.cmd.CommandType = System.Data.CommandType.StoredProcedure;
                DB.cmd.Parameters.AddWithValue("@Titel", splitted[1].ToString());
                DB.cmd.Parameters.AddWithValue("@Navn", splitted[3].ToString());
            }
            //else if (splitted[0] == "Bog")
            //{
            //    DB.cmd = new SqlCommand("GodkendUdlån_Bog", DB.cnn);
            //    DB.cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //    DB.cmd.Parameters.AddWithValue("@titel", splitted[1]);
            //}
            DB.cmd.ExecuteNonQuery();
            DB.cnn.Close();
        }

        public void RejectRequest(string titel) 
        {
            string[] splitted = titel.Split(',');
            DB.cnn.Open();
            DB.cmd = new SqlCommand("UPDATE Brætspil SET Udlånes=@udlånes WHERE Navn=@titel", DB.cnn);
            DB.cmd.Parameters.AddWithValue("@titel", splitted[1].ToString());
            DB.cmd.Parameters.AddWithValue("@udlånes", false);
            DB.cmd.ExecuteNonQuery();
            DB.cnn.Close();
        }
        #endregion

        public void Logout() 
        {
            Name = null;
            Adress = null;
            City = null;
            EmergencyContact = null;
            Nickname = null;
            Code = null;
        }
    }
}
