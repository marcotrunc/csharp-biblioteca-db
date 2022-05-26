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
        public long iCodiceAutore;

        public Autore(string Nome, string Cognome, string Email) : base(Nome, Cognome)
        {
            this.sMail = Email;
            this.iCodiceAutore = GeneraCodiceAutore();
        }
        public long GeneraCodiceAutore()
        {
            return db.GetUniqueId();
        }
    }
}
