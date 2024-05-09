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
        public int KnihaID { get; set; }
        public string nazev { get; set; }
        public int AutorID { get; set; }
        public int ZanrID { get; set; }
        public int ZakaznikID { get; set; }
        public Kniha(int KnihaID, string nazev, int AutorID, int ZanrID, int ZakaznikID) { 
            this.KnihaID = KnihaID;
            this.nazev = nazev;
            this.AutorID = AutorID;
            this.ZanrID = ZanrID;
            this.ZakaznikID = ZakaznikID; 
        }
    }
}
