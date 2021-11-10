using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Votaciones_App.Formularios
{
    public partial class FormMandosConfig : Form
    {
        private int numeroTotalMandos;
        private Dictionary<string, string> rangos = new Dictionary<string, string>();

        public FormMandosConfig()
        {
            InitializeComponent();
        }

        private void crearRangos(string rawRanges)
        {
            string[] ranges = rawRanges.Split(',');
            int counter = 1;
            foreach (string range in ranges)
            {
                this.rangos.Add("rango_" + counter, range);
                counter++;
            }
        }

        private void setNumMandosTotales()
        {
            foreach (KeyValuePair<string, string> entry in this.rangos)
                this.numeroTotalMandos += counter(entry.Value);
        }
        public static int counterFromString(string rawData)
        {
            int result = 0;
            string[] ranges = rawData.Split(',');
            foreach (string range in ranges)
            {
                result += counter(range);
            }
            return result;
        }

        public static int counter(string data)
        {
            string[] range = data.Split('-');
            int numeroInferior = int.Parse(range[0]);
            int numeroSuperior = int.Parse(range[1]);

            return numeroSuperior - numeroInferior + 1;
        }
        public int getMandosTotales()
        {
            return this.numeroTotalMandos;
        }

        public List<List<int>> getRanges()
        {
            List<List<int>> result = new List<List<int>>();
            List<int> data = new List<int>();
            foreach (KeyValuePair<string, string> entry in this.rangos)
            {
                string[] range = entry.Value.Split('-');
                int numeroInferior = int.Parse(range[0]);
                int numeroSuperior = int.Parse(range[1]);
                data.Add(numeroInferior);
                data.Add(numeroSuperior);
                result.Add(data);
            }
            return result;
        }

        private bool validarEntrada()
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
        private void button_aceptar_Click(object sender, EventArgs e)
        {
            if (validarEntrada())
            {
                // Guarda en memoria los ajustes proporcionados desde la UI
                CAjustes.num_mandos = int.Parse(this.textBox_mandos.Text);
                CAjustes.rangos = this.textBox_rangos.Text;

                crearRangos(CAjustes.rangos);
                setNumMandosTotales();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button_cancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormMandosConfig_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.textBox_mandos.Text = CAjustes.num_mandos.ToString();
            this.textBox_rangos.Text = CAjustes.rangos;
        }
    }
}
