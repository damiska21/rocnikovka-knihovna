﻿using System;
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
        SQLClass SQLClass = null;
        public Form1()
        {
            InitializeComponent();
            SQLClass = new SQLClass();
        }
        //stránky - půjčit/vrátit knihu
        //          přidat knihu/autora/žánr/zákazníka
        //          zobrazení
        //          vytvoření db
        private void button1_Click(object sender, EventArgs e)
        {
            SQLClass.VytvorDatabazi();
        }
    }
}
