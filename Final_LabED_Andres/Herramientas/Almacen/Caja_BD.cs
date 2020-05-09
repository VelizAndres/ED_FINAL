using Final_LabED_Andres.Models;
using Final_LabED_Andres.Herramientas.Estructuras;
using System.Collections.Generic;

namespace Final_LabED_Andres.Herramientas.Almacen
{
    public class Caja_BD
    {
        private static Caja_BD _instance = null;
      
        public static Caja_BD Instance
        {
            get
            {
                if (_instance == null) _instance = new Caja_BD();
                return _instance;
            }
        }
        public List<mPaciente> List_all = new List<mPaciente>();
        public BiArbol<mPaciente> arbol_Dpi = new BiArbol<mPaciente>();
        public BiArbol<mPaciente> arbol_Nom = new BiArbol<mPaciente>();
        public BiArbol<mPaciente> arbol_Ape = new BiArbol<mPaciente>();
        public bool first;
    }
}