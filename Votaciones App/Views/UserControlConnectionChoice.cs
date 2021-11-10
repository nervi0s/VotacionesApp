using SunVote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Votaciones_App.Formularios;

namespace Votaciones_App.Views
{
    public partial class UserControlConnectionChoice : UserControl
    {
        CFileXML xmlFile = new CFileXML();
        private int connectionMode;
        private Action<int> selectConnectionType;

        public UserControlConnectionChoice(Action<int> selectConnectionType)
        {
            InitializeComponent();
            this.selectConnectionType = selectConnectionType;
        }

        private void UserControlConnectionChoice_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            if (File.Exists(CAjustes.ruta_ajustes))
            {
                this.textBox_id.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "BaseAntena");
            }
            else
            {
                this.textBox_id.Text = "1";
            }
        }

        private void button_usb_Click(object sender, EventArgs e)
        {
            if (validaEntrada())
            {
                CAjustes.base_antena = int.Parse(this.textBox_id.Text);
                if (File.Exists(CAjustes.ruta_ajustes))
                {
                    xmlFile.EscribirXml(CAjustes.ruta_ajustes, "BaseAntena", CAjustes.base_antena.ToString());
                }

                this.connectionMode = 1;
                this.selectConnectionType(this.connectionMode);
            }
        }

        private void button_ethernet_Click(object sender, EventArgs e)
        {
            if (validaEntrada())
            {
                CAjustes.base_antena = int.Parse(this.textBox_id.Text);
                if (File.Exists(CAjustes.ruta_ajustes))
                {
                    xmlFile.EscribirXml(CAjustes.ruta_ajustes, "BaseAntena", CAjustes.base_antena.ToString());
                }

                this.connectionMode = 2;
                // Abrir nuevo form
                EthernetOptions ethernetOptions = new EthernetOptions();
                ethernetOptions.StartPosition = FormStartPosition.CenterParent;
                if (ethernetOptions.ShowDialog() == DialogResult.OK)
                {
                    this.selectConnectionType(this.connectionMode);

                }
            }
        }

        private bool validaEntrada()
        {
            if (string.IsNullOrEmpty(this.textBox_id.Text))
            {
                MessageBox.Show("Debe un ID en el campo. Proporcione datos correctos", "Error de entrada");
                return false;
            }

            if (!int.TryParse(this.textBox_id.Text, out _))
            {
                MessageBox.Show("El ID debe ser un número entero. Proporcione datos correctos", "Error de entrada");
                return false;
            }
            return true;
        }
    }
}
