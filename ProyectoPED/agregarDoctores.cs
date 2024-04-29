using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoPED
{
    public partial class agregarDoctores : Form
    {
        public agregarDoctores()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
                    int result = EmpleadoFun.EliminarEmpleado(id);

                    if (result > 0)
                    {
                        MessageBox.Show("Empleado Eliminado");
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar Empleado");
                    }
                }

            }
        }

        private void agregarDoctores_Load(object sender, EventArgs e)
        {
            recargarScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Capturar los valores ingresados
            string dui = textBox1.Text;
            string nombre = textBox2.Text;
            int estado = comboBox1.Text.ToLower() == "activo" ? 1 : 2; // Asignar 1 para "Activo" y 2 para "Inactivo"
            int tipoEmpleado = comboBox2.Text.ToLower() == "médico" ? 1 : 2; // Asignar 1 para "Médico" y 2 para "Recepcionista"
            string usuario = textBox3.Text;
            string password = textBox4.Text;
            string especialidad = comboBox3.Text;

            // Validar el formato del DUI (ejemplo: 00000000-0)
            if (!IsValidDUI(dui))
            {
                MessageBox.Show("Formato de DUI inválido. Debe ser 00000000-0.");
                return;
            }

            // Validar el formato del nombre (Nombre Apellido)
            if (!IsValidNombre(nombre))
            {
                MessageBox.Show("Formato de nombre inválido. Debe ser Nombre Apellido.");
                return;
            }

            // Validar el usuario (no debe estar vacío y tener al menos 5 caracteres)
            if (string.IsNullOrWhiteSpace(usuario) || usuario.Length < 5)
            {
                MessageBox.Show("Usuario inválido. Debe contener al menos 5 caracteres.");
                return;
            }

            // Validar la contraseña (no debe estar vacía y tener al menos 6 caracteres)
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                MessageBox.Show("Contraseña inválida. Debe contener al menos 6 caracteres.");
                return;
            }

            // Validar que se haya seleccionado una especialidad
            if (string.IsNullOrWhiteSpace(especialidad))
            {
                MessageBox.Show("Debe seleccionar una especialidad.");
                return;
            }

            // Si pasa todas las validaciones, procedemos a insertar el empleado
            Empleado empleado = new Empleado
            {
                dui = dui,
                nombre = nombre,
                estado = estado,
                tipoEmpleado = tipoEmpleado,
                usuario = usuario,
                password = password,
                especialidad = especialidad
            };

            int result = EmpleadoFun.AgregarEmpleado(empleado);

            if (result > 0)
            {
                MessageBox.Show("Empleado ingresado exitosamente.");
                LimpiarCampos(); // Método para limpiar los campos después de la inserción
                recargarScreen(); // Método para recargar la pantalla
            }
            else
            {
                MessageBox.Show("Error al guardar al empleado.");
            }
        }
        private bool IsValidDUI(string dui)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(dui, @"^\d{8}-\d$");
        }

        private bool IsValidNombre(string nombre)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(nombre, @"^[a-zA-Z]+\s[a-zA-Z]+$");
        }

        private void LimpiarCampos()
        {
            textBox1.Text = string.Empty; // DUI
            textBox2.Text = string.Empty; // Nombre
            comboBox1.SelectedIndex = -1; // Estado
            comboBox2.SelectedIndex = -1; // Tipo de Empleado
            textBox3.Text = string.Empty; // Usuario
            textBox4.Text = string.Empty; // Contraseña
            comboBox3.SelectedIndex = -1; // Especialidad
        }
     


        private void agregarPacientes_Load(object sender, EventArgs e)
        {
           
            recargarScreen();
        }


        public void recargarScreen()
        {
            dataGridView1.DataSource = EmpleadoFun.EmpleadoRegistro();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["dui"].Value);
            textBox2.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["nombre"].Value);
            comboBox1.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["estado"].Value);
            comboBox2.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["tipoEmpleado"].Value);
            textBox3.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["usuario"].Value);
            textBox4.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["password"].Value);
            comboBox3.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["especialidad"].Value);
          


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            selectActionAdmin seleccionarAccion = new selectActionAdmin();
            seleccionarAccion.Show();
        }
    }
}
