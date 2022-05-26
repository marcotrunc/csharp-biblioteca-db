using System;
using System.Data.SqlClient;

namespace csharp_biblioteca_db 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cosa vuoi fare?");
            Console.WriteLine("1: Aggiungi Libro con relativi Autori");
            Console.WriteLine("2: Aggiungi Scaffale");
            string sResp = Console.ReadLine();
            int iResp = Convert.ToInt32(sResp);

            Biblioteca b = new Biblioteca("Civica");

            if(sResp != "")
            {
                if(iResp == 1)
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
                    Console.WriteLine("Inserisci Il titolo del Libro");
                    string sTitolo = Console.ReadLine();
                    Console.WriteLine("Inserisci Anno di pubblicazione");
                    string sAnno = Console.ReadLine();
                    int iAnno = Convert.ToInt32(sAnno);
                    Console.WriteLine("Inserisci genere");
                    string sGenere = Console.ReadLine();
                    Console.WriteLine("Inserisci Numero di Pagine");
                    string sNumPagine = Console.ReadLine();
                    int iNumPagine = Convert.ToInt32(sNumPagine);   
                    Console.WriteLine("Inserisci Scaffale");
                    string sScaffale = Console.ReadLine();
                    b.AggiungiLibro(db.GetUniqueId(), sTitolo, iAnno, sGenere, iNumPagine, sScaffale, lAutoriLibro);
                }
                if (iResp == 2)
                {
                    Console.WriteLine("Inserisci il codice dello scaffale");
                    string sCodScaffale = Console.ReadLine();
                    b.AggiungiScaffale(sCodScaffale);
                    b.ScaffaliBiblioteca.ForEach(item => Console.WriteLine(item.Numero));
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
