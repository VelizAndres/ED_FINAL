using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Final_LabED_Andres.Models;
using Final_LabED_Andres.Herramientas.Almacen;

namespace Final_LabED_Andres.Controllers
{
    public class HospitalesController : Controller
    {

        // GET: Hospitales/CrearPaciente
        public ActionResult CrearPaciente()
        {
            return View();
        }

        // POST: Hospitales/Create
        [HttpPost]
        public ActionResult CrearPaciente(FormCollection contenedor)
        {
            try
            {
                string info="";
                mPaciente paciente_nuevo = new mPaciente();
                paciente_nuevo.Nombre = contenedor["Nombre"];
                paciente_nuevo.Apellido = contenedor["Apellido"];
                paciente_nuevo.Dpi = contenedor["Dpi"];
                paciente_nuevo.Fecha_Nac = Convert.ToDateTime(contenedor["Fecha_Nac"]);
                paciente_nuevo.Municipio_resi = contenedor["Municipio_resi"];
                paciente_nuevo.Departamento_resi = contenedor["Depart"];
                //Determinar si esta infectado
                if (contenedor["infectado"].Equals("Si"))
                {
                    paciente_nuevo.Infectado = true;
                }
                else
                {
                    paciente_nuevo.Infectado = false;
                }
                paciente_nuevo.Fecha_ingreso = Convert.ToDateTime(contenedor["Fecha_ingreso"]);
                paciente_nuevo.Sintomas = contenedor["Sintomas"];
                paciente_nuevo.Descripcion_contagio = contenedor["Descripcion_contagio"];
         

                //Verificaciones  
                if (paciente_nuevo.Nombre==null || paciente_nuevo.Apellido == null || paciente_nuevo.Dpi==null || paciente_nuevo.Sintomas==null || paciente_nuevo.Descripcion_contagio==null)
                {
                    ViewBag.Error = "Debe ingresar datos a los campos de nombre, apellido, DPI, sintomas y descripción de contagio";
                    return View("CrearPaciente");
                }
                else if (paciente_nuevo.Nombre == "" || paciente_nuevo.Apellido == "" || paciente_nuevo.Dpi == "" || paciente_nuevo.Sintomas == "" || paciente_nuevo.Descripcion_contagio == "")
                {
                    ViewBag.Error = "Debe llenar datos a los campos de nombre, apellido, DPI, sintomas y descripción de contagio";
                    return View("CrearPaciente");
                }
                if(paciente_nuevo.Dpi.Length!=13)
                {
                    ViewBag.Error = "Ingresar DPI correcto";
                    return View("CrearPaciente");
                }
                try
                {
                    Convert.ToInt64(paciente_nuevo.Dpi);
                }
                catch
                {
                    ViewBag.Error = "Ingresar DPI correcto";
                    return View("CrearPaciente");
                }

                int fecha = (DateTime.Today.Day - paciente_nuevo.Fecha_Nac.Day) + ((DateTime.Today.Month - paciente_nuevo.Fecha_Nac.Month) * 30) + ((DateTime.Today.Year - paciente_nuevo.Fecha_Nac.Year) * 365);
                if(fecha<0)
                {
                    ViewBag.Error = "Ingresar Fecha de nacimiento  correcta";
                    return View("CrearPaciente");
                }
                fecha = (DateTime.Today.Day - paciente_nuevo.Fecha_ingreso.Day) + ((DateTime.Today.Month - paciente_nuevo.Fecha_ingreso.Month) * 30) + ((DateTime.Today.Year - paciente_nuevo.Fecha_ingreso.Year) * 365);
                if (fecha < 0)
                {
                    ViewBag.Error = "Ingresar Fecha de ingreso correcta";
                    return View("CrearPaciente");
                }
                //Finaliza Verificaciones

                //Determinar a que hospital debe ser enviado
                if (paciente_nuevo.Departamento_resi.Equals("Alta Verapaz")
                    || paciente_nuevo.Departamento_resi.Equals("Petén")
                    || paciente_nuevo.Departamento_resi.Equals("Izabal")
                    || paciente_nuevo.Departamento_resi.Equals("Quiché"))
                {
                    paciente_nuevo.Name_hosp = "Petén";
                }
                else if (paciente_nuevo.Departamento_resi.Equals("Guatemala")
                || paciente_nuevo.Departamento_resi.Equals("Baja Verapaz")
                || paciente_nuevo.Departamento_resi.Equals("El Progreso")
                || paciente_nuevo.Departamento_resi.Equals("Chimaltenango")
                || paciente_nuevo.Departamento_resi.Equals("Sacatepéquez"))
                {
                    paciente_nuevo.Name_hosp = "Guatemala";
                }
                else if (paciente_nuevo.Departamento_resi.Equals("Escuintla")
                || paciente_nuevo.Departamento_resi.Equals("Suchitepéquez")
                || paciente_nuevo.Departamento_resi.Equals("Retalhuleu")
                || paciente_nuevo.Departamento_resi.Equals("Sololá"))
                {
                    paciente_nuevo.Name_hosp = "Escuintla";
                }
                else if (paciente_nuevo.Departamento_resi.Equals("Chiquimula")
                || paciente_nuevo.Departamento_resi.Equals("Jalapa")
                || paciente_nuevo.Departamento_resi.Equals("Jutiapa")
                || paciente_nuevo.Departamento_resi.Equals("Santa Rosa")
                || paciente_nuevo.Departamento_resi.Equals("Zacapa"))
                {
                    paciente_nuevo.Name_hosp = "Chiquimula";
                }
                else if (paciente_nuevo.Departamento_resi.Equals("Quetzaltenango")
                || paciente_nuevo.Departamento_resi.Equals("Quiché")
                || paciente_nuevo.Departamento_resi.Equals("Huehuetenango")
                || paciente_nuevo.Departamento_resi.Equals("Totonicapán")
                || paciente_nuevo.Departamento_resi.Equals("San Marcos"))
                {
                    paciente_nuevo.Name_hosp = "Quetzaltenango";
                }
                double cant_year = ((DateTime.Today.Day - paciente_nuevo.Fecha_Nac.Day) + ((DateTime.Today.Month - paciente_nuevo.Fecha_Nac.Month) * 30) + ((DateTime.Today.Year - paciente_nuevo.Fecha_Nac.Year) * 365))/365;
                //Determinar Prioridad
                if (cant_year<1)
                {
                    if(paciente_nuevo.Infectado)
                    {
                        paciente_nuevo.Prioridad = 2;
                    }
                    else
                    {
                        paciente_nuevo.Prioridad = 6;
                    }

                }
                else if (cant_year < 18)
                {
                    if (paciente_nuevo.Infectado)
                    {
                        paciente_nuevo.Prioridad = 5;
                    }
                    else
                    {
                        paciente_nuevo.Prioridad = 8;
                    }
                }
                else if (cant_year < 60)
                {
                    if (paciente_nuevo.Infectado)
                    {
                        paciente_nuevo.Prioridad = 3;
                    }
                    else
                    {
                        paciente_nuevo.Prioridad = 7;
                    }
                }
                else 
                {
                    if (paciente_nuevo.Infectado)
                    {
                        paciente_nuevo.Prioridad = 1;
                    }
                    else
                    {
                        paciente_nuevo.Prioridad = 4;
                    }
                }
           
                
                //Agregar a los arboles
                Caja_BD.Instance.arbol_Dpi.Agregar(paciente_nuevo, mPaciente.Comparar_DPI);
                Caja_BD.Instance.arbol_Nom.Agregar(paciente_nuevo, mPaciente.Comparar_Nombre);
                Caja_BD.Instance.arbol_Ape.Agregar(paciente_nuevo, mPaciente.Comparar_Apellido);
                //Seleccionar a que hospital ira
                int pos = 0;
                if(paciente_nuevo.Name_hosp.Equals("Petén")) { pos = 1; }
                else if (paciente_nuevo.Name_hosp.Equals("Chiquimula")) { pos = 2; }
                else if (paciente_nuevo.Name_hosp.Equals("Escuintla")) { pos = 3; }
                else if (paciente_nuevo.Name_hosp.Equals("Quetzaltenango")) { pos = 4; }

                //Agregar a la cola
                if(paciente_nuevo.Infectado)
                {
                    Caja_Hospitales.Instance.hospitals[pos].cola_infect.Agregar(paciente_nuevo,mPaciente.EsPrioritario);
                    if(Caja_Hospitales.Instance.hospitals[pos].camillas.cant<10)
                    {
                        Asignar_Camilla(paciente_nuevo, pos);
                        info = "El paciente " + paciente_nuevo.Nombre + " " + paciente_nuevo.Apellido + ", identificado con el DPI: " + paciente_nuevo.Dpi + ". Fue ingresado a la sala de camillas en el hospital "+paciente_nuevo.Name_hosp+".";
                    }
                    else {

                        info = "El paciente " + paciente_nuevo.Nombre + " " + paciente_nuevo.Apellido + ", identificado con el DPI: " + paciente_nuevo.Dpi + ". Fue ingresado a la sala de infectado en el hospital "+paciente_nuevo.Name_hosp+".";
                        Caja_Hospitales.Instance.Stadistica.Activos++;
                    }
                    Caja_Hospitales.Instance.Stadistica.Ingres_infect++;
                }
                else
                {
                    Caja_Hospitales.Instance.hospitals[pos].cola_sospech.Agregar(paciente_nuevo, mPaciente.EsPrioritario);
                    Caja_Hospitales.Instance.Stadistica.Ingres_sospech++;
                    info = "El paciente " + paciente_nuevo.Nombre + " " + paciente_nuevo.Apellido + ", identificado con el DPI: " + paciente_nuevo.Dpi + ". Fue ingresado a la sala de sospechosos en el hospital "+ paciente_nuevo.Name_hosp+".";
                }
                Caja_BD.Instance.List_all.Add(paciente_nuevo);
                  return RedirectToAction("Index_info", "Home",new { inf = info });
            }
            catch
            {
                ViewBag.Error = "Datos Incorrectos";
                return View("CrearPaciente");
            }
        }


