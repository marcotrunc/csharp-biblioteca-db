using System;
using System.Data.SqlClient;

namespace csharp_biblioteca_db 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Biblioteca b = new Biblioteca("Civica");
            /*StreamReader reader = new StreamReader("elenco.txt");
            Random rand = new Random(); 
            string linea;
            while ((linea = reader.ReadLine()) != null)
            {
                var vett =linea.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);  
                string s = vett[0];
                var cn = s.Split(new char[] { ' ' });
                string nome = cn[0];
                string cognome = "";
                try
                {
                    if (cognome.Length == 0)
                        cognome = "n.d.";
                    else
                        cognome = s.Substring(cn[0].Length + 1);

                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);    
                }
                string titolo = vett[1];
                
                List<Autore> lAutoriLibro = new List<Autore>();
                Autore AutoreLibro = new Autore(nome, cognome, "nd");
                lAutoriLibro.Add(AutoreLibro);
                int iGenerico = rand.Next(1700, 2022);

                b.AggiungiLibro(db.GetUniqueId(), titolo, iGenerico, Documento.GenereCasuale(), iGenerico, "SS1", lAutoriLibro);
            }
            Environment.Exit(-1);
            */
       
            Console.WriteLine("Cosa vuoi fare?");
            Console.WriteLine("1: Aggiungi Libro con relativi Autori");
            Console.WriteLine("2: Aggiungi DVD con relativi Autori");
            Console.WriteLine("3: Aggiungi Scaffale");
            Console.WriteLine("4: Cerca Documento per Autore");
            string sResp = Console.ReadLine();
            int iResp = Convert.ToInt32(sResp);


            if(sResp != "")
            {
                if(iResp == 1 || iResp == 2)
                {
                    Console.WriteLine("Inserisci Nome Autore");
                    string sNomeAutore = Console.ReadLine();
                    Console.WriteLine("Inserisci Cognome Autore");
                    string sCognomeAutore = Console.ReadLine();
                    Console.WriteLine("Inserisci Email");
                    string sEmailAutore = Console.ReadLine();
                    List<Autore> lAutoriLibro = new List<Autore>();
                    Autore AutoreLibro = new Autore(sNomeAutore, sCognomeAutore, sEmailAutore);
                    lAutoriLibro.Add(AutoreLibro);
                    Console.WriteLine("Inserisci Il Titolo");
                    string sTitolo = Console.ReadLine();
                    Console.WriteLine("Inserisci l'anno di pubblicazione");
                    string sAnno = Console.ReadLine();
                    int iAnno = Convert.ToInt32(sAnno);
                    Console.WriteLine("Inserisci il genere");
                    string sGenere = Console.ReadLine();
                    Console.WriteLine("Inserisci Scaffale");
                    string sScaffale = Console.ReadLine();
                    if(iResp == 1)
                    {
                        Console.WriteLine("Inserisci Numero di Pagine");
                        string sNumPagine = Console.ReadLine();
                        int iNumPagine = Convert.ToInt32(sNumPagine);   
                        b.AggiungiLibro(db.GetUniqueId(), sTitolo, iAnno, sGenere, iNumPagine, sScaffale, lAutoriLibro);
                    }
                    else
                    {
                        Console.WriteLine("Inserisci Durata");
                        string sDurata = Console.ReadLine();
                        int iDurata = Convert.ToInt32(sDurata);
                        b.AggiungiDvd(db.GetUniqueId(), sTitolo, iAnno, sGenere, iDurata, sScaffale, lAutoriLibro);
                    }
                }
                if (iResp == 3)
                {
                    Console.WriteLine("Inserisci il codice dello scaffale");
                    string sCodScaffale = Console.ReadLine();
                    b.AggiungiScaffale(sCodScaffale);
                    b.ScaffaliBiblioteca.ForEach(item => Console.WriteLine(item.Numero));
                }
                if(iResp == 4)
                {
                    Console.WriteLine("Inserisci Nome Autore");
                    string sNomAutCercato = Console.ReadLine();
                    Console.WriteLine("Inserisci Cognome Autore");
                    string sCogAutCercato = Console.ReadLine();
                    b.CercaAutore(sNomAutCercato, sCogAutCercato);
                }
            }
            

            
/*
          
              
 
            Prestito p1 = new Prestito("P00001", new DateTime(2019,1, 20),  new DateTime(2019, 2, 20), u1, l1);
            Prestito p2 = new Prestito("P00002", new DateTime(2019, 3, 20), new DateTime(2019, 4, 20), u1, l2);
 
               
 
            Console.WriteLine("\n\nSearchByCodice: ISBN1\n\n");
 
            List<Documento> results = b.SearchByCodice("ISBN1");
 
            foreach(Documento doc in results)
            {
                Console.WriteLine(doc.ToString());
 
                if (doc.Autori.Count > 0)
                {
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Autori");
                    Console.WriteLine("--------------------------");
                    foreach (Autore a in doc.Autori)
                    {
                        Console.WriteLine(a.ToString());
                        Console.WriteLine("--------------------------");
                    }
                } 
            }
 
            Console.WriteLine("\n\nSearchPrestiti: Nome 1, Cognome 1\n\n");
 
            List<Prestito> prestiti = b.SearchPrestiti("Nome 1", "Cognome 1");
 
            foreach (Prestito p in prestiti)
            {
                Console.WriteLine(p.ToString());
                Console.WriteLine("--------------------------");
            }
            */
        }
    }
}
