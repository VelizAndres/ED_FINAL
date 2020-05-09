using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Final_LabED_Andres.Herramientas.Interfaz;

namespace Final_LabED_Andres.Herramientas.Estructuras
{
    public class BiArbol <T> : IArbol<T>
    {
        public Nodo<T> raiz;

        /// <summary>
        /// Metodo para agregar el valor al árbol
        /// </summary>
        /// <param name="valor">Valor que se desea guardar</param>
        /// <param name="comparar">Delegado para realizar comparaciones</param>
        public void Agregar(T valor, Delegate comparar)
        {
            Nodo<T> Nuevo = new Nodo<T>();
            Nuevo.Valor = valor;
            if (raiz == null)
            {
                raiz = Nuevo;
            }
            else
            {
                if ((int)comparar.DynamicInvoke(raiz.Valor, Nuevo.Valor) < 0)
                {
                    if (raiz.Hijoder == null)
                    {
                        raiz.Hijoder = Nuevo;
                        raiz.Hijoder.Padre = raiz;
                    }
                    else
                    {
                        Recorrer_Asig(raiz.Hijoder, Nuevo, comparar);
                    }
                }
                //hijo izquierdo
                else
                {
                    if (raiz.Hijoizq == null)
                    {
                        raiz.Hijoizq = Nuevo;
                        raiz.Hijoizq.Padre = raiz;
                    }
                    else
                    {
                        Recorrer_Asig(raiz.Hijoizq, Nuevo, comparar);
                    }
                }
                raiz.Altura = Det_Altura(raiz.Hijoder, raiz.Hijoizq);
                Rotaciones(raiz, comparar);
                raiz.Altura = Det_Altura(raiz.Hijoder, raiz.Hijoizq);
            }
        }

        private void Recorrer_Asig(Nodo<T> dad, Nodo<T> nuevo, Delegate Comparar)
        {
            if ((int)Comparar.DynamicInvoke(dad.Valor, nuevo.Valor) < 0)
            {
                if (dad.Hijoder == null)
                {
                    dad.Hijoder = nuevo;
                    dad.Hijoder.Padre = dad;
                }
                else
                {
                    //se debe buscar un valor vacio(recursividad)
                    Recorrer_Asig(dad.Hijoder, nuevo, Comparar);
                }
            }
            else
            {
                if (dad.Hijoizq == null)
                {
                    dad.Hijoizq = nuevo;
                    dad.Hijoizq.Padre = dad;
                }
                else
                {
                    //se debe buscar un valor vacio(recursividad)
                    Recorrer_Asig(dad.Hijoizq, nuevo, Comparar);
                }
            }
            dad.Altura = Det_Altura(dad.Hijoder, dad.Hijoizq);
            Rotaciones(dad, Comparar);
            dad.Altura = Det_Altura(dad.Hijoder, dad.Hijoizq);
        }



        /// <summary>
        /// Metodo de busqueda
        /// </summary>
        /// <param name="valor">Valor que se desea buscar</param>
        /// <param name="comparar">Delegador para comparaciones</param>
        /// <returns>Valor encontrando</returns>
        public T Buscar(T valor, Delegate comparar)
        {
            Nodo<T> vacio = new Nodo<T>();
            Nodo<T> nodoBuscado = new Nodo<T>();
            nodoBuscado.Valor = valor;
            if (raiz == null)
            {
                return vacio.Valor;
            }
            else
            {
                return Recorrer_Busqueda(raiz, nodoBuscado, comparar).Valor;
            }

        }


