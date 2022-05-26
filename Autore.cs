using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_biblioteca_db
{
    class Autore : Persona
    {
        public string sMail { get; set; }
        public int iCodiceAutore;

        public Autore(string Nome, string Cognome, string Email) : base(Nome, Cognome)
        {
            this.sMail = Email;
            this.iCodiceAutore = GeneraCodiceAutore();
        }
        public int GeneraCodiceAutore()
        {
            return 10000 + base.Nome.Length + base.Cognome.Length + this.sMail.Length;
        }
    }
}
