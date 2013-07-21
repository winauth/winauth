namespace WinAuth
{
	partial class SetShortcutKeyForm
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
			this.introLabel = new MetroFramework.Controls.MetroLabel();
			this.shiftToggle = new MetroFramework.Controls.MetroToggle();
			this.ctrlLabel = new MetroFramework.Controls.MetroLabel();
			this.shiftLabel = new MetroFramework.Controls.MetroLabel();
			this.altLabel = new MetroFramework.Controls.MetroLabel();
			this.ctrlToggle = new MetroFramework.Controls.MetroToggle();
			this.altToggle = new MetroFramework.Controls.MetroToggle();
			this.keyCombo = new MetroFramework.Controls.MetroComboBox();
			this.keyLabel = new MetroFramework.Controls.MetroLabel();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.SuspendLayout();
			// 
			// introLabel
			// 
			this.introLabel.Location = new System.Drawing.Point(23, 60);
			this.introLabel.Name = "introLabel";
			this.introLabel.Size = new System.Drawing.Size(196, 67);
			this.introLabel.TabIndex = 0;
			this.introLabel.Text = "Select a shortcut to either inject your code or run a script";
			// 
			// shiftToggle
			// 
			this.shiftToggle.AutoSize = true;
			this.shiftToggle.Location = new System.Drawing.Point(75, 177);
			this.shiftToggle.Name = "shiftToggle";
			this.shiftToggle.Size = new System.Drawing.Size(80, 17);
			this.shiftToggle.TabIndex = 1;
			this.shiftToggle.Text = "Off";
			this.shiftToggle.UseSelectable = true;
			// 
			// ctrlLabel
			// 
			this.ctrlLabel.AutoSize = true;
			this.ctrlLabel.Location = new System.Drawing.Point(26, 198);
			this.ctrlLabel.Name = "ctrlLabel";
			this.ctrlLabel.Size = new System.Drawing.Size(30, 19);
			this.ctrlLabel.TabIndex = 2;
			this.ctrlLabel.Text = "Ctrl";
			// 
			// shiftLabel
			// 
			this.shiftLabel.AutoSize = true;
			this.shiftLabel.Location = new System.Drawing.Point(26, 175);
			this.shiftLabel.Name = "shiftLabel";
			this.shiftLabel.Size = new System.Drawing.Size(34, 19);
			this.shiftLabel.TabIndex = 2;
			this.shiftLabel.Text = "Shift";
			// 
			// altLabel
			// 
			this.altLabel.AutoSize = true;
			this.altLabel.Location = new System.Drawing.Point(26, 221);
			this.altLabel.Name = "altLabel";
			this.altLabel.Size = new System.Drawing.Size(25, 19);
			this.altLabel.TabIndex = 2;
			this.altLabel.Text = "Alt";
			// 
			// ctrlToggle
			// 
			this.ctrlToggle.AutoSize = true;
			this.ctrlToggle.Location = new System.Drawing.Point(75, 200);
			this.ctrlToggle.Name = "ctrlToggle";
			this.ctrlToggle.Size = new System.Drawing.Size(80, 17);
			this.ctrlToggle.TabIndex = 1;
			this.ctrlToggle.Text = "Off";
			this.ctrlToggle.UseSelectable = true;
			// 
			// altToggle
			// 
			this.altToggle.AutoSize = true;
			this.altToggle.Location = new System.Drawing.Point(75, 223);
			this.altToggle.Name = "altToggle";
			this.altToggle.Size = new System.Drawing.Size(80, 17);
			this.altToggle.TabIndex = 1;
			this.altToggle.Text = "Off";
			this.altToggle.UseSelectable = true;
			// 
			// keyCombo
			// 
			this.keyCombo.FormattingEnabled = true;
			this.keyCombo.ItemHeight = 23;
			this.keyCombo.Items.AddRange(new object[] {
            "[None]",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "[",
            "]",
            ";",
            "\'",
            "#",
            "<",
            ">",
            "/"});
			this.keyCombo.Location = new System.Drawing.Point(75, 130);
			this.keyCombo.Name = "keyCombo";
			this.keyCombo.Size = new System.Drawing.Size(144, 29);
			this.keyCombo.TabIndex = 3;
			this.keyCombo.UseSelectable = true;
			this.keyCombo.SelectedIndexChanged += new System.EventHandler(this.keyCombo_SelectedIndexChanged);
			// 
			// keyLabel
			// 
			this.keyLabel.AutoSize = true;
			this.keyLabel.Location = new System.Drawing.Point(26, 136);
			this.keyLabel.Name = "keyLabel";
			this.keyLabel.Size = new System.Drawing.Size(29, 19);
			this.keyLabel.TabIndex = 2;
			this.keyLabel.Text = "Key";
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(145, 267);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseSelectable = true;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(64, 267);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 4;
			this.okButton.Text = "OK";
			this.okButton.UseSelectable = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// SetShortcutKeyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(243, 313);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.keyCombo);
			this.Controls.Add(this.shiftLabel);
			this.Controls.Add(this.keyLabel);
			this.Controls.Add(this.altLabel);
			this.Controls.Add(this.ctrlLabel);
			this.Controls.Add(this.altToggle);
			this.Controls.Add(this.ctrlToggle);
			this.Controls.Add(this.shiftToggle);
			this.Controls.Add(this.introLabel);
			this.Name = "SetShortcutKeyForm";
			this.Resizable = false;
			this.Text = "Shortcut Key";
			this.Load += new System.EventHandler(this.SetShortcutKeyForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroLabel introLabel;
		private MetroFramework.Controls.MetroToggle shiftToggle;
		private MetroFramework.Controls.MetroLabel ctrlLabel;
		private MetroFramework.Controls.MetroLabel shiftLabel;
		private MetroFramework.Controls.MetroLabel altLabel;
		private MetroFramework.Controls.MetroToggle ctrlToggle;
		private MetroFramework.Controls.MetroToggle altToggle;
		private MetroFramework.Controls.MetroComboBox keyCombo;
		private MetroFramework.Controls.MetroLabel keyLabel;
		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Controls.MetroButton okButton;
	}
}