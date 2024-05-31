using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProyectoPED
{
    class BDD 
    {

        public static SqlConnection ObtnerConexion()
        {
            SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ProgramaMedico;Data Source=DESKTOP-1GLH020");
            conn.Open();

            return conn;
        }
    }
}
