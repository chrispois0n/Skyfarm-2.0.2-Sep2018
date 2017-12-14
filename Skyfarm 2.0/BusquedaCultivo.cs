using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
//using Conector;

namespace Skyfarm_2._0
{
    public partial class BusquedaCultivo : Form
    {
        //Class1 instanciaMySql = new Class1();
        string dArduino, tI, hI, tE, hE, I, L;
        double tId, hId, tEd, hEd, Id, Ld;
        int Ti, Hi, Te, He, i, l;

        string[] data;
        bool x = false;
        Thread hilo3;

        public BusquedaCultivo()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        Conexion conexion = new Conexion();
        
        public Perfil perfilSeleccionado { get; set; }
        public int t_max_;
        public int t_min_;
        public int h_min_;

        void dataRecive()
        {//try catch
            try
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
                    hilo3.Abort(); //Termina el subproceso de hilo  

                    tId = (int)(Math.Round(Convert.ToDecimal(tI)));
                    hId = (int)(Math.Round(Convert.ToDecimal(hI)));
                    tEd = (int)(Math.Round(Convert.ToDecimal(tEd)));
                    hEd = (int)(Math.Round(Convert.ToDecimal(hE)));
                    Id = (int)(Math.Round(Convert.ToDecimal(I)));
                    Ld = (int)(Math.Round(Convert.ToDecimal(L)));

                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Ha ocurrido un error con el formato");

            }
        }
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try {
                dArduino = serialPort1.ReadLine().ToString();
                //MessageBox.Show(dArduino);
                // Console.Write("capturado");

                hilo3 = new Thread(new ThreadStart(dataRecive));// un hilo que tiene un nuevo hilo se activa el open dialog
                hilo3.ApartmentState = ApartmentState.STA; //Se indica la forma que se va a ejecturar el subproceso por medio del pformato STA
                hilo3.Start(); //try catch
               
            }
            catch (ThreadStartException err) {
                MessageBox.Show("Intente de nuevo la acción");
                this.Hide();
            }

        }

        public void timerSensores_Tick(object sender, EventArgs e)
        {
            try
            {
                tI2txt.Text = tI.ToString() + " °C";
                hItxt.Text = hI.ToString() + " %";
                tEtxt.Text = tE.ToString() + " °C";
                hEtxt.Text = hE.ToString() + " %";
                inttxt.Text = I.ToString() + "  Ω";
                ltxt.Text = L.ToString() + " %";

                Ti =
                (int)(Math.Round(Convert.ToDecimal(data[0]), 0));
                Hi =
                (int)(Math.Round(Convert.ToDecimal(data[1]), 0));
                Te =
                (int)(Math.Round(Convert.ToDecimal(data[2]), 0));
                He =
                (int)(Math.Round(Convert.ToDecimal(data[3]), 0));
                i =
                (int)(Math.Round(Convert.ToDecimal(data[4]), 0));
                l =
                (int)(Math.Round(Convert.ToDecimal(data[5]), 0));

                tIpb.Value = Ti;
                hIpb.Value = Hi;
                hEpb.Value = He;
                tEpb.Value = Te;
                lpb.Value = l;
                ipb.Value = i;

                x = false;
            }
            catch (NullReferenceException ex) {
                MessageBox.Show("Ocurrio un error, intente ingresar de nuevo");

            }

        }

        private void principalbtn_Click(object sender, EventArgs e)
        {
            principalP.Visible = true;
            sensoresP.Visible = false;
            registrosP.Visible = false;
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            principalP.Visible = false;
            sensoresP.Visible = true;
            registrosP.Visible = false;
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            principalP.Visible = false;
            sensoresP.Visible = false;
            registrosP.Visible = true;
        }


        int pA; //Perfil actual
        String cultivo;
        private void bunifuFlatButton2_Click_1(object sender, EventArgs e)
        {
            //USAR PERFIL
            try
            {
                if (dgvBuscar.SelectedRows.Count == 1)
                {
                    int idPerfil = Convert.ToInt32(dgvBuscar.CurrentRow.Cells[0].Value);
                    if (conexion.AbrirConexion() == true)
                    {
                        perfilSeleccionado = Perfil.ObtenerPerfil(conexion.conexion, idPerfil);

                        cultivo = (perfilSeleccionado.tipoCultivo);
                        pA = (perfilSeleccionado.idPerfil);
                        t_min_ = Int32.Parse(perfilSeleccionado.t_min);
                        t_max_ = Int32.Parse(perfilSeleccionado.t_max);
                        h_min_ = Int32.Parse(perfilSeleccionado.h_min);
                        h_max_ = Int32.Parse(perfilSeleccionado.h_max);
                        tluz_ = Int32.Parse(perfilSeleccionado.tiempoluz);

                        
                        ctxt.Text = cultivo;

                        tmintxt.Text = t_min_.ToString();
                        tmaxtxt.Text = t_max_.ToString();
                        hmintxt.Text = h_min_.ToString();
                        hmaxtxt.Text = h_max_.ToString();
                        conexion.CerrarConexion();
                        }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro");
                }

            }
            catch (Exception ew)
            {

                MessageBox.Show(ew + "");
            }
            
            serialPort2.Write("2");
            timerControl.Enabled = true;
            principalP.Visible = false;
            sensoresP.Visible = true;
            registrosP.Visible = false;
        }

        private void bunifuFlatButton3_Click_1(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            //NO EXISTE
            serialPort2.Write("9");
            serialPort2.Close();
            this.Close();
        }



        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            // NO EXISTE
            serialPort2.Write("9");
            serialPort2.Close();
            this.Close();
        }

        private void bunifuImageButton1_Click_1(object sender, EventArgs e)
        {
            //SI EXISTE
            serialPort2.Write("9");
            serialPort2.Close();
            this.Close();
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //BORRAR
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (conexion.AbrirConexion() == true)
                {
                    Perfil aPerfil = new Perfil();
                    aPerfil.tipoCultivo = txtTipoCultivo.Text.Trim();
                    aPerfil.t_min = txtTemperaturaminn.Text.Trim();
                    aPerfil.t_max = txtTemperaturamax.Text.Trim();
                    aPerfil.h_min = txtHumedadmin.Text.Trim();
                    aPerfil.h_max = txtHumedadmax.Text.Trim();
                    aPerfil.tiempoluz = txtLuz.Text.Trim();


                    int resultado;

                    if (string.IsNullOrEmpty(txtidPerfil.Text))
                    {
                        resultado = Perfil.AgregarPerfil(conexion.conexion, aPerfil);

                    }
                    else
                    {
                        aPerfil.idPerfil = Convert.ToInt32(txtidPerfil.Text);
                        resultado = Perfil.ActualizarPerfil(conexion.conexion, aPerfil);
                    }


                    if (resultado > 0)
                    {


                        txtTipoCultivo.Text = "";
                        txtTemperaturaminn.Text = "";
                        txtHumedadmin.Text = "";
                        txtLuz.Text = "";
                        txtTemperaturamax.Text = "";
                        txtHumedadmax.Text = "";
                        ListarPerfiles(conexion.conexion, txtTipoCultivo.Text);

                    }

                    conexion.CerrarConexion();
                }
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void principalP_Paint(object sender, PaintEventArgs e)
        {

        }

        private void registrosP_Paint(object sender, PaintEventArgs e)
        {

        }

        private void perfilesDG_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvBuscar.SelectedRows.Count == 1)
                {
                    int idPerfil = Convert.ToInt32(dgvBuscar.CurrentRow.Cells[0].Value);
                    if (conexion.AbrirConexion() == true)
                    {
                        perfilSeleccionado = Perfil.ObtenerPerfil(conexion.conexion, idPerfil);

                        txtidPerfil.Text = perfilSeleccionado.idPerfil.ToString();
                        txtTipoCultivo.Text = perfilSeleccionado.tipoCultivo;
                        txtTemperaturaminn.Text = perfilSeleccionado.t_min;
                        txtTemperaturamax.Text = perfilSeleccionado.t_max;
                        txtHumedadmin.Text = perfilSeleccionado.h_min;
                        txtHumedadmax.Text = perfilSeleccionado.h_max;
                        txtLuz.Text = perfilSeleccionado.tiempoluz;

                        conexion.CerrarConexion();
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro");
                }

            }
            catch (Exception ew)
            {

                MessageBox.Show(ew + "");
            }
        }

        private void btnBorrar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvBuscar.SelectedRows.Count == 1)
                {
                    int idPerfil = Convert.ToInt32(dgvBuscar.CurrentRow.Cells[0].Value);

                    DialogResult confirEliminar = MessageBox.Show("Se eliminará el registro " + idPerfil + "¿Desea continuar?", "Alerta de eliminación", MessageBoxButtons.YesNo);

                    if (confirEliminar == DialogResult.Yes)
                    {
                        if (conexion.AbrirConexion() == true)
                        {
                            int resultado;
                            resultado = Perfil.EliminarPerfil(conexion.conexion, idPerfil);

                            if (resultado > 0)
                            {

                                ListarPerfiles(conexion.conexion, txtTipoCultivo.Text);
                                txtTipoCultivo.Text = "";
                                txtTemperaturaminn.Text = "";
                                txtHumedadmin.Text = "";
                                txtLuz.Text = "";
                                txtTemperaturamax.Text = "";
                                txtHumedadmax.Text = "";


                                conexion.CerrarConexion();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Eliminación cancelada");
                        ListarPerfiles(conexion.conexion, txtTipoCultivo.Text);
                        txtTipoCultivo.Text = "";
                        txtTemperaturaminn.Text = "";
                        txtHumedadmin.Text = "";
                        txtLuz.Text = "";
                        txtTemperaturamax.Text = "";
                        txtHumedadmax.Text = "";


                        conexion.CerrarConexion();

                    }

                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro");
                }

            }
            catch (Exception ew)
            {

                MessageBox.Show(ew + "");
            }
        }
            
