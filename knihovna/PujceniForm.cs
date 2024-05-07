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
        }
        BindingList<Kniha> bb = new BindingList<Kniha>();
        //vyhledat zákazníka
        private void button1_Click(object sender, EventArgs e)
        {
            string[] exit = SQLClass.FindZakaznik(textBox4.Text, textBox3.Text, textBox5.Text);
            MessageBox.Show(exit[0] + exit[1]);
            button1.Text = "Uživatel nalezen!";
            textBox4.Text = exit[0];
            textBox3.Text = exit[1];
            textBox5.Text = exit[2];
            bb = SQLClass.FindKniha(Convert.ToInt32(exit[2]));
            
            var source = new BindingSource();
            source.DataSource = bb;
            dataGridView2.DataSource = source;
            MessageBox.Show("b");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dataGridView2.CurrentCell.RowIndex.ToString());
            SQLClass.KnihaZakaznikEdit(dataGridView2.CurrentCell.RowIndex + 1, -1);
            MessageBox.Show("Kniha úspěšně navrácena!");
            bb.RemoveAt(dataGridView2.CurrentCell.RowIndex);
        }
    }
}
