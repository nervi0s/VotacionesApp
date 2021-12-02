using System;

namespace Votaciones_App
{
    // Clase que representa un mando que realiza una votación
    public class Mando
    {
        public static int NUMERO_OPCIONES_MAXIMAS = 1;

        private int id;
        private System.Windows.Forms.DataGridViewRow row;

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

        public void setRow(System.Windows.Forms.DataGridViewRow row)
        {
            this.row = row;
        }

        public System.Windows.Forms.DataGridViewRow getRow()
        {
            return this.row;
        }
    }
}
