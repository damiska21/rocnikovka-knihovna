using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace knihovna
{
    internal class Kniha
    {
        public int KnihaID;
        public string nazev;
        public int AutorID;
        public int ZanrID;
        public bool pujcena;
        public int ZakaznikID;
        public Kniha(int KnihaID, string nazev, int AutorID, int ZanrID, bool pujcena, int ZakaznikID) { 
            this.KnihaID = KnihaID;
            this.nazev = nazev;
            this.AutorID = AutorID;
            this.ZanrID = ZanrID;
            this.pujcena = pujcena;
            this.ZakaznikID = ZakaznikID; 
        }
    }
}
