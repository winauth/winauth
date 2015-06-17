namespace WinAuth
{
	partial class BetaForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BetaForm));
			this.betaLabel = new MetroFramework.Controls.MetroLabel();
			this.ckAgree = new MetroFramework.Controls.MetroCheckBox();
			this.btnCancel = new MetroFramework.Controls.MetroButton();
			this.btnOK = new MetroFramework.Controls.MetroButton();
			this.SuspendLayout();
			// 
			// betaLabel
			// 
			this.betaLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.betaLabel.Location = new System.Drawing.Point(23, 60);
			this.betaLabel.Name = "betaLabel";
			this.betaLabel.Size = new System.Drawing.Size(631, 297);
			this.betaLabel.TabIndex = 4;
			this.betaLabel.Text = resources.GetString("betaLabel.Text");
			// 
			// ckAgree
			// 
			this.ckAgree.AutoSize = true;
			this.ckAgree.Location = new System.Drawing.Point(23, 360);
			this.ckAgree.Name = "ckAgree";
			this.ckAgree.Size = new System.Drawing.Size(340, 15);
			this.ckAgree.TabIndex = 1;
			this.ckAgree.Text = "I understand and accept the conditions of this BETA version.";
			this.ckAgree.UseSelectable = true;
			this.ckAgree.CheckedChanged += new System.EventHandler(this.ckAgree_CheckedChanged);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(579, 384);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseSelectable = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Enabled = false;
			this.btnOK.Location = new System.Drawing.Point(489, 384);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseSelectable = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// BetaForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(683, 430);
			this.Controls.Add(this.ckAgree);
			this.Controls.Add(this.betaLabel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "BetaForm";
			this.Text = "This is an BETA version of WinAuth {0}";
			this.Load += new System.EventHandler(this.BetaForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroButton btnCancel;
		private MetroFramework.Controls.MetroCheckBox ckAgree;
		private MetroFramework.Controls.MetroButton btnOK;
		private MetroFramework.Controls.MetroLabel betaLabel;
	}
}