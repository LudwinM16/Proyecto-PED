using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProyectoPED
{
    public partial class GestionarCitas : Form
    {
        private string connectionString = "Data Source=.;Initial Catalog=ProgramaMedico;Integrated Security=True";
        private int selectedCitaID;
        private Dictionary<int, Cita> citasDictionary;
        public GestionarCitas()
        {
            InitializeComponent();
            ConfigurarControles();
            LoadCitas(DateTime.Now.Date);


        }

        private void GestionarCitas_Load(object sender, EventArgs e)
        {

        }

        private void ConfigurarControles()
        {
            dateTimePickerFechaHora.Format = DateTimePickerFormat.Short;

            for (int hora = 7; hora <= 17; hora++)
            {
                comboBoxHora.Items.Add(new DateTime(1, 1, 1, hora, 0, 0).ToString("hh:mm tt"));
            }
            comboBoxHora.DropDownStyle = ComboBoxStyle.DropDownList;

            dateTimePickerFiltro.ValueChanged += new EventHandler(dateTimePickerFiltro_ValueChanged);
            dataGridViewCitas.SelectionChanged += new EventHandler(dataGridViewCitas_SelectionChanged);

            buttonGuardar.Click += new EventHandler(buttonGuardar_Click);
            buttonEliminar.Click += new EventHandler(buttonEliminar_Click);
            button1.Click += new EventHandler(button1_Click);
        }

        private void dateTimePickerFiltro_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePickerFiltro.Value.Date;
            LoadCitas(selectedDate);
        }


        private void LoadCitas(DateTime fechaFiltro)
        {
            citasDictionary = new Dictionary<int, Cita>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
        SELECT c.CitaID, p.NombreCompleto AS Paciente, e.NombreCompleto AS Medico, c.FechaHora, c.Descripcion, c.EmpleadoID AS MedicoID
        FROM Citas c
        INNER JOIN Pacientes p ON c.PacienteID = p.PacienteID
        INNER JOIN Empleados e ON c.EmpleadoID = e.EmpleadoID
        WHERE CONVERT(date, c.FechaHora) = @FechaFiltro";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FechaFiltro", fechaFiltro.ToString("yyyy-MM-dd")); // Usar formato ISO

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtCitas = new DataTable();
                da.Fill(dtCitas);

                foreach (DataRow row in dtCitas.Rows)
                {
                    int citaID = Convert.ToInt32(row["CitaID"]);
                    Cita cita = new Cita
                    {
                        CitaID = citaID,
                        Paciente = row["Paciente"].ToString(),
                        Medico = row["Medico"].ToString(),
                        MedicoID = Convert.ToInt32(row["MedicoID"]),
                        FechaHora = Convert.ToDateTime(row["FechaHora"]),
                        Descripcion = row["Descripcion"].ToString()
                    };
                    citasDictionary[citaID] = cita;
                }

                dataGridViewCitas.DataSource = dtCitas;
            }
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {

            if (dataGridViewCitas.SelectedRows.Count > 0)
            {
                selectedCitaID = Convert.ToInt32(dataGridViewCitas.SelectedRows[0].Cells["CitaID"].Value);

                string descripcion = textBoxDescripcion.Text;
                DateTime nuevaFechaHora = dateTimePickerFechaHora.Value;
                string horaSeleccionada = comboBoxHora.SelectedItem.ToString();
                DateTime fechaHora = DateTime.ParseExact($"{nuevaFechaHora.ToShortDateString()} {horaSeleccionada}", "dd/MM/yyyy hh:mm tt", null);

                int medicoID = citasDictionary[selectedCitaID].MedicoID;

                if (PuedeAgendarCita(selectedCitaID, fechaHora, medicoID))
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = "UPDATE Citas SET Descripcion = @Descripcion, FechaHora = @FechaHora WHERE CitaID = @CitaID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                        cmd.Parameters.AddWithValue("@FechaHora", fechaHora);
                        cmd.Parameters.AddWithValue("@CitaID", selectedCitaID);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Cita actualizada exitosamente");
                        LoadCitas(dateTimePickerFiltro.Value.Date);
                    }
                }
                else
                {
                    MessageBox.Show("No se puede cambiar la hora de la cita. Ya hay 3 citas en esta hora o el médico ya tiene una cita a esta hora.");
                }
            }
        }

            private void buttonEliminar_Click(object sender, EventArgs e)
        {
            if (selectedCitaID > 0)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "DELETE FROM Citas WHERE CitaID = @CitaID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CitaID", selectedCitaID);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cita eliminada exitosamente");
                    LoadCitas(dateTimePickerFiltro.Value.Date);
                    textBoxDescripcion.Clear();
                    dateTimePickerFechaHora.Value = DateTime.Now;
                }
            }
            else
            {
                MessageBox.Show("Seleccione una cita para eliminar.");
            }
        }

        private void dataGridViewCitas_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCitas.SelectedRows.Count > 0)
            {
                selectedCitaID = Convert.ToInt32(dataGridViewCitas.SelectedRows[0].Cells["CitaID"].Value);
                if (citasDictionary.TryGetValue(selectedCitaID, out Cita cita))
                {
                    textBoxDescripcion.Text = cita.Descripcion;
                    dateTimePickerFechaHora.Value = cita.FechaHora;
                    comboBoxHora.SelectedItem = cita.FechaHora.ToString("hh:mm tt");
                }
            }
        }


        private bool PuedeAgendarCita(int citaID, DateTime fechaHora, int medicoID)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
        SELECT 
            (SELECT COUNT(*) FROM Citas WHERE FechaHora = @FechaHora AND EmpleadoID = @MedicoID AND CitaID <> @CitaID) AS CitasMedico,
            (SELECT COUNT(*) FROM Citas WHERE FechaHora = @FechaHora) AS CitasTotales";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FechaHora", fechaHora);
                cmd.Parameters.AddWithValue("@MedicoID", medicoID);
                cmd.Parameters.AddWithValue("@CitaID", citaID);

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

        private void dateTimePickerFechaHora_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerFiltro_ValueChanged_1(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePickerFiltro.Value.Date;
            LoadCitas(selectedDate);
        }
    }
    public class Cita
    {
        public int CitaID { get; set; }
        public string Paciente { get; set; }
        public string Medico { get; set; }
        public int MedicoID { get; set; } // Añade esta propiedad
        public DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }
    }
    }
