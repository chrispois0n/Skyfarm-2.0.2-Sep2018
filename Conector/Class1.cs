using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace Conector
{
    public class Class1
    {
        MySqlConnection Conexion;
        public string error { get; set; }
        public bool CreateConnection()
        {
            string con = "server=localhost;uid=root;pwd=Dolares1;database=skyfarmdb;";

            try
            {
                Conexion = new MySqlConnection(con);
                Conexion.Open();
                return true;
            }
            catch (Exception exc)
            {
                error = exc.Message;
                
                return false;
            }

        }

        public int Execute(string pQuery)
        {
            if (Conexion.State != ConnectionState.Open)
                CreateConnection();
            try
            {
                MySqlCommand myCommand = new MySqlCommand(pQuery, Conexion);
                myCommand.CommandType = CommandType.Text;
                return myCommand.ExecuteNonQuery();

            }
            catch (Exception exc)
            {
                error = exc.Message;
                return -1;
            }

        }

        public DataTable fillTable(string pQuery)
        {
            DataTable dt = new DataTable();
            if (Conexion.State != ConnectionState.Open)
                CreateConnection();  
            try
            {
                MySqlCommand myCommand = new MySqlCommand(pQuery, Conexion);
                myCommand.CommandType = CommandType.Text;
                MySqlDataAdapter ad = new MySqlDataAdapter(myCommand);
                ad.Fill(dt);

            }
            catch (Exception exc)
            {
                error = exc.Message;

            }
            return dt;

        }


    }
}
