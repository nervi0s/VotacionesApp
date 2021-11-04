using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.IO;


namespace Votaciones_App
{
    public static class CSeguridad
    {
        // Devuelve el código interno de la máquina
        public static string GetCode()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                cpuInfo = mo.Properties["processorID"].Value.ToString();
                break;
            }
            return cpuInfo;
        }        


        /// Encripta una cadena
        /// public static string Encriptar(this string _cadenaAencriptar)
        public static string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }


        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        /// public static string DesEncriptar(this string _cadenaAdesencriptar)
        public static string DesEncriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
    }
}
