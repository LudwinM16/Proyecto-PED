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
    public partial class agregarPacientes : Form
    {
        public agregarPacientes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Paciente paciente = new Paciente();
            paciente.id = Convert.ToInt32(textBox7.Text);
            paciente.dui = textBox1.Text;
            paciente.nombre = textBox2.Text;
            paciente.edad = Convert.ToInt32(textBox3.Text);
            paciente.telefono = textBox4.Text;
            paciente.sexo = comboBox1.Text;
            paciente.peso = Convert.ToDecimal(textBox5.Text);
            paciente.tipoSangre = comboBox2.Text;
            paciente.estatura = Convert.ToDecimal(textBox6.Text);


           
           

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);

            if(id != null)
            {
                int result = PacienteFun.ModificarPaciente(paciente);


                if (result > 0)
                {
                    MessageBox.Show("Paciente Modificado Exitosamente");

                }
                else
                {
                    MessageBox.Show("Error al Modificar al Paciente" );
                }

                recargarScreen();
            }
            else
            {
                paciente.id = id;
                int result = PacienteFun.AgregarPaciente(paciente);


                if (result > 0)
                {
                    MessageBox.Show("Paciente Ingresado Exitosamente");

                }
                else
                {
                    MessageBox.Show("Error al Guardar al Paciente");
                }

                recargarScreen();
            }

            dataGridView1.DataSource = PacienteFun.PacienteRegistro();

        }

        private void agregarPacientes_Load(object sender, EventArgs e)
        {
            recargarScreen();
        }


        public void recargarScreen()
        {
            dataGridView1.DataSource = PacienteFun.PacienteRegistro();
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
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
                int result = PacienteFun.EliminarPaciente(id);

                if (result > 0)
                {
                    MessageBox.Show("Paciente Eliminado");
                }
                else
                {
                    MessageBox.Show("Error al eliminar Paciente");
                }
            }
            
        }
    }
}
