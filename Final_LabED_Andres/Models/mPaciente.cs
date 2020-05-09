using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Final_LabED_Andres.Models
{
    public class mPaciente
    {
        private string nombre;
        private string apellido;
        private string dpi;
        private string municipio_resi;
        private string departamento_resi;
        private bool infectado;
        private string sintomas;
        private string descripcion_contagio;
        private int prioridad;
        private string name_hosp;


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha_Nac")]
        public DateTime Fecha_Nac { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha_ingreso")]
        public DateTime Fecha_ingreso { get; set; }
     

        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Dpi { get => dpi; set => dpi = value; }
        public string Municipio_resi { get => municipio_resi; set => municipio_resi = value; }
        public string Departamento_resi { get => departamento_resi; set => departamento_resi = value; }
        public string Sintomas { get => sintomas; set => sintomas = value; }
        public string Descripcion_contagio { get => descripcion_contagio; set => descripcion_contagio = value; }
        public int Prioridad { get => prioridad; set => prioridad = value; }
        public bool Infectado { get => infectado; set => infectado = value; }
        public string Name_hosp { get => name_hosp; set => name_hosp = value; }


        //Delegados
        public static Comparison<mPaciente> Comparar_Nombre = delegate (mPaciente paciente1, mPaciente paciente2)
        {
            return paciente1.Nombre.CompareTo(paciente2.Nombre);
        };
        public static Comparison<mPaciente> Comparar_Apellido = delegate (mPaciente paciente1, mPaciente paciente2)
        {
            return paciente1.Apellido.CompareTo(paciente2.Apellido);
        };

        public static Comparison<mPaciente> Comparar_DPI = delegate (mPaciente paciente1, mPaciente paciente2)
        {
               return paciente1.Dpi.CompareTo(paciente2.Dpi);
        };


        public static Comparison<mPaciente> Comparar_Prioridad = delegate (mPaciente paciente1, mPaciente paciente2)
        {
            if (paciente1.Prioridad == paciente2.Prioridad)
            {
                int fecha_prio = (paciente1.Fecha_ingreso.Day - paciente2.Fecha_ingreso.Day) + ((paciente1.Fecha_ingreso.Month - paciente2.Fecha_ingreso.Month) * 30) + ((paciente1.Fecha_ingreso.Year - paciente2.Fecha_ingreso.Year) * 365);
                if (fecha_prio <= 0)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return paciente1.Prioridad > paciente2.Prioridad ? 1 : paciente1.Prioridad < paciente2.Prioridad ? -1:0;
            }
        };


        public static Func<mPaciente, mPaciente, bool> EsPrioritario = delegate (mPaciente paciente1, mPaciente paciente2)
        {
            if(paciente1.Prioridad == paciente2.Prioridad)
            {
                int fecha_prio= (paciente1.Fecha_ingreso.Day - paciente2.Fecha_ingreso.Day) + ((paciente1.Fecha_ingreso.Month - paciente2.Fecha_ingreso.Month) * 30) + ((paciente1.Fecha_ingreso.Year - paciente2.Fecha_ingreso.Year) * 365);
                if(fecha_prio<0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return (paciente1.Prioridad > paciente2.Prioridad);
            }
        };

        public static Action<mPaciente> Cambiar_infect = delegate (mPaciente paciente)
        {
            paciente.Infectado = true;
            if(paciente.Prioridad==8)
            {
                paciente.Prioridad =5;
            }
            else if (paciente.Prioridad == 7)
            {
                paciente.Prioridad = 3;
            }
            else if (paciente.Prioridad == 6)
            {
                paciente.Prioridad = 2;
            }
            else if (paciente.Prioridad == 4)
            {
                paciente.Prioridad = 1;
            }

        };

        public static Action<mPaciente> Cambiar_sano = delegate (mPaciente paciente)
        {
            paciente.Infectado = false;
            paciente.Name_hosp = paciente.Name_hosp +" (Recuperado)";
        };

        public static Action<mPaciente> Cambiar_Noinfect = delegate (mPaciente paciente)
         {
             paciente.Infectado = false;
             paciente.Name_hosp = paciente.Name_hosp + " (No Infectado)";
         };


        public static Func<mPaciente, string> Obt_DPI = delegate (mPaciente paciente)
          {
              return paciente.Dpi;
          };

        public static Func<string, int> Del_CodeHash = delegate (string llave)
        {
            int code = 0;
            for (int i = 0; i < llave.Length; i++)
            {
                code += Convert.ToInt16(llave[i]);
            }
            code = code % 10;
            return code;
        };

        //Fin delegados






    }
}