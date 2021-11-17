using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Votaciones_App
{
    // Clase que contiene los atributos y metodos necesarios para el tratamiento de ficheros
    public class CFichero
    {
        // Metodo que comprueba si el fichero ya existe
        public bool siExiste(string nombreFichero)
        {
            return File.Exists(nombreFichero);
        }


        // Metodo que crea un fichero vacío
        public void CreaFichero(string nombreFichero)
        {
            File.Create(nombreFichero).Close();
        }


        public void DestruyeFichero(string nombreFichero)
        {
            File.Delete(nombreFichero);
        }


        // Metodo que lee un fichero que le llega como parametro linea a linea, 
        // y lo almacena en las entidades de datos
        // El separador usado para los bloques llega como un parametro (por ejemplo, un punto y coma)
        public bool LeeFicheroPorLineas(string nombreFichero, char separador)
        {
            string[] arrayLinea = null;
            string linea = "";

            try
            {
                // Se abre el fichero para leerlo posteriormente
                StreamReader sr = File.OpenText(nombreFichero);

                // Se lee el fichero linea a linea
                while ((linea = sr.ReadLine()) != null)
                {
                    // Metemos en un array la linea que acabamos de leer. En cada una de las posiciones del array 
                    // se tiene almacenado cada uno de los trozitos que van separados por ";"
                    arrayLinea = linea.Split(separador);

                    // Guardamos los datos de la linea actual del fichero en las entidades de datos
                    //AlmacenarDatos(arrayLinea);
                }
                sr.Close();
                return true;
            }

            catch (Exception excep)
            {
                Console.WriteLine(excep.Message);
                return false;
            }
        }



        // Metodo que lee un fichero que le llega como parametro completamente, 
        // y lo almacena en las entidades de datos
        // El separador usado para los bloques llega como un parametro (por ejemplo, un punto y coma)
        public string[] LeeFichero(string nombreFichero, char separador)
        {
            string[] arrayFichero = null;
            string fichero = "";

            try
            {
                // NUEVO !!!
                StreamReader sr = new StreamReader(nombreFichero, System.Text.Encoding.Default);

                // Se abre el fichero para leerlo posteriormente
                //StreamReader sr = File.OpenText(nombreFichero);

                // Se lee el fichero completamente, es decir, hasta final de fichero
                fichero = sr.ReadToEnd();

                // Metemos en un array el fichero que acabamos de leer. En cada una de las posiciones del array 
                // se tiene almacenado cada uno de los trozitos que van separados por ";"
                arrayFichero = fichero.Split(separador);

                sr.Close();
                return arrayFichero;
            }

            catch (Exception excep)
            {
                Console.WriteLine(excep.Message);
                return arrayFichero;
            }
        }



        // Metodo que escribe un string en el fichero. Ambos llegan al metodo como parametros
        public void EscribeFichero(string nombreFichero,bool append, string linea)
        {
            // El segundo parametro a "true" indica que se va a hacer un Append
            // Es decir, no va a machacar lo que hay en el fichero
            StreamWriter sw = new StreamWriter(nombreFichero, append, System.Text.Encoding.UTF8);

            sw.Write(linea);
            sw.Close();
        }
    }
}