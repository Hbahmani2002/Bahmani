namespace DesktopPdfSigner
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.txtUsbDonglePassword = new System.Windows.Forms.TextBox();
            this.btnSign = new System.Windows.Forms.Button();
            this.bckWorker = new System.ComponentModel.BackgroundWorker();
            this.chBoxPassword = new System.Windows.Forms.CheckBox();
            this.fileUpload = new System.Windows.Forms.OpenFileDialog();
            this.bckWorkerXsig = new System.ComponentModel.BackgroundWorker();
            this.loadingCircle = new MRG.Controls.UI.LoadingCircle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.btnImzala = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.bw3 = new System.ComponentModel.BackgroundWorker();
            this.bwrungrid = new System.ComponentModel.BackgroundWorker();
            this.bwImzala = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSahib = new System.Windows.Forms.TextBox();
            this.txtTC = new System.Windows.Forms.TextBox();
            this.bwImzalaXsigsiz = new System.ComponentModel.BackgroundWorker();
            this.chkXsig = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 51);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Usb-Dongle Şifreniz :";
            // 
            // txtUsbDonglePassword
            // 
            this.txtUsbDonglePassword.Location = new System.Drawing.Point(110, 51);
            this.txtUsbDonglePassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsbDonglePassword.Name = "txtUsbDonglePassword";
            this.txtUsbDonglePassword.PasswordChar = '*';
            this.txtUsbDonglePassword.Size = new System.Drawing.Size(145, 20);
            this.txtUsbDonglePassword.TabIndex = 1;
            // 
            // btnSign
            // 
            this.btnSign.Location = new System.Drawing.Point(9, 75);
            this.btnSign.Margin = new System.Windows.Forms.Padding(2);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(262, 34);
            this.btnSign.TabIndex = 2;
            this.btnSign.Text = "İmzala ve Xsig\'e Çevir";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // bckWorker
            // 
            this.bckWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckWorker_DoWork_1);
            // 
            // chBoxPassword
            // 
            this.chBoxPassword.AutoSize = true;
            this.chBoxPassword.Location = new System.Drawing.Point(260, 54);
            this.chBoxPassword.Name = "chBoxPassword";
            this.chBoxPassword.Size = new System.Drawing.Size(15, 14);
            this.chBoxPassword.TabIndex = 3;
            this.chBoxPassword.UseVisualStyleBackColor = true;
            this.chBoxPassword.CheckedChanged += new System.EventHandler(this.chBoxPassword_CheckedChanged);
            // 
            // fileUpload
            // 
            this.fileUpload.FileName = "openFileDialog1";
            // 
            // bckWorkerXsig
            // 
            this.bckWorkerXsig.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckWorkerXsig_DoWork);
            this.bckWorkerXsig.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bckWorkerXsig_RunWorkerCompleted);
            // 
            // loadingCircle
            // 
            this.loadingCircle.Active = false;
            this.loadingCircle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.loadingCircle.Color = System.Drawing.Color.Transparent;
            this.loadingCircle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.loadingCircle.InnerCircleRadius = 8;
            this.loadingCircle.Location = new System.Drawing.Point(69, 6);
            this.loadingCircle.Name = "loadingCircle";
            this.loadingCircle.NumberSpoke = 24;
            this.loadingCircle.OuterCircleRadius = 9;
            this.loadingCircle.RotationSpeed = 80;
            this.loadingCircle.Size = new System.Drawing.Size(158, 135);
            this.loadingCircle.SpokeThickness = 4;
            this.loadingCircle.StylePreset = MRG.Controls.UI.LoadingCircle.StylePresets.IE7;
            this.loadingCircle.TabIndex = 22;
            this.loadingCircle.Text = "loadingCircle";
            this.loadingCircle.Click += new System.EventHandler(this.loadingCircle_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(9, 267);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(267, 54);
            this.dataGridView1.TabIndex = 23;
            this.dataGridView1.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(316, 286);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 19);
            this.button1.TabIndex = 24;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnImzala
            // 
            this.btnImzala.BackColor = System.Drawing.Color.Green;
            this.btnImzala.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnImzala.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnImzala.Location = new System.Drawing.Point(316, 225);
            this.btnImzala.Margin = new System.Windows.Forms.Padding(2);
            this.btnImzala.Name = "btnImzala";
            this.btnImzala.Size = new System.Drawing.Size(268, 41);
            this.btnImzala.TabIndex = 25;
            this.btnImzala.Text = "Imzala ve Xsig\'e Çevir";
            this.btnImzala.UseVisualStyleBackColor = false;
            this.btnImzala.Click += new System.EventHandler(this.btnImzala_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(14, 236);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(267, 17);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 26;
            this.progressBar1.Visible = false;
            // 
            // bw3
            // 
            this.bw3.WorkerReportsProgress = true;
            this.bw3.WorkerSupportsCancellation = true;
            this.bw3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw3_DoWork);
            this.bw3.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw3_ProgressChanged);
            this.bw3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw3_RunWorkerCompleted);
            // 
            // bwrungrid
            // 
            this.bwrungrid.WorkerReportsProgress = true;
            this.bwrungrid.WorkerSupportsCancellation = true;
            this.bwrungrid.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwrungrid_DoWork);
            this.bwrungrid.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwrungrid_ProgressChanged);
            this.bwrungrid.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwrungrid_RunWorkerCompleted);
            // 
            // bwImzala
            // 
            this.bwImzala.WorkerReportsProgress = true;
            this.bwImzala.WorkerSupportsCancellation = true;
            this.bwImzala.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwImzala_DoWork);
            this.bwImzala.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwImzala_ProgressChanged);
            this.bwImzala.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwImzala_RunWorkerCompleted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PayFlex.Smartbox.EDesktopPdfSigner.Properties.Resources.protek_logo_011;
            this.pictureBox1.Location = new System.Drawing.Point(117, 121);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(52, 20);
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Sertifika Sahibi :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "TC Kimlik NUmarasi :";
            // 
            // txtSahib
            // 
            this.txtSahib.Location = new System.Drawing.Point(110, 6);
            this.txtSahib.Margin = new System.Windows.Forms.Padding(2);
            this.txtSahib.Name = "txtSahib";
            this.txtSahib.Size = new System.Drawing.Size(164, 20);
            this.txtSahib.TabIndex = 30;
            // 
            // txtTC
            // 
            this.txtTC.Location = new System.Drawing.Point(110, 28);
            this.txtTC.Margin = new System.Windows.Forms.Padding(2);
            this.txtTC.Name = "txtTC";
            this.txtTC.Size = new System.Drawing.Size(164, 20);
            this.txtTC.TabIndex = 31;
            // 
            // bwImzalaXsigsiz
            // 
            this.bwImzalaXsigsiz.WorkerReportsProgress = true;
            this.bwImzalaXsigsiz.WorkerSupportsCancellation = true;
            this.bwImzalaXsigsiz.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwImzalaXsigsiz_DoWork);
            this.bwImzalaXsigsiz.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwImzalaXsigsiz_RunWorkerCompleted);
            // 
            // chkXsig
            // 
            this.chkXsig.AutoSize = true;
            this.chkXsig.Location = new System.Drawing.Point(10, 122);
            this.chkXsig.Name = "chkXsig";
            this.chkXsig.Size = new System.Drawing.Size(79, 17);
            this.chkXsig.TabIndex = 32;
            this.chkXsig.Text = "xsig olacak";
            this.chkXsig.UseVisualStyleBackColor = true;
            this.chkXsig.CheckedChanged += new System.EventHandler(this.chkXsig_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(280, 151);
            this.Controls.Add(this.chkXsig);
            this.Controls.Add(this.txtTC);
            this.Controls.Add(this.txtSahib);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnImzala);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.loadingCircle);
            this.Controls.Add(this.chBoxPassword);
            this.Controls.Add(this.btnSign);
            this.Controls.Add(this.txtUsbDonglePassword);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDF İmzala ve Xsig";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUsbDonglePassword;
        private System.Windows.Forms.Button btnSign;
        private System.ComponentModel.BackgroundWorker bckWorker;
        private System.Windows.Forms.CheckBox chBoxPassword;
        private System.Windows.Forms.OpenFileDialog fileUpload;
        private System.ComponentModel.BackgroundWorker bckWorkerXsig;
        private MRG.Controls.UI.LoadingCircle loadingCircle;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnImzala;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker bw3;
        private System.ComponentModel.BackgroundWorker bwrungrid;
        private System.ComponentModel.BackgroundWorker bwImzala;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSahib;
        private System.Windows.Forms.TextBox txtTC;
        private System.ComponentModel.BackgroundWorker bwImzalaXsigsiz;
        private System.Windows.Forms.CheckBox chkXsig;
    }
}

