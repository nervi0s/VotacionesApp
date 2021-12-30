using System;
using System.Collections.Generic;
using Votaciones_App.Negocio;

namespace Votaciones_App
{
    // Clase que representa un mando que realiza una votación
    public class Mando
    {
        public static int NUMERO_OPCIONES_MAXIMAS = 1;

        private readonly int id;
        private readonly List<Option> options;
        private List<string> respuestas = new List<string>();
        public bool respondido;

        private System.Windows.Forms.DataGridViewRow row;

        public Mando(int id, List<Option> options)
        {
            this.id = id;
            this.options = options;
        }

        public int getID()
        {
            return id;
        }

        public void setRow(System.Windows.Forms.DataGridViewRow row)
        {
            this.row = row;
        }

        public void vote(string respuestasNuevasRaw)
        {
            List<string> respuestasNuevas = new List<string>();
            respuestasNuevas.AddRange(respuestasNuevasRaw.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries));

            checkAndRemoveVote(respuestasNuevas);
            checkAndAddVote(respuestasNuevas);

            this.respuestas = respuestasNuevas;
            this.respondido = true;
        }

        // Examina las respuestas nuevas y en caso de que alguna no esté en las respuestas antiguas, suma un voto en su correspondiente Option
        private void checkAndAddVote(List<string> respuestasNuevas)
        {
            foreach (string votoNuevo in respuestasNuevas)
            {
                if (!this.respuestas.Contains(votoNuevo))
                    getOptionById(votoNuevo).addVote();
            }
        }

        // Examina las respuestas antiguas y en caso de que alguna este en las respuesta nuevas, elimina un voto de su correspndiente Option
        private void checkAndRemoveVote(List<string> respuestasNuevas)
        {
            foreach (string votoAntiguo in this.respuestas)
            {
                if (!respuestasNuevas.Contains(votoAntiguo))
                    getOptionById(votoAntiguo).removeVote();
            }
        }

        // Obtiene una objeto Option por su ID
        public Option getOptionById(string id)
        {
            if (Views.UserControlVoting.array_nombres != null)
            {
                id = Views.UserControlVoting.array_nombres[int.Parse(id) - 1];
            }
            foreach (Option option in this.options)
            {
                if (option.id == id)
                    return option;
            }
            return null;
        }

        public System.Windows.Forms.DataGridViewRow getRow()
        {
            return this.row;
        }

        public string getRespuestas()
        {
            string respuestasString = string.Empty;
            foreach (string respuesta in this.respuestas)
            {
                respuestasString += ";" + respuesta;
            }
            return respuestasString;
        }
    }
}