private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (conexion.AbrirConexion() == true)
                {
                    ListarPerfiles(conexion.conexion, txtTipoCultivo.Text);
                    conexion.CerrarConexion();
                }
            }
            catch (MySqlException ex)
            {

                throw ex;
            }
        }

        private void bunifuCustomLabel3_ForeColorChanged(object sender, EventArgs e)
        {

        }

        public int h_max_;
        public int tluz_;






        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (conexion.AbrirConexion() == true)
                {
                    Perfil aPerfil = new Perfil();
                    aPerfil.tipoCultivo = txtTipoCultivo.Text.Trim();
                    aPerfil.t_min = txtTemperaturaminn.Text.Trim();
                    aPerfil.t_max = txtTemperaturamax.Text.Trim();
                    aPerfil.h_min = txtHumedadmin.Text.Trim();
                    aPerfil.h_max = txtHumedadmax.Text.Trim();
                    aPerfil.tiempoluz = txtLuz.Text.Trim();
                    

                    int resultado;

                    if (string.IsNullOrEmpty(txtidPerfil.Text))
                    {
                        resultado = Perfil.AgregarPerfil(conexion.conexion, aPerfil);
                       
                    }
                    else
                    {
                        aPerfil.idPerfil = Convert.ToInt32(txtidPerfil.Text);
                        resultado = Perfil.ActualizarPerfil(conexion.conexion, aPerfil);
                    }


                    if (resultado > 0)
                    {

                       
                        txtTipoCultivo.Text = "";
                        txtTemperaturaminn.Text = "";
                        txtHumedadmin.Text = "";
                        txtLuz.Text = "";
                        txtTemperaturamax.Text = "";
                        txtHumedadmax.Text = "";
                        ListarPerfiles(conexion.conexion, txtTipoCultivo.Text);

                    }

                    conexion.CerrarConexion();
                }
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void ListarPerfiles(MySqlConnection conexion, string atipoCultivo)
        {
            dgvBuscar.DataSource = Perfil.Buscar(conexion, atipoCultivo);
        }

        private void BusquedaCultivo_Load(object sender, EventArgs e)
        {
            serialPort1.Open();
            serialPort2.Open();
            try
            {
                if (conexion.AbrirConexion() == true)
                {
                    ListarPerfiles(conexion.conexion, txtTipoCultivo.Text);
                    conexion.CerrarConexion();
                }
            }
            catch (MySqlException ex)
            {

                throw ex;
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBuscar.SelectedRows.Count == 1)
                {
                    int idPerfil = Convert.ToInt32(dgvBuscar.CurrentRow.Cells[0].Value);
                    if (conexion.AbrirConexion() == true)
                    {
                        perfilSeleccionado = Perfil.ObtenerPerfil(conexion.conexion, idPerfil);

                        txtidPerfil.Text = perfilSeleccionado.idPerfil.ToString();
                        txtTipoCultivo.Text = perfilSeleccionado.tipoCultivo;
                        txtTemperaturaminn.Text = perfilSeleccionado.t_min;
                        txtTemperaturamax.Text = perfilSeleccionado.t_max;
                        txtHumedadmin.Text = perfilSeleccionado.h_min;
                        txtHumedadmax.Text = perfilSeleccionado.h_max;
                        txtLuz.Text = perfilSeleccionado.tiempoluz;

                        conexion.CerrarConexion();
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro");
                }

            }
            catch (Exception ew)
            {

                MessageBox.Show(ew + "");
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBuscar.SelectedRows.Count == 1)
                {
                    int idPerfil = Convert.ToInt32(dgvBuscar.CurrentRow.Cells[0].Value);

                    DialogResult confirEliminar = MessageBox.Show("Se eliminará el registro " + idPerfil + "¿Desea continuar?", "Alerta de eliminación", MessageBoxButtons.YesNo);

                    if (confirEliminar == DialogResult.Yes)
                    {
                        if (conexion.AbrirConexion() == true)
                        {
                            int resultado;
                            resultado = Perfil.EliminarPerfil(conexion.conexion, idPerfil);

                            if (resultado > 0)
                            {

                                ListarPerfiles(conexion.conexion, txtTipoCultivo.Text);
                                txtTipoCultivo.Text = "";
                                txtTemperaturaminn.Text = "";
                                txtHumedadmin.Text = "";
                                txtLuz.Text = "";
                                txtTemperaturamax.Text = "";
                                txtHumedadmax.Text = "";
                                

                                conexion.CerrarConexion();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Eliminación cancelada");
                        ListarPerfiles(conexion.conexion, txtTipoCultivo.Text);
                        txtTipoCultivo.Text = "";
                        txtTemperaturaminn.Text = "";
                        txtHumedadmin.Text = "";
                        txtLuz.Text = "";
                        txtTemperaturamax.Text = "";
                        txtHumedadmax.Text = "";
                      

                        conexion.CerrarConexion();

                    }

                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro");
                }

            }
            catch (Exception ew)
            {

                MessageBox.Show(ew + "");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (conexion.AbrirConexion() == true)
                {
                    ListarPerfiles(conexion.conexion, txtTipoCultivo.Text);
                    conexion.CerrarConexion();
                }
            }
            catch (MySqlException ex)
            {

                throw ex;
            }
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBuscar.SelectedRows.Count == 1)
                {
                    int idPerfil = Convert.ToInt32(dgvBuscar.CurrentRow.Cells[0].Value);
                    if (conexion.AbrirConexion() == true)
                    {
                        perfilSeleccionado = Perfil.ObtenerPerfil(conexion.conexion, idPerfil);
                        
                        t_min_ = Int32.Parse(perfilSeleccionado.t_min);
                        t_max_ = Int32.Parse(perfilSeleccionado.t_max);
                        h_min_ = Int32.Parse(perfilSeleccionado.h_min);
                        h_max_ = Int32.Parse(perfilSeleccionado.h_max);
                        tluz_ = Int32.Parse(perfilSeleccionado.tiempoluz);

                        conexion.CerrarConexion();

                        MessageBox.Show("var= "+ t_min_+", "+t_max_+", " + h_min_+ ", "+ h_max_);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro");
                }

            }
            catch (Exception ew)
            {

                MessageBox.Show(ew + "");
            }
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void timerControl_Tick(object sender, EventArgs e)
        {
            if (Hi > h_min_ && Hi < h_max_)
            {
                eHumtxt.Text = "Humedad equilibrada";
            }
            if (Hi > h_max_)
            {              
                serialPort2.Write("1");
                eHumtxt.Text="Humedad actual mayor";

            }
            if(Hi < h_min_)
            {
                serialPort2.Write("2");
                eHumtxt.Text="Humedad actual menor";
            }
            if(Ti > t_max_)
            {
                serialPort2.Write("3");
                eTemtxt.Text="mayor actual mayor";
            }
            if(Ti> t_min_ && Ti < t_max_)
            {
                eTemtxt.Text = "Temperatura equilibrada";
            }
            else
            {

            }
          
        }
        void perf1() //perfiles primera ejecucion 
        {

        }
    }
}
