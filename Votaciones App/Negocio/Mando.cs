using System;

namespace Votaciones_App
{
    public class Mando
    {
        public static int NUMERO_OPCIONES_MAXIMAS = 3;

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
