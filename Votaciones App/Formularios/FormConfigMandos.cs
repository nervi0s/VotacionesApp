using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Votaciones_App.Formularios
{
    public partial class FormConfigMandos : Form
    {
        private CFileXML xmlFile = new CFileXML();
        private int numeroTotalMandos;

        public FormConfigMandos()
        {
            InitializeComponent();
        }

        private void FormConfigMandos_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            checkAndSetFileData();
        }

        private void button_aceptar_Click(object sender, EventArgs e)
        {
            // Se guardan en el fichero los datos proporcionados desde la UI
            if (this.radioButton_rangos.Checked)
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MandosTotales", counterFromString(this.textBox_rangos.Text).ToString());
            else
                xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MandosTotales", this.numericUpDown_num_mandos_totales.Value.ToString());

            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Rangos", this.textBox_rangos.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Automode", this.radioButton_automode.Checked.ToString());

            // Se cargan en memoria (clase CAjustes) los ajustes
            CAjustes.num_mandos = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales"));
            CAjustes.rangos = this.textBox_rangos.Text;
            CAjustes.automode = this.radioButton_automode.Checked;

            this.numeroTotalMandos = CAjustes.num_mandos;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioButton_rangos_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton_rangos.Checked)
            {
                this.textBox_rangos.Visible = true;
                this.label_num_mandos.Visible = false;
                this.numericUpDown_num_mandos_totales.Visible = false;
            }
            else
            {
                this.textBox_rangos.Visible = false;
                this.label_num_mandos.Visible = true;
                this.numericUpDown_num_mandos_totales.Visible = true;
            }
        }

        private void textBox_rangos_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox_rangos.Text == string.Empty)
            {
                this.button_aceptar.Enabled = false;
            }
            else if (this.textBox_rangos.Text.Contains("."))
            {
                this.button_aceptar.Enabled = false;
            }
            else if (this.textBox_rangos.Text.Contains(" "))
            {
                this.button_aceptar.Enabled = false; ;
            }
            else if (this.textBox_rangos.Text.Any(x => char.IsLetter(x)))
            {
                this.button_aceptar.Enabled = false; ;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(this.textBox_rangos.Text, @"^(\d*-\d*-+\d*)$"))
            {
                this.button_aceptar.Enabled = false; ;
            }
            else if (this.textBox_rangos.Text.EndsWith(","))
            {
                this.button_aceptar.Enabled = false; ;
            }
            else if (counterFromString(this.textBox_rangos.Text) == -1)
            {
                this.button_aceptar.Enabled = false;
            }
            else
            {
                this.numeroTotalMandos = counterFromString(this.textBox_rangos.Text);
                this.button_aceptar.Enabled = true;
            }
        }

        // ##############   Validation   ############## \\
        private void checkAndSetFileData()
        {
            if (validaAjustesFicheroXml())
            {
                // Leer Ajustes del fichero XML y establecer controles
                this.numericUpDown_num_mandos_totales.Value = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales"));
                this.textBox_rangos.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Rangos");

                if (CAjustes.automode)
                {
                    this.radioButton_automode.Checked = true;
                }
                else
                {
                    this.radioButton_rangos.Checked = true;
                }
            }
            else
            {
                // Cargar ajustes por defecto
                this.numericUpDown_num_mandos_totales.Value = 100;
                this.textBox_rangos.Text = "1-100";
            }
        }

        private bool validaAjustesFicheroXml()
        {
            if (!int.TryParse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales"), out _))
            {
                MessageBox.Show("Error al cargar el número de madnos totales. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }
            if (!comprobadorFormatoRangos(xmlFile.LeerXml(CAjustes.ruta_ajustes, "Rangos")))
            {
                MessageBox.Show("Error al cargar los rangos de los mandos. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }
            return true;
        }

        private bool comprobadorFormatoRangos(string formatoRangos)
        {
            if (formatoRangos.Contains("."))
            {
                return false;
            }
            else if (formatoRangos.Contains(" "))
            {
                return false;
            }
            else if (formatoRangos.Any(x => char.IsLetter(x)))
            {
                return false;
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(formatoRangos, @"^(\d*-\d*-+\d*)$"))
            {
                return false;
            }
            else if (formatoRangos.EndsWith(","))
            {
                return false;
            }
            else if (counterFromString(formatoRangos) == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // ##############   Public methods   ############## \\
        public int getMandosTotales()
        {
            return this.numeroTotalMandos;
        }

        // ##############   Static methods   ############## \\
        public static int counterFromString(string rawData)
        {
            try
            {
                int result = 0;
                string[] ranges = rawData.Split(',');
                foreach (string range in ranges)
                {
                    result += counterBetweenNumbers(range);
                }
                return result;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static int counterBetweenNumbers(string data)
        {
            try
            {
                if (data.Contains("-"))
                {
                    string[] range = data.Split('-');
                    int numeroInferior = int.Parse(range[0]);
                    int numeroSuperior = int.Parse(range[1]);

                    return numeroSuperior - numeroInferior + 1;
                }
                return 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<int> createIDsList()
        {
            string[] ranges = CAjustes.rangos.Split(',');
            List<int> ids = new List<int>();

            foreach (string range in ranges)
            {
                if (range.Contains("-"))
                {
                    string[] twoValues = range.Split('-');
                    int numeroInferior = int.Parse(twoValues[0]);
                    int numeroSuperior = int.Parse(twoValues[1]);

                    for (int i = numeroInferior; i <= numeroSuperior; i++)
                    {
                        ids.Add(i);
                    }
                }
                else
                {
                    ids.Add(int.Parse(range));
                }
            }
            return ids;
        }
    }
}
