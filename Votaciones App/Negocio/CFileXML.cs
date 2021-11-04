using System;
using System.Collections.Generic;
using System.Collections;
//using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.XPath;

// Clase que contiene los atributos y metodos necesarios para el tratamiento de ficheros XML
// Partimos de un fichero XML creado con la configuración por defecto

namespace Votaciones_App
{

    class CFileXML
    {
        // Directorio de ficheros XML de configuracion de sesion
        //public string nombreFichero = "PreferenciasElecciones.xml";
        //public string dirFicherosConfiguracion = @"C:\ELECCIONES\FicherosConfiguracion\";



        public void EscribirXml(string rutaXml, string campo, string valor)
        {

            CFileXML xml = new CFileXML(); 

            if (xml.siExiste(rutaXml))
            {
                xml.EscribeNodo_XmlDocument(rutaXml, campo, valor);
            }

        }

        public string LeerXml(string rutaXml, string campo)
        {

            CFileXML xml = new CFileXML();            
            if (xml.siExiste(rutaXml))
            {
                return xml.LeeNodo_TextReader(rutaXml, campo);
            }
            else
                return "";
        }


        // Metodo que comprueba si el fichero ya existe
        public bool siExiste(string nombreFichero)
        {
            return File.Exists(nombreFichero);
        }


        // Metodo que carga en memoria el documento XML que le llega como parametro
        public void CargaFicheroXML(string nombreFichero)
        {
            XmlDocument doc = new XmlDocument();
            XmlTextReader lector = new XmlTextReader(nombreFichero);

            try
            {
                doc.Load(lector);
                lector.Close();
                //this.nombreFichero = nombreFichero;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }


        
        // Metodo que crea un fichero XML vacio, solo contendra el prologo
        // y el elemento raiz
        public void CreaFicheroVacio(String header ,ArrayList lista, string rutaxml )
        {
            // Consigo el directorio de ejecución de la aplicación

            string ruta_fichero = rutaxml;
            XmlTextWriter escritor = new XmlTextWriter(ruta_fichero, Encoding.GetEncoding("ISO-8859-1"));
            escritor.Formatting = Formatting.Indented;


            escritor.WriteStartDocument(true);
            escritor.WriteStartElement(header);
            for (int i = 0; i < lista.Count; i++)
            {
                escritor.WriteStartElement(Convert.ToString(lista[i]));
                escritor.WriteEndElement();
            }
            escritor.WriteEndElement();
            escritor.WriteEndDocument();
            escritor.Close();
        }
        


        // Metodo que lee el valor del nodo que le llega especificado como parametro
        // usando XmlTextReader
        public string LeeNodo_TextReader(string nombreFichero, string nombreNodo)
        {
            XmlTextReader lector = new XmlTextReader(nombreFichero);
            string valorNodo = "";

            try
            {
                while (lector.Read())
                {
                    if (lector.Name == nombreNodo)
                    {
                        // Obtiene el valor del atributo del nodo actual
                        valorNodo = lector.ReadString();
                    }
                }
            }

            catch (XmlException excp)
            {
                string mnj_excep = "";
                mnj_excep += excp.Message + "\n";
                mnj_excep += "Excepcion de codigo: Linea " + excp.LineNumber + "Columna " + excp.LinePosition;
                mnj_excep += "Excepcion XML: Linea " + lector.LineNumber + "Columna " + lector.LinePosition;
                Console.WriteLine(mnj_excep);
            }
            lector.Close();
            return valorNodo;
        }


        // Metodo que lee el valor del nodo que le llega especificado como parametro
        // usando XPath
        public string LeeNodo_XPath(string nombreFichero, string nombreNodo)
        {
            XPathDocument doc = new XPathDocument(nombreFichero, XmlSpace.Preserve);
            XPathNavigator navegador = doc.CreateNavigator();
            XPathExpression xpathExpresion;
            string valorNodo = "";

            try
            {
                xpathExpresion = navegador.Compile("//" + nombreNodo);

                switch (xpathExpresion.ReturnType)
                {
                    case XPathResultType.Number:
                        valorNodo = (string)navegador.Evaluate(xpathExpresion);
                        break;
                    case XPathResultType.NodeSet:
                        XPathNodeIterator i = navegador.Select(xpathExpresion);
                        while (i.MoveNext())
                        {
                            valorNodo = i.Current.ToString();
                        }
                        break;
                    case XPathResultType.String:
                        valorNodo = (string)navegador.Evaluate(xpathExpresion);
                        break;
                }
                return valorNodo;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }


        // Metodo que lee el valor del nodo que le llega especificado como parametro
        // usando XmlDocument
        public string LeeNodo_XmlDocument(string nombreFichero, string nombreNodo)
        {
            XmlDocument doc = new XmlDocument();
            string valorNodo = "";
            XmlNode elemento;

            try
            {
                doc.Load(nombreFichero);
                XmlNode root = doc.DocumentElement;
                IEnumerator NodePointer = root.GetEnumerator();

                // Leemos cada uno de los elementos
                while (NodePointer.MoveNext())
                {
                    elemento = (XmlNode)NodePointer.Current;
                    if (elemento.Name == nombreNodo)
                        valorNodo = elemento.InnerText;
                }
                return valorNodo;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }



        // Metodo que escribe un determinado valor a un nodo (ambos van especificados como parametro)
        // usando XmlDocument --> OK
        public void EscribeNodo_XmlDocument(string nombreFichero, string nombreNodo, string valorNodo)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode elemento;
            //int posUnidad = 0;
            //string ruta = "";

            try
            {
                // Consigo el directorio de ejecución de la aplicación
                //string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                //posUnidad = path.IndexOf('C');
                //ruta = path.Substring(posUnidad, path.Length - posUnidad);
                //nombreFichero = ruta + nombreFichero;

                //nombreFichero = path + nombreFichero;
                //nombreFichero = @"C:\Documents and Settings\Carol\Mis documentos\BRAINSTORM MULTIMEDIA\Proyectos Produccion\Parser Datos Elecciones\Aplicacion\Codigo\Segundo Modulo\SegundoModulo_26-01\ParserElecciones\bin\Debug\Sesiones\sesion1.xml";
                //Console.WriteLine("Escribo en : " + nombreFichero);

                doc.Load(nombreFichero);

                XmlNode root = doc.DocumentElement;
                IEnumerator NodePointer = root.GetEnumerator();

                // Leemos cada uno de los elementos
                while (NodePointer.MoveNext())
                {
                    elemento = (XmlNode)NodePointer.Current;
                    if (elemento.Name == nombreNodo)
                        elemento.InnerText = valorNodo;
                }

                // Salvamos el fichero a disco
                doc.Save(nombreFichero);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}