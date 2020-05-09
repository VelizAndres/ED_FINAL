using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Final_LabED_Andres.Models
{
    public class mEstadist
    {
        private int ingres_infect;
        private int ingres_sospech;
        private int activos;
        private int recuperados;
        private int no_Infect;

        public int Ingres_infect { get => ingres_infect; set => ingres_infect = value; }
        public int Ingres_sospech { get => ingres_sospech; set => ingres_sospech = value; }
        public int Activos { get => activos; set => activos = value; }
        public int Recuperados { get => recuperados; set => recuperados = value; }
        public int No_Infect { get => no_Infect; set => no_Infect = value; }
    }
}