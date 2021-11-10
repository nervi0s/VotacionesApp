using System;
using System.Windows.Forms;

namespace Votaciones_App.Formularios
{
    public partial class FormMandosConfig : Form
    {
        CFileXML xmlFile = new CFileXML();
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

        private void button_aceptar_Click(object sender, EventArgs e)
        {
            if (validaAjustesInterfaz())
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
        }

        private void button_cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // To prevent non numeric values
        private void textBox_mandos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
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

            //ToDO expresión regular para validar de los rangos que viene en el XML

            return true;
        }

        private bool validaAjustesInterfaz()
        {
            if (!int.TryParse(this.textBox_mandos.Text, out _))
            {
                MessageBox.Show("Debe introducir un número entero en los mandos totales. Proporcione datos correctos", "Error de entrada");
                return false;
            }

            if (counterFromString(this.textBox_rangos.Text) != int.Parse(this.textBox_mandos.Text))
            {
                MessageBox.Show("Número de mandos totales distinto al número de mandos en los rangos", "Error de entrada");
                return false;
            }

            //ToDo expresión regular para validar datos de entrada de los rangos

            return true;
        }

        // ##############   Public events   ############## \\
        public int getMandosTotales()
        {
            return this.numeroTotalMandos;
        }

        // ##############   Static methods   ############## \\
        public static int counterFromString(string rawData)
        {
            int result = 0;
            string[] ranges = rawData.Split(',');
            foreach (string range in ranges)
            {
                result += counterBetweenNumbers(range);
            }
            return result;
        }

        private static int counterBetweenNumbers(string data)
        {
            string[] range = data.Split('-');
            int numeroInferior = int.Parse(range[0]);
            int numeroSuperior = int.Parse(range[1]);

            return numeroSuperior - numeroInferior + 1;
        }
    }
}
