using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knihovna
{
    internal class Autor
    {
        public int AutorID { get; set; }
        public string jmeno {  get; set; }
        public string prijmeni { get; set; }
        public Autor(int AutorID, string jmeno, string prijmeni) {
            this.AutorID = AutorID;
            this.jmeno = jmeno;
            this.prijmeni = prijmeni;
        }
    }
}
