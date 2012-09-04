namespace SerialPortCommunicator.Modbus
{
    partial class Main
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
            this.cmdClose = new System.Windows.Forms.Button();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.txtFunction = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdSend = new System.Windows.Forms.Button();
            this.txtMessageText = new System.Windows.Forms.TextBox();
            this.rtbDisplay = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.cboBaud = new System.Windows.Forms.ComboBox();
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.cmdOpen = new System.Windows.Forms.Button();
            this.pingButton = new System.Windows.Forms.Button();
            this.pingLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboHandshake = new System.Windows.Forms.ComboBox();
            this.pingTimeoutValue = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rtuRadioButton = new System.Windows.Forms.RadioButton();
            this.asciiRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.n2RadioButton1 = new System.Windows.Forms.RadioButton();
            this.o1RadioButton = new System.Windows.Forms.RadioButton();
            this.e1RadioButton = new System.Windows.Forms.RadioButton();
            this.GroupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pingTimeoutValue)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(672, 363);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(90, 23);
            this.cmdClose.TabIndex = 5;
            this.cmdClose.Text = "Close Port";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.txtFunction);
            this.GroupBox1.Controls.Add(this.label4);
            this.GroupBox1.Controls.Add(this.txtAddress);
            this.GroupBox1.Controls.Add(this.label3);
            this.GroupBox1.Controls.Add(this.cmdSend);
            this.GroupBox1.Controls.Add(this.txtMessageText);
            this.GroupBox1.Controls.Add(this.rtbDisplay);
            this.GroupBox1.Location = new System.Drawing.Point(12, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(556, 343);
            this.GroupBox1.TabIndex = 4;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Serial Port Communication";
            // 
            // txtFunction
            // 
            this.txtFunction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFunction.Location = new System.Drawing.Point(63, 317);
            this.txtFunction.Name = "txtFunction";
            this.txtFunction.Size = new System.Drawing.Size(51, 20);
            this.txtFunction.TabIndex = 9;
            this.txtFunction.Text = "06";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 301);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Funkcja:";
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAddress.Location = new System.Drawing.Point(6, 317);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(51, 20);
            this.txtAddress.TabIndex = 7;
            this.txtAddress.Text = "11";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 301);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Adres:";
            // 
            // cmdSend
            // 
            this.cmdSend.Location = new System.Drawing.Point(466, 314);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(75, 23);
            this.cmdSend.TabIndex = 5;
            this.cmdSend.Text = "Wyœlij";
            this.cmdSend.UseVisualStyleBackColor = true;
            this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
            // 
            // txtMessageText
            // 
            this.txtMessageText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessageText.Location = new System.Drawing.Point(120, 317);
            this.txtMessageText.Name = "txtMessageText";
            this.txtMessageText.Size = new System.Drawing.Size(340, 20);
            this.txtMessageText.TabIndex = 4;
            this.txtMessageText.Text = "00010003";
            // 
            // rtbDisplay
            // 
            this.rtbDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbDisplay.Location = new System.Drawing.Point(7, 19);
            this.rtbDisplay.Name = "rtbDisplay";
            this.rtbDisplay.ReadOnly = true;
            this.rtbDisplay.Size = new System.Drawing.Size(543, 279);
            this.rtbDisplay.TabIndex = 3;
            this.rtbDisplay.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.Label1);
            this.groupBox2.Controls.Add(this.cboBaud);
            this.groupBox2.Controls.Add(this.cboPort);
            this.groupBox2.Location = new System.Drawing.Point(574, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(188, 107);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parametry po³¹czenia";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Prêdkoœæ";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(6, 18);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(26, 13);
            this.Label1.TabIndex = 15;
            this.Label1.Text = "Port";
            // 
            // cboBaud
            // 
            this.cboBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBaud.FormattingEnabled = true;
            this.cboBaud.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "28800",
            "36000",
            "115000"});
            this.cboBaud.Location = new System.Drawing.Point(9, 74);
            this.cboBaud.Name = "cboBaud";
            this.cboBaud.Size = new System.Drawing.Size(173, 21);
            this.cboBaud.TabIndex = 11;
            // 
            // cboPort
            // 
            this.cboPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new System.Drawing.Point(9, 34);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(173, 21);
            this.cboPort.TabIndex = 10;
            // 
            // cmdOpen
            // 
            this.cmdOpen.Location = new System.Drawing.Point(574, 363);
            this.cmdOpen.Name = "cmdOpen";
            this.cmdOpen.Size = new System.Drawing.Size(92, 23);
            this.cmdOpen.TabIndex = 8;
            this.cmdOpen.Text = "Open Port";
            this.cmdOpen.UseVisualStyleBackColor = true;
            this.cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
            // 
            // pingButton
            // 
            this.pingButton.Location = new System.Drawing.Point(154, 361);
            this.pingButton.Name = "pingButton";
            this.pingButton.Size = new System.Drawing.Size(100, 23);
            this.pingButton.TabIndex = 9;
            this.pingButton.Text = "Ping";
            this.pingButton.UseVisualStyleBackColor = true;
            this.pingButton.Click += new System.EventHandler(this.pingButton_Click);
            // 
            // pingLabel
            // 
            this.pingLabel.AutoSize = true;
            this.pingLabel.Location = new System.Drawing.Point(9, 368);
            this.pingLabel.Name = "pingLabel";
            this.pingLabel.Size = new System.Drawing.Size(70, 13);
            this.pingLabel.TabIndex = 10;
            this.pingLabel.Text = "Timeout [ms]:";
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.cboHandshake);
            this.groupBox3.Location = new System.Drawing.Point(574, 285);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(188, 64);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Parametry transmisji";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "XON/XOFF";
            // 
            // cboHandshake
            // 
            this.cboHandshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHandshake.FormattingEnabled = true;
            this.cboHandshake.Location = new System.Drawing.Point(6, 36);
            this.cboHandshake.Name = "cboHandshake";
            this.cboHandshake.Size = new System.Drawing.Size(175, 21);
            this.cboHandshake.TabIndex = 3;
            // 
            // pingTimeoutValue
            // 
            this.pingTimeoutValue.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.pingTimeoutValue.Location = new System.Drawing.Point(85, 364);
            this.pingTimeoutValue.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.pingTimeoutValue.Name = "pingTimeoutValue";
            this.pingTimeoutValue.Size = new System.Drawing.Size(63, 20);
            this.pingTimeoutValue.TabIndex = 13;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.rtuRadioButton);
            this.groupBox4.Controls.Add(this.asciiRadioButton);
            this.groupBox4.Location = new System.Drawing.Point(574, 125);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(188, 48);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Format znaków";
            // 
            // rtuRadioButton
            // 
            this.rtuRadioButton.AutoSize = true;
            this.rtuRadioButton.Location = new System.Drawing.Point(68, 19);
            this.rtuRadioButton.Name = "rtuRadioButton";
            this.rtuRadioButton.Size = new System.Drawing.Size(48, 17);
            this.rtuRadioButton.TabIndex = 1;
            this.rtuRadioButton.Text = "RTU";
            this.rtuRadioButton.UseVisualStyleBackColor = true;
            // 
            // asciiRadioButton
            // 
            this.asciiRadioButton.AutoSize = true;
            this.asciiRadioButton.Checked = true;
            this.asciiRadioButton.Location = new System.Drawing.Point(10, 19);
            this.asciiRadioButton.Name = "asciiRadioButton";
            this.asciiRadioButton.Size = new System.Drawing.Size(52, 17);
            this.asciiRadioButton.TabIndex = 0;
            this.asciiRadioButton.TabStop = true;
            this.asciiRadioButton.Text = "ASCII";
            this.asciiRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.n2RadioButton1);
            this.groupBox5.Controls.Add(this.o1RadioButton);
            this.groupBox5.Controls.Add(this.e1RadioButton);
            this.groupBox5.Location = new System.Drawing.Point(574, 179);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(188, 48);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Parzystoœæ i bity stopu";
            // 
            // n2RadioButton1
            // 
            this.n2RadioButton1.AutoSize = true;
            this.n2RadioButton1.Location = new System.Drawing.Point(97, 19);
            this.n2RadioButton1.Name = "n2RadioButton1";
            this.n2RadioButton1.Size = new System.Drawing.Size(39, 17);
            this.n2RadioButton1.TabIndex = 2;
            this.n2RadioButton1.Text = "N2";
            this.n2RadioButton1.UseVisualStyleBackColor = true;
            // 
            // o1RadioButton
            // 
            this.o1RadioButton.AutoSize = true;
            this.o1RadioButton.Location = new System.Drawing.Point(54, 19);
            this.o1RadioButton.Name = "o1RadioButton";
            this.o1RadioButton.Size = new System.Drawing.Size(39, 17);
            this.o1RadioButton.TabIndex = 1;
            this.o1RadioButton.Text = "O1";
            this.o1RadioButton.UseVisualStyleBackColor = true;
            // 
            // e1RadioButton
            // 
            this.e1RadioButton.AutoSize = true;
            this.e1RadioButton.Checked = true;
            this.e1RadioButton.Location = new System.Drawing.Point(10, 19);
            this.e1RadioButton.Name = "e1RadioButton";
            this.e1RadioButton.Size = new System.Drawing.Size(38, 17);
            this.e1RadioButton.TabIndex = 0;
            this.e1RadioButton.TabStop = true;
            this.e1RadioButton.Text = "E1";
            this.e1RadioButton.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 396);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.pingTimeoutValue);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pingLabel);
            this.Controls.Add(this.pingButton);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmdOpen);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serial Port Communication";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pingTimeoutValue)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.TextBox txtMessageText;
        private System.Windows.Forms.RichTextBox rtbDisplay;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ComboBox cboBaud;
        private System.Windows.Forms.ComboBox cboPort;
        private System.Windows.Forms.Button cmdOpen;
        private System.Windows.Forms.Button pingButton;
        private System.Windows.Forms.Label pingLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboHandshake;
        private System.Windows.Forms.NumericUpDown pingTimeoutValue;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rtuRadioButton;
        private System.Windows.Forms.RadioButton asciiRadioButton;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton n2RadioButton1;
        private System.Windows.Forms.RadioButton o1RadioButton;
        private System.Windows.Forms.RadioButton e1RadioButton;
        private System.Windows.Forms.TextBox txtFunction;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label3;
    }
}