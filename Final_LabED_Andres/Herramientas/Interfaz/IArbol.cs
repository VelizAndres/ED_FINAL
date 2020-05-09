using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_LabED_Andres.Herramientas.Interfaz
{
    interface IArbol <T>
    {
        void Agregar(T valor, Delegate comparar);
        void Eliminar(T valor, Delegate Comparar);
        T Buscar(T valor, Delegate comparar);
    }
}