        // GET: Hospitales/MenuHospital
        public ActionResult MenuHospital(string hosp)
        {
            switch(hosp)
            {
                case "Guatemala":
                    ViewBag.Hospital = "Guatemala";
                    break;
                case "Petén":
                    ViewBag.Hospital = "Petén";
                    break;
                case "Chiquimula":
                    ViewBag.Hospital = "Chiquimula";
                    break;
                case "Escuintla":
                    ViewBag.Hospital = "Escuintla";
                    break;
                case "Quetzaltenango":
                    ViewBag.Hospital = "Quetzaltenango";
                    break;
            }
            return View();
        }

        // GET: Hospitales/MenuHospital/Sospechosos
        public ActionResult Sospechosos(string hosp)
        {
            mPaciente Paciente_Raiz = new mPaciente();
            switch (hosp)
            {
                case "Guatemala":
                    if (Caja_Hospitales.Instance.hospitals[0].cola_sospech.raiz != null)
                    { 
                    Paciente_Raiz = Caja_Hospitales.Instance.hospitals[0].cola_sospech.raiz.Valor;
                    }
                    ViewBag.Hospital = "Guatemala";
                    break;
                case "Petén":
                    if (Caja_Hospitales.Instance.hospitals[1].cola_sospech.raiz != null)
                    {
                        Paciente_Raiz = Caja_Hospitales.Instance.hospitals[1].cola_sospech.raiz.Valor;
                    } ViewBag.Hospital = "Petén";
                    break;
                case "Chiquimula":
                    if (Caja_Hospitales.Instance.hospitals[2].cola_sospech.raiz != null)
                    {
                        Paciente_Raiz = Caja_Hospitales.Instance.hospitals[2].cola_sospech.raiz.Valor;
                    }
                    ViewBag.Hospital = "Chiquimula";
                    break;
                case "Escuintla":
                    if (Caja_Hospitales.Instance.hospitals[3].cola_sospech.raiz != null)
                    {
                        Paciente_Raiz = Caja_Hospitales.Instance.hospitals[3].cola_sospech.raiz.Valor;
                    }
                    ViewBag.Hospital = "Escuintla";
                    break;
                case "Quetzaltenango":
                    if (Caja_Hospitales.Instance.hospitals[4].cola_sospech.raiz != null)
                    {
                        Paciente_Raiz = Caja_Hospitales.Instance.hospitals[4].cola_sospech.raiz.Valor;
                    }
                    ViewBag.Hospital = "Quetzaltenango";
                    break;
            }
            return View(Paciente_Raiz);

        }

