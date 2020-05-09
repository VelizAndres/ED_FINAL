using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_LabED_Andres.Herramientas.Interfaz
{
    interface ICola_Prioridad <T>
    {
        void Agregar(T valor, Delegate Det_Prioridad);
        T Eliminar(Delegate Comparar, Delegate Det_prioridad, Delegate Comparador_Val);
        T Buscar(T valor, Delegate Det_Prioridad);
    }
}
