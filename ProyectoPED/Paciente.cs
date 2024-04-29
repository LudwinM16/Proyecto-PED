using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPED
{
    class Paciente
    {

        //Propeidades de la clase

        public int id { get; set; }

        public string dui {get; set;}

        public string nombre { get; set; }

        public string sexo { get; set; }

        public string telefono { get; set; }


        public int edad { get; set; }

        public string tipoSangre { get; set; }

        public decimal estatura { get; set; }

        public decimal peso { get; set; }


        //constructor

        public Paciente() { }

        public Paciente(int id, string dui, string nombre, string sexo, string telefono, int edad, string tipoSangre, decimal estatura, decimal peso)
        {
            this.id = id;

            this.dui = dui;

            this.nombre = nombre;

            this.sexo = sexo;
            this.telefono = telefono;
            this.edad = edad;
            this.tipoSangre = tipoSangre;
            this.estatura = estatura;
            this.peso = peso;
        }
    }
}
