
namespace Votaciones_App.Formularios
{
    partial class EthernetOptions
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
            this.panel_base = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox_gateway = new System.Windows.Forms.TextBox();
            this.textBox_mac = new System.Windows.Forms.TextBox();
            this.textBox_mask = new System.Windows.Forms.TextBox();
            this.label_gateway = new System.Windows.Forms.Label();
            this.label_mask = new System.Windows.Forms.Label();
            this.label_mac = new System.Windows.Forms.Label();
            this.label_ip = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.panel_botton = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button_cancelar = new System.Windows.Forms.Button();
            this.button_aceptar = new System.Windows.Forms.Button();
            this.panel_base.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel_botton.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_base
            // 
            this.panel_base.Controls.Add(this.tableLayoutPanel1);
            this.panel_base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_base.Location = new System.Drawing.Point(0, 0);
            this.panel_base.Name = "panel_base";
            this.panel_base.Size = new System.Drawing.Size(455, 258);
            this.panel_base.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel1.Controls.Add(this.textBox_gateway, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBox_mac, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox_mask, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_gateway, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label_mask, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_mac, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_ip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox_ip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel_botton, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(455, 258);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textBox_gateway
            // 
            this.textBox_gateway.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_gateway.Location = new System.Drawing.Point(154, 168);
            this.textBox_gateway.Name = "textBox_gateway";
            this.textBox_gateway.Size = new System.Drawing.Size(298, 20);
            this.textBox_gateway.TabIndex = 11;
            // 
            // textBox_mac
            // 
            this.textBox_mac.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_mac.Location = new System.Drawing.Point(154, 66);
            this.textBox_mac.Name = "textBox_mac";
            this.textBox_mac.Size = new System.Drawing.Size(298, 20);
            this.textBox_mac.TabIndex = 10;
            // 
            // textBox_mask
            // 
            this.textBox_mask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_mask.Location = new System.Drawing.Point(154, 117);
            this.textBox_mask.Name = "textBox_mask";
            this.textBox_mask.Size = new System.Drawing.Size(298, 20);
            this.textBox_mask.TabIndex = 9;
            // 
            // label_gateway
            // 
            this.label_gateway.AutoSize = true;
            this.label_gateway.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_gateway.Location = new System.Drawing.Point(3, 153);
            this.label_gateway.Name = "label_gateway";
            this.label_gateway.Size = new System.Drawing.Size(145, 51);
            this.label_gateway.TabIndex = 6;
            this.label_gateway.Text = "Puerta de enlace:";
            this.label_gateway.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_mask
            // 
            this.label_mask.AutoSize = true;
            this.label_mask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_mask.Location = new System.Drawing.Point(3, 102);
            this.label_mask.Name = "label_mask";
            this.label_mask.Size = new System.Drawing.Size(145, 51);
            this.label_mask.TabIndex = 4;
            this.label_mask.Text = "Máscara de subred:";
            this.label_mask.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_mac
            // 
            this.label_mac.AutoSize = true;
            this.label_mac.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_mac.Location = new System.Drawing.Point(3, 51);
            this.label_mac.Name = "label_mac";
            this.label_mac.Size = new System.Drawing.Size(145, 51);
            this.label_mac.TabIndex = 2;
            this.label_mac.Text = "Dirección MAC:";
            this.label_mac.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_ip
            // 
            this.label_ip.AutoSize = true;
            this.label_ip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_ip.Location = new System.Drawing.Point(3, 0);
            this.label_ip.Name = "label_ip";
            this.label_ip.Size = new System.Drawing.Size(145, 51);
            this.label_ip.TabIndex = 0;
            this.label_ip.Text = "IP de la antena base:";
            this.label_ip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_ip
            // 
            this.textBox_ip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ip.Location = new System.Drawing.Point(154, 15);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(298, 20);
            this.textBox_ip.TabIndex = 7;
            // 
            // panel_botton
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel_botton, 2);
            this.panel_botton.Controls.Add(this.tableLayoutPanel2);
            this.panel_botton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_botton.Location = new System.Drawing.Point(3, 207);
            this.panel_botton.Name = "panel_botton";
            this.panel_botton.Size = new System.Drawing.Size(449, 48);
            this.panel_botton.TabIndex = 12;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.button_cancelar, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.button_aceptar, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(449, 48);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // button_cancelar
            // 
            this.button_cancelar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_cancelar.Location = new System.Drawing.Point(254, 3);
            this.button_cancelar.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
            this.button_cancelar.Name = "button_cancelar";
            this.button_cancelar.Size = new System.Drawing.Size(165, 42);
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
            this.button_aceptar.Size = new System.Drawing.Size(164, 42);
            this.button_aceptar.TabIndex = 0;
            this.button_aceptar.Text = "Aceptar";
            this.button_aceptar.UseVisualStyleBackColor = true;
            this.button_aceptar.Click += new System.EventHandler(this.button_aceptar_Click);
            // 
            // EthernetOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 258);
            this.Controls.Add(this.panel_base);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EthernetOptions";
            this.Text = "EthernetOptions";
            this.Load += new System.EventHandler(this.EthernetOptions_Load);
            this.panel_base.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel_botton.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_base;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_gateway;
        private System.Windows.Forms.Label label_mask;
        private System.Windows.Forms.Label label_mac;
        private System.Windows.Forms.Label label_ip;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_gateway;
        private System.Windows.Forms.TextBox textBox_mac;
        private System.Windows.Forms.TextBox textBox_mask;
        private System.Windows.Forms.Panel panel_botton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button_cancelar;
        private System.Windows.Forms.Button button_aceptar;
    }
}