
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
            this.button_ethernet = new System.Windows.Forms.Button();
            this.button_usb = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox_id = new System.Windows.Forms.TextBox();
            this.label_info_1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel_base.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_base
            // 
            this.tableLayoutPanel_base.ColumnCount = 3;
            this.tableLayoutPanel_base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel_base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel_base.Controls.Add(this.button_ethernet, 1, 2);
            this.tableLayoutPanel_base.Controls.Add(this.button_usb, 1, 1);
            this.tableLayoutPanel_base.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tableLayoutPanel_base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_base.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_base.Name = "tableLayoutPanel_base";
            this.tableLayoutPanel_base.RowCount = 4;
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel_base.Size = new System.Drawing.Size(404, 385);
            this.tableLayoutPanel_base.TabIndex = 0;
            // 
            // button_ethernet
            // 
            this.button_ethernet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_ethernet.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ethernet.Location = new System.Drawing.Point(38, 195);
            this.button_ethernet.Name = "button_ethernet";
            this.button_ethernet.Size = new System.Drawing.Size(328, 136);
            this.button_ethernet.TabIndex = 17;
            this.button_ethernet.Text = "ETHERNET";
            this.button_ethernet.UseVisualStyleBackColor = true;
            this.button_ethernet.Click += new System.EventHandler(this.button_ethernet_Click);
            // 
            // button_usb
            // 
            this.button_usb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_usb.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_usb.Location = new System.Drawing.Point(38, 53);
            this.button_usb.Name = "button_usb";
            this.button_usb.Size = new System.Drawing.Size(328, 136);
            this.button_usb.TabIndex = 10;
            this.button_usb.Text = "USB";
            this.button_usb.UseVisualStyleBackColor = true;
            this.button_usb.Click += new System.EventHandler(this.button_usb_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textBox_id, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_info_1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(35, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(334, 50);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // textBox_id
            // 
            this.textBox_id.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox_id.Location = new System.Drawing.Point(95, 15);
            this.textBox_id.Margin = new System.Windows.Forms.Padding(0);
            this.textBox_id.Name = "textBox_id";
            this.textBox_id.Size = new System.Drawing.Size(66, 20);
            this.textBox_id.TabIndex = 6;
            this.textBox_id.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_info_1
            // 
            this.label_info_1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_info_1.AutoSize = true;
            this.label_info_1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_info_1.Location = new System.Drawing.Point(3, 15);
            this.label_info_1.Name = "label_info_1";
            this.label_info_1.Size = new System.Drawing.Size(74, 19);
            this.label_info_1.TabIndex = 0;
            this.label_info_1.Text = "ID Antena:";
            // 
            // UserControlConnectionChoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel_base);
            this.Name = "UserControlConnectionChoice";
            this.Size = new System.Drawing.Size(404, 385);
            this.Load += new System.EventHandler(this.UserControlConnectionChoice_Load);
            this.tableLayoutPanel_base.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_base;
        private System.Windows.Forms.Button button_ethernet;
        private System.Windows.Forms.Button button_usb;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBox_id;
        private System.Windows.Forms.Label label_info_1;
    }
}
