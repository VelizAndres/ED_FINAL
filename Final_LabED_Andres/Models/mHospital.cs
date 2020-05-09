using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Final_LabED_Andres.Herramientas.Estructuras;
using Final_LabED_Andres.Models;

namespace Final_LabED_Andres.Models
{
    public class mHospital
    {
        public string nombre_hospital;
        public TablaHash<mPaciente> camillas;
        public Heap<mPaciente> cola_sospech;
        public Heap<mPaciente> cola_infect;
    }
}