using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Votaciones_App.Formularios;

namespace Votaciones_App
{
    // Clase que crea una ventana auxiliar que muestra información de los mandos que votan y sus respuestas
    public partial class FormResultados : Form
    {
        private bool inicializado = false;

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

            if (this.inicializado)
            {
                for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                {
                    for (int j = 0; j < this.dataGridView1.Rows.Count; j++)
                    {
                        this.dataGridView1.Rows[j].Cells[i].Value = "";
                    }
                }

                for (int j = 0; j < this.dataGridView1.Rows.Count; j++)
                {
                    this.dataGridView1.Rows[j].Cells[0].Value = ids[j];
                }
                this.dataGridView1.ClearSelection();
                return;
            }


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
            this.inicializado = true;
        }

        public void actualizar_grid(List<Mando> lista_mandos)
        {
            for (int i = 0; i < CAjustes.num_mandos; i++)
            {
                try
                {
                    if (lista_mandos[i].respuesta.Length > 1)
                    {
                        this.dataGridView1.Rows[i].Cells[1].Value = lista_mandos[i].respuesta.Substring(1);
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
    }
}