        /// <summary>
        /// Metodo para recorrer el arbol para buscar
        /// </summary>
        /// <param name="dad">Nodo sobre el cual se realiza la comparacion</param>
        /// <param name="buscado">Nodo que se esta buscando</param>
        /// <param name="Comparar">Delegado de comparacion</param>
        /// <returns>Nodo donde se encuentra el valor que se busca</returns>
        private Nodo<T> Recorrer_Busqueda(Nodo<T> dad, Nodo<T> buscado, Delegate Comparar)
        {
            Nodo<T> vacio = new Nodo<T>();
            int resulcompar = (int)Comparar.DynamicInvoke(dad.Valor, buscado.Valor);
            if (resulcompar == 0)
            {
                return dad;
            }
            if (resulcompar < 0)
            {
                //se debe buscar un valor vacio(recursividad)
                if (dad.Hijoder != null)
                {
                    return Recorrer_Busqueda(dad.Hijoder, buscado, Comparar);
                }
                return vacio;
            }
            else
            {
                //se debe buscar un valor vacio(recursividad)
                if (dad.Hijoizq != null)
                {
                    return Recorrer_Busqueda(dad.Hijoizq, buscado, Comparar);
                }
                return vacio;
            }
        }
      
        
        /// <summary>
        /// Metodo para realizar una busqueda grupal
        /// </summary>
        /// <param name="valor">Valor que se busca</param>
        /// <param name="comparar">Delegado para realizar comparacion</param>
        /// <returns>Lista de elementos que posean el mismo valor</returns>
        public List<T> Busqueda_Same(T valor, Delegate comparar)
        {
            List<T> list_elementos = new List<T>();
            Nodo<T> nodoBuscado = new Nodo<T>();
            nodoBuscado.Valor = valor;
            if (raiz == null)
            {
                return list_elementos;
            }
            else
            {
             Nodo<T> first_busq = Recorrer_Busqueda(raiz, nodoBuscado, comparar);
                if(first_busq!=null && first_busq.Valor !=null)
                {
                    list_elementos.Add(first_busq.Valor);
                    foreach (T element in Enredo_Busqueda(first_busq.Hijoder, valor, comparar))
                    {
                        list_elementos.Add(element);
                    }
                    foreach (T element in Enredo_Busqueda(first_busq.Hijoizq, valor, comparar))
                    {
                        list_elementos.Add(element);
                    }
                }
                return list_elementos;
            }
        }

        /// <summary>
        /// Metodo para agregar todos los valores repetidos encontrados en la busqueda
        /// </summary>
        /// <param name="guia">Nodo donde se encontro el valor</param>
        /// <param name="valor">valor que se busca</param>
        /// <param name="comparar">Delegado para realizar comparacion</param>
        /// <returns></returns>
        private List<T> Enredo_Busqueda(Nodo<T> guia, T valor, Delegate comparar)
        {
            List<T> elementos = new List<T>();
            Nodo<T> nodoBuscado = new Nodo<T>();
            nodoBuscado.Valor = valor;
            if(guia!=null)
            {
                Nodo<T> Aux = Recorrer_Busqueda(guia, nodoBuscado, comparar);
                if (Aux != null)
                {
                    elementos.Add(Aux.Valor);
                    foreach (T element in Enredo_Busqueda(guia.Hijoder, valor, comparar))
                    {
                        elementos.Add(element);
                    }
                    foreach (T element in Enredo_Busqueda(guia.Hijoizq, valor, comparar))
                    {
                        elementos.Add(element);
                    }
                }
            }
            return elementos;
        }


        /// <summary>
        /// Metodo para modificar el estado de un valor
        /// </summary>
        /// <param name="valor">Valor que se modificara</param>
        /// <param name="Modificar_val">Delegado para la modificación</param>
        /// <param name="ComparadorPrincipal">Delegado de comparación principal</param>
        /// <param name="ComparadorSecund">Delegado de comparación secundario</param>
        public void Modificar_Status(T valor, Delegate Modificar_val,Delegate ComparadorPrincipal, Delegate ComparadorSecund)
        {
            Nodo<T> nodoBuscado = new Nodo<T>();
            nodoBuscado.Valor = valor;
            if (raiz == null)
            {
            }
            else
            {
                Nodo_regresadero(raiz, valor, ComparadorPrincipal, ComparadorSecund, Modificar_val);
            }
        }

