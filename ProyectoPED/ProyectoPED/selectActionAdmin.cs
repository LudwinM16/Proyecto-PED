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
    public partial class selectActionAdmin : Form
    {
        public selectActionAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            agregarDoctores adminDoc = new agregarDoctores();
            adminDoc.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            agregarPacientes adminPacientes = new agregarPacientes();
            adminPacientes.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            GestionarCitas gestionarCitas = new GestionarCitas();
            gestionarCitas.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            agregarCitas agregarCitas = new agregarCitas();
            agregarCitas.Show();
        }
    }
}
