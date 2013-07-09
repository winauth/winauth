namespace WinAuth
{
	partial class DiagnosticForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagnosticForm));
			this.dataText = new System.Windows.Forms.TextBox();
			this.label = new System.Windows.Forms.Label();
			this.closeButton = new MetroFramework.Controls.MetroButton();
			this.SuspendLayout();
			// 
			// dataText
			// 
			this.dataText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataText.Location = new System.Drawing.Point(26, 230);
			this.dataText.Multiline = true;
			this.dataText.Name = "dataText";
			this.dataText.ReadOnly = true;
			this.dataText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dataText.Size = new System.Drawing.Size(465, 131);
			this.dataText.TabIndex = 5;
			// 
			// label
			// 
			this.label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label.Location = new System.Drawing.Point(26, 60);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(465, 167);
			this.label.TabIndex = 4;
			this.label.Text = resources.GetString("label.Text");
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.closeButton.Location = new System.Drawing.Point(416, 379);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 6;
			this.closeButton.Text = "Close";
			this.closeButton.UseSelectable = true;
			// 
			// DiagnosticForm
			// 
			this.AcceptButton = this.closeButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.ClientSize = new System.Drawing.Size(514, 425);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.dataText);
			this.Controls.Add(this.label);
			this.Name = "DiagnosticForm";
			this.Resizable = false;
			this.Text = "Diagnostic Information";
			this.Load += new System.EventHandler(this.ErrorReportForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label;
		private System.Windows.Forms.TextBox dataText;
		private MetroFramework.Controls.MetroButton closeButton;
	}
}