        /// <summary>
        /// Metodo que busca el nodo que se le cambiara el valor
        /// </summary>
        /// <param name="guia">Nodo sub-raiz</param>
        /// <param name="valor">Valor que se esta buscando</param>
        /// <param name="ComparadorPrincipal">Delegado comparador principal</param>
        /// <param name="ComparadorSecund">Delegado comparador secundario</param>
        /// <param name="Modificar_val">Delegado para realizar la modificación al valor</param>
        private void Nodo_regresadero(Nodo<T> guia, T valor, Delegate ComparadorPrincipal, Delegate ComparadorSecund, Delegate Modificar_val)
        {
            if (guia != null)
            { 
            Nodo<T> nodoBuscado = new Nodo<T>();
            nodoBuscado.Valor = valor;
            Nodo<T> nodo_find = Recorrer_Busqueda(guia, nodoBuscado, ComparadorPrincipal);
            if (nodo_find != null)
            { 
                if ((int)ComparadorSecund.DynamicInvoke(nodo_find.Valor, nodoBuscado.Valor) == 0)
                {
                    Modificar_val.DynamicInvoke(nodo_find.Valor);
                }
                else
                {
                    Nodo_regresadero(nodo_find.Hijoder, valor, ComparadorPrincipal, ComparadorSecund, Modificar_val);
                    Nodo_regresadero(nodo_find.Hijoizq, valor, ComparadorPrincipal, ComparadorSecund, Modificar_val);
                }
            }
            }
        }


        /************Metodos de eliminacion****************/
        public void Eliminar(T valor, Delegate Comparar)
        {
            Nodo<T> nododel = new Nodo<T>();
            nododel.Valor = valor;
            int comparcion_valor = (int)Comparar.DynamicInvoke(raiz.Valor, nododel.Valor);
            if (comparcion_valor == 0)
            {
                int hijos = 0;
                if (raiz.Hijoder != null) { hijos++; }
                if (raiz.Hijoizq != null) { hijos++; }
                Metodo_Eliminacion_Raiz(hijos, raiz, Comparar);
            }
            else
            {
                //Hijo derecho
                if (comparcion_valor < 0)
                {
                    Buscar_Nodo_Eliminar(raiz.Hijoder, nododel, Comparar);
                }
                //hijo izquierdo
                else
                {
                    Buscar_Nodo_Eliminar(raiz.Hijoizq, nododel, Comparar);
                }
            }

            raiz.Altura = Det_Altura(raiz.Hijoder, raiz.Hijoizq);
            Rotaciones(raiz, Comparar);

        }

        private void Buscar_Nodo_Eliminar(Nodo<T> padre, Nodo<T> del_nodo, Delegate Comparar)
        {
            int resulcompar = (int)Comparar.DynamicInvoke(padre.Valor, del_nodo.Valor);
            if (resulcompar == 0)
            {
                int hijos = 0;
                if (padre.Hijoder != null) { hijos++; }
                if (padre.Hijoizq != null) { hijos++; }
                Metodo_Eliminacion(hijos, padre, Comparar);
            }
            else
            {
                if (resulcompar < 0)
                {
                    Buscar_Nodo_Eliminar(padre.Hijoder, del_nodo, Comparar);
                }
                //hijo izquierdo
                else
                {
                    Buscar_Nodo_Eliminar(padre.Hijoizq, del_nodo, Comparar);
                }
            }
            padre.Altura = Det_Altura(padre.Hijoder, padre.Hijoizq);
            Rotaciones(padre, Comparar);
        }

