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
    public partial class ShowTableForm : Form
    {
        public ShowTableForm()
        {
            InitializeComponent();
            this.Text = "Zobrazení Tabulek";
        }
        BindingList<Kniha> knihaDisplayList = new BindingList<Kniha>();
        BindingList<Zakaznik> zakaznikDisplayList = new BindingList<Zakaznik>();
        BindingList<Autor> autorDisplayList = new BindingList<Autor>();
        BindingList<Zanr> zanrDisplayList = new BindingList<Zanr>();
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    zakaznikDisplayList = SQLClass.ListZakaznik();
                    dataGridView1.DataSource = new BindingSource().DataSource = zakaznikDisplayList;
                    break;
                case 1:
                    knihaDisplayList = SQLClass.FindKniha(-1, "", -1, -1, -1);
                    dataGridView1.DataSource = new BindingSource().DataSource = knihaDisplayList;
                    break;
                case 2:
                    autorDisplayList = SQLClass.ListAutor();
                    dataGridView1.DataSource = new BindingSource().DataSource = autorDisplayList;
                    break;
                case 3:
                    zanrDisplayList = SQLClass.ListZanr();
                    dataGridView1.DataSource = new BindingSource().DataSource = zanrDisplayList;
                    break;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var b = dataGridView1.Rows[e.RowIndex];
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    SQLClass.ZmenZakaznika(b.Cells[1].Value.ToString(), b.Cells[2].Value.ToString(), Convert.ToInt32(b.Cells[0].Value));
                    break;
                case 1:
                    SQLClass.ZmenKnihu(b.Cells[1].Value.ToString(), Convert.ToInt32(b.Cells[2].Value), Convert.ToInt32(b.Cells[0].Value), Convert.ToInt32(b.Cells[4].Value), Convert.ToInt32(b.Cells[3].Value));
                    break;
                case 2:
                    SQLClass.ZmenAutora(b.Cells[1].Value.ToString(), b.Cells[2].Value.ToString(), Convert.ToInt32(b.Cells[0].Value));
                    break;
                case 3:
                    SQLClass.ZmenZanr(b.Cells[1].Value.ToString(), Convert.ToInt32(b.Cells[0].Value));
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.SelectedRows 
        }
    }
}
