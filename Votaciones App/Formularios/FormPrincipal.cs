using System;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using Votaciones_App.Views;
using Votaciones_App.Negocio;

namespace Votaciones_App
{
    public partial class FormPpal : Form
    {
        UserControlConnectionChoice connectionChoicePanel;
        UserControlSettings settingsPanel;
        UserControlVoting votingPanel;

        VoteManager voteManager;

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
            this.connectionChoicePanel.communicatorCallBack += customsViewsCommHandler;
            this.panel_root.Controls.Add(connectionChoicePanel);
        }

        // Carga el panel de settings
        private void loadSettingsPanel()
        {
            this.settingsPanel = new UserControlSettings();
            this.settingsPanel.communicatorCallBack += customsViewsCommHandler;
            this.panel_root.Controls.Add(settingsPanel);
        }

        // Carga el panel de las votaciones
        private void loadVotingPanel()
        {
            this.votingPanel = new UserControlVoting();
            this.voteManager.setVotingPanel(this.votingPanel);
            this.votingPanel.communicatorCallBack += customsViewsCommHandler;
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

        // Maneja las llamadas desde otros paneles
        private void customsViewsCommHandler(string msg)
        {
            if (msg.Contains("UserControlConnectionChoice"))
            {
                loadMultichoiseSelecctionDialog();
                removeConnectionChoicePanel();
                initializeVoteManager();
                loadSettingsPanel();
            }
            else if (msg.Contains("UserControlSettings"))
            {
                removeSettingsPanel();
                loadVotingPanel();
            }
            else if (msg.Contains("UserControlVoting"))
            {
                if (msg.Contains("cambiaPanel"))
                {
                    //ToDo cambiar entre paneles
                    //voteMan resetVota
                    // panelVota tiempoCrono CA
                    // panelVota rojo
                }
                else if (msg.Contains("iniciaVotacion"))
                {
                    if (!voteManager.isVoting())
                    {
                        voteManager.resetVotacion();
                        voteManager.iniciaVotacion();
                        // panelVota verde
                    }
                }
                else if (msg.Contains("recuentoClick"))
                {
                    this.voteManager.actulizarRecuento();
                }
            }
        }

        // Maneja las llamada desde VoteManager
        private void voteManagerCommHandler(int status)
        {
            switch (status)
            {
                case 0:
                    this.settingsPanel.setImageConnectionStatus(Properties.Resources.rojo);
                    // ToDo quizas deshabilitar botón apagar mandos del panel votingPanel
                    this.settingsPanel.setEnableButtonAceptar(false);
                    break;
                case 1:
                    this.settingsPanel.setImageConnectionStatus(Properties.Resources.verde);
                    // ToDo quizas habilitar botón apagar mandos del panel votingPanel
                    this.settingsPanel.setEnableButtonAceptar(true);
                    break;
                case -1:
                    this.settingsPanel.setImageConnectionStatus(Properties.Resources.rojo);
                    // ToDo quizas deshabilitar botón apagar mandos del panel votingPanel
                    this.settingsPanel.setEnableButtonAceptar(false);
                    break;
                case -2:
                    this.settingsPanel.setImageConnectionStatus(Properties.Resources.rojo);
                    // ToDo quizas deshabilitar botón apagar mandos del panel votingPanel
                    this.settingsPanel.setEnableButtonAceptar(false);
                    break;
                case -3:
                    this.settingsPanel.setImageConnectionStatus(Properties.Resources.rojo);
                    // ToDo quizas deshabilitar botón apagar mandos del panel votingPanel
                    this.settingsPanel.setEnableButtonAceptar(false);
                    break;
                case -4:
                    this.settingsPanel.setImageConnectionStatus(Properties.Resources.rojo);
                    // ToDo quizas deshabilitar botón apagar mandos del panel votingPanel
                    this.settingsPanel.setEnableButtonAceptar(false);
                    break;
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
            else
            {   // Se cargan en memoria ajustes básicos
                CAjustes.num_mandos = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales"));
                CAjustes.rangos = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Rangos");
            }
        }

        // Carga un diálogo de selección para la multirespuesta
        private void loadMultichoiseSelecctionDialog()
        {
            string message = "¿Desea activar la opción Multirespuesta?";
            string title = "Multichoice selector";
            DialogResult dialogResult = MessageBox.Show(message, title, MessageBoxButtons.YesNo);

            // Se guardan en el fichero los datos proporcionados desde la UI
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "PermitirMultichoice", dialogResult == DialogResult.Yes ? "True" : "False");

            // Se cargan en memoria (clase CAjustes) los ajustes
            CAjustes.permitir_multichoice = dialogResult == DialogResult.Yes;
        }

        // Inicializa una instancia de VoteManager
        private void initializeVoteManager()
        {
            this.voteManager = new VoteManager(CAjustes.base_antena_id, CAjustes.tipo_conexion);
            this.voteManager.communicatorCallBack += voteManagerCommHandler;
            this.voteManager.connectToAntena();
        }
    }
}
