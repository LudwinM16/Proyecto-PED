using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;



namespace ProyectoPED
{

    public partial class login : Form
    {

        //conexion bdd
        private string connectionString = @"Data Source=DESKTOP-1GLH020;Initial Catalog=ProgramaMedico;Integrated Security=True;";


        public login()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //capturamos valores del login
            string usuario = textBoxUsuario.Text;
            string password = textBox1.Text;

            //Consulta SQL
            string query = "SELECT EmpleadoID, TipoEmpleadoID, NombreCompleto FROM Empleados WHERE Usuario = @Usuario AND Password = @Password";

            //creamos la cnn con la bdd

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Usuario", usuario);
                    command.Parameters.AddWithValue("@Password", password);

                    //Ejecutamos la consulta
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Inicio de sesión exitoso
                            MessageBox.Show("Inicio de sesión exitoso");

                            // Asignar los datos del usuario a la clase estática de sesión
                            SesionUsuarios.EmpleadoID = reader.GetInt32(0);
                            SesionUsuarios.TipoEmpleado = reader.GetInt32(1);
                                
                            // Redirigir dependiendo del tipo de empleado
                            if (SesionUsuarios.TipoEmpleado == 2)
                            {
                                // Redirigir a la ventana de administrador
                                selectActionAdmin seleccionarAccion = new selectActionAdmin();
                                seleccionarAccion.Show();

                            }
                            else if (SesionUsuarios.TipoEmpleado == 1)
                            {
                                // Redirigir a la ventana de médico

                            }

                            this.Hide(); // Ocultar el formulario de inicio de sesión
                        }
                        else
                        {
                            // Credenciales inválidas
                            MessageBox.Show("Credenciales inválidas");
                        }
                    }

                }



            }
        }

        private void login_Load(object sender, EventArgs e)
        {
            textBox1.UseSystemPasswordChar = true;
        }
    }
}
