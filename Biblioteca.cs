using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    class Biblioteca
    {
        public string Nome { get; set; }
        public List<Scaffale> ScaffaliBiblioteca { get; set; }
        public Biblioteca(string Nome)
        {
            this.Nome = Nome;
            this.ScaffaliBiblioteca = new List<Scaffale>();
            List<string> elencoScaffali = db.scaffaliGet();
            elencoScaffali.ForEach(item =>
            {
                Scaffale nuovo = new Scaffale(item);
                this.ScaffaliBiblioteca.Add(nuovo);
            });
        }
       
        public void AggiungiScaffale(string snomeScaffale)
        {
            Scaffale s1 = new Scaffale(snomeScaffale);
            this.ScaffaliBiblioteca.Add(s1);
            db.scaffaleAdd(s1.Numero);
        }
        public void AggiungiLibro(long iCodice, string sTitolo, int iAnno, string sSettore, int iNumPagine, string sScaffale, List<Autore> lListaAutori)
        {
            Libro lib1 = new Libro(iCodice,sTitolo, iAnno,sSettore, iNumPagine, sScaffale);
            db.libroAdd(lib1, lListaAutori);
        }
            
        public void AggiungiDvd(long iCodice, string sTitolo, int iAnno, string sSettore, int iDurata, string sScaffale, List<Autore> lListaAutori)
        {
            DVD dvd1 = new DVD(iCodice, sTitolo, iAnno, sSettore, iDurata, sScaffale);
            db.dvdAdd(dvd1, lListaAutori);
        }

        public void CercaAutore(string sNome, string sCogn)
        {
            db.CercaPerAutore(sNome, sCogn);
        }
        public void StampaListaDocumenti(List<Documento> lListaDoc)
        {
            return;
        }
        /*public int GestisciOperazioneBiblioteca(int iCodiceOperazione)
        {
            List<Documento> lRes;
            string sAppo;
            switch(iCodiceOperazione)
            {
                case 1:
                    {
                        Console.WriteLine("Inserisci Autore:");
                        sAppo = Console.ReadLine();
                        lRes = CercaAutore(sAppo);
                        if (lRes == null)
                            return 1;
                        else
                            StampaListaDocumenti(lRes);
                        break;
                    }
            }    
            return 0;
        }*/
    }
}
