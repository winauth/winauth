namespace WinAuth
{
	partial class UpdateCheckForm
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
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.versionInfoLabel = new MetroFramework.Drawing.Html.HtmlLabel();
			this.autoCheckbox = new MetroFramework.Controls.MetroCheckBox();
			this.autoDropdown = new MetroFramework.Controls.MetroComboBox();
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.autoLabel = new MetroFramework.Controls.MetroLabel();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(206, 249);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseSelectable = true;
			// 
			// versionInfoLabel
			// 
			this.versionInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.versionInfoLabel.AutoScroll = true;
			this.versionInfoLabel.AutoScrollMinSize = new System.Drawing.Size(141, 23);
			this.versionInfoLabel.AutoSize = false;
			this.versionInfoLabel.BackColor = System.Drawing.SystemColors.Window;
			this.versionInfoLabel.Location = new System.Drawing.Point(23, 63);
			this.versionInfoLabel.Name = "versionInfoLabel";
			this.versionInfoLabel.Size = new System.Drawing.Size(258, 67);
			this.versionInfoLabel.TabIndex = 3;
			this.versionInfoLabel.Text = "Latest Version: checking...";
			// 
			// autoCheckbox
			// 
			this.autoCheckbox.Location = new System.Drawing.Point(27, 136);
			this.autoCheckbox.Name = "autoCheckbox";
			this.autoCheckbox.Size = new System.Drawing.Size(254, 26);
			this.autoCheckbox.TabIndex = 4;
			this.autoCheckbox.UseSelectable = true;
			this.autoCheckbox.CheckedChanged += new System.EventHandler(this.autoCheckbox_CheckedChanged);
			// 
			// autoDropdown
			// 
			this.autoDropdown.FormattingEnabled = true;
			this.autoDropdown.ItemHeight = 23;
			this.autoDropdown.Items.AddRange(new object[] {
            "day",
            "2 days",
            "week",
            "2 weeks",
            "month"});
			this.autoDropdown.Location = new System.Drawing.Point(46, 188);
			this.autoDropdown.Name = "autoDropdown";
			this.autoDropdown.Size = new System.Drawing.Size(117, 29);
			this.autoDropdown.TabIndex = 5;
			this.autoDropdown.UseSelectable = true;
			this.autoDropdown.SelectedIndexChanged += new System.EventHandler(this.autoDropdown_SelectedIndexChanged);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(122, 249);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "OK";
			this.okButton.UseSelectable = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// autoLabel
			// 
			this.autoLabel.Location = new System.Drawing.Point(46, 139);
			this.autoLabel.Name = "autoLabel";
			this.autoLabel.Size = new System.Drawing.Size(235, 45);
			this.autoLabel.TabIndex = 6;
			this.autoLabel.Text = "Automatically check for updates when WinAuth is started";
			this.autoLabel.Click += new System.EventHandler(this.autoLabel_Click);
			// 
			// UpdateCheckForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(304, 295);
			this.Controls.Add(this.autoLabel);
			this.Controls.Add(this.autoDropdown);
			this.Controls.Add(this.autoCheckbox);
			this.Controls.Add(this.versionInfoLabel);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UpdateCheckForm";
			this.Resizable = false;
			this.Text = "Check for Updates";
			this.Load += new System.EventHandler(this.UpdateCheckForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Drawing.Html.HtmlLabel versionInfoLabel;
		private MetroFramework.Controls.MetroCheckBox autoCheckbox;
		private MetroFramework.Controls.MetroComboBox autoDropdown;
		private MetroFramework.Controls.MetroButton okButton;
		private MetroFramework.Controls.MetroLabel autoLabel;
	}
}