        // GET: Hospitales/MenuHospital/Infectados
        public ActionResult Infectados(string hosp)
        {
            mPaciente Paciente_Raiz = new mPaciente();
            switch (hosp)
            {
                case "Guatemala":
                    if (Caja_Hospitales.Instance.hospitals[0].cola_infect.raiz != null)
                    {
                        Paciente_Raiz = Caja_Hospitales.Instance.hospitals[0].cola_infect.raiz.Valor;
                    }
                    ViewBag.Hospital = "Guatemala";

                    break;
                case "Petén":
                    if (Caja_Hospitales.Instance.hospitals[1].cola_infect.raiz != null)
                    {
                        Paciente_Raiz = Caja_Hospitales.Instance.hospitals[1].cola_infect.raiz.Valor;
                    }
                    ViewBag.Hospital = "Petén";
                    break;
                case "Chiquimula":
                    if (Caja_Hospitales.Instance.hospitals[2].cola_infect.raiz != null)
                    {
                        Paciente_Raiz = Caja_Hospitales.Instance.hospitals[2].cola_infect.raiz.Valor;
                    }
                    ViewBag.Hospital = "Chiquimula";
                    break;
                case "Escuintla":
                    if (Caja_Hospitales.Instance.hospitals[3].cola_infect.raiz != null)
                    {
                        Paciente_Raiz = Caja_Hospitales.Instance.hospitals[3].cola_infect.raiz.Valor;
                    }
                    ViewBag.Hospital = "Escuintla";
                    break;
                case "Quetzaltenango":
                    if (Caja_Hospitales.Instance.hospitals[4].cola_infect.raiz != null)
                    {
                        Paciente_Raiz = Caja_Hospitales.Instance.hospitals[4].cola_infect.raiz.Valor;
                    }
                    ViewBag.Hospital = "Quetzaltenango";
                    break;
            }
            return View(Paciente_Raiz);
        }


