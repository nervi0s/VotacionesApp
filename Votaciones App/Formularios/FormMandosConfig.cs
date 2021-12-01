using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Votaciones_App.Formularios
{
    // Clase encargada de configurar la cantidad de mandos a usar en una votación y sus rangos
    public partial class FormMandosConfig : Form
    {
        private CFileXML xmlFile = new CFileXML();
        private int numeroTotalMandos;

        // ##############   Constructor  ############## \\
        public FormMandosConfig()
        {
            InitializeComponent();
        }

        // ##############   Event controls   ############## \\
        private void FormMandosConfig_Load(object sender, EventArgs e)
        {
            checkAndSetFileData();
        }

        private void textBox_rangos_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox_rangos.Text.Contains("."))
            {
                this.button_aceptar.Enabled = false;
                this.textBox_mandos.Text = "Error en el formato de rangos";
            }
            else if (this.textBox_rangos.Text.Contains(" "))
            {
                this.button_aceptar.Enabled = false; ;
                this.textBox_mandos.Text = "Error en el formato de rangos";
            }
            else if (this.textBox_rangos.Text.Any(x => char.IsLetter(x)))
            {
                this.button_aceptar.Enabled = false; ;
                this.textBox_mandos.Text = "Error en el formato de rangos";
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(this.textBox_rangos.Text, @"^(\d*-\d*-+\d*)$"))
            {
                this.button_aceptar.Enabled = false; ;
                this.textBox_mandos.Text = "Error en el formato de rangos";
            }
            else if (this.textBox_rangos.Text.EndsWith(","))
            {
                this.button_aceptar.Enabled = false; ;
                this.textBox_mandos.Text = "Error en el formato de rangos";
            }
            else if (counterFromString(this.textBox_rangos.Text) == -1)
            {
                this.button_aceptar.Enabled = false;
                this.textBox_mandos.Text = "Error en el formato de rangos";
            }
            else
            {
                this.textBox_mandos.Text = counterFromString(this.textBox_rangos.Text).ToString();
                this.button_aceptar.Enabled = true;
            }
        }

        private void button_aceptar_Click(object sender, EventArgs e)
        {
            // Se guardan en el fichero los datos proporcionados desde la UI
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "MandosTotales", this.textBox_mandos.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Rangos", this.textBox_rangos.Text);

            // Se cargan en memoria (clase CAjustes) los ajustes
            CAjustes.num_mandos = int.Parse(this.textBox_mandos.Text);
            CAjustes.rangos = this.textBox_rangos.Text;

            this.numeroTotalMandos = counterFromString(this.textBox_rangos.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
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
                this.textBox_mandos.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales");
                this.textBox_rangos.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Rangos");
            }
            else
            {
                // Cargar ajustes por defecto
                this.textBox_mandos.Text = "100";
                this.textBox_rangos.Text = "1-100";
            }
        }

        private bool validaAjustesFicheroXml()
        {
            if (int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "MandosTotales")) != counterFromString(xmlFile.LeerXml(CAjustes.ruta_ajustes, "Rangos")))
            {
                MessageBox.Show("El número de mandos totales debe ser igual que el número de mandos en los rangos. Cargando ajustes por defecto", "Error en el archivo XML");
                return false;
            }

            return true;
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
