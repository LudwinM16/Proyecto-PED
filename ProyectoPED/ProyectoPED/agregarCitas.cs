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
    public partial class agregarCitas : Form
    {
        public agregarCitas()
        {
            InitializeComponent();
            LoadComboBoxes();
            ConfigurarControles();
        }

        private void comboBoxMedico_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private string connectionString = "Data Source=.;Initial Catalog=ProgramaMedico;Integrated Security=True";


        private void LoadComboBoxes()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT PacienteID, NombreCompleto FROM Pacientes", conn);
                DataTable dtPacientes = new DataTable();
                da.Fill(dtPacientes);
                comboBoxPaciente.DataSource = dtPacientes;
                comboBoxPaciente.DisplayMember = "NombreCompleto";
                comboBoxPaciente.ValueMember = "PacienteID";

                da = new SqlDataAdapter("SELECT EmpleadoID, NombreCompleto FROM Empleados WHERE TipoEmpleadoID = 1", conn); // Assuming TipoEmpleadoID = 1 for doctors
                DataTable dtMedicos = new DataTable();
                da.Fill(dtMedicos);
                comboBoxMedico.DataSource = dtMedicos;
                comboBoxMedico.DisplayMember = "NombreCompleto";
                comboBoxMedico.ValueMember = "EmpleadoID";
            }
        }

        private void buttonAgendar_Click(object sender, EventArgs e)
        {
            if (comboBoxPaciente.SelectedItem == null || comboBoxMedico.SelectedItem == null || comboBoxHora.SelectedItem == null ||
         string.IsNullOrWhiteSpace(textBoxDescripcion.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int pacienteID = ((KeyValuePair<int, string>)comboBoxPaciente.SelectedItem).Key;
            int medicoID = ((KeyValuePair<int, string>)comboBoxMedico.SelectedItem).Key;
            DateTime fecha = dateTimePickerFechaHora.Value.Date;
            string horaSeleccionada = comboBoxHora.SelectedItem.ToString();
            DateTime fechaHora;

            bool isValidDateTime = DateTime.TryParseExact($"{fecha.ToShortDateString()} {horaSeleccionada}",
                                                           "M/d/yyyy hh:mm tt",
                                                           null,
                                                           System.Globalization.DateTimeStyles.None,
                                                           out fechaHora);

            if (!isValidDateTime)
            {
                MessageBox.Show("La fecha y hora seleccionadas no son válidas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string descripcion = textBoxDescripcion.Text;

            if (PuedeAgendarCita(-1, fechaHora, medicoID))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO Citas (PacienteID, EmpleadoID, FechaHora, Descripcion) VALUES (@PacienteID, @MedicoID, @FechaHora, @Descripcion)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PacienteID", pacienteID);
                    cmd.Parameters.AddWithValue("@MedicoID", medicoID);
                    cmd.Parameters.AddWithValue("@FechaHora", fechaHora);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cita agregada exitosamente");
                    LimpiarCampos();
                }
            }
            else
            {
                MessageBox.Show("No se puede agendar la cita. Ya hay 3 citas en esta hora o el médico ya tiene una cita a esta hora.");
            }
        }

        private void LimpiarCampos()
        {
            comboBoxPaciente.SelectedIndex = -1;
            comboBoxMedico.SelectedIndex = -1;
            comboBoxHora.SelectedIndex = -1;
            textBoxDescripcion.Clear();
            dateTimePickerFechaHora.Value = DateTime.Now;
        }

        private bool PuedeAgendarCita(int citaID, DateTime fechaHora, int medicoID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
            SELECT 
                (SELECT COUNT(*) FROM Citas WHERE FechaHora = @FechaHora AND EmpleadoID = @MedicoID) AS CitasMedico,
                (SELECT COUNT(*) FROM Citas WHERE FechaHora = @FechaHora) AS CitasTotales";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FechaHora", fechaHora);
                cmd.Parameters.AddWithValue("@MedicoID", medicoID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int citasMedico = reader.GetInt32(0);
                        int citasTotales = reader.GetInt32(1);

                        return citasMedico == 0 && citasTotales < 3;
                    }
                }
            }
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            this.Hide();
            selectActionAdmin selectActionAdmin = new selectActionAdmin();
            selectActionAdmin.Show();
        }

        private void ConfigurarControles()
        {
            // Configura dateTimePickerFecha solo para fechas
            dateTimePickerFechaHora.Format = DateTimePickerFormat.Short;

            // Configura comboBoxHora con las horas de atención
            var horas = new List<string>();
            for (int hora = 7; hora <= 17; hora++)
            {
                horas.Add(new DateTime(1, 1, 1, hora, 0, 0).ToString("hh:mm tt"));
            }
            comboBoxHora.DataSource = horas;
            comboBoxHora.DropDownStyle = ComboBoxStyle.DropDownList;

            // Configura comboBoxPaciente con los pacientes disponibles
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string queryPacientes = "SELECT PacienteID, NombreCompleto FROM Pacientes";
                SqlCommand cmdPacientes = new SqlCommand(queryPacientes, conn);
                SqlDataReader readerPacientes = cmdPacientes.ExecuteReader();

                var pacientes = new List<KeyValuePair<int, string>>();
                while (readerPacientes.Read())
                {
                    pacientes.Add(new KeyValuePair<int, string>((int)readerPacientes["PacienteID"], readerPacientes["NombreCompleto"].ToString()));
                }
                comboBoxPaciente.DataSource = pacientes;
                comboBoxPaciente.DisplayMember = "Value";
                comboBoxPaciente.ValueMember = "Key";
                readerPacientes.Close();

                // Configura comboBoxMedico con los médicos disponibles
                string queryMedicos = "SELECT EmpleadoID, NombreCompleto FROM Empleados WHERE TipoEmpleadoID = (SELECT TipoEmpleadoID FROM TipoEmpleado WHERE Tipo = 'Medico')";
                SqlCommand cmdMedicos = new SqlCommand(queryMedicos, conn);
                SqlDataReader readerMedicos = cmdMedicos.ExecuteReader();

                var medicos = new List<KeyValuePair<int, string>>();
                while (readerMedicos.Read())
                {
                    medicos.Add(new KeyValuePair<int, string>((int)readerMedicos["EmpleadoID"], readerMedicos["NombreCompleto"].ToString()));
                }
                comboBoxMedico.DataSource = medicos;
                comboBoxMedico.DisplayMember = "Value";
                comboBoxMedico.ValueMember = "Key";
                readerMedicos.Close();
            }
        }
    }
}
