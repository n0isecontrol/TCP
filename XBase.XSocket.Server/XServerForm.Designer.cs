namespace XBase.XSocket
{
    partial class XServerForm
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
            this.components = new System.ComponentModel.Container();
            this.textLog = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textControlID = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSendControl = new System.Windows.Forms.Button();
            this.textControlData = new System.Windows.Forms.TextBox();
            this.buttonFileBrowse = new System.Windows.Forms.Button();
            this.textSendFile = new System.Windows.Forms.TextBox();
            this.textSend = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSendData = new System.Windows.Forms.Button();
            this.buttonSendText = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textServerPort = new System.Windows.Forms.TextBox();
            this.buttonStartServer = new System.Windows.Forms.Button();
            this.textServerIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textClientList = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textDBName = new System.Windows.Forms.TextBox();
            this.textDBPassword = new System.Windows.Forms.TextBox();
            this.textDBServer = new System.Windows.Forms.TextBox();
            this.buttonDBConnect = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.textDBUser = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label12 = new System.Windows.Forms.Label();
            this.textClientIP = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.mUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.buttonClear = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // textLog
            // 
            this.textLog.Location = new System.Drawing.Point(424, 223);
            this.textLog.Name = "textLog";
            this.textLog.Size = new System.Drawing.Size(455, 283);
            this.textLog.TabIndex = 5;
            this.textLog.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(176, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Control Data :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Control ID :";
            // 
            // textControlID
            // 
            this.textControlID.Location = new System.Drawing.Point(71, 20);
            this.textControlID.Name = "textControlID";
            this.textControlID.Size = new System.Drawing.Size(99, 20);
            this.textControlID.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonSendControl);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.textControlData);
            this.groupBox3.Controls.Add(this.textControlID);
            this.groupBox3.Location = new System.Drawing.Point(424, 44);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(455, 51);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Control Code";
            // 
            // buttonSendControl
            // 
            this.buttonSendControl.Location = new System.Drawing.Point(364, 19);
            this.buttonSendControl.Name = "buttonSendControl";
            this.buttonSendControl.Size = new System.Drawing.Size(75, 23);
            this.buttonSendControl.TabIndex = 2;
            this.buttonSendControl.Text = "Send";
            this.buttonSendControl.UseVisualStyleBackColor = true;
            this.buttonSendControl.Click += new System.EventHandler(this.buttonSendControl_Click);
            // 
            // textControlData
            // 
            this.textControlData.Location = new System.Drawing.Point(254, 20);
            this.textControlData.Name = "textControlData";
            this.textControlData.Size = new System.Drawing.Size(88, 20);
            this.textControlData.TabIndex = 1;
            // 
            // buttonFileBrowse
            // 
            this.buttonFileBrowse.Location = new System.Drawing.Point(287, 53);
            this.buttonFileBrowse.Name = "buttonFileBrowse";
            this.buttonFileBrowse.Size = new System.Drawing.Size(55, 23);
            this.buttonFileBrowse.TabIndex = 4;
            this.buttonFileBrowse.Text = "Browse";
            this.buttonFileBrowse.UseVisualStyleBackColor = true;
            this.buttonFileBrowse.Click += new System.EventHandler(this.buttonFileBrowse_Click);
            // 
            // textSendFile
            // 
            this.textSendFile.Enabled = false;
            this.textSendFile.Location = new System.Drawing.Point(69, 53);
            this.textSendFile.Name = "textSendFile";
            this.textSendFile.Size = new System.Drawing.Size(211, 20);
            this.textSendFile.TabIndex = 3;
            // 
            // textSend
            // 
            this.textSend.Location = new System.Drawing.Point(71, 24);
            this.textSend.Name = "textSend";
            this.textSend.Size = new System.Drawing.Size(273, 20);
            this.textSend.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "File : ";
            // 
            // buttonSendData
            // 
            this.buttonSendData.Location = new System.Drawing.Point(364, 52);
            this.buttonSendData.Name = "buttonSendData";
            this.buttonSendData.Size = new System.Drawing.Size(75, 23);
            this.buttonSendData.TabIndex = 5;
            this.buttonSendData.Text = "Send";
            this.buttonSendData.UseVisualStyleBackColor = true;
            this.buttonSendData.Click += new System.EventHandler(this.buttonSendData_Click);
            // 
            // buttonSendText
            // 
            this.buttonSendText.Location = new System.Drawing.Point(364, 23);
            this.buttonSendText.Name = "buttonSendText";
            this.buttonSendText.Size = new System.Drawing.Size(75, 23);
            this.buttonSendText.TabIndex = 2;
            this.buttonSendText.Text = "Send";
            this.buttonSendText.UseVisualStyleBackColor = true;
            this.buttonSendText.Click += new System.EventHandler(this.buttonSendText_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonFileBrowse);
            this.groupBox2.Controls.Add(this.textSendFile);
            this.groupBox2.Controls.Add(this.textSend);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.buttonSendData);
            this.groupBox2.Controls.Add(this.buttonSendText);
            this.groupBox2.Location = new System.Drawing.Point(424, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(460, 86);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Send Data ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Text : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textServerPort);
            this.groupBox1.Controls.Add(this.buttonStartServer);
            this.groupBox1.Controls.Add(this.textServerIP);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(14, 125);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(351, 66);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server IP:";
            // 
            // textServerPort
            // 
            this.textServerPort.Location = new System.Drawing.Point(224, 22);
            this.textServerPort.Name = "textServerPort";
            this.textServerPort.Size = new System.Drawing.Size(51, 20);
            this.textServerPort.TabIndex = 1;
            this.textServerPort.Text = "55555";
            // 
            // buttonStartServer
            // 
            this.buttonStartServer.Location = new System.Drawing.Point(287, 21);
            this.buttonStartServer.Name = "buttonStartServer";
            this.buttonStartServer.Size = new System.Drawing.Size(54, 23);
            this.buttonStartServer.TabIndex = 2;
            this.buttonStartServer.Text = "Start";
            this.buttonStartServer.UseVisualStyleBackColor = true;
            this.buttonStartServer.Click += new System.EventHandler(this.buttonStartServer_Click);
            // 
            // textServerIP
            // 
            this.textServerIP.Location = new System.Drawing.Point(74, 22);
            this.textServerIP.Name = "textServerIP";
            this.textServerIP.Size = new System.Drawing.Size(90, 20);
            this.textServerIP.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port : ";
            // 
            // textClientList
            // 
            this.textClientList.Location = new System.Drawing.Point(20, 223);
            this.textClientList.Multiline = true;
            this.textClientList.Name = "textClientList";
            this.textClientList.Size = new System.Drawing.Size(345, 283);
            this.textClientList.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Client List:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.textDBName);
            this.groupBox4.Controls.Add(this.textDBPassword);
            this.groupBox4.Controls.Add(this.textDBServer);
            this.groupBox4.Controls.Add(this.buttonDBConnect);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.textDBUser);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new System.Drawing.Point(19, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(351, 90);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Database";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 28);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Host:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "User:";
            // 
            // textDBName
            // 
            this.textDBName.Location = new System.Drawing.Point(200, 24);
            this.textDBName.Name = "textDBName";
            this.textDBName.Size = new System.Drawing.Size(67, 20);
            this.textDBName.TabIndex = 1;
            this.textDBName.Text = "db_safetcp";
            // 
            // textDBPassword
            // 
            this.textDBPassword.Location = new System.Drawing.Point(202, 54);
            this.textDBPassword.Name = "textDBPassword";
            this.textDBPassword.Size = new System.Drawing.Size(65, 20);
            this.textDBPassword.TabIndex = 3;
            // 
            // textDBServer
            // 
            this.textDBServer.Location = new System.Drawing.Point(47, 24);
            this.textDBServer.Name = "textDBServer";
            this.textDBServer.Size = new System.Drawing.Size(79, 20);
            this.textDBServer.TabIndex = 0;
            this.textDBServer.Text = "localhost";
            // 
            // buttonDBConnect
            // 
            this.buttonDBConnect.Location = new System.Drawing.Point(273, 21);
            this.buttonDBConnect.Name = "buttonDBConnect";
            this.buttonDBConnect.Size = new System.Drawing.Size(68, 50);
            this.buttonDBConnect.TabIndex = 4;
            this.buttonDBConnect.Text = "Connect";
            this.buttonDBConnect.UseVisualStyleBackColor = true;
            this.buttonDBConnect.Click += new System.EventHandler(this.buttonDBConnect_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(139, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Database: ";
            // 
            // textDBUser
            // 
            this.textDBUser.Location = new System.Drawing.Point(47, 54);
            this.textDBUser.Name = "textDBUser";
            this.textDBUser.Size = new System.Drawing.Size(79, 20);
            this.textDBUser.TabIndex = 2;
            this.textDBUser.Text = "root";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(139, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Password: ";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(421, 198);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(28, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Log:";
            // 
            // textClientIP
            // 
            this.textClientIP.Location = new System.Drawing.Point(495, 16);
            this.textClientIP.Name = "textClientIP";
            this.textClientIP.Size = new System.Drawing.Size(384, 20);
            this.textClientIP.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(433, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Client IP:";
            // 
            // mUpdateTimer
            // 
            this.mUpdateTimer.Enabled = true;
            this.mUpdateTimer.Tick += new System.EventHandler(this.mUpdateTimer_Tick);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(788, 192);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 5;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // XServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 527);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textClientList);
            this.Controls.Add(this.textClientIP);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Name = "XServerForm";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.XServerForm_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.RichTextBox textLog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textControlID;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSendControl;
        private System.Windows.Forms.TextBox textControlData;
        private System.Windows.Forms.Button buttonFileBrowse;
        private System.Windows.Forms.TextBox textSendFile;
        private System.Windows.Forms.TextBox textSend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSendData;
        private System.Windows.Forms.Button buttonSendText;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textServerPort;
        private System.Windows.Forms.Button buttonStartServer;
        private System.Windows.Forms.TextBox textServerIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textClientList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textDBName;
        private System.Windows.Forms.TextBox textDBPassword;
        private System.Windows.Forms.TextBox textDBServer;
        private System.Windows.Forms.Button buttonDBConnect;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textDBUser;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textClientIP;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Timer mUpdateTimer;
        private System.Windows.Forms.Button buttonClear;

    }
}

