using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyfarm_2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // serialPort1.Open();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }
        
        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {

        }

       
        private void TilebtnPersonal_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
           BusquedaCultivo frm = new BusquedaCultivo();
            frm.Show();
            //this.Visible = false;
           // serialPort1.Write("2");
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void bunifuThinButton21_Click_1(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            int temp = 1;
           // serialPort1.Write(temp.ToString());
            //serialPort1.Close()
            
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
