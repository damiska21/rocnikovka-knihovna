using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knihovna
{
    internal class Zakaznik
    {
        public int ZakaznikID {  get; set; }
        public string jmeno { get; set; }
        public string prijmeni { get; set; }
        public Zakaznik(int ZakaznikID, string jmeno, string prijmeni)
        {
            this.ZakaznikID = ZakaznikID;
            this.jmeno = jmeno;
            this.prijmeni = prijmeni;
        }
    }
}
