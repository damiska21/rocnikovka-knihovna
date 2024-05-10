using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knihovna
{
    internal class Zanr
    {
        public int ZanrID { get; set; }
        public string nazev {  get; set; }
        public Zanr(int ZanrID, string nazev) {
            this.ZanrID = ZanrID;
            this.nazev = nazev;
        }
    }
}
