using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPED
{
    class Empleado
    {

        //Propeidades de la clase

        public int id { get; set; }

        public string dui { get; set; }

        public string nombre { get; set; }

        public int estado { get; set; }

        public int tipoEmpleado { get; set; }


        public string usuario { get; set; }

        public string password { get; set; }

        public string especialidad { get; set; }



        //constructor

        public Empleado() { }

        public Empleado(int id, string dui, string nombre, int estado, int tipoEmpleado, string usuario, string password, string especialidad)
        {
            this.id = id;

            this.dui = dui;

            this.nombre = nombre;

            this.estado = estado;
            this.tipoEmpleado = tipoEmpleado;
            this.usuario = usuario;
            this.password = password;
            this.especialidad = especialidad;
        }
    }
}
