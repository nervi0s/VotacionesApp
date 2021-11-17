using System;
using System.Drawing;
using System.Windows.Forms;
using Votaciones_App.Formularios;

namespace Votaciones_App.Views
{
    // Clase encargada de establecer los ajustes generales para una votación
    public partial class UserControlSettings : UserControl
    {
        public delegate void CommunicatorDelegate(string msg);
        public CommunicatorDelegate communicatorCallBack;

        private CFileXML xmlFile = new CFileXML();

        // ##############   Constructor  ############## \\
        public UserControlSettings()
        {
            InitializeComponent();
        }

        // ##############   Event controls   ############## \\
        private void UserControlSettings_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill; // Dock style
            checkAndSetFileData();
        }

        private void radioButton_ajustes_conexion_grafismo_Si_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton_ajustes_conexion_grafismo_Si.Checked)
            {
                this.textBox_ip.Visible = true;
                this.textBox_puerto.Visible = true;
                this.label_ajustes_ip.Visible = true;
                this.label_ajustes_puerto.Visible = true;
            }
        }

        private void radioButton_ajustes_conexion_grafismo_No_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton_ajustes_conexion_grafismo_No.Checked)
            {
                this.textBox_ip.Visible = false;
                this.textBox_puerto.Visible = false;
                this.label_ajustes_ip.Visible = false;
                this.label_ajustes_puerto.Visible = false;
            }
        }

        private void comboBox_ajustes_tipo_votacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panel_ajustes_num_opciones.Visible = this.comboBox_ajustes_tipo_votacion.SelectedIndex != 2;
            if (!this.panel_ajustes_num_opciones.Visible)
            {
                this.numericUpDown_ajustes_num_opciones.Value = 2;
            }
        }

        private void button_mandos_Click(object sender, EventArgs e)
        {
            // Abrir formulario de ajustes de Mandos
            FormMandosConfig formMandosConfig = new FormMandosConfig();
            formMandosConfig.StartPosition = FormStartPosition.CenterParent;

            if (formMandosConfig.ShowDialog() == DialogResult.OK)
            {
                this.button_mandos.Text = formMandosConfig.getMandosTotales().ToString() + " Mandos";
            }
        }

        private void button_ajustes_resultados_path_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox_ajustes_resultados_path.Text = this.folderBrowserDialog.SelectedPath + "\\";
            }
        }
        private void button_conf_names_Click(object sender, EventArgs e)
        {
            // Abrir formulario de relación de nombres con voto
            FormNamesBind formNamesBind = new FormNamesBind();
            formNamesBind.StartPosition = FormStartPosition.CenterParent;

            if (formNamesBind.ShowDialog() == DialogResult.OK)
            {
                UserControlVoting.array_nombres = FormNamesBind.names;
            }
        }

        private void button_ajustes_aceptar_Click(object sender, EventArgs e)
        {
            if (validaAjustesInterfaz())
            {
                // Se guardan en el fichero los datos proporcionados desde la UI
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "TiempoCrono", this.numericUpDown_ajustes_tiempo_crono.Value.ToString());
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta", this.radioButton_ajustes_cambiar_resp_Si.Checked.ToString());
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "TipoVotacion", this.comboBox_ajustes_tipo_votacion.SelectedIndex.ToString());
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "NumeroOpciones", this.numericUpDown_ajustes_num_opciones.Value.ToString());
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "ConexionGrafismo", this.radioButton_ajustes_conexion_grafismo_Si.Checked.ToString());
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Ip", this.textBox_ip.Text);
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "RutaResultados", this.textBox_ajustes_resultados_path.Text);

                // Se cargan en memoria (clase CAjustes) los ajustes
                CAjustes.tiempo_crono = (int)this.numericUpDown_ajustes_tiempo_crono.Value;
                CAjustes.permitir_cambio_respuesta = this.radioButton_ajustes_cambiar_resp_Si.Checked;
                CAjustes.tipo_votacion = this.comboBox_ajustes_tipo_votacion.SelectedIndex;
                CAjustes.numero_opciones = (int)this.numericUpDown_ajustes_num_opciones.Value;
                CAjustes.conexion_grafismo = this.radioButton_ajustes_conexion_grafismo_Si.Checked;
                CAjustes.ip = this.textBox_ip.Text;
                CAjustes.ruta_resultados = this.textBox_ajustes_resultados_path.Text;

                this.communicatorCallBack("UserControlSettings"); // Comunicación al Form Principal
            }
        }

        // ##############   Validation   ############## \\
        private void checkAndSetFileData()
        {
            if (validaAjustesFicheroXml())
            {
                // Leer Ajustes del fichero XML y establecer controles
                this.button_mandos.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales") + " Mandos";
                this.numericUpDown_ajustes_tiempo_crono.Value = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TiempoCrono"));
                this.label_base_id.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "BaseAntena");
                this.radioButton_ajustes_cambiar_resp_Si.Checked = bool.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta"));
                this.radioButton_ajustes_cambiar_resp_No.Checked = !bool.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "PermitirCambioRespuesta")); ;
                this.comboBox_ajustes_tipo_votacion.SelectedIndex = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TipoVotacion"));
                this.numericUpDown_ajustes_num_opciones.Value = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "NumeroOpciones"));
                this.textBox_ajustes_resultados_path.Text = System.IO.Path.GetFullPath(xmlFile.LeerXml(CAjustes.ruta_ajustes, "RutaResultados"));
                this.radioButton_ajustes_conexion_grafismo_Si.Checked = bool.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "ConexionGrafismo"));
                this.radioButton_ajustes_conexion_grafismo_No.Checked = !bool.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "ConexionGrafismo"));
                this.textBox_ip.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Ip");
            }
            else
            {
                // Cargar ajustes por defecto si se encuentra un error en el XML
                this.button_mandos.Text = "100" + " Mandos";
                this.numericUpDown_ajustes_tiempo_crono.Value = 120;
                this.label_base_id.Text = "1";
                this.radioButton_ajustes_cambiar_resp_Si.Checked = false;
                this.radioButton_ajustes_cambiar_resp_No.Checked = true;
                this.comboBox_ajustes_tipo_votacion.SelectedIndex = 0;
                this.numericUpDown_ajustes_num_opciones.Value = 3;
                this.textBox_ajustes_resultados_path.Text = System.IO.Path.GetFullPath("./ ");
                this.radioButton_ajustes_conexion_grafismo_Si.Checked = false;
                this.radioButton_ajustes_conexion_grafismo_No.Checked = true;
                this.textBox_ip.Text = "127.0.0.1";
            }

            // Comprobar si se ha elejido el multichoise cuando se arrancó la aplicación
            if (CAjustes.permitir_multichoice)
            {
                this.radioButton_ajustes_cambiar_resp_Si.Enabled = false;
                this.radioButton_ajustes_cambiar_resp_No.Enabled = false;
                this.panel_op_max.Visible = true;
                Mando.NUMERO_OPCIONES_MAXIMAS = (int)this.numericUpDown_max_op.Value;
            }
        }

        private bool validaAjustesFicheroXml()
        {
            if (int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales")) != FormMandosConfig.counterFromString(xmlFile.LeerXml(CAjustes.ruta_ajustes, "Rangos")))
            {
                MessageBox.Show("El número de mandos totales debe ser igual que el número de mandos en los rangos. Revisar los ajustes de los mandos", "Error en el archivo XML");
                return false;
            }

            if (int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TiempoCrono")) < 1 || int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TiempoCrono")) > 1000)
            {
                MessageBox.Show("El tiempo del crono debe estar comprendido entre 1 y 1000. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            if (int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TipoVotacion")) < 0 || int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "TipoVotacion")) > 2)
            {
                MessageBox.Show("Tipo de votación incorrecta. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            if (int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "NumeroOpciones")) < 1 || int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "NumeroOpciones")) > 10)
            {
                MessageBox.Show("EL número de opciones debe estar comprendido entre 1 y 10. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            if (!System.Net.IPAddress.TryParse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "Ip"), out _))
            {
                MessageBox.Show("EL número de IP proporcionado no es válido. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            if (!System.IO.Directory.Exists(xmlFile.LeerXml(CAjustes.ruta_ajustes, "RutaResultados")))
            {
                MessageBox.Show("El directorio seleccionado no existe. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            return true;
        }

        private bool validaAjustesInterfaz()
        {
            if (!System.Net.IPAddress.TryParse(this.textBox_ip.Text, out _))
            {
                MessageBox.Show("EL número de IP proporcionado no es válido.", "Error de entrada");
                return false;
            }

            if (!System.IO.Directory.Exists(this.textBox_ajustes_resultados_path.Text))
            {
                MessageBox.Show("El directorio seleccionado no existe", "Error de entrada");
                return false;
            }

            return true;
        }


        // ##############   Public functions   ############## \\
        public void setImageConnectionStatus(Image image)
        {
            this.panel_indicador_conex_base.BackgroundImage = image;
        }

        public void setEnableButtonAceptar(bool value)
        {
            this.button_ajustes_aceptar.Enabled = value;
        }

        private void numericUpDown_max_op_ValueChanged(object sender, EventArgs e)
        {
            Mando.NUMERO_OPCIONES_MAXIMAS = (int)this.numericUpDown_max_op.Value;
        }
    }
}
