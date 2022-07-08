namespace Client
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IpField = new System.Windows.Forms.TextBox();
            this.PortField = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ConnectionInfoLabel = new System.Windows.Forms.Label();
            this.VideoBox = new System.Windows.Forms.PictureBox();
            this.CameraSelection = new System.Windows.Forms.ComboBox();
            this.MyVideoBox = new System.Windows.Forms.PictureBox();
            this.MicSelection = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.VideoBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MyVideoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // IpField
            // 
            this.IpField.Location = new System.Drawing.Point(296, 192);
            this.IpField.Name = "IpField";
            this.IpField.PlaceholderText = "Ip";
            this.IpField.Size = new System.Drawing.Size(100, 23);
            this.IpField.TabIndex = 0;
            // 
            // PortField
            // 
            this.PortField.Location = new System.Drawing.Point(402, 192);
            this.PortField.Name = "PortField";
            this.PortField.PlaceholderText = "Port";
            this.PortField.Size = new System.Drawing.Size(100, 23);
            this.PortField.TabIndex = 1;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(358, 221);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 2;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // ConnectionInfoLabel
            // 
            this.ConnectionInfoLabel.AutoSize = true;
            this.ConnectionInfoLabel.Location = new System.Drawing.Point(396, 258);
            this.ConnectionInfoLabel.Name = "ConnectionInfoLabel";
            this.ConnectionInfoLabel.Size = new System.Drawing.Size(0, 15);
            this.ConnectionInfoLabel.TabIndex = 3;
            this.ConnectionInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VideoBox
            // 
            this.VideoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VideoBox.Location = new System.Drawing.Point(0, 0);
            this.VideoBox.Name = "VideoBox";
            this.VideoBox.Size = new System.Drawing.Size(800, 450);
            this.VideoBox.TabIndex = 4;
            this.VideoBox.TabStop = false;
            this.VideoBox.Visible = false;
            // 
            // CameraSelection
            // 
            this.CameraSelection.FormattingEnabled = true;
            this.CameraSelection.Location = new System.Drawing.Point(343, 152);
            this.CameraSelection.Name = "CameraSelection";
            this.CameraSelection.Size = new System.Drawing.Size(121, 23);
            this.CameraSelection.TabIndex = 5;
            // 
            // MyVideoBox
            // 
            this.MyVideoBox.Location = new System.Drawing.Point(623, 12);
            this.MyVideoBox.Name = "MyVideoBox";
            this.MyVideoBox.Size = new System.Drawing.Size(165, 120);
            this.MyVideoBox.TabIndex = 6;
            this.MyVideoBox.TabStop = false;
            // 
            // MicSelection
            // 
            this.MicSelection.FormattingEnabled = true;
            this.MicSelection.Location = new System.Drawing.Point(343, 109);
            this.MicSelection.Name = "MicSelection";
            this.MicSelection.Size = new System.Drawing.Size(121, 23);
            this.MicSelection.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MyVideoBox);
            this.Controls.Add(this.MicSelection);
            this.Controls.Add(this.CameraSelection);
            this.Controls.Add(this.ConnectionInfoLabel);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.PortField);
            this.Controls.Add(this.IpField);
            this.Controls.Add(this.VideoBox);
            this.Name = "Form1";
            this.Text = "VideoChatClient";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.VideoBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MyVideoBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox IpField;
        private TextBox PortField;
        private Button ConnectButton;
        private Label ConnectionInfoLabel;
        private PictureBox VideoBox;
        private ComboBox CameraSelection;
        private PictureBox MyVideoBox;
        private ComboBox MicSelection;
    }
}