using System;

namespace Votaciones_App
{
    // Clase que representa un mando que realiza una votación
    public class Mando
    {
        public static int NUMERO_OPCIONES_MAXIMAS = 1;

        private int id;

        public int cantidadRespuestas;
        public bool respondido;
        public string respuesta = string.Empty;

        public Mando(int id)
        {
            this.id = id;
            respondido = false;
            respuesta = "";
        }

        public int getID()
        {
            return id;
        }
    }
}
