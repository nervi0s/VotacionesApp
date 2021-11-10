using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Votaciones_App.Formularios;

namespace Votaciones_App.Views
{
    public partial class UserControlSettings : UserControl
    {
        private Action<string> loadGraphicsPanel;


        public UserControlSettings(Action<string> loadGraphicsPanel)
        {
            InitializeComponent();
            this.loadGraphicsPanel = loadGraphicsPanel;

        }

        private void UserControlSettings_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            establecerControles();
        }

        private void establecerControles()
        {
            // Leer Ajustes de memoria y establecer controles
            this.button_mandos.Text = CAjustes.num_mandos.ToString() + " mandos";
            this.numericUpDown_ajustes_tiempo_crono.Value = CAjustes.tiempo_crono;
            this.label_base_id.Text = CAjustes.base_antena.Value.ToString();
            this.radioButton_ajustes_cambiar_resp_Si.Checked = CAjustes.permitir_cambio_respuesta;
            this.radioButton_ajustes_cambiar_resp_No.Checked = !CAjustes.permitir_cambio_respuesta;

            this.comboBox_ajustes_tipo_votacion.SelectedIndex = CAjustes.tipo_votacion;
            this.numericUpDown_ajustes_num_opciones.Value = CAjustes.numero_opciones;
            this.textBox_ajustes_resultados_path.Text = System.IO.Path.GetFullPath(CAjustes.ruta_resultados);
            this.radioButton_ajustes_conexion_grafismo_Si.Checked = CAjustes.conexion_grafismo;
            this.radioButton_ajustes_conexion_grafismo_No.Checked = !CAjustes.conexion_grafismo;
            this.textBox_ip.Text = CAjustes.ip;
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

        }

        private bool valida_ajustes()
        {
            if (this.numericUpDown_ajustes_tiempo_crono.Value < 1 || this.numericUpDown_ajustes_tiempo_crono.Value > 1000)
            {
                MessageBox.Show("El tiempo del crono debe estar comprendido entre 1 y 1000", "Error de entrada");
                return false;
            }
            if (!this.radioButton_ajustes_cambiar_resp_Si.Checked && !this.radioButton_ajustes_cambiar_resp_No.Checked)
            {
                MessageBox.Show("Por favor, elija si se permite cambiar de respuesta o no", "Error de entrada");
                return false;
            }
            if (this.comboBox_ajustes_tipo_votacion.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, elija un tipo de votación", "Error de entrada");
                return false;
            }

            if (this.numericUpDown_ajustes_num_opciones.Value < 1 || this.numericUpDown_ajustes_num_opciones.Value > 10)
            {
                MessageBox.Show("EL número de opciones debe estar comprendido entre 1 y 10", "Error de entrada");
                return false;
            }
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

        private void button_ajustes_aceptar_Click(object sender, EventArgs e)
        {
            if (valida_ajustes())
            {
                // Guarda en memoria los ajustes proporcionados desde la UI
                CAjustes.tiempo_crono = (int)this.numericUpDown_ajustes_tiempo_crono.Value;
                //CAjustes.base_antena = (int)this.numericUpDown_base_antena.Value;
                CAjustes.permitir_cambio_respuesta = this.radioButton_ajustes_cambiar_resp_Si.Checked;
               
                CAjustes.tipo_votacion = this.comboBox_ajustes_tipo_votacion.SelectedIndex;
                CAjustes.numero_opciones = (int)this.numericUpDown_ajustes_num_opciones.Value;
                CAjustes.ruta_resultados = this.textBox_ajustes_resultados_path.Text;
                CAjustes.conexion_grafismo = this.radioButton_ajustes_conexion_grafismo_Si.Checked;
                CAjustes.ip = this.textBox_ip.Text;

                loadGraphicsPanel("Cargando panel gráfico");
            }
        }

        public void setImageConnectionStatus(Image image)
        {
            this.panel_indicador_conex_base.BackgroundImage = image;
        }

        public void setEnableButtonAceptar(bool value)
        {
            this.button_ajustes_aceptar.Enabled = value;
        }



        private void button_mandos_Click(object sender, EventArgs e)
        {
            FormMandosConfig formMandosConfig = new FormMandosConfig();
            formMandosConfig.StartPosition = FormStartPosition.CenterParent;
            if (formMandosConfig.ShowDialog() == DialogResult.OK)
            {
                this.button_mandos.Text = formMandosConfig.getMandosTotales().ToString() + " mandos";
                formMandosConfig.getRanges();
            }
        }
    }
}
