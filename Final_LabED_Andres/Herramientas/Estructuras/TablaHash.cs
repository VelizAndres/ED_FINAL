using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Final_LabED_Andres.Herramientas.Estructuras
{
    public class TablaHash <T>
    {
      public  T[] TablaHash_V = new T[10];
        public int cant = 0;

        public void Guardar_V(string llave, T nuevo_elemento, Delegate CodeHash)
        {
            int posicion = (int)CodeHash.DynamicInvoke(llave);
            if (TablaHash_V[posicion] == null)
            {
                TablaHash_V[posicion] = nuevo_elemento;
            }
            else
            {
                while(TablaHash_V[posicion]!=null)
                {
                    if(posicion==9)
                    {
                        posicion = 0;
                    }
                    else
                    {
                        posicion++;
                    }
                }
                TablaHash_V[posicion] = nuevo_elemento;
            }
            cant++;
        }

        public T Buscar(string llave, Delegate CodeHash, Delegate Obt_ValorInt)
        {
            int ciclo = 0;
            int posicion = (int)CodeHash.DynamicInvoke(llave);
            T Elemento_Buscado = default(T);
            while(llave!= (string)Obt_ValorInt.DynamicInvoke(TablaHash_V[posicion]) && ciclo<10)
            {
                ciclo++;
                if(posicion==9)
                {
                    posicion = 0;
                }
                else
                {
                    posicion++;
                }
            }
            if(llave == (string)Obt_ValorInt.DynamicInvoke(TablaHash_V[posicion]))
            {
                return TablaHash_V[posicion];
            }
            else
            {
                return Elemento_Buscado;
            }
        }

        public T[] Retorna_Tabla()
        {
            return TablaHash_V;
        }

        public void Eliminar(string llave, Delegate Obt_ValorInt, Delegate CodeHash)
        {
            int ciclo = 0;
            int ciclo2 = 0;
            int posicion = (int)CodeHash.DynamicInvoke(llave);
            while (TablaHash_V[posicion] == null && ciclo2<10)
            {
                ciclo2++;
                if (posicion == 9)
                {
                    posicion = 0;
                }
                else
                {
                    posicion++;
                }
            }
            if(ciclo2<10)
            {
                while (llave != (string)Obt_ValorInt.DynamicInvoke(TablaHash_V[posicion]) && ciclo < 10)
                {
                    ciclo++;
                    if (posicion == 9)
                    {
                        posicion = 0;
                    }
                    else
                    {
                        posicion++;
                    }
                }
                if (llave == (string)Obt_ValorInt.DynamicInvoke(TablaHash_V[posicion]))
                {
                    TablaHash_V[posicion] = default(T);
                    cant--;
                }
            }

        }
        
    }
}
