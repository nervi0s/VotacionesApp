using System;
using System.Windows.Forms;
using Votaciones_App.Formularios;

namespace Votaciones_App.Views
{
    // Clase encargada de seleccionar el ID de la antena base y el modo de conexión a esta
    public partial class UserControlConnectionChoice : UserControl
    {
        public delegate void CommunicatorDelegate(string msg);
        public CommunicatorDelegate communicatorCallBack;

        private CFileXML xmlFile = new CFileXML();

        // ##############   Constructor  ############## \\
        public UserControlConnectionChoice()
        {
            InitializeComponent();
        }

        // ##############   Event controls   ############## \\
        private void UserControlConnectionChoice_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill; // Dock style
            this.numericUpDown_id.Value = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "BaseAntena"));
        }

        private void button_usb_Click(object sender, EventArgs e)
        {
            CAjustes.tipo_conexion = 1;

            // Se guardan en el fichero los datos proporcionados desde la UI
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "BaseAntena", this.numericUpDown_id.Value.ToString());

            // Se cargan en memoria (clase CAjustes) los ajustes
            CAjustes.base_antena_id = (int)this.numericUpDown_id.Value;

            this.communicatorCallBack("UserControlConnectionChoice"); // Comunicación al Form Principal
        }

        private void button_ethernet_Click(object sender, EventArgs e)
        {
            CAjustes.tipo_conexion = 2;

            // Abrir formulario de ajustes de Ethernet
            EthernetOptions ethernetOptions = new EthernetOptions();
            ethernetOptions.StartPosition = FormStartPosition.CenterParent;

            if (ethernetOptions.ShowDialog() == DialogResult.OK)
            {
                // Se guardan en el fichero los datos proporcionados desde la UI
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "BaseAntena", this.numericUpDown_id.Value.ToString());

                // Se cargan en memoria (clase CAjustes) los ajustes
                CAjustes.base_antena_id = (int)this.numericUpDown_id.Value;

                this.communicatorCallBack("UserControlConnectionChoice"); // Comunicación al Form Principal
            }
        }
    }
}
