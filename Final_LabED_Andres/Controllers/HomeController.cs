using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final_LabED_Andres.Models;
using Final_LabED_Andres.Herramientas.Almacen;
using Final_LabED_Andres.Herramientas.Estructuras;

namespace Final_LabED_Andres.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!Caja_BD.Instance.first)
            {
                Caja_Hospitales.Instance.hospitals[0] = new mHospital { nombre_hospital = "Guatemala", cola_infect = new Heap<mPaciente>(), cola_sospech = new Heap<mPaciente>(), camillas = new TablaHash<mPaciente>() };
                Caja_Hospitales.Instance.hospitals[1] = new mHospital { nombre_hospital = "Petén", cola_infect = new Heap<mPaciente>(), cola_sospech = new Heap<mPaciente>(), camillas = new TablaHash<mPaciente>() };
                Caja_Hospitales.Instance.hospitals[2] = new mHospital { nombre_hospital = "Chiquimula", cola_infect = new Heap<mPaciente>(), cola_sospech = new Heap<mPaciente>(), camillas = new TablaHash<mPaciente>() };
                Caja_Hospitales.Instance.hospitals[3] = new mHospital { nombre_hospital = "Escuintla", cola_infect = new Heap<mPaciente>(), cola_sospech = new Heap<mPaciente>(), camillas = new TablaHash<mPaciente>() };
                Caja_Hospitales.Instance.hospitals[4] = new mHospital { nombre_hospital = "Quetzaltenango", cola_infect = new Heap<mPaciente>(), cola_sospech = new Heap<mPaciente>(), camillas = new TablaHash<mPaciente>() };
                Caja_BD.Instance.first = true;
                Caja_Hospitales.Instance.Stadistica = new mEstadist { Activos = 0, Ingres_infect = 0, Ingres_sospech = 0, No_Infect = 0, Recuperados = 0 };
            }
            return View();
        }

        public ActionResult Index_info(string inf)
        {
            ViewBag.Info = inf;
            return View("Index");
        }

        public ActionResult Busquedas()
        {
            List<mPaciente> Lista = new List<mPaciente>();
            return View(Lista);
        }

        public ActionResult Buscar(string Texto, string Tipo)
        {
            List<mPaciente> Encontrados = new List<mPaciente>();
            List<mPaciente> Correct = new List<mPaciente>();
            mPaciente paciente = new mPaciente();
            switch (Tipo)
            {
                case "Nombre":
                    paciente.Nombre = Texto;
                    Encontrados = Caja_BD.Instance.arbol_Nom.Busqueda_Same(paciente, mPaciente.Comparar_Nombre);
                    break;
                case "Apellido":
                    paciente.Apellido = Texto;
                    Encontrados = Caja_BD.Instance.arbol_Ape.Busqueda_Same(paciente, mPaciente.Comparar_Apellido);
                    break;
                case "DPI":
                    paciente.Dpi = Texto;
                    if (Caja_BD.Instance.arbol_Dpi.Buscar(paciente, mPaciente.Comparar_DPI) != null)
                    {
                        Encontrados.Add(Caja_BD.Instance.arbol_Dpi.Buscar(paciente, mPaciente.Comparar_DPI));
                    }
                    break;
            }
            if (Encontrados.Count == 0 || Encontrados == null)
            {
                ViewBag.Info = "No se encontraron resultados";
            }
            try
            {
                foreach(mPaciente elem in Encontrados)
                {
                    if(elem==null)
                    {
                        Encontrados = null;
                    }
                    else
                    {
                        Correct.Add(elem);
                    }
                }
            }
            catch
            {
                ViewBag.Info = "No se encontraron resultados";
            }
            return View("Busquedas", Correct);

        }

        public ActionResult Estadisticas()
        {
            return View(Caja_Hospitales.Instance.Stadistica);
        }

        //Se habilita para ver todos los pacientes ingresados
       /* public ActionResult All_info()
        {
            return View(Caja_BD.Instance.List_all);
        }*/
    }
}