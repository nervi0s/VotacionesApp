using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votaciones_App
{
    public class Mando
    {
        public bool respondido;
        public string respuesta;
        public int id;
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
