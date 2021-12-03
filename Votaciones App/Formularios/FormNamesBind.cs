using System;
using System.Windows.Forms;

namespace Votaciones_App.Formularios
{
    // Clase encargada de relacionar el número de votación con un nombre;
    public partial class FormNamesBind : Form
    {
        public static string[] names = new string[10];
        private CFileXML xmlFile = new CFileXML();

        // ##############   Constructor  ############## \\
        public FormNamesBind()
        {
            InitializeComponent();
        }

        // ##############   Event controls   ############## \\
        private void FormNamesBind_Load(object sender, EventArgs e)
        {
            loadNamesFromFile();
            loadOptionsFromFile();
        }

        private void button_aceptar_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (Control control in this.tableLayoutPanel_base.Controls)
            {
                if (control.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    names[i] = control.Text.Trim();
                    i++;
                }
            }

            saveNamesToFile();
            saveOptionsToFile();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void numericUpDown_ajustes_num_opciones_ValueChanged(object sender, EventArgs e)
        {
            enableControls((int)this.numericUpDown_ajustes_num_opciones.Value);
        }

        private void numericUpDown_opciones_elegibles_ValueChanged(object sender, EventArgs e)
        {
            Mando.NUMERO_OPCIONES_MAXIMAS = (int)this.numericUpDown_opciones_elegibles.Value;
        }

        // ##############   Auxiliary private functions   ############## \\
        private void loadNamesFromFile()
        {
            this.textBox1.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre1").Replace("\r\n", string.Empty).Trim();
            this.textBox2.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre2").Replace("\r\n", string.Empty).Trim();
            this.textBox3.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre3").Replace("\r\n", string.Empty).Trim();
            this.textBox4.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre4").Replace("\r\n", string.Empty).Trim();
            this.textBox5.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre5").Replace("\r\n", string.Empty).Trim();
            this.textBox6.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre6").Replace("\r\n", string.Empty).Trim();
            this.textBox7.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre7").Replace("\r\n", string.Empty).Trim();
            this.textBox8.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre8").Replace("\r\n", string.Empty).Trim();
            this.textBox9.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre9").Replace("\r\n", string.Empty).Trim();
            this.textBox10.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre10").Replace("\r\n", string.Empty).Trim();
        }

        private void saveNamesToFile()
        {
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre1", this.textBox1.Text.Trim());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre2", this.textBox2.Text.Trim());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre3", this.textBox3.Text.Trim());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre4", this.textBox4.Text.Trim());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre5", this.textBox5.Text.Trim());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre6", this.textBox6.Text.Trim());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre7", this.textBox7.Text.Trim());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre8", this.textBox8.Text.Trim());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre9", this.textBox9.Text.Trim());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre10", this.textBox10.Text.Trim());
        }

        private void loadOptionsFromFile()
        {
            this.numericUpDown_ajustes_num_opciones.Value = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "NumeroOpciones"));
            this.numericUpDown_opciones_elegibles.Value = int.Parse(xmlFile.LeerXml(CAjustes.ruta_ajustes, "OpcionesElegibles"));

            enableControls((int)this.numericUpDown_ajustes_num_opciones.Value);

            // Visibilidad de los controles en función de si se ha elejido multichoice
            this.label12.Visible = CAjustes.permitir_multichoice;
            this.numericUpDown_opciones_elegibles.Visible = CAjustes.permitir_multichoice;
        }

        private void saveOptionsToFile()
        {
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "NumeroOpciones", this.numericUpDown_ajustes_num_opciones.Value.ToString().Trim());
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "OpcionesElegibles", this.numericUpDown_opciones_elegibles.Value.ToString().Trim());

            // Cargamos los ajustes también en memoria
            CAjustes.numero_opciones = (int)this.numericUpDown_ajustes_num_opciones.Value;
            CAjustes.numero_opciones_elegibles = (int)this.numericUpDown_opciones_elegibles.Value;
        }

        private void enableControls(int options)
        {
            foreach (Control control in this.tableLayoutPanel_base.Controls)
            {
                if (control.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    int textBoxNumber = int.Parse(System.Text.RegularExpressions.Regex.Match(control.Name, @"\d+$").Value);
                    control.Enabled = textBoxNumber <= options;
                }
            }

            this.numericUpDown_opciones_elegibles.Maximum = options;
        }
    }
}
