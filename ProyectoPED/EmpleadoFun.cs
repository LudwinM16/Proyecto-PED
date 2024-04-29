using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProyectoPED
{
    class EmpleadoFun
    {

        //funcion para agregar empleado
        public static int AgregarEmpleado(Empleado empleado)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDD.ObtnerConexion())
            {
                string query = "INSERT INTO Empleados (DUI, NombreCompleto, Estado, TipoEmpleadoID, Usuario, Password, Especialidad) " +
               "VALUES ('" + empleado.dui + "', '" + empleado.nombre + "', '" + empleado.estado + "', '" +
               empleado.tipoEmpleado + "', '" + empleado.usuario+ "', '" + empleado.password + "', '" +
               empleado.especialidad + "')";
                SqlCommand comando = new SqlCommand(query, conexion);


                retorno = comando.ExecuteNonQuery();
            }

            return retorno;
        }

        // funcion para listar empleado

        public static List<Empleado> EmpleadoRegistro()
        {
            List<Empleado> Lista = new List<Empleado>();


            using (SqlConnection connection = BDD.ObtnerConexion())
            {
                string query = "SELECT * FROM Empleados";
                SqlCommand comando = new SqlCommand(query, connection);

                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    Empleado empleado = new Empleado();
                    //ATRIBUTOS QUE TENDREMOS

                    empleado.id = reader.GetInt32(0);
                    empleado.dui = reader.GetString(1);
                    empleado.nombre = reader.GetString(2);
                    empleado.estado = reader.GetInt32(3);
                    empleado.tipoEmpleado = reader.GetInt32(4);
                    empleado.usuario = reader.GetString(5);
                    empleado.password = reader.GetString(6);
                    empleado.especialidad = reader.GetString(7);
                   

                    Lista.Add(empleado);
                }

                connection.Close();
                return Lista;
            }
            
        }

        public static int EliminarEmpleado(int id)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDD.ObtnerConexion())
            {
                string query = "DELETE FROM Empleados WHERE PAcienteID = " + id + "";
                SqlCommand comando = new SqlCommand(query, conexion);


                retorno = comando.ExecuteNonQuery();
            }

            return retorno;
        }

    }
}
