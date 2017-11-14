namespace WindowsFormsApplication1
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.data_tx = new System.Windows.Forms.TextBox();
            this.Start_bt = new System.Windows.Forms.Button();
            this.stop_bt = new System.Windows.Forms.Button();
            this.save_bt = new System.Windows.Forms.Button();
            this.comboBoxDKH = new System.Windows.Forms.ComboBox();
            this.svmdata_text = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.PortName = "COM8";
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // data_tx
            // 
            this.data_tx.Location = new System.Drawing.Point(21, 29);
            this.data_tx.Multiline = true;
            this.data_tx.Name = "data_tx";
            this.data_tx.Size = new System.Drawing.Size(1131, 278);
            this.data_tx.TabIndex = 0;
            // 
            // Start_bt
            // 
            this.Start_bt.Location = new System.Drawing.Point(1214, 223);
            this.Start_bt.Name = "Start_bt";
            this.Start_bt.Size = new System.Drawing.Size(154, 43);
            this.Start_bt.TabIndex = 1;
            this.Start_bt.Text = "start";
            this.Start_bt.UseVisualStyleBackColor = true;
            this.Start_bt.Click += new System.EventHandler(this.Start_bt_Click);
            // 
            // stop_bt
            // 
            this.stop_bt.Location = new System.Drawing.Point(1214, 294);
            this.stop_bt.Name = "stop_bt";
            this.stop_bt.Size = new System.Drawing.Size(154, 43);
            this.stop_bt.TabIndex = 2;
            this.stop_bt.Text = "stop";
            this.stop_bt.UseVisualStyleBackColor = true;
            this.stop_bt.Click += new System.EventHandler(this.stop_bt_Click);
            // 
            // save_bt
            // 
            this.save_bt.Location = new System.Drawing.Point(1214, 357);
            this.save_bt.Name = "save_bt";
            this.save_bt.Size = new System.Drawing.Size(154, 43);
            this.save_bt.TabIndex = 3;
            this.save_bt.Text = "save";
            this.save_bt.UseVisualStyleBackColor = true;
            this.save_bt.Click += new System.EventHandler(this.save_bt_Click);
            // 
            // comboBoxDKH
            // 
            this.comboBoxDKH.FormattingEnabled = true;
            this.comboBoxDKH.Location = new System.Drawing.Point(1196, 48);
            this.comboBoxDKH.Name = "comboBoxDKH";
            this.comboBoxDKH.Size = new System.Drawing.Size(186, 23);
            this.comboBoxDKH.TabIndex = 6;
            this.comboBoxDKH.SelectedIndexChanged += new System.EventHandler(this.comboBoxDKH_SelectedIndexChanged);
            // 
            // svmdata_text
            // 
            this.svmdata_text.Location = new System.Drawing.Point(21, 339);
            this.svmdata_text.Multiline = true;
            this.svmdata_text.Name = "svmdata_text";
            this.svmdata_text.Size = new System.Drawing.Size(1131, 132);
            this.svmdata_text.TabIndex = 7;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(1158, 113);
            this.progressBar1.Maximum = 20000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(278, 25);
            this.progressBar1.TabIndex = 8;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(1158, 161);
            this.progressBar2.Maximum = 20000;
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(278, 25);
            this.progressBar2.TabIndex = 9;
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(1158, 141);
            this.progressBar3.Maximum = 20000;
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(278, 14);
            this.progressBar3.TabIndex = 10;
            this.progressBar3.Value = 5000;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1448, 483);
            this.Controls.Add(this.progressBar3);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.svmdata_text);
            this.Controls.Add(this.comboBoxDKH);
            this.Controls.Add(this.save_bt);
            this.Controls.Add(this.stop_bt);
            this.Controls.Add(this.Start_bt);
            this.Controls.Add(this.data_tx);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button Start_bt;
        private System.Windows.Forms.Button stop_bt;
        private System.Windows.Forms.Button save_bt;
        private System.Windows.Forms.TextBox data_tx;
        private System.Windows.Forms.ComboBox comboBoxDKH;
        private System.Windows.Forms.TextBox svmdata_text;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
    }
}

