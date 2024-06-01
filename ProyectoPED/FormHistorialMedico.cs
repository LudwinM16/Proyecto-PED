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
    public partial class FormHistorialMedico : Form
    {

        private int citaId;
        private int pacienteId;
        private string connectionString = @"Data Source=DESKTOP-1GLH020;Initial Catalog=ProgramaMedico;Integrated Security=True;";
        public FormHistorialMedico(int citaId, int pacienteId)
        {
            InitializeComponent();
            this.citaId = citaId;
            this.pacienteId = pacienteId;
            CargarHistorialMedico();
        }

        private void FormHistorialMedico_Load(object sender, EventArgs e)
        {

        }

        private void CargarHistorialMedico()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT FechaConsulta, Descripcion, Diagnostico, Tratamiento FROM HistorialMedico WHERE PacienteID = @PacienteID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PacienteID", pacienteId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewHistorial.DataSource = dt;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string descripcion = textBoxDescripcion.Text;
            string diagnostico = textBoxDiagnostico.Text;
            string tratamiento = textBoxTratamiento.Text;

            if (string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(diagnostico) || string.IsNullOrWhiteSpace(tratamiento))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO HistorialMedico (PacienteID, FechaConsulta, Descripcion, Diagnostico, Tratamiento) VALUES (@PacienteID, @FechaConsulta, @Descripcion, @Diagnostico, @Tratamiento)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PacienteID", pacienteId);
                cmd.Parameters.AddWithValue("@FechaConsulta", DateTime.Now);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.Parameters.AddWithValue("@Diagnostico", diagnostico);
                cmd.Parameters.AddWithValue("@Tratamiento", tratamiento);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Historial médico actualizado exitosamente.");
                CargarHistorialMedico();
            }
        }
    }
}
