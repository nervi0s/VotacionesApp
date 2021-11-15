using System;
using System.Windows.Forms;

namespace Votaciones_App.Formularios
{
    // Clase encargada de relacionar el número de votación con un nombre;
    public partial class FormNamesBind : Form
    {
        public static string[] names = new string[10];

        // ##############   Constructor  ############## \\
        public FormNamesBind()
        {
            InitializeComponent();
        }

        // ##############   Event controls   ############## \\
        private void button_aceptar_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (Control control in this.tableLayoutPanel_base.Controls)
            {
                Console.WriteLine(control.GetType().ToString());
                if (control.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    names[i] = control.Text;
                    i++;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
