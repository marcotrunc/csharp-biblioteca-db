using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace csharp_biblioteca_db
{
    internal class db
    {
        private static string stringaDiConnessione =
"Data Source=localhost;Initial Catalog=biblioteca-db;Integrated Security=True;Pooling=False";

        private static SqlConnection Connect()
        {
            SqlConnection conn = new SqlConnection(stringaDiConnessione);
            try 
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return conn;
        }
        internal static int scaffaleAdd(string s1)
        {

            //Devo collegarmi e inviare un comando per inserire uno scaffale
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to Database");
            }

            //Inserisco lo scaffale nella tabella scaffali
            var cmd = String.Format("insert into Scaffale (scaffale) values ('{0}')", s1);

            using (SqlCommand insert = new SqlCommand(cmd, conn))
            {
                try
                {

                    var numrows = insert.ExecuteNonQuery();
                    return numrows;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        internal static List<string> scaffaliGet()
        {
            List<string> ls = new List<string>();

            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to Database");
            }

            //Inserisco lo scaffale nella tabella scaffali
            var cmd = String.Format("select Scaffale from Scaffale");

            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
               using(SqlDataReader reader = select.ExecuteReader())
                {
                    while (reader.Read()) 
                    { 
                        ls.Add(reader.GetString(0));    
                    }
                }
            }
            conn.Close();
            return ls;
        }
        internal static List<Tuple<int, string, string, string, string, string>> documentiGet()
        {
            var ld = new List<Tuple<int, string, string, string, string, string>>();
            var conn = Connect();
            if (conn == null)
                throw new Exception("Unable to connect to the dabatase");
            var cmd = String.Format("select codice, Titolo, Settore, Stato, Tipo, Scaffale from Documenti");  //Li prendo tutti
            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var data = new Tuple<Int32, string, string, string, string, string>(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5));
                        ld.Add(data);
                    }
                }
            }
            conn.Close();
            return ld;
        }
    }
}

