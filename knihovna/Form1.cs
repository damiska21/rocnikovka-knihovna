using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace knihovna
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SQLClass SQLClass = new SQLClass();
            this.Text = "Vyberte Funkci";
        }
        //stránky - půjčit/vrátit knihu
        //          přidat knihu/autora/žánr/zákazníka
        //          zobrazení
        //          vytvoření db
        private void button1_Click(object sender, EventArgs e)
        {
            SQLClass.VytvorDatabazi();
            MessageBox.Show("Dat");
        }

        //půjčit/vrátit
        private void button2_Click(object sender, EventArgs e)
        {
            PujceniForm pujceniForm = new PujceniForm();
            pujceniForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowTableForm showTableForm = new ShowTableForm();
            showTableForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddNewForm addNewForm = new AddNewForm();
            addNewForm.Show();
        }
    }
}
