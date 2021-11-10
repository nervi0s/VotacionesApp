using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections;
using SunVote;
using System.Net;
using System.Net.Sockets;
using Votaciones_App.Views;
using System.Text.RegularExpressions;
using Votaciones_App.Formularios;

namespace Votaciones_App
{
    public partial class FormPpal : Form
    {
        UserControlConnectionChoice connectionChoicePanel;
        UserControlSettings settingsPanel;
        UserControlVoting votingPanel;

        CFileXML xmlFile = new CFileXML();

        // ##############   Constructor  ############## \\
        public FormPpal()
        {
            InitializeComponent();
        }

        // ##############   Event controls   ############## \\
        private void FormPpal_Load(object sender, EventArgs e)
        {
            createDefaultValuesFile(); // Crear el fichero de ajustes con valores por defecto en caso de no existir
            loadConnectionChoisePanel();
        }

        private void FormPpal_FormClosed(object sender, FormClosedEventArgs e)
        {
            string message = "Cerrando Aplicación";
            string title = "Info";
            MessageBox.Show(message, title, MessageBoxButtons.OK);
        }



        // ##############   Panels control   ############## \\

        // Carga el panel de elección de tipo de conexión
        private void loadConnectionChoisePanel()
        {
            this.connectionChoicePanel = new UserControlConnectionChoice();
            this.connectionChoicePanel.communicatorCallBack += customsViewsCommManager;
            this.panel_root.Controls.Add(connectionChoicePanel);
        }

        // Carga el panel de settings
        private void loadSettingsPanel()
        {
            this.settingsPanel = new UserControlSettings();
            this.settingsPanel.communicatorCallBack += customsViewsCommManager;
            this.panel_root.Controls.Add(settingsPanel);
        }

        // Carga el panel de las votaciones
        private void loadVotingPanel()
        {
            this.votingPanel = new UserControlVoting();
            //this.settingsPanel.communicatorCallBack += customsViewsCommManager;
            this.panel_root.Controls.Add(votingPanel);
        }

        private void removeConnectionChoicePanel()
        {
            this.panel_root.Controls.Remove(this.connectionChoicePanel);

        }
        private void removeSettingsPanel()
        {
            this.panel_root.Controls.Remove(this.settingsPanel);
        }

        // Maneja las llamades desde otros paneles
        private void customsViewsCommManager(string msg)
        {
            if (msg.Contains("UserControlConnectionChoice"))
            {
                loadMultichoiseSelecctionDialog();
                removeConnectionChoicePanel();
                loadSettingsPanel();
            }
            else if (msg.Contains("UserControlSettings"))
            {
                removeSettingsPanel();
                loadXmlToMemory();
                loadVotingPanel();
            }
        }

        // ##############   Auxiliary private functions   ############## \\

        // Crea el archivo de Ajustes
        private void createDefaultValuesFile()
        {
            if (!File.Exists(CAjustes.ruta_ajustes))
            {
                // Crear XML
                ArrayList lista = new ArrayList();
                lista.Add("MandosTotales");
                lista.Add("Rangos");
                lista.Add("TiempoCrono");
                lista.Add("BaseAntena");
                lista.Add("PermitirCambioRespuesta");
                lista.Add("PermitirMultichoice");
                lista.Add("TipoVotacion");
                lista.Add("NumeroOpciones");
                lista.Add("RutaResultados");
                lista.Add("ConexionGrafismo");
                lista.Add("Ip");
                lista.Add("IpAntena");
                lista.Add("MacAntena");
                lista.Add("MaskAntena");
                lista.Add("GatewayAntena");
                xmlFile.CreaFicheroVacio("Ajustes", lista, CAjustes.ruta_ajustes);

                // Escribe el XML con valores por defecto
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MandosTotales", "100");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Rangos", "1-100");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "TiempoCrono", "120");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "BaseAntena", "1");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta", "False");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "PermitirMultichoice", "False");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "TipoVotacion", "0");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "NumeroOpciones", "3");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "RutaResultados", "./");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "ConexionGrafismo", "False");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Ip", "127.0.0.1");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "IpAntena", "192.168.0.199");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MacAntena", "74-30-13-02-05-36");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MaskAntena", "255.255.255.0");
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "GatewayAntena", "192.168.0.1");
            }
        }

        // Lee el XML para setear las variables de la clase CAjustes
        private void loadXmlToMemory()
        {
            // Cargar los ajustes del fichero XML en memoria
            CAjustes.num_mandos = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales"));
            CAjustes.rangos = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Rangos");
            CAjustes.tiempo_crono = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TiempoCrono"));
            CAjustes.base_antena_id = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "BaseAntena"));
            CAjustes.permitir_cambio_respuesta = bool.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta"));
            CAjustes.tipo_votacion = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TipoVotacion"));
            CAjustes.numero_opciones = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "NumeroOpciones"));
            CAjustes.ruta_resultados = xmlFile.LeerXml(CAjustes.ruta_ajustes, "RutaResultados");
            CAjustes.conexion_grafismo = bool.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "ConexionGrafismo"));
            CAjustes.ip = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Ip");
            CAjustes.ip_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "IpAntena");
            CAjustes.mac_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "MacAntena");
            CAjustes.mask_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "MaskAntena");
            CAjustes.gateway_antena = xmlFile.LeerXml(CAjustes.ruta_ajustes, "GatewayAntena");
        }

        // Carga un diálogo de selección para la multirespuesta
        private void loadMultichoiseSelecctionDialog()
        {
            string message = "¿Desea activar la opción Multirespuesta?";
            string title = "Multichoice selector";
            DialogResult dialogResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "PermitirMultichoice", "True");
            else
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "PermitirMultichoice", "False");
        }

    }
}
