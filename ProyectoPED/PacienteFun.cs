using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProyectoPED
{
    class PacienteFun
    {

        //funcion para insertar paciente

        public static int AgregarPaciente(Paciente paciente)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDD.ObtnerConexion())
            {
                string query = "INSERT INTO Pacientes (DUI, NombreCompleto, Telefono, Sexo, Edad, TipoSangre, Estatura, Peso) " +
               "VALUES ('" + paciente.dui + "', '" + paciente.nombre + "', '" + paciente.telefono + "', '" +
               paciente.sexo + "', '" + paciente.edad + "', '" + paciente.tipoSangre + "', '" +
               paciente.estatura + "', '" + paciente.peso + "')";
                SqlCommand comando = new SqlCommand(query, conexion);


                retorno = comando.ExecuteNonQuery();
            }

            return retorno;
        }

        // funcion para listar paciente

        public static List<Paciente> PacienteRegistro()
        {
            List<Paciente> Lista = new List<Paciente>();


            using (SqlConnection connection = BDD.ObtnerConexion())
            {
                string query = "SELECT * FROM Pacientes";
                SqlCommand comando = new SqlCommand(query, connection);

                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    Paciente paciente = new Paciente();
                    //ATRIBUTOS QUE TENDREMOS

                    paciente.id = reader.GetInt32(0);
                    paciente.dui = reader.GetString(1);
                    paciente.nombre = reader.GetString(2);
                    paciente.telefono = reader.GetString(3);
                    paciente.sexo = reader.GetString(4);
                    paciente.edad = reader.GetInt32(5);
                    paciente.tipoSangre = reader.GetString(6);
                    paciente.estatura = reader.GetDecimal(7);
                    paciente.peso = reader.GetDecimal(8);

                    Lista.Add(paciente);
                }
            }
            return Lista;
        }


        //funcion editar

        public static int ModificarPaciente(Paciente paciente)
        {
            int result = 0;
            using (SqlConnection conn = BDD.ObtnerConexion())
            {
                string query = "UPDATE Pacientes set DUI = '" + paciente.dui + "', NombreCompleto='" + paciente.nombre + "', Telefono='" + paciente.telefono + "', Sexo='" +
               paciente.sexo + "', Edad='" + paciente.edad + "', TipoSangre= '" + paciente.tipoSangre + "', Estatura= '" +
               paciente.estatura + "', Peso ='" + paciente.peso + "' where PacienteID =" + paciente.id + "";
                SqlCommand comando = new SqlCommand(query, conn);

                try
                {

                    result = comando.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // Manejar la excepción, por ejemplo, registrarla o lanzarla nuevamente
                    Console.WriteLine("Error al modificar el paciente: " + ex.Message);
                    Console.ReadLine();
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        //funcion eliminar

        public static int EliminarPaciente(int id)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDD.ObtnerConexion())
            {
                string query = "DELETE FROM Pacientes WHERE PAcienteID = " + id + "";
                SqlCommand comando = new SqlCommand(query, conexion);


                retorno = comando.ExecuteNonQuery();
            }

            return retorno;
        }


    }
}
