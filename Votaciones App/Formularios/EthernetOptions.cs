using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Votaciones_App.Formularios
{
    public partial class EthernetOptions : Form
    {
        CFileXML xmlFile = new CFileXML();

        public EthernetOptions()
        {
            InitializeComponent();
        }

        private void EthernetOptions_Load(object sender, EventArgs e)
        {
            checkAndSetFileData();
        }

        private void checkAndSetFileData()
        {
            if (File.Exists(CAjustes.ruta_ajustes) && validaAjustesFicheroXml())
            {
                // Cargar los ajustes del fichero en memoria
                CAjustes.ip_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "IpAntena");
                CAjustes.mac_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "MacAntena");
                CAjustes.mask_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "MaskAntena");
                CAjustes.gateway_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "GatewayAntena");
            }
            else
            {
                // Cargar ajustes por defecto en memoria en caso de que no exista el fichero o no se pase la validación (en ese orden)
                CAjustes.ip_antena = "192.168.0.199";
                CAjustes.mac_antena = "74-30-13-02-05-36";
                CAjustes.mask_antena = "255.255.255.0";
                CAjustes.gateway_antena = "192.168.0.1";
            }

            this.textBox_ip.Text = CAjustes.ip_antena;
            this.textBox_mac.Text = CAjustes.mac_antena;
            this.textBox_mask.Text = CAjustes.mask_antena;
            this.textBox_gateway.Text = CAjustes.gateway_antena;
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

        private bool validaAjustes()
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
        private void button_aceptar_Click(object sender, EventArgs e)
        {
            if (validaAjustes())
            {
                // Guarda en memoria los ajustes proporcionados desde la UI
                CAjustes.ip_antena = this.textBox_ip.Text;
                CAjustes.mac_antena = this.textBox_mac.Text;
                CAjustes.mask_antena = this.textBox_mask.Text;
                CAjustes.gateway_antena = this.textBox_gateway.Text;

                // Si el fichero ya existe guarda los datos
                if (File.Exists(CAjustes.ruta_ajustes))
                {
                    xmlFile.EscribirXml(CAjustes.ruta_ajustes, "IpAntena", CAjustes.ip_antena);
                    xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MacAntena", CAjustes.mac_antena);
                    xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MaskAntena", CAjustes.mask_antena);
                    xmlFile.EscribirXml(CAjustes.ruta_ajustes, "GatewayAntena", CAjustes.gateway_antena);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button_cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
