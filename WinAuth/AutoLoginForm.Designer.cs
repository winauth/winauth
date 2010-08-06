namespace WindowsAuthenticator
{
	partial class AutoLoginForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoLoginForm));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.ckHotKey = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbAdvanced = new System.Windows.Forms.RichTextBox();
			this.cbHotKey = new System.Windows.Forms.ComboBox();
			this.cbHotKeyMod3 = new System.Windows.Forms.ComboBox();
			this.cbHotKeyMod2 = new System.Windows.Forms.ComboBox();
			this.tbWindowTitle = new System.Windows.Forms.TextBox();
			this.cbHotKeyMod1 = new System.Windows.Forms.ComboBox();
			this.ckAdvanced = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(453, 328);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(372, 328);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// ckHotKey
			// 
			this.ckHotKey.AutoSize = true;
			this.ckHotKey.Location = new System.Drawing.Point(16, 93);
			this.ckHotKey.Name = "ckHotKey";
			this.ckHotKey.Size = new System.Drawing.Size(190, 17);
			this.ckHotKey.TabIndex = 0;
			this.ckHotKey.Text = "Enable system hotkey to auto-login";
			this.ckHotKey.UseVisualStyleBackColor = true;
			this.ckHotKey.CheckedChanged += new System.EventHandler(this.ckHotKey_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.tbAdvanced);
			this.groupBox2.Controls.Add(this.cbHotKey);
			this.groupBox2.Controls.Add(this.cbHotKeyMod3);
			this.groupBox2.Controls.Add(this.cbHotKeyMod2);
			this.groupBox2.Controls.Add(this.tbWindowTitle);
			this.groupBox2.Controls.Add(this.cbHotKeyMod1);
			this.groupBox2.Controls.Add(this.ckAdvanced);
			this.groupBox2.Controls.Add(this.ckHotKey);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(516, 310);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label3.Location = new System.Drawing.Point(127, 193);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(371, 32);
			this.label3.TabIndex = 7;
			this.label3.Text = "e.g. {CODE}{ENTER}\r\n       my@email.com{TAB}password{ENTER 3000}{CODE}{ENTER}";
			// 
			// tbAdvanced
			// 
			this.tbAdvanced.Enabled = false;
			this.tbAdvanced.Location = new System.Drawing.Point(16, 228);
			this.tbAdvanced.Name = "tbAdvanced";
			this.tbAdvanced.Size = new System.Drawing.Size(482, 66);
			this.tbAdvanced.TabIndex = 6;
			this.tbAdvanced.Text = "";
			// 
			// cbHotKey
			// 
			this.cbHotKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHotKey.Enabled = false;
			this.cbHotKey.FormattingEnabled = true;
			this.cbHotKey.Items.AddRange(new object[] {
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
			this.cbHotKey.Location = new System.Drawing.Point(285, 116);
			this.cbHotKey.MaxDropDownItems = 3;
			this.cbHotKey.Name = "cbHotKey";
			this.cbHotKey.Size = new System.Drawing.Size(99, 21);
			this.cbHotKey.TabIndex = 4;
			// 
			// cbHotKeyMod3
			// 
			this.cbHotKeyMod3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHotKeyMod3.Enabled = false;
			this.cbHotKeyMod3.FormattingEnabled = true;
			this.cbHotKeyMod3.Location = new System.Drawing.Point(160, 116);
			this.cbHotKeyMod3.MaxDropDownItems = 3;
			this.cbHotKeyMod3.Name = "cbHotKeyMod3";
			this.cbHotKeyMod3.Size = new System.Drawing.Size(66, 21);
			this.cbHotKeyMod3.TabIndex = 3;
			// 
			// cbHotKeyMod2
			// 
			this.cbHotKeyMod2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHotKeyMod2.Enabled = false;
			this.cbHotKeyMod2.FormattingEnabled = true;
			this.cbHotKeyMod2.Location = new System.Drawing.Point(88, 116);
			this.cbHotKeyMod2.MaxDropDownItems = 3;
			this.cbHotKeyMod2.Name = "cbHotKeyMod2";
			this.cbHotKeyMod2.Size = new System.Drawing.Size(66, 21);
			this.cbHotKeyMod2.TabIndex = 2;
			// 
			// tbWindowTitle
			// 
			this.tbWindowTitle.Enabled = false;
			this.tbWindowTitle.Location = new System.Drawing.Point(99, 155);
			this.tbWindowTitle.Name = "tbWindowTitle";
			this.tbWindowTitle.Size = new System.Drawing.Size(399, 20);
			this.tbWindowTitle.TabIndex = 5;
			// 
			// cbHotKeyMod1
			// 
			this.cbHotKeyMod1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbHotKeyMod1.Enabled = false;
			this.cbHotKeyMod1.FormattingEnabled = true;
			this.cbHotKeyMod1.Location = new System.Drawing.Point(16, 116);
			this.cbHotKeyMod1.MaxDropDownItems = 3;
			this.cbHotKeyMod1.Name = "cbHotKeyMod1";
			this.cbHotKeyMod1.Size = new System.Drawing.Size(66, 21);
			this.cbHotKeyMod1.TabIndex = 1;
			// 
			// ckAdvanced
			// 
			this.ckAdvanced.AutoSize = true;
			this.ckAdvanced.Enabled = false;
			this.ckAdvanced.Location = new System.Drawing.Point(16, 193);
			this.ckAdvanced.Name = "ckAdvanced";
			this.ckAdvanced.Size = new System.Drawing.Size(105, 17);
			this.ckAdvanced.TabIndex = 0;
			this.ckAdvanced.Text = "Advanced Mode";
			this.ckAdvanced.UseVisualStyleBackColor = true;
			this.ckAdvanced.CheckedChanged += new System.EventHandler(this.ckAdvanced_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(243, 119);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(25, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "and";
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(13, 26);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(497, 64);
			this.label5.TabIndex = 0;
			this.label5.Text = resources.GetString("label5.Text");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 158);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Window Title";
			// 
			// tooltip
			// 
			this.tooltip.AutoPopDelay = 30000;
			this.tooltip.InitialDelay = 500;
			this.tooltip.ReshowDelay = 100;
			// 
			// AutoLoginForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(540, 363);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AutoLoginForm";
			this.ShowIcon = false;
			this.Text = "Auto Login";
			this.Load += new System.EventHandler(this.OptionsForm_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox ckHotKey;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox cbHotKey;
		private System.Windows.Forms.ComboBox cbHotKeyMod2;
		private System.Windows.Forms.TextBox tbWindowTitle;
		private System.Windows.Forms.ComboBox cbHotKeyMod1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RichTextBox tbAdvanced;
		private System.Windows.Forms.CheckBox ckAdvanced;
		private System.Windows.Forms.ComboBox cbHotKeyMod3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ToolTip tooltip;
	}
}