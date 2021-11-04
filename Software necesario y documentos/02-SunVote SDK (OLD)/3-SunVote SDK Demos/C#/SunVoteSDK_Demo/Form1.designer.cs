namespace SunVoteSDK_Demo
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lstMsg = new System.Windows.Forms.ListBox();
            this.btnOpenConn = new System.Windows.Forms.Button();
            this.btnCloseConn = new System.Windows.Forms.Button();
            this.btnStartSignIn = new System.Windows.Forms.Button();
            this.btnStopSignIn = new System.Windows.Forms.Button();
            this.button_vota = new System.Windows.Forms.Button();
            this.button_stopvota = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstMsg
            // 
            this.lstMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstMsg.FormattingEnabled = true;
            this.lstMsg.Location = new System.Drawing.Point(0, 115);
            this.lstMsg.Name = "lstMsg";
            this.lstMsg.Size = new System.Drawing.Size(624, 342);
            this.lstMsg.TabIndex = 0;
            // 
            // btnOpenConn
            // 
            this.btnOpenConn.Location = new System.Drawing.Point(222, 23);
            this.btnOpenConn.Name = "btnOpenConn";
            this.btnOpenConn.Size = new System.Drawing.Size(135, 25);
            this.btnOpenConn.TabIndex = 3;
            this.btnOpenConn.Text = "Connect base station";
            this.btnOpenConn.UseVisualStyleBackColor = true;
            this.btnOpenConn.Click += new System.EventHandler(this.btnOpenConn_Click);
            // 
            // btnCloseConn
            // 
            this.btnCloseConn.Location = new System.Drawing.Point(373, 23);
            this.btnCloseConn.Name = "btnCloseConn";
            this.btnCloseConn.Size = new System.Drawing.Size(135, 25);
            this.btnCloseConn.TabIndex = 4;
            this.btnCloseConn.Text = "Close connection";
            this.btnCloseConn.UseVisualStyleBackColor = true;
            this.btnCloseConn.Click += new System.EventHandler(this.btnCloseConn_Click);
            // 
            // btnStartSignIn
            // 
            this.btnStartSignIn.Location = new System.Drawing.Point(222, 65);
            this.btnStartSignIn.Name = "btnStartSignIn";
            this.btnStartSignIn.Size = new System.Drawing.Size(135, 25);
            this.btnStartSignIn.TabIndex = 5;
            this.btnStartSignIn.Text = "Start to sign in";
            this.btnStartSignIn.UseVisualStyleBackColor = true;
            this.btnStartSignIn.Click += new System.EventHandler(this.btnStartSignIn_Click);
            // 
            // btnStopSignIn
            // 
            this.btnStopSignIn.Location = new System.Drawing.Point(373, 65);
            this.btnStopSignIn.Name = "btnStopSignIn";
            this.btnStopSignIn.Size = new System.Drawing.Size(135, 25);
            this.btnStopSignIn.TabIndex = 6;
            this.btnStopSignIn.Text = "Stop signing in";
            this.btnStopSignIn.UseVisualStyleBackColor = true;
            this.btnStopSignIn.Click += new System.EventHandler(this.btnStopSignIn_Click);
            // 
            // button_vota
            // 
            this.button_vota.Location = new System.Drawing.Point(528, 23);
            this.button_vota.Name = "button_vota";
            this.button_vota.Size = new System.Drawing.Size(62, 25);
            this.button_vota.TabIndex = 7;
            this.button_vota.Text = "Vota";
            this.button_vota.UseVisualStyleBackColor = true;
            this.button_vota.Click += new System.EventHandler(this.button_vota_Click);
            // 
            // button_stopvota
            // 
            this.button_stopvota.Location = new System.Drawing.Point(528, 65);
            this.button_stopvota.Name = "button_stopvota";
            this.button_stopvota.Size = new System.Drawing.Size(62, 25);
            this.button_stopvota.TabIndex = 8;
            this.button_stopvota.Text = "Stop vota";
            this.button_stopvota.UseVisualStyleBackColor = true;
            this.button_stopvota.Click += new System.EventHandler(this.button_stopvota_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 457);
            this.Controls.Add(this.button_stopvota);
            this.Controls.Add(this.button_vota);
            this.Controls.Add(this.btnStopSignIn);
            this.Controls.Add(this.btnStartSignIn);
            this.Controls.Add(this.btnCloseConn);
            this.Controls.Add(this.btnOpenConn);
            this.Controls.Add(this.lstMsg);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SunVoteSDK_Demo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstMsg;
        private System.Windows.Forms.Button btnOpenConn;
        private System.Windows.Forms.Button btnCloseConn;
        private System.Windows.Forms.Button btnStartSignIn;
        private System.Windows.Forms.Button btnStopSignIn;
        private System.Windows.Forms.Button button_vota;
        private System.Windows.Forms.Button button_stopvota;
    }
}



