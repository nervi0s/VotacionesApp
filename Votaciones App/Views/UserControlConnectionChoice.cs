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

        CFileXML xmlFile = new CFileXML();

        // ##############   Constructor  ############## \\
        public UserControlConnectionChoice()
        {
            InitializeComponent();
        }

        // ##############   Event controls   ############## \\
        private void UserControlConnectionChoice_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill; // Dock style
            this.textBox_id.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "BaseAntena");
        }

        private void button_usb_Click(object sender, EventArgs e)
        {
            if (validaAjustesInterfaz())
            {
                CAjustes.tipo_conexion = 1;

                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "BaseAntena", this.textBox_id.Text);
                this.communicatorCallBack("UserControlConnectionChoice"); // Comunicación al Form Principal
            }
        }

        private void button_ethernet_Click(object sender, EventArgs e)
        {
            if (validaAjustesInterfaz())
            {
                CAjustes.tipo_conexion = 2;

                // Abrir formulario de ajustes de Ethernet
                EthernetOptions ethernetOptions = new EthernetOptions();
                ethernetOptions.StartPosition = FormStartPosition.CenterParent;

                if (ethernetOptions.ShowDialog() == DialogResult.OK)
                {
                    xmlFile.EscribirXml(CAjustes.ruta_ajustes, "BaseAntena", this.textBox_id.Text);
                    this.communicatorCallBack("UserControlConnectionChoice"); // Comunicación al Form Principal
                }
            }
        }

        // To prevent non numeric values
        private void textBox_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // ##############   Validation   ############## \\
        private bool validaAjustesInterfaz()
        {
            if (string.IsNullOrEmpty(this.textBox_id.Text))
            {
                MessageBox.Show("Debe introducir un ID en el campo. Por favor, proporcione datos correctos.", "Error de entrada");
                return false;
            }

            if (!int.TryParse(this.textBox_id.Text, out _))
            {
                MessageBox.Show("El ID debe ser un número entero. Por favor, Proporcione datos correctos", "Error de entrada");
                return false;
            }
            return true;
        }
    }
}
