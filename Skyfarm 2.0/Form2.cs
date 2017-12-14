/*
Christopher Luis Fernando Martinez Delgado
Ana Karem Arguelles
Universidad politecnica de Durango 2017
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Conector;
using System.Threading;

namespace Skyfarm_2._0
{
    public partial class Form2 : Form
    {
        Class1 instanciaMySql = new Class1();

        //VARIABLES
        int n, v, l, N,V ,LL;
        string dArduino,tI,hI,tE,hE,I,L;
        string[] data;
        bool x = false;

        Thread hilo;

        //Sensores

        public Form2()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            serialPort2.Open();
            serialPort1.Open();
            serialPort2.Write("1");
            cnct();
            //cnct();



            if (instanciaMySql.CreateConnection())
            {
               // MessageBox.Show("Conexion Exitosa");
            }// Aqui se crea la conexion cada que se carga el formulario
            else
            {
                MessageBox.Show("Conexion fallida");
            }
            //dataGridView1.DataSource = instanciaMySQL.fillTable("select Fecha_Hora,Temperatura ,Humedad, Intensidad,Lux , PH, Concentrado from registros;");
            DGV.DataSource = instanciaMySql.fillTable("select fecha,tiempo,T_int,H_int,T_ext,H_ext, lux,intensidad from personalizado order by fecha desc;");
            

        }
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        // Manda informacion a arduino 
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            dArduino = serialPort1.ReadLine().ToString();
            //MessageBox.Show(dArduino);
            // Console.Write("capturado");

            hilo = new Thread(new ThreadStart(dataRecive));// un hilo que tiene un nuevo hilo se activa el open dialog
            hilo.ApartmentState = ApartmentState.STA; //Se indica la forma que se va a ejecturar el subproceso por medio del pformato STA
            hilo.Start();
        }
        void dataRecive()
        {

            //puerto.Text = dataFromArduino.ToString();
            if (!dArduino.Equals("\r"))
            {
                //Separa la informacion colectada por arduino y la separa desde la coma
                data = dArduino.Split(',');
                //Toma los valores y la inserta en arreglos
                tI = data[0];
                hI = data[1];
                tE = data[2];
                hE = data[3];
                I = data[4];
                L = data[5];
                x = true;
                hilo.Abort(); //Termina el subproceso de hilo         
            }
        }


        public void timerSensores_Tick(object sender, EventArgs e)
        {
            try
            {
                if (x)
                {
                    //puertotxt.Text = dataFromArduino.ToString();
                    tI2txt.Text = tI.ToString() + " °C";
                    hItxt.Text = hI.ToString() + " %";
                    tEtxt.Text = tE.ToString() + " °C";
                    hEtxt.Text = hE.ToString() + " %";
                    inttxt.Text = I.ToString() + "  Ω";
                    ltxt.Text = L.ToString() + " %";

                    int Ti =
                    (int)(Math.Round(Convert.ToDecimal(data[0]), 0));
                    int Hi =
                    (int)(Math.Round(Convert.ToDecimal(data[1]), 0));
                    int Te =
                    (int)(Math.Round(Convert.ToDecimal(data[2]), 0));
                    int He =
                    (int)(Math.Round(Convert.ToDecimal(data[3]), 0));
                    int i =
                    (int)(Math.Round(Convert.ToDecimal(data[4]), 0));
                    int l =
                    (int)(Math.Round(Convert.ToDecimal(data[5]), 0));

                    tIpb.Value = Ti;
                    hIpb.Value = Hi;
                    hEpb.Value = He;
                    tEpb.Value = Te;
                    lpb.Value = l;
                    ipb.Value = i;

                    x = false;
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Ocurrio un error, intente ingresar de nuevo");

            }
        }


        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------

            
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            //cierra la forma1
            serialPort2.Write("9");
            serialPort2.Close();
            this.Close();
        }


        private void menuBtn_Click(object sender, EventArgs e)
        {


            if (sideMenu.Width == 50)
            {
                //Expande
                //1.- Expante el menu a width 250px
                //2.- Muestra el logo
                sideMenu.Visible = false;
                sideMenu.Width = 205;
                PanelAnimator.ShowSync(sideMenu);
                LogoAnimation.ShowSync(logo);
                LogoAnimation.ShowSync(logo2);
                logo2.Visible = false;

            }
            else
            {
                //Minimiza
                // 1.- Esconde el logo
                // 2.- Hace el panel pequeno  

                LogoAnimation.Hide(logo);
                LogoAnimation.ShowSync(logo2);
                sideMenu.Visible = true;
                sideMenu.Width = 50;
                PanelAnimator.ShowSync(sideMenu);
                logo2.Visible = true;

            }

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            panelControles.Visible = true;
            panelRegistros.Visible = false;
            panelDatos.Visible = false;
        }

        private void bttnDatos_Click(object sender, EventArgs e)
        {
            panelControles.Visible = false;
            panelRegistros.Visible = false;
            panelDatos.Visible = true;
        }


 

        private void bunifuFlatButton1_Click_1(object sender, EventArgs e)
        {
            serialPort2.Write("0");
            cnct();
        }



        private void timerbd_Tick(object sender, EventArgs e)
        {
            string qwerty = string.Format(
     "Insert into personalizado(T_int,H_int,T_ext,H_ext,lux,intensidad,fecha,tiempo) values({0},{1},{2},{3},{4},{5},{6},{7});",
     tI,hI,tE,hE,L,I,"CURDATE()", "NOW()" );
            if (instanciaMySql.Execute(qwerty) == -1) //Si sale -1 es un error (es un error)
            {
                MessageBox.Show("Error" + instanciaMySql.error);
            }
            DGV.DataSource = instanciaMySql.fillTable("select fecha,tiempo,T_int,H_int,T_ext,H_ext, lux,intensidad from personalizado order by fecha desc;");
        }

        private void bttnRegistros_Click(object sender, EventArgs e)
        {
            panelControles.Visible = false;
            panelRegistros.Visible = true;
            panelDatos.Visible = false;
        }

        private void logo2_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            
            string AX = N + "," + V + "," + LL + "," + "1";   //tal vez deba convertirlos a string
          //  MessageBox.Show(AX);
            serialPort2.Write(AX);
            cnct2();
            
        }

        private void timerTabla_Tick(object sender, EventArgs e)
        {
           // DGV.DataSource = instanciaMySql.fillTable("select fecha,tiempo,T_int,H_int,T_ext,H_ext, lux,intensidad from personalizado, order by desc;");

        }




        private void panelControles_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Conectarbtn_Click(object sender, EventArgs e)
        {
            serialPort2.Write("1");
        }

        public void trackL_ValueChanged(object sender, EventArgs e)
        {
            l = trackL.Value;
            lapsoTxt.Text = l.ToString() + "s";
             LL = l * 1000;
            //serialPort2.Write(LL.ToString());

        }



        public void trackV_ValueChanged_1(object sender, EventArgs e)
        {
            v = trackV.Value;
            txtV.Text = v.ToString() + "s";
             V = v * 1000;
            //serialPort2.Write(V.ToString());
        }

        public void trackN_ValueChanged_1(object sender, EventArgs e)
        {
            n = trackN.Value;
            txtN.Text = n.ToString() + "s";
             N = n * 1000;
            //serialPort2.Write(N.ToString());
        }

        //IGNORAR EVENTOS

        void cnct()
        {

            

            bunifuFlatButton1.Enabled = false;

            bunifuFlatButton5.Enabled = true;

        }

        void cnct2()
        {



            bunifuFlatButton1.Enabled = true;
            bunifuFlatButton5.Enabled = false;

        }


        private void lapsoTxt_OnValueChanged(object sender, EventArgs e)
        {

        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panelDatos_Paint(object sender, PaintEventArgs e)
        {
            //borrar

        }

        private void tEtxt_OnValueChanged(object sender, EventArgs e)
        {
            //borrar
        }

        private void hItxt_OnValueChanged(object sender, EventArgs e)
        {
            //borrar
        }

    }
}
