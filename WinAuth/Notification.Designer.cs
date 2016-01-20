namespace WinAuth
{
	partial class Notification
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
			this.lifeTimer = new System.Windows.Forms.Timer(this.components);
			this.labelBody = new MetroFramework.Controls.MetroLabel();
			this.htmlBody = new System.Windows.Forms.Panel();
			this.labelTitle = new MetroFramework.Controls.MetroLabel();
			this.button1 = new MetroFramework.Controls.MetroButton();
			this.buttonPanel = new System.Windows.Forms.Panel();
			this.button3 = new MetroFramework.Controls.MetroButton();
			this.button2 = new MetroFramework.Controls.MetroButton();
			this.tradeSep = new System.Windows.Forms.PictureBox();
			this.closeLink = new MetroFramework.Controls.MetroLink();
			this.buttonPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tradeSep)).BeginInit();
			this.SuspendLayout();
			// 
			// lifeTimer
			// 
			this.lifeTimer.Tick += new System.EventHandler(this.lifeTimer_Tick);
			// 
			// labelBody
			// 
			this.labelBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBody.BackColor = System.Drawing.SystemColors.Window;
			this.labelBody.ForeColor = System.Drawing.SystemColors.ControlText;
			this.labelBody.Location = new System.Drawing.Point(7, 39);
			this.labelBody.Name = "labelBody";
			this.labelBody.Size = new System.Drawing.Size(306, 72);
			this.labelBody.TabIndex = 0;
			this.labelBody.Text = "Body goes here and here and here and here and here";
			this.labelBody.Visible = false;
			this.labelBody.Click += new System.EventHandler(this.Notification_Click);
			// 
			// htmlBody
			// 
			this.htmlBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.htmlBody.Location = new System.Drawing.Point(7, 39);
			this.htmlBody.Name = "htmlBody";
			this.htmlBody.Size = new System.Drawing.Size(306, 72);
			this.htmlBody.TabIndex = 0;
			this.htmlBody.Visible = false;
			this.htmlBody.Click += new System.EventHandler(this.Notification_Click);
			// 
			// labelTitle
			// 
			this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTitle.FontSize = MetroFramework.MetroLabelSize.Tall;
			this.labelTitle.FontWeight = MetroFramework.MetroLabelWeight.Regular;
			this.labelTitle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.labelTitle.Location = new System.Drawing.Point(7, 8);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(282, 28);
			this.labelTitle.TabIndex = 0;
			this.labelTitle.Text = "title goes here";
			this.labelTitle.Click += new System.EventHandler(this.Notification_Click);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(233, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.UseSelectable = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// buttonPanel
			// 
			this.buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonPanel.Controls.Add(this.button3);
			this.buttonPanel.Controls.Add(this.button2);
			this.buttonPanel.Controls.Add(this.button1);
			this.buttonPanel.Location = new System.Drawing.Point(7, 84);
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Size = new System.Drawing.Size(311, 29);
			this.buttonPanel.TabIndex = 2;
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.Location = new System.Drawing.Point(71, 3);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 1;
			this.button3.Text = "button3";
			this.button3.UseSelectable = true;
			this.button3.Visible = false;
			this.button3.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(152, 3);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "button2";
			this.button2.UseSelectable = true;
			this.button2.Visible = false;
			this.button2.Click += new System.EventHandler(this.button1_Click);
			// 
			// tradeSep
			// 
			this.tradeSep.Image = global::WinAuth.Properties.Resources.BluePixel;
			this.tradeSep.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.tradeSep.Location = new System.Drawing.Point(12, 35);
			this.tradeSep.Name = "tradeSep";
			this.tradeSep.Size = new System.Drawing.Size(301, 1);
			this.tradeSep.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.tradeSep.TabIndex = 7;
			this.tradeSep.TabStop = false;
			// 
			// closeLink
			// 
			this.closeLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.closeLink.Cursor = System.Windows.Forms.Cursors.Hand;
			this.closeLink.FontSize = MetroFramework.MetroLinkSize.Medium;
			this.closeLink.FontWeight = MetroFramework.MetroLinkWeight.Regular;
			this.closeLink.Location = new System.Drawing.Point(298, 8);
			this.closeLink.Name = "closeLink";
			this.closeLink.Size = new System.Drawing.Size(23, 23);
			this.closeLink.TabIndex = 8;
			this.closeLink.Text = "X";
			this.closeLink.UseSelectable = true;
			this.closeLink.Click += new System.EventHandler(this.closeLink_Click);
			// 
			// Notification
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(324, 120);
			this.ControlBox = false;
			this.Controls.Add(this.closeLink);
			this.Controls.Add(this.tradeSep);
			this.Controls.Add(this.buttonPanel);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.labelBody);
			this.Controls.Add(this.htmlBody);
			this.Name = "Notification";
			this.Resizable = false;
			this.ShadowType = MetroFramework.Forms.MetroFormShadowType.Flat;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.Activated += new System.EventHandler(this.Notification_Activated);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Notification_FormClosed);
			this.Load += new System.EventHandler(this.Notification_Load);
			this.Shown += new System.EventHandler(this.Notification_Shown);
			this.Click += new System.EventHandler(this.Notification_Click);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Notification_Paint);
			this.buttonPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tradeSep)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer lifeTimer;
		private MetroFramework.Controls.MetroLabel labelBody;
		private System.Windows.Forms.Panel htmlBody;
		private MetroFramework.Controls.MetroLabel labelTitle;
		private MetroFramework.Controls.MetroButton button1;
		private System.Windows.Forms.Panel buttonPanel;
		private MetroFramework.Controls.MetroButton button2;
		private MetroFramework.Controls.MetroButton button3;
		private System.Windows.Forms.PictureBox tradeSep;
		private MetroFramework.Controls.MetroLink closeLink;
	}
}