        private void Metodo_Eliminacion(int tipo, Nodo<T> Nodo_Borrar, Delegate Comparar)
        {
            int valor_com = (int)Comparar.DynamicInvoke(Nodo_Borrar.Padre.Valor, Nodo_Borrar.Valor);
            if (tipo == 0)
            {
                if (valor_com < 0)
                {
                    Nodo_Borrar.Padre.Hijoder = null;
                }
                else
                {
                    Nodo_Borrar.Padre.Hijoizq = null;
                }
            }
            if (tipo == 1)
            {
                if (Nodo_Borrar.Hijoder != null)
                {
                    if (valor_com < 0)
                    {
                        Nodo_Borrar.Padre.Hijoder = Nodo_Borrar.Hijoder;
                    }
                    else
                    {
                        Nodo_Borrar.Padre.Hijoizq = Nodo_Borrar.Hijoder;
                    }
                    Nodo_Borrar.Hijoder.Padre = Nodo_Borrar.Padre;
                }
                else
                {
                    if (valor_com < 0)
                    {
                        Nodo_Borrar.Padre.Hijoder = Nodo_Borrar.Hijoizq;
                    }
                    else
                    {
                        Nodo_Borrar.Padre.Hijoizq = Nodo_Borrar.Hijoizq;
                    }
                    Nodo_Borrar.Hijoizq.Padre = Nodo_Borrar.Padre;
                }
            }
            if (tipo == 2)
            {
                Nodo<T> Mayor_Izq = Obtener_MayorIzq(Nodo_Borrar.Hijoizq);
                Buscar_Nodo_Eliminar(Mayor_Izq.Padre, Mayor_Izq, Comparar);
                if (valor_com < 0)
                {
                    Nodo_Borrar.Padre.Hijoder = Mayor_Izq;
                }
                else
                {
                    Nodo_Borrar.Padre.Hijoizq = Mayor_Izq;
                }
                Mayor_Izq.Padre = Nodo_Borrar.Padre;
                Mayor_Izq.Hijoder = Nodo_Borrar.Hijoder;
                Mayor_Izq.Hijoizq = Nodo_Borrar.Hijoizq;
                Mayor_Izq.Hijoder.Padre = Mayor_Izq;
                Mayor_Izq.Hijoizq.Padre = Mayor_Izq;
                Mayor_Izq.Altura = Det_Altura(Mayor_Izq.Hijoder, Mayor_Izq.Hijoizq);
            }
        }

        private void Metodo_Eliminacion_Raiz(int tipo, Nodo<T> Nodo_Borrar, Delegate Comparar)
        {
            if (tipo == 0)
            {
                raiz = null;
            }
            if (tipo == 1)
            {
                if (Nodo_Borrar.Hijoder != null)
                {
                    raiz = Nodo_Borrar.Hijoder;
                }
                else
                {
                    raiz = Nodo_Borrar.Hijoizq;
                }
            }
            if (tipo == 2)
            {
                Nodo<T> Mayor_Izq = Obtener_MayorIzq(Nodo_Borrar.Hijoizq);
                Buscar_Nodo_Eliminar(Mayor_Izq.Padre, Mayor_Izq, Comparar);
                raiz = Mayor_Izq;
                Mayor_Izq.Padre = null;
                Mayor_Izq.Hijoder = Nodo_Borrar.Hijoder;
                Mayor_Izq.Hijoizq = Nodo_Borrar.Hijoizq;
                Mayor_Izq.Hijoder.Padre = Mayor_Izq;
                Mayor_Izq.Hijoizq.Padre = Mayor_Izq;
                Mayor_Izq.Altura = Det_Altura(Mayor_Izq.Hijoder, Mayor_Izq.Hijoizq);
            }
        }

        private Nodo<T> Obtener_MayorIzq(Nodo<T> guia)
        {
            if (guia.Hijoder == null)
            {
                return guia;
            }
            return Obtener_MayorIzq(guia.Hijoder);
        }
        /************Finaliza metodos de eliminacion*******/


        /************Rotaciones****************/
        private int Det_Altura(Nodo<T> NHder, Nodo<T> NHizq)
        {
            if (NHder == null && NHizq == null)
            {
                return 0;
            }
            if (NHder != null && NHizq == null)
            {
                return NHder.Altura + 1;
            }
            if (NHizq != null && NHder == null)
            {
                return NHizq.Altura + 1;
            }
            else
            {
                if (NHder.Altura >= NHizq.Altura)
                {
                    return NHder.Altura + 1;
                }
                else
                {
                    return NHizq.Altura + 1;
                }
            }
        }

        private int Det_FactEqui(Nodo<T> NHder, Nodo<T> NHizq)
        {
            if (NHder == null && NHizq == null)
            {
                return 0;
            }
            if (NHder != null && NHizq == null)
            {
                return NHder.Altura + 1;
            }
            if (NHizq != null && NHder == null)
            {
                return -NHizq.Altura - 1;
            }
            else
            {
                return NHder.Altura - NHizq.Altura;
            }
        }

