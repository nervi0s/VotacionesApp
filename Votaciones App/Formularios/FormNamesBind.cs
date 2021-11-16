using System;
using System.Windows.Forms;

namespace Votaciones_App.Formularios
{
    // Clase encargada de relacionar el número de votación con un nombre;
    public partial class FormNamesBind : Form
    {
        public static string[] names = new string[10];
        CFileXML xmlFile = new CFileXML();

        // ##############   Constructor  ############## \\
        public FormNamesBind()
        {
            InitializeComponent();
        }

        // ##############   Event controls   ############## \\
        private void FormNamesBind_Load(object sender, EventArgs e)
        {
            loadNamesFromFile();
        }

        private void button_aceptar_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (Control control in this.tableLayoutPanel_base.Controls)
            {
                if (control.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    names[i] = control.Text;
                    i++;
                }
            }

            saveNamesToFile();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ##############   Auxiliary private functions   ############## \\
        private void loadNamesFromFile()
        {
            this.textBox1.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre1");
            this.textBox2.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre2");
            this.textBox3.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre3");
            this.textBox4.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre4");
            this.textBox5.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre5");
            this.textBox6.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre6");
            this.textBox7.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre7");
            this.textBox8.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre8");
            this.textBox9.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre9");
            this.textBox10.Text = xmlFile.LeerXml(CAjustes.ruta_ajustes, "Nombre10");
        }

        private void saveNamesToFile()
        {
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre1", this.textBox1.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre2", this.textBox2.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre3", this.textBox3.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre4", this.textBox4.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre5", this.textBox5.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre6", this.textBox6.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre7", this.textBox7.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre8", this.textBox8.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre9", this.textBox9.Text);
            xmlFile.EscribirXml(CAjustes.ruta_ajustes, "Nombre10", this.textBox10.Text);
        }
    }
}
