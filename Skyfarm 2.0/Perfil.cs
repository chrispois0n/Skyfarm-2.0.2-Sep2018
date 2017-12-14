using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Skyfarm_2._0
{
    public class Perfil
    {
        public int idPerfil { get; set; }
        public string tipoCultivo { get; set; }
        public string t_min { get; set; }
        public string h_min { get; set; }
        public string tiempoluz { get; set; }
        public string t_max { get; set; }
        public string h_max { get; set; }
       

        public Perfil() { }

        public Perfil(
            int aidPerfil,
            string atipoCultivo,
            string at_min,
            string ah_min,
            string atiempoluz,
            string at_max,
            string ah_max
           
            )
        {
            this.idPerfil = aidPerfil;
            this.tipoCultivo = atipoCultivo;
            this.h_min = ah_min;
            this.t_min = at_min;
            this.tiempoluz = atiempoluz;
            this.t_max = at_max;
            this.h_max = ah_max;
           
        }

        public static int AgregarPerfil(MySqlConnection conexion, Perfil aPerfil)
        {
            int retorno = 0;
            MySqlCommand comando = new MySqlCommand(String.Format("INSERT INTO perfiles (tipoCultivo, t_min,t_max, h_min,h_max, tiempoluz) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')", aPerfil.tipoCultivo, aPerfil.t_min, aPerfil.t_max, aPerfil.h_min, aPerfil.h_max, aPerfil.tiempoluz), conexion);
            retorno = comando.ExecuteNonQuery();

            return retorno;
        }

        public static int ActualizarPerfil(MySqlConnection conexion, Perfil aPerfil)
        {

            int retorno = 0;
            MySqlCommand comando = new MySqlCommand(String.Format("UPDATE perfiles SET tipoCultivo='{1}', t_min='{2}', t_max='{3}', h_min='{4}', h_max='{5}', tiempoluz ='{6}' where idPerfil={0}", aPerfil.idPerfil, aPerfil.tipoCultivo, aPerfil.t_min, aPerfil.t_max, aPerfil.h_min, aPerfil.h_max, aPerfil.tiempoluz), conexion);
            retorno = comando.ExecuteNonQuery();

            return retorno;
        }
        public static int EliminarPerfil(MySqlConnection conexion, int aidPerfil)
        {
            int retorno = 0;
            MySqlCommand comando = new MySqlCommand(String.Format("DELETE FROM perfiles where idPerfil={0}", aidPerfil), conexion);
            retorno = comando.ExecuteNonQuery();

            return retorno;

        }

        public static IList<Perfil> Buscar(MySqlConnection conexion, string atipoCultivo)
        {
            List<Perfil> lista = new List<Perfil>();
            MySqlCommand comando = new MySqlCommand(String.Format("SELECT idPerfil, tipoCultivo, t_min,t_max, h_min,h_max, tiempoluz FROM perfiles where tipoCultivo LIKE ('%{0}%')", atipoCultivo), conexion);
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                Perfil aPerfil = new Perfil();
                aPerfil.idPerfil = reader.GetInt32(0);
                aPerfil.tipoCultivo = reader.GetString(1);
                aPerfil.t_min = reader.GetString(2);
                aPerfil.t_max = reader.GetString(3);
                aPerfil.h_min = reader.GetString(4);
                aPerfil.h_max = reader.GetString(5);
                aPerfil.tiempoluz = reader.GetString(6);

                lista.Add(aPerfil);

            }

            return lista;

        }

        public static Perfil ObtenerPerfil(MySqlConnection conexion, int aidPerfil)
        {
            Perfil aPerfil = new Perfil();
            MySqlCommand comando = new MySqlCommand(String.Format("SELECT idPerfil, tipoCultivo, t_min,t_max, h_min,h_max, tiempoluz FROM perfiles where idPerfil LIKE ('%{0}%')", aidPerfil), conexion);
            MySqlDataReader reader = comando.ExecuteReader();

            while (reader.Read())
            {
                aPerfil.idPerfil = reader.GetInt32(0);
                aPerfil.tipoCultivo = reader.GetString(1);
                aPerfil.t_min = reader.GetString(2);
                aPerfil.t_max = reader.GetString(3);
                aPerfil.h_min = reader.GetString(4);
                aPerfil.h_max = reader.GetString(5);
                aPerfil.tiempoluz = reader.GetString(6);
            }
            return aPerfil;

        }
    }
}
