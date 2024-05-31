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


    public partial class FormDoctor : Form
    {

        private int doctorId;
        private string nombreDoctor;
        private string connectionString = @"Data Source=DESKTOP-1GLH020;Initial Catalog=ProgramaMedico;Integrated Security=True;";
        public FormDoctor(int doctorId, string nombreDoctor)
        {
            InitializeComponent();
            this.doctorId = doctorId;
            this.nombreDoctor = nombreDoctor;
            labelDoctor.Text = $"Doctor: {nombreDoctor}";
            dateTimePickerFecha.ValueChanged += dateTimePickerFecha_ValueChanged;
            CargarCitas(DateTime.Now);
        }

        private void FormDoctor_Load(object sender, EventArgs e)
        {
            InitializeComponent();
          
        }


        private void dateTimePickerFecha_ValueChanged(object sender, EventArgs e)
        {
            DateTime fechaSeleccionada = dateTimePickerFecha.Value.Date;
            CargarCitas(fechaSeleccionada);
        }

        private void CargarCitas(DateTime fecha)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CitaID, PacienteID, FechaHora, Descripcion FROM Citas WHERE EmpleadoID = @DoctorID AND CONVERT(date, FechaHora) = @Fecha";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DoctorID", doctorId);
                cmd.Parameters.AddWithValue("@Fecha", fecha);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewCitas.DataSource = dt;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewCitas_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void dataGridViewCitas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int citaId = Convert.ToInt32(dataGridViewCitas.Rows[e.RowIndex].Cells["CitaID"].Value);
                int pacienteId = Convert.ToInt32(dataGridViewCitas.Rows[e.RowIndex].Cells["PacienteID"].Value);

                FormHistorialMedico formHistorial = new FormHistorialMedico(citaId, pacienteId);
                formHistorial.Show();
            }
        }
    }
}
