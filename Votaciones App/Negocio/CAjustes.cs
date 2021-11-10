using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votaciones_App
{

    public class CAjustes
    {
        public static int tiempo_crono;
        public static int num_mandos;
        public static string rangos;
        public static string ruta_resultados;
        public static int tipo_votacion;
        public static int numero_opciones;
        public static int puerto_programa_externo_envio;
        public static bool permitir_cambio_respuesta;
        public static bool? permitir_multichoice;
        public static string ruta_ajustes = "./Ajustes.xml";
        public static int comBaseChanel;
        public static bool conexion_grafismo;
        public static string ip;
        public static int puerto = 5123;
        public static int? base_antena;
        public static string ip_antena;
        public static string mac_antena;
        public static string mask_antena;
        public static string gateway_antena;
    }
}
