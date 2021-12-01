
namespace Votaciones_App.Formularios
{
    partial class FormConfigMandos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigMandos));
            this.tableLayoutPanel_base = new System.Windows.Forms.TableLayoutPanel();
            this.radioButton_rangos = new System.Windows.Forms.RadioButton();
            this.textBox_rangos = new System.Windows.Forms.TextBox();
            this.panel_botton = new System.Windows.Forms.Panel();
            this.tableLayoutPanel_botton = new System.Windows.Forms.TableLayoutPanel();
            this.button_cancelar = new System.Windows.Forms.Button();
            this.button_aceptar = new System.Windows.Forms.Button();
            this.radioButton_automode = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_num_mandos = new System.Windows.Forms.Label();
            this.numericUpDown_num_mandos_totales = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel_base.SuspendLayout();
            this.panel_botton.SuspendLayout();
            this.tableLayoutPanel_botton.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_num_mandos_totales)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_base
            // 
            this.tableLayoutPanel_base.ColumnCount = 2;
            this.tableLayoutPanel_base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66667F));
            this.tableLayoutPanel_base.Controls.Add(this.radioButton_rangos, 0, 1);
            this.tableLayoutPanel_base.Controls.Add(this.textBox_rangos, 1, 1);
            this.tableLayoutPanel_base.Controls.Add(this.panel_botton, 0, 2);
            this.tableLayoutPanel_base.Controls.Add(this.radioButton_automode, 0, 0);
            this.tableLayoutPanel_base.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tableLayoutPanel_base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_base.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_base.Name = "tableLayoutPanel_base";
            this.tableLayoutPanel_base.RowCount = 3;
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_base.Size = new System.Drawing.Size(457, 200);
            this.tableLayoutPanel_base.TabIndex = 2;
            // 
            // radioButton_rangos
            // 
            this.radioButton_rangos.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.radioButton_rangos.Checked = true;
            this.radioButton_rangos.Location = new System.Drawing.Point(41, 98);
            this.radioButton_rangos.Name = "radioButton_rangos";
            this.radioButton_rangos.Size = new System.Drawing.Size(108, 28);
            this.radioButton_rangos.TabIndex = 14;
            this.radioButton_rangos.TabStop = true;
            this.radioButton_rangos.Text = "Rango/s.";
            this.radioButton_rangos.UseVisualStyleBackColor = true;
            this.radioButton_rangos.CheckedChanged += new System.EventHandler(this.radioButton_rangos_CheckedChanged);
            // 
            // textBox_rangos
            // 
            this.textBox_rangos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_rangos.Location = new System.Drawing.Point(155, 102);
            this.textBox_rangos.Name = "textBox_rangos";
            this.textBox_rangos.Size = new System.Drawing.Size(299, 20);
            this.textBox_rangos.TabIndex = 10;
            this.textBox_rangos.TextChanged += new System.EventHandler(this.textBox_rangos_TextChanged);
            // 
            // panel_botton
            // 
            this.tableLayoutPanel_base.SetColumnSpan(this.panel_botton, 2);
            this.panel_botton.Controls.Add(this.tableLayoutPanel_botton);
            this.panel_botton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_botton.Location = new System.Drawing.Point(3, 153);
            this.panel_botton.Name = "panel_botton";
            this.panel_botton.Size = new System.Drawing.Size(451, 44);
            this.panel_botton.TabIndex = 12;
            // 
            // tableLayoutPanel_botton
            // 
            this.tableLayoutPanel_botton.ColumnCount = 2;
            this.tableLayoutPanel_botton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_botton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_botton.Controls.Add(this.button_cancelar, 1, 0);
            this.tableLayoutPanel_botton.Controls.Add(this.button_aceptar, 0, 0);
            this.tableLayoutPanel_botton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_botton.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_botton.Name = "tableLayoutPanel_botton";
            this.tableLayoutPanel_botton.RowCount = 1;
            this.tableLayoutPanel_botton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_botton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel_botton.Size = new System.Drawing.Size(451, 44);
            this.tableLayoutPanel_botton.TabIndex = 0;
            // 
            // button_cancelar
            // 
            this.button_cancelar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_cancelar.Location = new System.Drawing.Point(255, 3);
            this.button_cancelar.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
            this.button_cancelar.Name = "button_cancelar";
            this.button_cancelar.Size = new System.Drawing.Size(166, 38);
            this.button_cancelar.TabIndex = 1;
            this.button_cancelar.Text = "Cancelar";
            this.button_cancelar.UseVisualStyleBackColor = true;
            this.button_cancelar.Click += new System.EventHandler(this.button_cancelar_Click);
            // 
            // button_aceptar
            // 
            this.button_aceptar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_aceptar.Location = new System.Drawing.Point(30, 3);
            this.button_aceptar.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
            this.button_aceptar.Name = "button_aceptar";
            this.button_aceptar.Size = new System.Drawing.Size(165, 38);
            this.button_aceptar.TabIndex = 0;
            this.button_aceptar.Text = "Aceptar";
            this.button_aceptar.UseVisualStyleBackColor = true;
            this.button_aceptar.Click += new System.EventHandler(this.button_aceptar_Click);
            // 
            // radioButton_automode
            // 
            this.radioButton_automode.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.radioButton_automode.Location = new System.Drawing.Point(41, 23);
            this.radioButton_automode.Name = "radioButton_automode";
            this.radioButton_automode.Size = new System.Drawing.Size(108, 28);
            this.radioButton_automode.TabIndex = 13;
            this.radioButton_automode.Text = "Automode.";
            this.radioButton_automode.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label_num_mandos, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown_num_mandos_totales, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(152, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(305, 75);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // label_num_mandos
            // 
            this.label_num_mandos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_num_mandos.AutoSize = true;
            this.label_num_mandos.Location = new System.Drawing.Point(3, 31);
            this.label_num_mandos.Name = "label_num_mandos";
            this.label_num_mandos.Size = new System.Drawing.Size(146, 13);
            this.label_num_mandos.TabIndex = 0;
            this.label_num_mandos.Text = "Número de mandos totales:";
            this.label_num_mandos.Visible = false;
            // 
            // numericUpDown_num_mandos_totales
            // 
            this.numericUpDown_num_mandos_totales.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDown_num_mandos_totales.Location = new System.Drawing.Point(155, 27);
            this.numericUpDown_num_mandos_totales.Name = "numericUpDown_num_mandos_totales";
            this.numericUpDown_num_mandos_totales.Size = new System.Drawing.Size(88, 20);
            this.numericUpDown_num_mandos_totales.TabIndex = 1;
            this.numericUpDown_num_mandos_totales.Visible = false;
            // 
            // FormConfigMandos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 200);
            this.Controls.Add(this.tableLayoutPanel_base);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormConfigMandos";
            this.Text = "Configuración de mandos";
            this.Load += new System.EventHandler(this.FormConfigMandos_Load);
            this.tableLayoutPanel_base.ResumeLayout(false);
            this.tableLayoutPanel_base.PerformLayout();
            this.panel_botton.ResumeLayout(false);
            this.tableLayoutPanel_botton.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_num_mandos_totales)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_base;
        private System.Windows.Forms.RadioButton radioButton_rangos;
        private System.Windows.Forms.TextBox textBox_rangos;
        private System.Windows.Forms.Panel panel_botton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_botton;
        private System.Windows.Forms.Button button_cancelar;
        private System.Windows.Forms.Button button_aceptar;
        private System.Windows.Forms.RadioButton radioButton_automode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_num_mandos;
        private System.Windows.Forms.NumericUpDown numericUpDown_num_mandos_totales;
    }
}