        // GET: Hospitales/Simular/
        public ActionResult Simular(string dpi)
        {
            mPaciente paciente = new mPaciente { Dpi = dpi };
            paciente = Caja_BD.Instance.arbol_Dpi.Buscar(paciente, mPaciente.Comparar_DPI);
            string Hosp_name = paciente.Name_hosp;
            int pos = 0;
            switch (paciente.Name_hosp)
            {
                case "Guatemala":
                    pos=0;
                    break;
                case "Petén":
                    pos = 1;
                    break;
                case "Chiquimula":
                    pos = 2;
                    break;
                case "Escuintla":
                    pos = 3;
                    break;
                case "Quetzaltenango":
                    pos = 4;
                    break;
            }
            Caja_Hospitales.Instance.hospitals[pos].cola_sospech.Eliminar(mPaciente.Comparar_Prioridad, mPaciente.EsPrioritario,mPaciente.Comparar_DPI);
            if (Simulacion(paciente.Descripcion_contagio))
            {
                Caja_BD.Instance.arbol_Dpi.Modificar_Status(paciente, mPaciente.Cambiar_infect, mPaciente.Comparar_DPI, mPaciente.Comparar_DPI);
                /*  Caja_BD.Instance.arbol_Nom.Modificar_Status(paciente, mPaciente.Cambiar_infect, mPaciente.Comparar_Nombre, mPaciente.Comparar_DPI);
                  Caja_BD.Instance.arbol_Ape.Modificar_Status(paciente, mPaciente.Cambiar_infect, mPaciente.Comparar_Apellido, mPaciente.Comparar_DPI);
                 */
                Caja_Hospitales.Instance.hospitals[pos].cola_infect.Agregar(Caja_BD.Instance.arbol_Dpi.Buscar(paciente, mPaciente.Comparar_DPI), mPaciente.EsPrioritario);
                if (Caja_Hospitales.Instance.hospitals[pos].camillas.cant < 10)
                {
                    Asignar_Camilla(paciente, pos);
                    ViewBag.Info = "El paciente " + paciente.Nombre + " " + paciente.Apellido + ", identificado con el DPI: " + paciente.Dpi + ". Fue ingresado a una camilla.";
                }
                else
                {
                    Caja_Hospitales.Instance.Stadistica.Activos++;
                    ViewBag.Info = "El paciente " + paciente.Nombre + " " + paciente.Apellido + ", identificado con el DPI: " + paciente.Dpi + ". Está infectado.";
                }
            }
            else
            {
                Caja_BD.Instance.arbol_Dpi.Modificar_Status(paciente, mPaciente.Cambiar_Noinfect, mPaciente.Comparar_DPI, mPaciente.Comparar_DPI);
                Caja_Hospitales.Instance.Stadistica.No_Infect++;
                ViewBag.Info = "El paciente " + paciente.Nombre + " " + paciente.Apellido + ", identificado con el DPI: " + paciente.Dpi + ". No está infectado.";
            }
            ViewBag.Hospital = Hosp_name;
            return View("MenuHospital");
        }

