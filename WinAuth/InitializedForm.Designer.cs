namespace WindowsAuthenticator
{
	partial class InitializedForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitializedForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnNo = new System.Windows.Forms.Button();
			this.btnYes = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(582, 415);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(6, 116);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(304, 290);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(316, 354);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(254, 52);
			this.label3.TabIndex = 1;
			this.label3.Text = "Do you want to send a backup copy of you authenticator data to your email account" +
					"?";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(316, 116);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(254, 210);
			this.label2.TabIndex = 1;
			this.label2.Text = resources.GetString("label2.Text");
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(564, 97);
			this.label1.TabIndex = 1;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// btnNo
			// 
			this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnNo.Location = new System.Drawing.Point(518, 433);
			this.btnNo.Name = "btnNo";
			this.btnNo.Size = new System.Drawing.Size(75, 23);
			this.btnNo.TabIndex = 3;
			this.btnNo.Text = "No";
			this.btnNo.UseVisualStyleBackColor = true;
			this.btnNo.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnYes
			// 
			this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.btnYes.Location = new System.Drawing.Point(437, 433);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new System.Drawing.Size(75, 23);
			this.btnYes.TabIndex = 4;
			this.btnYes.Text = "Yes";
			this.btnYes.UseVisualStyleBackColor = true;
			// 
			// InitializedForm
			// 
			this.AcceptButton = this.btnYes;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnNo;
			this.ClientSize = new System.Drawing.Size(607, 465);
			this.Controls.Add(this.btnYes);
			this.Controls.Add(this.btnNo);
			this.Controls.Add(this.groupBox1);
			this.Name = "InitializedForm";
			this.ShowIcon = false;
			this.Text = "New Authenticator";
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnNo;
		private System.Windows.Forms.Button btnYes;
		private System.Windows.Forms.Label label3;
	}
}