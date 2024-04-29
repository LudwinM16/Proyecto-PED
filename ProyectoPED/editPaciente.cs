using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;


namespace ProyectoPED
{
    public partial class editPaciente : Form
    {
        public editPaciente()
        {
            InitializeComponent();
        }

        //boton de menu

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            selectActionAdmin seleccionarAccion = new selectActionAdmin();
            seleccionarAccion.Show();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["dui"].Value);
            textBox2.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["nombre"].Value);
            textBox3.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["edad"].Value);
            textBox4.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["telefono"].Value);
            textBox5.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["peso"].Value);
            textBox6.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["estatura"].Value);
            comboBox1.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["sexo"].Value);
            comboBox2.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["tipoSangre"].Value);
            textBox7.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["id"].Value);
        }

        private void LimpiarCampos()
        {
            // Limpiar todos los campos del formulario
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            comboBox1.SelectedIndex = -1; // Limpiar selección en el combo box
            textBox5.Text = string.Empty;
            comboBox2.SelectedIndex = -1; // Limpiar selección en el combo box
            textBox6.Text = string.Empty;
        }

        private void editPaciente_Load(object sender, EventArgs e)
        {
            textBox7.Enabled = false;
            recargarScreen();
        }

        public void recargarScreen()
        {
            dataGridView1.DataSource = PacienteFun.PacienteRegistro();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dui = textBox1.Text;
            string nombre = textBox2.Text;
            int edad = 0;
            if (!int.TryParse(textBox3.Text, out edad))
            {
                MessageBox.Show("Edad inválida. Debe ser un número entero.");
                return;
            }

            string telefono = textBox4.Text;
            string sexo = comboBox1.Text;
            decimal peso = 0;
            string pesoTexto = textBox5.Text.Replace(',', '.'); // Reemplaza comas por puntos
            if (!decimal.TryParse(pesoTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out peso))
            {
                MessageBox.Show("Peso inválido. Debe ser un número decimal.");
                return;
            }

            string tipoSangre = comboBox2.Text;
            decimal estatura = 0;
            string estaturaTexto = textBox6.Text.Replace(',', '.'); // Reemplaza comas por puntos
            if (!decimal.TryParse(estaturaTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out estatura))
            {
                MessageBox.Show("Estatura inválida. Debe ser un número decimal.");
                return;
            }

            // Validar el formato del DUI (ejemplo: 00000000-0)
            if (!Regex.IsMatch(dui, @"^\d{8}-\d$"))
            {
                MessageBox.Show("Formato de DUI inválido. Debe ser 00000000-0.");
                return;
            }

            // Validar el formato del nombre (Nombre Apellido)
            if (!Regex.IsMatch(nombre, @"^[a-zA-Z]+\s[a-zA-Z]+$"))
            {
                MessageBox.Show("Formato de nombre inválido. Debe ser Nombre Apellido.");
                return;
            }

            // Validar el formato del teléfono (0000-0000)
            if (!Regex.IsMatch(telefono, @"^\d{4}-\d{4}$"))
            {
                MessageBox.Show("Formato de teléfono inválido. Debe ser 0000-0000.");
                return;
            }

            // Validar el sexo (solo Masculino o Femenino)
            if (sexo != "Masculino" && sexo != "Femenino")
            {
                MessageBox.Show("Sexo inválido. Debe ser Masculino o Femenino.");
                return;
            }

            // Validar el tipo de sangre (tipos de sangre comunes)
            List<string> tiposSangreValidos = new List<string> { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            if (!tiposSangreValidos.Contains(tipoSangre))
            {
                MessageBox.Show("Tipo de sangre inválido.");
                return;
            }

            // Si pasa todas las validaciones, procedemos a modificar el paciente
            Paciente paciente = new Paciente
            {
                dui = dui,
                nombre = nombre,
                edad = edad,
                telefono = telefono,
                sexo = sexo,
                peso = peso,
                tipoSangre = tipoSangre,
                estatura = estatura
            };

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

            paciente.id = id;
            int result = PacienteFun.ModificarPaciente(paciente);

            if (result > 0)
            {
                MessageBox.Show("El paciente se modifico con exito");
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error al modificar el paciente");
                LimpiarCampos();
            }

            recargarScreen();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            PacientesGestion pacientesGestion = new PacientesGestion();
            pacientesGestion.Show();
        }

        // eliminar paciente
        private void button2_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
            int result = PacienteFun.EliminarPaciente(id);

            if(result > 0)
            {
                MessageBox.Show("Paciente Eliminado con Exito");
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error al Eliminar el Paciente");
                LimpiarCampos();
            }

            recargarScreen();
        }
    }
}
