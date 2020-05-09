using System;
using System.Linq;
using System.Web;
using Final_LabED_Andres.Herramientas.Estructuras;
using Final_LabED_Andres.Models;
using System.Collections.Generic;


namespace Final_LabED_Andres.Herramientas.Almacen
{
    public class Caja_Hospitales
    {
        private static Caja_Hospitales _instance = null;

        public static Caja_Hospitales Instance
        {
            get
            {
                if (_instance == null) _instance = new Caja_Hospitales();
                return _instance;
            }
        }

        public mHospital[] hospitals = new mHospital[5];
        public mEstadist Stadistica = new mEstadist();
    }
}