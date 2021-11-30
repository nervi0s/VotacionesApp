using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Votaciones_App.Formularios;
using Votaciones_App.Views;

namespace Votaciones_App
{
    // Clase que crea una ventana auxiliar que muestra información de los mandos que votan y sus respuestas
    public partial class FormResultados : Form
    {

        // ##############   Constructor  ############## \\
        public FormResultados(Point location)
        {
            InitializeComponent();
            this.Location = location;
        }

        // ##############   Event controls   ############## \\
        private void FormResultados_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Consume the close event
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        // ##############   Public functions   ############## \\
        public void inicializa_grid()
        {
            List<int> ids = FormMandosConfig.createIDsList();

            this.dataGridView1.Columns.Clear(); // Se limpia el dataGridView
            this.dataGridView1.Rows.Clear();  // Se limpia el dataGridView
            this.dataGridView1.Refresh();  // Se limpia el dataGridView

            this.dataGridView1.Columns.Add("Id", "ID");
            this.dataGridView1.Columns["Id"].Width = 50;
            this.dataGridView1.Columns["Id"].ReadOnly = false;
            this.dataGridView1.Columns["Id"].Visible = true;

            this.dataGridView1.Columns.Add("Voto", "VOTO");
            this.dataGridView1.Columns["Voto"].Width = 100;
            this.dataGridView1.Columns["Voto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns["Voto"].ReadOnly = false;
            this.dataGridView1.Columns["Voto"].Visible = true;

            this.dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.Columns[0].DefaultCellStyle.Font = new Font("Microsoft Sans", 8, FontStyle.Bold);

            this.dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.Columns[1].DefaultCellStyle.Font = new Font("Microsoft Sans", 8, FontStyle.Regular);

            //Meto las filas
            for (int i = 0; i < CAjustes.num_mandos; i++)
            {
                this.dataGridView1.Rows.Add(ids[i], "");
            }

            // Pongo de otro color las celdas alternando
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (i % 2 != 0)
                {
                    Color color = Color.FromArgb(227, 219, 236);
                    this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = color;
                }
            }
            this.dataGridView1.ClearSelection();
        }

        public void actualizar_grid(List<Mando> lista_mandos)
        {
            for (int i = 0; i < CAjustes.num_mandos; i++)
            {
                try
                {
                    if (lista_mandos[i].respuesta.Length > 1)
                    {
                        if (UserControlVoting.array_nombres == null)
                        {
                            this.dataGridView1.Rows[i].Cells[1].Value = lista_mandos[i].respuesta.Substring(1);
                        }
                        else
                        {
                            string toUserInterface = string.Empty;
                            string[] respuestas = lista_mandos[i].respuesta.Substring(1).Split(';');
                            foreach (string respuesta in respuestas)
                            {
                                if (CAjustes.tipo_votacion == 0) // Votación de números
                                    toUserInterface += "; " + respuesta + "-" + UserControlVoting.array_nombres[int.Parse(respuesta) - 1];
                                else if (CAjustes.tipo_votacion == 1) // Votación de letras
                                    toUserInterface += "; " + respuesta + "-" + UserControlVoting.array_nombres[int.Parse(parseLetter(respuesta)) - 1];
                            }
                            this.dataGridView1.Rows[i].Cells[1].Value = toUserInterface.Substring(2);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void setLocation(Point location)
        {
            this.Location = location;
        }

        // ##############   Private functions   ############## \\
        private string parseLetter(string letter)
        {
            switch (letter)
            {
                case "A":
                    return "1";
                case "B":
                    return "2";
                case "C":
                    return "3";
                case "D":
                    return "4";
                case "E":
                    return "5";
                case "F":
                    return "6";
                case "G":
                    return "7";
                case "H":
                    return "8";
                case "I":
                    return "9";
                case "J":
                    return "10";
                default:
                    return "_";
            }
        }
    }
}
