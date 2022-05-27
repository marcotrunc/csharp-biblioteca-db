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

        //Aprire la connessione
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
        //Ricerca per autore
        internal static List<Tuple<string, string, string, string, string, string>> CercaPerAutore(string sNomeCerc, string sCognomeCerc)
        {
            var lDoc = new List<Tuple<string, string, string, string, string, string>>();
            var conn = Connect();

            if (conn == null)
            {
                throw new Exception("Unable to connect to database");
            }
            var cmd = String.Format(@"SELECT Documenti.Titolo, Documenti.Settore, Documenti.Scaffale, Documenti.Stato, Autori.Cognome, Autori.Nome
                        FROM Documenti
                        INNER JOIN Autori_documenti ON Documenti.codice = Autori_documenti.codice_documento 
                        INNER JOIN Autori ON Autori.codice = Autori_Documenti.codice_autore
                        WHERE Autori.Nome = '{0}' AND Autori.Cognome = '{1}'",sNomeCerc, sCognomeCerc);
            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    string sValoriRicercati= "";
                    while (reader.Read())
                    {
                        var data = new Tuple<string, string, string, string, string, string>(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5));
                        lDoc.Add(data);
                    }
                }
            }
            string sPrint = "";
            foreach(Tuple<string, string, string, string, string, string> t in lDoc)
            {
                sPrint += @"Il Titolo è:" + "\n" + t.Item1 + " \n" + "Il Genere è:\n" + t.Item2 + "\n" + "Numero Scaffale:\n" + t.Item3 + "\n" +
                    "Status:\n" + t.Item4 + "\n" + "Cognome:\n" + t.Item5 + "\n" + "Nome:\n" + t.Item6 + "\n";
            }
            Console.WriteLine(sPrint);
            conn.Close();
            return lDoc;
        }
        //Aggiungi Scaffale
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
        // Comando SQL
        internal static bool DoSql(SqlConnection conn, string sql)
        {
            using (SqlCommand sqlCmd = new SqlCommand(sql, conn))
            {
                try
                {
                    var numrows = sqlCmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    conn.Close();
                    return false;
                }
            }
            return true;
        }
        //Aggiungere Libro
        internal static int libroAdd(Libro libro, List<Autore> lAutori) 
        {
            //mi collego al db
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to database");
            }
            var ok = DoSql(conn, "begin transaction");
            if (!ok)
            {
                throw new Exception("Errore in begin transaction");
                conn.Close();
            }
            //comando per inserire un nuovo documento
            var cmd = string.Format(@"insert into dbo.Documenti(codice,Titolo,Settore,Stato,Tipo,Scaffale) 
                        VALUES({0}, '{1}', '{2}', '{3}', 'Libro', '{4}')", libro.Codice, libro.Titolo, libro.Settore, libro.Stato.ToString(), libro.Scaffale.Numero);
            using (SqlCommand insert = new SqlCommand(cmd, conn))
            {
                try
                {
                    var numrows = insert.ExecuteNonQuery();
                    if (numrows != 1)
                    {
                        conn.Close();
                        throw new Exception("Valore di ritorno errato");
                    }    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //DoSql(conn, "rollback transaction");
                    //conn.Close();
                    //return 0;
                }
            }
            var cmd_1 = string.Format(@"insert into dbo.Libri(Codice,NumPagine) 
                        VALUES('{0}',{1})", libro.Codice, libro.NumeroPagine);
            using (SqlCommand insert = new SqlCommand(cmd_1, conn))
            {
                try
                {
                    var numrows = insert.ExecuteNonQuery();
                    if (numrows != 1)
                    {
                        throw new Exception("Valore di ritorno errato seconda query");
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //DoSql(conn, "rollback transaction");
                    //conn.Close();
                    //return 0;
                }
            }

            foreach (Autore autore in lAutori)
            {
                long lCodiceAutore = 0;
                int iInsertFlag = 0;
                cmd = String.Format("Select codice From Autori where Nome='{0}' and Cognome='{1}' and mail='{2}';", autore.Nome, autore.Cognome, autore.sMail);
                // Verifico che nella tebella Autori esiste già un Autore
                using (SqlCommand query = new SqlCommand(cmd, conn))
                {
                    using (SqlDataReader reader = query.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lCodiceAutore = reader.GetInt64(0);
                            Console.WriteLine("Codice Autore {0} già esistente",lCodiceAutore);
                        }
                        else
                        {
                            lCodiceAutore = autore.iCodiceAutore;
                            iInsertFlag = 1;
                        }
                        reader.Close();
                    }
                }
                
                //Se l'autore non è già inserito, lo registro nella tebella Autori
                if(iInsertFlag == 1)
                {
                    var cmd2 = String.Format("INSERT INTO Autori(codice,Nome, Cognome, mail) values ('{0}', '{1}','{2}','{3}')",
                            autore.iCodiceAutore, autore.Nome, autore.Cognome, autore.sMail);

                    using (SqlCommand insert = new SqlCommand(cmd2, conn))
                    {
                        try
                        {

                            var numrows = insert.ExecuteNonQuery();
                            if (numrows != 1)
                            {
                                DoSql(conn, "rollback transaction");
                                conn.Close();
                                throw new Exception("Valore di ritorno errato terza query");

                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            //DoSql(conn, "rollback transaction");
                            //conn.Close();
                            //return 0;
                        }

                    }
                }
                // Associo, nella tabella pivot, il documento con l'autore
                string cmd3 = string.Format(@"INSERT INTO Autori_Documenti(codice_autore, codice_documento) values({0},{1})", lCodiceAutore, libro.Codice);
                using (SqlCommand insert = new SqlCommand(cmd3, conn))
                {
                    try
                    {
                        var numrows = insert.ExecuteNonQuery();
                        if (numrows != 1)
                        {
                            DoSql(conn, "rollback transaction");
                            conn.Close();
                            throw new System.Exception("Valore di ritorno errato seconda query");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //DoSql(conn, "rollback transaction");
                        //conn.Close();
                        //return 0;
                    }
                }
            }
            DoSql(conn, "commit transaction");
            conn.Close();
            return 0;
        }
        //Aggingo DVD
        internal static int dvdAdd(DVD dvd, List<Autore> lAutori)
        {
            //mi collego al db
            var conn = Connect();
            if (conn == null)
            {
                throw new Exception("Unable to connect to database");
            }
            var ok = DoSql(conn, "begin transaction");
            if (!ok)
            {
                throw new Exception("Errore in begin transaction");
                conn.Close();
            }
            //comando per inserire un nuovo documento
            var cmd = string.Format(@"insert into dbo.Documenti(codice,Titolo,Settore,Stato,Tipo,Scaffale) 
                        VALUES({0}, '{1}', '{2}', '{3}', 'DVD', '{4}')", dvd.Codice, dvd.Titolo, dvd.Settore, dvd.Stato.ToString(), dvd.Scaffale.Numero);
            using (SqlCommand insert = new SqlCommand(cmd, conn))
            {
                try
                {
                    var numrows = insert.ExecuteNonQuery();
                    if (numrows != 1)
                    {
                        conn.Close();
                        throw new Exception("Valore di ritorno errato");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    DoSql(conn, "rollback transaction");
                    conn.Close();
                    return 0;
                }
            }
            var cmd_1 = string.Format(@"insert into dbo.Dvd(Codice,Durata) 
                        VALUES('{0}',{1})", dvd.Codice, dvd.Durata);
            using (SqlCommand insert = new SqlCommand(cmd_1, conn))
            {
                try
                {
                    var numrows = insert.ExecuteNonQuery();
                    if (numrows != 1)
                    {
                        throw new Exception("Valore di ritorno errato seconda query");
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    DoSql(conn, "rollback transaction");
                    conn.Close();
                    return 0;
                }
            }

            foreach (Autore autore in lAutori)
            {
                long lCodiceAutore = 0;
                int iInsertFlag = 0;
                cmd = String.Format("Select codice From Autori where Nome='{0}' and Cognome='{1}' and mail='{2}';", autore.Nome, autore.Cognome, autore.sMail);
                // Verifico che nella tebella Autori esiste già un Autore
                using (SqlCommand query = new SqlCommand(cmd, conn))
                {
                    using (SqlDataReader reader = query.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lCodiceAutore = reader.GetInt64(0);
                            Console.WriteLine("Codice Autore {0} già esistente", lCodiceAutore);
                        }
                        else
                        {
                            lCodiceAutore = autore.iCodiceAutore;
                            iInsertFlag = 1;
                        }
                        reader.Close();
                    }
                }

                //Se l'autore non è già inserito, lo registro nella tebella Autori
                if (iInsertFlag == 1)
                {
                    var cmd2 = String.Format("INSERT INTO Autori(codice,Nome, Cognome, mail) values ('{0}', '{1}','{2}','{3}')",
                            autore.iCodiceAutore, autore.Nome, autore.Cognome, autore.sMail);

                    using (SqlCommand insert = new SqlCommand(cmd2, conn))
                    {
                        try
                        {

                            var numrows = insert.ExecuteNonQuery();
                            if (numrows != 1)
                            {
                                DoSql(conn, "rollback transaction");
                                conn.Close();
                                throw new Exception("Valore di ritorno errato terza query");

                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            DoSql(conn, "rollback transaction");
                            conn.Close();
                            return 0;
                        }

                    }
                }
                // Associo, nella tabella pivot, il documento con l'autore
                string cmd3 = string.Format(@"INSERT INTO Autori_Documenti(codice_autore, codice_documento) values({0},{1})", lCodiceAutore, dvd.Codice);
                using (SqlCommand insert = new SqlCommand(cmd3, conn))
                {
                    try
                    {
                        var numrows = insert.ExecuteNonQuery();
                        if (numrows != 1)
                        {
                            DoSql(conn, "rollback transaction");
                            conn.Close();
                            throw new System.Exception("Valore di ritorno errato seconda query");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        DoSql(conn, "rollback transaction");
                        conn.Close();
                        return 0;
                    }
                }
            }
            DoSql(conn, "commit transaction");
            conn.Close();
            return 0;
        }
        //Leggi Documento
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
        //Genero un ID unico
        internal static long GetUniqueId()
        {
            var conn = Connect();
            if (conn == null)
                throw new Exception("Unable to connect to the dabatase");
            string cmd = "UPDATE codiceunico SET codice = codice + 1 OUTPUT INSERTED.codice";
            long id;
            using (SqlCommand select = new SqlCommand(cmd, conn))
            {
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    reader.Read();
                    id = reader.GetInt64(0);
                }
            }
            conn.Close();
            return id;
        }
    }
}

