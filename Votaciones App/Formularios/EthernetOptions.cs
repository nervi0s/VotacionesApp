using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Votaciones_App.Formularios
{
    // Clase encargada de seleccionar los ajustes de red para una conexión por Ethernet
    public partial class EthernetOptions : Form
    {
        CFileXML xmlFile = new CFileXML();

        // ##############   Constructor  ############## \\
        public EthernetOptions()
        {
            InitializeComponent();
        }

        // ##############   Event controls   ############## \\
        private void EthernetOptions_Load(object sender, EventArgs e)
        {
            checkAndSetFileData();
        }
        private void button_aceptar_Click(object sender, EventArgs e)
        {
            if (validaAjustesInterfaz())
            {
                // Se guardan en el fichero los datos proporcionados desde la UI
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "IpAntena", this.textBox_ip.Text);
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MacAntena", this.textBox_mac.Text);
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MaskAntena", this.textBox_mask.Text);
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "GatewayAntena", this.textBox_gateway.Text);

                // Se cargan en memoria (clase CAjustes) los ajustes
                CAjustes.ip_antena = this.textBox_ip.Text;
                CAjustes.mac_antena = this.textBox_mac.Text;
                CAjustes.mask_antena = this.textBox_mask.Text;
                CAjustes.gateway_antena = this.textBox_gateway.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button_cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ##############   Validation   ############## \\
        private void checkAndSetFileData()
        {
            if (validaAjustesFicheroXml())
            {
                // Leer Ajustes del fichero XML y establecer controles
                this.textBox_ip.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "IpAntena");
                this.textBox_mac.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "MacAntena");
                this.textBox_mask.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "MaskAntena");
                this.textBox_gateway.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "GatewayAntena");
            }
            else
            {
                // Cargar ajustes por defecto
                this.textBox_ip.Text = "192.168.0.199";
                this.textBox_mac.Text = "74-30-13-02-05-36";
                this.textBox_mask.Text = "255.255.255.0";
                this.textBox_gateway.Text = "192.168.0.1";
            }
        }

        private bool validaAjustesFicheroXml()
        {
            if (!IPAddress.TryParse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "IpAntena"), out _))
            {
                MessageBox.Show("EL número de IP proporcionado no es válido. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            Match match = Regex.Match(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MacAntena"), "^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})|([0-9a-fA-F]{4}\\.[0-9a-fA-F]{4}\\.[0-9a-fA-F]{4})$");
            if (!match.Success)
            {
                MessageBox.Show("La dirección MAC proporcionada no es válida. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            if (!IPAddress.TryParse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MaskAntena"), out _))
            {
                MessageBox.Show("EL número de IP de la máscara de subred proporcionado no es válido. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            if (!IPAddress.TryParse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "GatewayAntena"), out _))
            {
                MessageBox.Show("EL número de IP de la puerta de enlace proporcionado no es válido. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            return true;
        }

        private bool validaAjustesInterfaz()
        {
            if (!IPAddress.TryParse(this.textBox_ip.Text, out _))
            {
                MessageBox.Show("EL número de IP proporcionado no es válido. Proporcione datos correctos", "Error de entrada");
                return false;
            }

            Match match = Regex.Match(this.textBox_mac.Text, "^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})|([0-9a-fA-F]{4}\\.[0-9a-fA-F]{4}\\.[0-9a-fA-F]{4})$");
            if (!match.Success)
            {
                MessageBox.Show("La dirección MAC proporcionada no es válida. Proporcione datos correctos", "Error de entrada");
                return false;
            }

            if (!IPAddress.TryParse(this.textBox_mask.Text, out _))
            {
                MessageBox.Show("EL número de IP de la máscara de subred proporcionado no es válido. Proporcione datos correctos", "Error de entrada");
                return false;
            }

            if (!IPAddress.TryParse(this.textBox_gateway.Text, out _))
            {
                MessageBox.Show("EL número de IP de la puerta de enlace proporcionado no es válido. Proporcione datos correctos", "Error de entrada");
                return false;
            }

            return true;
        }
    }
}
