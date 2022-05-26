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
        public List<Documento> SearchByAutore (string sAutore)
        {
            /* 
                SELECT titolo, Settore, Stato, Tipo
                FROM Docuemnti, AUTORE_DOCUMENTI
                INNER JOIN(Autori_Documenti) ON Documenti.codice_documento = Autori_Documenti.
             */
            return null;
        }
        public void StampaListaDocumenti(List<Documento> lListaDoc)
        {
            return;
        }
        public int GestisciOperazioneBiblioteca(int iCodiceOperazione)
        {
            List<Documento> lRes;
            string sAppo;
            switch(iCodiceOperazione)
            {
                case 1:
                    {
                        Console.WriteLine("Inserisci Autore:");
                        sAppo = Console.ReadLine();
                        lRes = SearchByAutore(sAppo);
                        if (lRes == null)
                            return 1;
                        else
                            StampaListaDocumenti(lRes);
                        break;
                    }
            }    
            return 0;
        }

    }
}