        private bool Simulacion(string descripcion)
        {
            Random Rand = new Random();
            int rand=Rand.Next(0, 100);
            int prob_infect = 5;
            string descrip = descripcion;
            string[] Contenedor_descrip = descrip.ToLower().Split(Convert.ToChar(" "));
            bool viaje = false;
            bool viaje2 = false;
            bool viaje_pase = false;
            bool conocido= false;
            bool conocido_pase = false;
            bool familia = false;
            bool familia_pase = false;
            bool contagio = false;
            bool reunion_sospech = false;
            bool reunion_sospech2 = false;
            bool reunion_pase = false;

            //Busqueda de palabras claves y si es necesario sumarle a la probabilidad 
            foreach (string palabra in Contenedor_descrip)
            {
                //Detencción de palabras claves
                if ((palabra.Equals("viaje") || palabra.Equals("ir") || palabra.Equals("fue") || palabra.Equals("visita") || palabra.Equals("paseo")) && viaje == false)
                {
                    viaje = true;
                }
                if ((palabra.Equals("europa") || palabra.Equals("italia") || palabra.Equals("españa") || palabra.Equals("china") || palabra.Equals("japon") || palabra.Equals("francia") || palabra.Equals("paris") || palabra.Equals("rusia") || palabra.Equals("holanda")) && viaje2 == false)
                {
                    viaje2 = true;
                }
                if ((palabra.Equals("conocido") || palabra.Equals("compañero") || palabra.Equals("compañera") || palabra.Equals("amigo") || palabra.Equals("amiga") || palabra.Equals("jefe") || palabra.Equals("empleado")) && conocido == false)
                {
                    conocido = true;
                }
                if ((palabra.Equals("papá") || palabra.Equals("papa") || palabra.Equals("mamá") || palabra.Equals("mama") || palabra.Equals("hermano") || palabra.Equals("hermana") || palabra.Equals("hijo") || palabra.Equals("hija") || palabra.Equals("tio") || palabra.Equals("tia") || palabra.Equals("primo") || palabra.Equals("prima") || palabra.Equals("abuela") || palabra.Equals("abuelo") || palabra.Equals("sobrino") || palabra.Equals("sobrina")) && familia == false)
                {
                    familia = true;
                }
                if ((palabra.Equals("contagiado") || palabra.Equals("infetado") || palabra.Equals("positivo") || palabra.Equals("confirmado")) && contagio == false)
                {
                    contagio = true;
                }
                if ((palabra.Equals("reunión") || palabra.Equals("reunion") || palabra.Equals("social") || palabra.Equals("junta") || palabra.Equals("fiesta")) && reunion_sospech == false)
                {
                    reunion_sospech = true;
                }
                if ((palabra.Equals("sospechoso") || palabra.Equals("posible")) && reunion_sospech2 == false)
                {
                    reunion_sospech2 = true;
                } 

                //Determinar si cumplen ambas pares
                if(viaje && viaje2 && !viaje_pase)
                {
                    prob_infect += 10;
                    viaje_pase = true;
                }
                if (conocido && contagio && !conocido_pase)
                {
                    prob_infect += 10;
                    conocido_pase= true;
                    contagio = false;
                }
                if (familia && contagio && !familia_pase)
                {
                    prob_infect += 15;
                    familia_pase = true;
                    contagio = false;
                }
                if (reunion_sospech && reunion_sospech2 && !reunion_pase)
                {
                    prob_infect += 5;
                    reunion_pase = true;
                }
            }

            return prob_infect > rand ? true : false;
        }

        // GET: Hospitales/Sanar/
        public ActionResult Curar(string dpi)
        {
            mPaciente paciente = new mPaciente { Dpi = dpi };
            paciente = Caja_BD.Instance.arbol_Dpi.Buscar(paciente, mPaciente.Comparar_DPI);
            string Hosp_name = paciente.Name_hosp;
            int pos = 0;
            switch (paciente.Name_hosp)
            {
                case "Guatemala":
                    pos = 0;
                    break;
                case "Petén":
                    pos = 1;
                    break;
                case "Chiquimula":
                    pos = 2;
                    break;
                case "Escuintla":
                    pos = 3;
                    break;
                case "Quetzaltenango":
                    pos = 4;
                    break;
            }
            Caja_BD.Instance.arbol_Dpi.Modificar_Status(paciente, mPaciente.Cambiar_sano, mPaciente.Comparar_DPI, mPaciente.Comparar_DPI);
            Caja_Hospitales.Instance.hospitals[pos].camillas.Eliminar(paciente.Dpi,mPaciente.Obt_DPI,mPaciente.Del_CodeHash);
            Caja_Hospitales.Instance.Stadistica.Recuperados++;
            Caja_Hospitales.Instance.Stadistica.Activos--;
            mPaciente paciente_aux= Caja_Hospitales.Instance.hospitals[pos].cola_infect.Eliminar(mPaciente.Comparar_Prioridad,mPaciente.EsPrioritario,mPaciente.Comparar_DPI);
            if (paciente_aux != null)
            {
                Caja_Hospitales.Instance.hospitals[pos].camillas.Guardar_V(paciente_aux.Dpi, paciente_aux, mPaciente.Del_CodeHash);
                Caja_Hospitales.Instance.Stadistica.Activos++;
            }
            ViewBag.Info = "El paciente " + paciente.Nombre + " " + paciente.Apellido + ", identificado con el DPI: " + paciente.Dpi + ". Se ha recuperado.";
            ViewBag.Hospital = Hosp_name;
            return View("MenuHospital");
        }