        private void Rotaciones(Nodo<T> N_dad, Delegate Comparador)
        {
            int Fact_Equi = Det_FactEqui(N_dad.Hijoder, N_dad.Hijoizq);
            if (Fact_Equi == 2)
            {
                if (Det_FactEqui(N_dad.Hijoder.Hijoder, N_dad.Hijoder.Hijoizq) == -1)
                {
                    //rotacion doble izquierda
                    Rot_Simple_Derecha(N_dad.Hijoder, Comparador);

                    if (N_dad == raiz)
                    {
                        Rot_Simple_Izquierda_Raiz(N_dad, Comparador);
                    }
                    else
                    {
                        Rot_Simple_Izquierda(N_dad, Comparador);
                    }
                }
                else
                {
                    //rotacion simple izquierda
                    if (N_dad == raiz)
                    {
                        Rot_Simple_Izquierda_Raiz(N_dad, Comparador);
                    }
                    else
                    {
                        Rot_Simple_Izquierda(N_dad, Comparador);
                    }
                }
            }
            if (Fact_Equi == -2)
            {
                if (Det_FactEqui(N_dad.Hijoizq.Hijoder, N_dad.Hijoizq.Hijoizq) == 1)
                {
                    //rotacion doble derecha
                    Rot_Simple_Izquierda(N_dad.Hijoizq, Comparador);

                    if (N_dad == raiz)
                    {
                        Rot_Simple_Derecha_Raiz(N_dad, Comparador);
                    }
                    else
                    {
                        Rot_Simple_Derecha(N_dad, Comparador);
                    }
                }
                else
                {
                    //rotacion simple derecha
                    if (N_dad == raiz)
                    {
                        Rot_Simple_Derecha_Raiz(N_dad, Comparador);
                    }
                    else
                    {
                        Rot_Simple_Derecha(N_dad, Comparador);
                    }
                }
            }
            if (Fact_Equi > 2 || Fact_Equi < -2)
            {
                int fact = Fact_Equi;
                //He flipado
            }
            else
            {
                //No pasa nada oiga
            }
        }

        private void Rot_Simple_Izquierda_Raiz(Nodo<T> Raiz_Rot, Delegate Comparador)
        {
            Nodo<T> N_Aux = Raiz_Rot;
            raiz = Raiz_Rot.Hijoder;
            N_Aux.Padre = N_Aux.Hijoder;
            //En caso de que el hijo derecho tenga un hijo izquierdo
            if (N_Aux.Hijoder.Hijoizq != null)
            {
                N_Aux.Hijoder.Hijoizq.Padre = N_Aux;
                N_Aux.Hijoder = N_Aux.Hijoder.Hijoizq;
                raiz.Hijoizq = N_Aux;
            }
            else
            {
                N_Aux.Hijoder.Hijoizq = N_Aux;
                N_Aux.Hijoder = null;
            }
            raiz.Padre = null;
            //Colocar las nuevas alturas
            N_Aux.Altura = Det_Altura(N_Aux.Hijoder, N_Aux.Hijoizq);
            raiz.Altura = Det_Altura(raiz.Hijoder, raiz.Hijoizq);
            raiz.Hijoder.Altura = Det_Altura(raiz.Hijoder.Hijoder, raiz.Hijoder.Hijoizq);
            raiz.Hijoizq.Altura = Det_Altura(raiz.Hijoizq.Hijoder, raiz.Hijoizq.Hijoizq);
        }

        private void Rot_Simple_Izquierda(Nodo<T> N_Rot, Delegate Comparador)
        {
            Nodo<T> HijoCambio = N_Rot.Hijoder.Hijoizq;
            Asignar_PadreHijo(N_Rot, N_Rot.Hijoder, Comparador);
            N_Rot.Hijoder.Padre = N_Rot.Padre;
            N_Rot.Padre = N_Rot.Hijoder;
            N_Rot.Padre.Hijoizq = N_Rot;
            //En caso de que el hijo derecho tenga un hijo izquierdo
            if (HijoCambio != null)
            {
                HijoCambio.Padre = N_Rot;
                N_Rot.Hijoder = HijoCambio;
            }
            else
            {
                N_Rot.Hijoder = null;
            }
            //Colocar la nueva altura
            N_Rot.Altura = Det_Altura(N_Rot.Hijoder, N_Rot.Hijoizq);
            N_Rot.Padre.Altura = Det_Altura(N_Rot.Padre.Hijoder, N_Rot.Padre.Hijoizq);
        }

