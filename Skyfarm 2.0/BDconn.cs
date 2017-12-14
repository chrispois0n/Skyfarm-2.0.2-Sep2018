using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Skyfarm_2._0
{
    public class BDconn
    {
        public static MySqlConnection ObtenerConexion() {
            MySqlConnection conectar= new MySqlConnection("server=127.0.0.1; database=skyfarmdb ;Uid=root; pwd=Dolares1;");
            conectar.Open();
            return conectar;
        }

    }
}
