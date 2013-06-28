namespace WinAuth
{
	partial class AddGoogleAuthenticator
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
			this.newAuthenticatorGroup = new System.Windows.Forms.GroupBox();
			this.showQrCodeButton = new System.Windows.Forms.Button();
			this.qrImage = new System.Windows.Forms.PictureBox();
			this.secretCodeField = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.summaryInfoLabel = new System.Windows.Forms.Label();
			this.newAuthenticatorGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.qrImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// newAuthenticatorGroup
			// 
			this.newAuthenticatorGroup.Controls.Add(this.summaryInfoLabel);
			this.newAuthenticatorGroup.Controls.Add(this.showQrCodeButton);
			this.newAuthenticatorGroup.Controls.Add(this.qrImage);
			this.newAuthenticatorGroup.Controls.Add(this.secretCodeField);
			this.newAuthenticatorGroup.Controls.Add(this.label4);
			this.newAuthenticatorGroup.Controls.Add(this.label2);
			this.newAuthenticatorGroup.Controls.Add(this.pictureBox1);
			this.newAuthenticatorGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.newAuthenticatorGroup.Location = new System.Drawing.Point(12, 12);
			this.newAuthenticatorGroup.Name = "newAuthenticatorGroup";
			this.newAuthenticatorGroup.Size = new System.Drawing.Size(458, 253);
			this.newAuthenticatorGroup.TabIndex = 1;
			this.newAuthenticatorGroup.TabStop = false;
			// 
			// showQrCodeButton
			// 
			this.showQrCodeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.showQrCodeButton.Location = new System.Drawing.Point(275, 117);
			this.showQrCodeButton.Name = "showQrCodeButton";
			this.showQrCodeButton.Size = new System.Drawing.Size(107, 23);
			this.showQrCodeButton.TabIndex = 7;
			this.showQrCodeButton.Text = "Show QR Code";
			this.showQrCodeButton.UseVisualStyleBackColor = true;
			this.showQrCodeButton.Click += new System.EventHandler(this.showQrCodeButton_Click);
			// 
			// qrImage
			// 
			this.qrImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.qrImage.Location = new System.Drawing.Point(149, 117);
			this.qrImage.Name = "qrImage";
			this.qrImage.Size = new System.Drawing.Size(120, 120);
			this.qrImage.TabIndex = 6;
			this.qrImage.TabStop = false;
			// 
			// secretCodeField
			// 
			this.secretCodeField.AllowDrop = true;
			this.secretCodeField.CausesValidation = false;
			this.secretCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.secretCodeField.Location = new System.Drawing.Point(149, 89);
			this.secretCodeField.Multiline = true;
			this.secretCodeField.Name = "secretCodeField";
			this.secretCodeField.Size = new System.Drawing.Size(291, 22);
			this.secretCodeField.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(60, 92);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(83, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "Secret Code";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(60, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(392, 62);
			this.label2.TabIndex = 1;
			this.label2.Text = "Enter the code for your authenticator. If you are shown a QR code, you can paste " +
    "/ drag the URL, image or image file into the Secret Code field below.\r\n";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::WinAuth.Properties.Resources.GoogleAuthenticatorIcon;
			this.pictureBox1.Location = new System.Drawing.Point(6, 25);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.okButton.Location = new System.Drawing.Point(315, 277);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 6;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(396, 277);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// summaryInfoLabel
			// 
			this.summaryInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.summaryInfoLabel.Location = new System.Drawing.Point(275, 152);
			this.summaryInfoLabel.Name = "summaryInfoLabel";
			this.summaryInfoLabel.Size = new System.Drawing.Size(165, 85);
			this.summaryInfoLabel.TabIndex = 8;
			// 
			// AddGoogleAuthenticator
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(483, 312);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.newAuthenticatorGroup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "AddGoogleAuthenticator";
			this.ShowIcon = false;
			this.Text = "Add Google Authenticator";
			this.Load += new System.EventHandler(this.AddGoogleAuthenticator_Load);
			this.newAuthenticatorGroup.ResumeLayout(false);
			this.newAuthenticatorGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.qrImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox newAuthenticatorGroup;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.PictureBox qrImage;
		private System.Windows.Forms.TextBox secretCodeField;
		private System.Windows.Forms.Button showQrCodeButton;
		private System.Windows.Forms.Label summaryInfoLabel;
	}
}