        private void Rot_Simple_Derecha_Raiz(Nodo<T> Raiz_Rot, Delegate Comparador)
        {
            Nodo<T> N_Aux = Raiz_Rot;
            raiz = Raiz_Rot.Hijoizq;
            N_Aux.Padre = N_Aux.Hijoizq;
            //En caso de que el hijo izquierdo tenga un hijo derecho
            if (N_Aux.Hijoizq.Hijoder != null)
            {
                N_Aux.Hijoizq.Hijoder.Padre = N_Aux;
                N_Aux.Hijoizq = N_Aux.Hijoizq.Hijoder;
                raiz.Hijoder = N_Aux;
            }
            else
            {
                N_Aux.Hijoizq.Hijoder = N_Aux;
                N_Aux.Hijoizq = null;
            }
            raiz.Padre = null;
            //Colocar las nuevas alturas
            N_Aux.Altura = Det_Altura(N_Aux.Hijoder, N_Aux.Hijoizq);
            raiz.Altura = Det_Altura(raiz.Hijoder, raiz.Hijoizq);
            raiz.Hijoder.Altura = Det_Altura(raiz.Hijoder.Hijoder, raiz.Hijoder.Hijoizq);
            raiz.Hijoizq.Altura = Det_Altura(raiz.Hijoizq.Hijoder, raiz.Hijoizq.Hijoizq);
        }

        private void Rot_Simple_Derecha(Nodo<T> N_Rot, Delegate Comparador)
        {
            Nodo<T> HijoCambio = N_Rot.Hijoizq.Hijoder;
            Asignar_PadreHijo(N_Rot, N_Rot.Hijoizq, Comparador);
            N_Rot.Hijoizq.Padre = N_Rot.Padre;
            N_Rot.Padre = N_Rot.Hijoizq;
            N_Rot.Padre.Hijoder = N_Rot;
            //En caso de que el hijo izquierdo tenga un hijo derecho
            if (HijoCambio != null)
            {
                HijoCambio.Padre = N_Rot;
                N_Rot.Hijoizq = HijoCambio;
            }
            else
            {
                N_Rot.Hijoizq = null;
            }
            //Colocar la nueva altura
            N_Rot.Altura = Det_Altura(N_Rot.Hijoder, N_Rot.Hijoizq);
            N_Rot.Padre.Altura = Det_Altura(N_Rot.Padre.Hijoder, N_Rot.Padre.Hijoizq);
        }

        private void Asignar_PadreHijo(Nodo<T> n_rot, Nodo<T> nod_aux, Delegate Comparador)
        {
            int result = (int)Comparador.DynamicInvoke(n_rot.Padre.Valor, n_rot.Valor);
            if (result < 0)
            {
                n_rot.Padre.Hijoder = nod_aux;
            }
            if (result > 0)
            {
                n_rot.Padre.Hijoizq = nod_aux;
            }
            if (n_rot.Padre.Hijoder == n_rot)
            {
                n_rot.Padre.Hijoder = nod_aux;
            }
            if (n_rot.Padre.Hijoizq == n_rot)
            {
                n_rot.Padre.Hijoizq = nod_aux;
            }
        }
        /*********Finaliza rotaciones************/



        /************Recorridos****************/
        private string texto_impresion = "";

        public string ExportarInorder(Delegate Obt_Info)
        {
            texto_impresion = "";
            try
            {
                if (raiz.Valor != null)
                {
                    Inorder(raiz, Obt_Info);
                }
                else
                {
                    return "Arbol Vacio";
                }
                return texto_impresion;
            }
            catch
            {
                return "Arbol Vacio";
            }
        }
        private void Inorder(Nodo<T> nodo_obt, Delegate ob_nom)
        {
            if (nodo_obt.Hijoizq != null)
            {
                Inorder(nodo_obt.Hijoizq, ob_nom);
            }
            texto_impresion += (string)ob_nom.DynamicInvoke(nodo_obt.Valor) + Environment.NewLine;
            if (nodo_obt.Hijoder != null)
            {
                Inorder(nodo_obt.Hijoder, ob_nom);
            }

        }
        /************Finaliza recorridos****************/
    }
}