        public void Asignar_Camilla(mPaciente paciente_nuevo, int pos)
        {
            Caja_Hospitales.Instance.hospitals[pos].cola_infect.Eliminar(mPaciente.Comparar_Prioridad,mPaciente.EsPrioritario,mPaciente.Comparar_DPI);
            Caja_Hospitales.Instance.hospitals[pos].camillas.Guardar_V(paciente_nuevo.Dpi, paciente_nuevo, mPaciente.Del_CodeHash);
            Caja_Hospitales.Instance.Stadistica.Activos++;
        }


        //GET: Hospitales/Camas_Hash
        public ActionResult Camas_Hash(string hosp)
        {
            List<mPaciente> Habitacion = new List<mPaciente>();
            string vacias = "";
            switch (hosp)
            {
                case "Guatemala":
                    if (Caja_Hospitales.Instance.hospitals[0].camillas != null)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (Caja_Hospitales.Instance.hospitals[0].camillas.TablaHash_V[i]!=null)
                            {
                                Habitacion.Add(Caja_Hospitales.Instance.hospitals[0].camillas.TablaHash_V[i]);
                            }
                            else
                            {
                                vacias += "-" + (i + 1);
                            }
                        }
                    }
                    ViewBag.Hospital = "Guatemala";
                    ViewBag.Vacias = vacias;
                    break;
                case "Petén":
                    if (Caja_Hospitales.Instance.hospitals[1].camillas != null)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (Caja_Hospitales.Instance.hospitals[1].camillas.TablaHash_V[i] != null)
                            {
                                Habitacion.Add(Caja_Hospitales.Instance.hospitals[1].camillas.TablaHash_V[i]);
                            }
                            else
                            {
                                vacias += "-" + (i + 1);
                            }
                        }
                    }
                    ViewBag.Hospital = "Petén";
                    ViewBag.Vacias = vacias;
                    break;
                case "Chiquimula":
                    if (Caja_Hospitales.Instance.hospitals[2].camillas != null)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (Caja_Hospitales.Instance.hospitals[2].camillas.TablaHash_V[i] != null)
                            {
                                Habitacion.Add(Caja_Hospitales.Instance.hospitals[2].camillas.TablaHash_V[i]);
                            }
                            else
                            {
                                vacias += "-" + (i + 1);
                            }
                        }
                    }
                    ViewBag.Vacias = vacias;
                    ViewBag.Hospital = "Chiquimula";
                    break;
                case "Escuintla":
                    if (Caja_Hospitales.Instance.hospitals[3].camillas != null)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (Caja_Hospitales.Instance.hospitals[3].camillas.TablaHash_V[i] != null)
                            {
                                Habitacion.Add(Caja_Hospitales.Instance.hospitals[3].camillas.TablaHash_V[i]);
                            }
                            else
                            {
                                vacias += "-" + (i + 1);
                            }
                        }
                    }
                    ViewBag.Vacias = vacias;
                    ViewBag.Hospital = "Escuintla";
                    break;
                case "Quetzaltenango":
                    if (Caja_Hospitales.Instance.hospitals[4].camillas!= null)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            if (Caja_Hospitales.Instance.hospitals[4].camillas.TablaHash_V[i] != null)
                            {
                                Habitacion.Add(Caja_Hospitales.Instance.hospitals[4].camillas.TablaHash_V[i]);
                            }
                            else
                            {
                                vacias += "-" + (i + 1);
                            }
                        }
                    }
                    ViewBag.Vacias = vacias;
                    ViewBag.Hospital = "Quetzaltenango";
                    break;
            }
            return View(Habitacion);
        }



    }
}
