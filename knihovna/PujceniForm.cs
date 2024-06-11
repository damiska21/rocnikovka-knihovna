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
    public partial class PujceniForm : Form
    {
        public PujceniForm()
        {
            InitializeComponent();
            this.Text = "Půjčení/Navrácení Knihy";
        }
        BindingList<Kniha> bb = new BindingList<Kniha>();
        BindingList<Kniha> ee = new BindingList<Kniha>();
        //vyhledat zákazníka
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] exit = SQLClass.FindZakaznik(textBox4.Text, textBox3.Text, textBox5.Text);
                button1.Text = "Uživatel nalezen!";
                textBox4.Text = exit[0];
                textBox3.Text = exit[1];
                textBox5.Text = exit[2];
                bb = SQLClass.FindKniha(-1, "", -1, -1, Convert.ToInt32(exit[2]));
                dataGridView2.DataSource = new BindingSource().DataSource = bb;
            }
            catch { MessageBox.Show("Nastala chyba při vyhledávání uživatele.");}
            
        }

        //vrátit knihu
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dataGridView2.CurrentCell.RowIndex.ToString());
            SQLClass.KnihaZakaznikEdit(bb[(dataGridView2.CurrentCell.RowIndex)].KnihaID, -1, "");
            MessageBox.Show("Kniha úspěšně navrácena!");
            bb.RemoveAt(dataGridView2.CurrentCell.RowIndex);
        }

        //hledání knihy
        private void button3_Click(object sender, EventArgs e)
        {
            int AutorID = -1;
            int ZanrID = -1;
            //hledání autora 6 
            AutorID = SQLClass.FindAutorByName(textBox6.Text);
            
            //zanr 7
            if (textBox7.Text.Length >2)
            {
                ZanrID = SQLClass.FindZanrByName(textBox7.Text);
            }

            int KnihaID = -1;
            if (textBox1.Text.Length>0)
            {
                KnihaID = Convert.ToInt32(textBox1.Text);
            }
            ee = SQLClass.FindKniha(KnihaID, textBox2.Text, AutorID, ZanrID, -2);
            dataGridView1.DataSource = new BindingSource().DataSource = ee;
        }

        //půjčit knihu
        private void button4_Click(object sender, EventArgs e)
        {
            string borrowDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            SQLClass.KnihaZakaznikEdit(ee[(dataGridView1.CurrentCell.RowIndex)].KnihaID, Convert.ToInt32(textBox5.Text), borrowDate);
            MessageBox.Show("Kniha úspěšně půjčena!");
        }
    }
}
