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
    }
}
