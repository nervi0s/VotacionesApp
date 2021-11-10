
namespace Votaciones_App.Views
{
    partial class UserControlConnectionChoice
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel_base = new System.Windows.Forms.TableLayoutPanel();
            this.label_info_2 = new System.Windows.Forms.Label();
            this.button_ethernet = new System.Windows.Forms.Button();
            this.label_info_1 = new System.Windows.Forms.Label();
            this.button_usb = new System.Windows.Forms.Button();
            this.textBox_id = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel_base.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_base
            // 
            this.tableLayoutPanel_base.ColumnCount = 3;
            this.tableLayoutPanel_base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.841F));
            this.tableLayoutPanel_base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.31799F));
            this.tableLayoutPanel_base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.84101F));
            this.tableLayoutPanel_base.Controls.Add(this.label_info_2, 0, 2);
            this.tableLayoutPanel_base.Controls.Add(this.button_ethernet, 2, 1);
            this.tableLayoutPanel_base.Controls.Add(this.label_info_1, 0, 0);
            this.tableLayoutPanel_base.Controls.Add(this.button_usb, 0, 1);
            this.tableLayoutPanel_base.Controls.Add(this.textBox_id, 1, 3);
            this.tableLayoutPanel_base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_base.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_base.Name = "tableLayoutPanel_base";
            this.tableLayoutPanel_base.RowCount = 4;
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_base.Size = new System.Drawing.Size(899, 377);
            this.tableLayoutPanel_base.TabIndex = 0;
            // 
            // label_info_2
            // 
            this.tableLayoutPanel_base.SetColumnSpan(this.label_info_2, 3);
            this.label_info_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_info_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_info_2.Location = new System.Drawing.Point(3, 188);
            this.label_info_2.Name = "label_info_2";
            this.label_info_2.Size = new System.Drawing.Size(893, 94);
            this.label_info_2.TabIndex = 4;
            this.label_info_2.Text = "Introduzca el ID de la antena base";
            this.label_info_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_ethernet
            // 
            this.button_ethernet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_ethernet.Location = new System.Drawing.Point(525, 97);
            this.button_ethernet.Margin = new System.Windows.Forms.Padding(3, 3, 35, 3);
            this.button_ethernet.Name = "button_ethernet";
            this.button_ethernet.Size = new System.Drawing.Size(339, 88);
            this.button_ethernet.TabIndex = 3;
            this.button_ethernet.Text = "ETHERNET";
            this.button_ethernet.UseVisualStyleBackColor = true;
            this.button_ethernet.Click += new System.EventHandler(this.button_ethernet_Click);
            // 
            // label_info_1
            // 
            this.tableLayoutPanel_base.SetColumnSpan(this.label_info_1, 3);
            this.label_info_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_info_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_info_1.Location = new System.Drawing.Point(3, 0);
            this.label_info_1.Name = "label_info_1";
            this.label_info_1.Size = new System.Drawing.Size(893, 94);
            this.label_info_1.TabIndex = 0;
            this.label_info_1.Text = "Seleccione un modo de conexión";
            this.label_info_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_usb
            // 
            this.button_usb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_usb.Location = new System.Drawing.Point(35, 97);
            this.button_usb.Margin = new System.Windows.Forms.Padding(35, 3, 3, 3);
            this.button_usb.Name = "button_usb";
            this.button_usb.Size = new System.Drawing.Size(338, 88);
            this.button_usb.TabIndex = 1;
            this.button_usb.Text = "USB";
            this.button_usb.UseVisualStyleBackColor = true;
            this.button_usb.Click += new System.EventHandler(this.button_usb_Click);
            // 
            // textBox_id
            // 
            this.textBox_id.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_id.Location = new System.Drawing.Point(386, 319);
            this.textBox_id.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.textBox_id.Name = "textBox_id";
            this.textBox_id.Size = new System.Drawing.Size(126, 20);
            this.textBox_id.TabIndex = 5;
            this.textBox_id.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // UserControlConnectionChoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel_base);
            this.Name = "UserControlConnectionChoice";
            this.Size = new System.Drawing.Size(899, 377);
            this.Load += new System.EventHandler(this.UserControlConnectionChoice_Load);
            this.tableLayoutPanel_base.ResumeLayout(false);
            this.tableLayoutPanel_base.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_base;
        private System.Windows.Forms.Button button_ethernet;
        private System.Windows.Forms.Label label_info_1;
        private System.Windows.Forms.Button button_usb;
        private System.Windows.Forms.Label label_info_2;
        private System.Windows.Forms.TextBox textBox_id;
    }
}
