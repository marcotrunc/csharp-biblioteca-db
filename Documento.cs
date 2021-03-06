using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    enum Stato { Disponibile, Prestito }
    internal class Documento
    {
        public long Codice { get; set; }
        public string Titolo { get; set; }
        public int Anno { get; set; }
        public string Settore { get; set; }
        public Stato Stato { get; set; }
        public List<Autore> Autori { get; set; }
        public Scaffale Scaffale { get; set; }

        public Documento(long Codice, string Titolo, int Anno, string Settore, string sScaffale)
        {
            this.Codice = Codice;
            this.Titolo = Titolo;
            this.Settore = Settore;
            this.Autori = new List<Autore>();
            this.Stato = Stato.Disponibile;
            this.Scaffale = new Scaffale(sScaffale);
        }

        public override string ToString()
        {
            return string.Format("Codice:{0}\nTitolo:{1}\nSettore:{2}\nStato:{3}\nScaffale numero:{4}",
                this.Codice,
                this.Titolo,
                this.Settore,
                this.Stato,
                this.Scaffale.Numero);
        }

        public void ImpostaInPrestito()
        {
            this.Stato = Stato.Prestito;
        }

        public void ImpostaDisponibile()
        {
            this.Stato = Stato.Disponibile;
        }

        internal static string GenereCasuale()
        {
            string[] generi = new string[] { "Biografia e autobiografia", "Romanzo Storico", "Giallo", "Avventura", "Azione", "Fantascienza", "Distopatia", "Fantasy" };
            Random rand = new Random();
            int genericRandom = rand.Next(0, generi.Length);
            string res = generi[genericRandom];
            return res;
        }
    }
}
