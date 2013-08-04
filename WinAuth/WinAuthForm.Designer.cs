namespace WinAuth
{
	using MetroFramework;
	using MetroFramework.Forms;
	
	partial class WinAuthForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinAuthForm));
			this.mainTimer = new System.Windows.Forms.Timer(this.components);
			this.authenticatorMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.commandPanel = new MetroFramework.Controls.MetroPanel();
			this.optionsButton = new MetroFramework.Controls.MetroButton();
			this.addAuthenticatorButton = new MetroFramework.Controls.MetroButton();
			this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
			this.metroStyleExtender = new MetroFramework.Components.MetroStyleExtender(this.components);
			this.addAuthenticatorMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.optionsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.introLabel = new MetroFramework.Controls.MetroLabel();
			this.passwordPanel = new MetroFramework.Controls.MetroPanel();
			this.passwordButton = new MetroFramework.Controls.MetroButton();
			this.passwordErrorLabel = new MetroFramework.Controls.MetroLabel();
			this.passwordLabel = new MetroFramework.Controls.MetroLabel();
			this.passwordField = new MetroFramework.Controls.MetroTextBox();
			this.passwordTimer = new System.Windows.Forms.Timer(this.components);
			this.authenticatorList = new WinAuth.AuthenticatorListBox();
			this.authenticatorMenu.SuspendLayout();
			this.commandPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
			this.passwordPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTimer
			// 
			this.mainTimer.Enabled = true;
			this.mainTimer.Interval = 500;
			this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
			// 
			// authenticatorMenu
			// 
			this.authenticatorMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
			this.authenticatorMenu.Name = "authenticatorMenu";
			this.authenticatorMenu.Size = new System.Drawing.Size(97, 26);
			// 
			// testToolStripMenuItem
			// 
			this.testToolStripMenuItem.Name = "testToolStripMenuItem";
			this.testToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
			this.testToolStripMenuItem.Text = "Test";
			// 
			// commandPanel
			// 
			this.commandPanel.Controls.Add(this.optionsButton);
			this.commandPanel.Controls.Add(this.addAuthenticatorButton);
			this.commandPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.commandPanel.HorizontalScrollbarBarColor = true;
			this.commandPanel.HorizontalScrollbarHighlightOnWheel = false;
			this.commandPanel.HorizontalScrollbarSize = 10;
			this.commandPanel.Location = new System.Drawing.Point(20, 128);
			this.commandPanel.Name = "commandPanel";
			this.commandPanel.Size = new System.Drawing.Size(380, 32);
			this.commandPanel.TabIndex = 1;
			this.commandPanel.VerticalScrollbarBarColor = true;
			this.commandPanel.VerticalScrollbarHighlightOnWheel = false;
			this.commandPanel.VerticalScrollbarSize = 10;
			this.commandPanel.Visible = false;
			this.commandPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.commandPanel_MouseDown);
			// 
			// optionsButton
			// 
			this.optionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.optionsButton.BackColor = System.Drawing.SystemColors.ControlLight;
			this.optionsButton.BackgroundImage = global::WinAuth.Properties.Resources.OptionsIcon;
			this.optionsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.optionsButton.Location = new System.Drawing.Point(352, 9);
			this.optionsButton.Name = "optionsButton";
			this.optionsButton.Size = new System.Drawing.Size(28, 23);
			this.optionsButton.TabIndex = 0;
			this.optionsButton.UseSelectable = true;
			this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
			// 
			// addAuthenticatorButton
			// 
			this.addAuthenticatorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addAuthenticatorButton.Location = new System.Drawing.Point(0, 9);
			this.addAuthenticatorButton.Name = "addAuthenticatorButton";
			this.addAuthenticatorButton.Size = new System.Drawing.Size(70, 23);
			this.addAuthenticatorButton.TabIndex = 0;
			this.addAuthenticatorButton.Text = "Add";
			this.addAuthenticatorButton.UseSelectable = true;
			this.addAuthenticatorButton.Click += new System.EventHandler(this.addAuthenticatorButton_Click);
			// 
			// metroStyleManager
			// 
			this.metroStyleManager.Owner = this;
			// 
			// addAuthenticatorMenu
			// 
			this.addAuthenticatorMenu.Name = "addMenu";
			this.addAuthenticatorMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// optionsMenu
			// 
			this.optionsMenu.Name = "addMenu";
			this.optionsMenu.Size = new System.Drawing.Size(61, 4);
			this.optionsMenu.Opening += new System.ComponentModel.CancelEventHandler(this.optionsMenu_Opening);
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.notifyMenu;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "WinAuth";
			this.notifyIcon.Visible = true;
			this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
			// 
			// notifyMenu
			// 
			this.notifyMenu.Name = "notifyMenu";
			this.notifyMenu.Size = new System.Drawing.Size(61, 4);
			this.notifyMenu.Opening += new System.ComponentModel.CancelEventHandler(this.notifyMenu_Opening);
			// 
			// introLabel
			// 
			this.introLabel.Location = new System.Drawing.Point(23, 76);
			this.introLabel.Name = "introLabel";
			this.introLabel.Size = new System.Drawing.Size(377, 35);
			this.introLabel.TabIndex = 3;
			this.introLabel.Text = "Click the \"Add\" button to create or import your authenticator";
			this.introLabel.Visible = false;
			// 
			// passwordPanel
			// 
			this.passwordPanel.Controls.Add(this.passwordButton);
			this.passwordPanel.Controls.Add(this.passwordErrorLabel);
			this.passwordPanel.Controls.Add(this.passwordLabel);
			this.passwordPanel.Controls.Add(this.passwordField);
			this.passwordPanel.HorizontalScrollbarBarColor = true;
			this.passwordPanel.HorizontalScrollbarHighlightOnWheel = false;
			this.passwordPanel.HorizontalScrollbarSize = 10;
			this.passwordPanel.Location = new System.Drawing.Point(20, 60);
			this.passwordPanel.Name = "passwordPanel";
			this.passwordPanel.Size = new System.Drawing.Size(380, 100);
			this.passwordPanel.TabIndex = 4;
			this.passwordPanel.VerticalScrollbarBarColor = true;
			this.passwordPanel.VerticalScrollbarHighlightOnWheel = false;
			this.passwordPanel.VerticalScrollbarSize = 10;
			// 
			// passwordButton
			// 
			this.passwordButton.Location = new System.Drawing.Point(278, 40);
			this.passwordButton.Name = "passwordButton";
			this.passwordButton.Size = new System.Drawing.Size(75, 23);
			this.passwordButton.TabIndex = 1;
			this.passwordButton.Text = "OK";
			this.passwordButton.UseSelectable = true;
			this.passwordButton.Click += new System.EventHandler(this.passwordButton_Click);
			// 
			// passwordErrorLabel
			// 
			this.passwordErrorLabel.ForeColor = System.Drawing.Color.Red;
			this.passwordErrorLabel.Location = new System.Drawing.Point(27, 68);
			this.passwordErrorLabel.Name = "passwordErrorLabel";
			this.passwordErrorLabel.Size = new System.Drawing.Size(326, 19);
			this.passwordErrorLabel.TabIndex = 3;
			this.passwordErrorLabel.UseCustomForeColor = true;
			// 
			// passwordLabel
			// 
			this.passwordLabel.AutoSize = true;
			this.passwordLabel.Location = new System.Drawing.Point(24, 16);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(63, 19);
			this.passwordLabel.TabIndex = 3;
			this.passwordLabel.Text = "Password";
			// 
			// passwordField
			// 
			this.passwordField.Location = new System.Drawing.Point(27, 42);
			this.passwordField.MaxLength = 32767;
			this.passwordField.Name = "passwordField";
			this.passwordField.PasswordChar = '●';
			this.passwordField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.passwordField.SelectedText = "";
			this.passwordField.Size = new System.Drawing.Size(236, 20);
			this.passwordField.TabIndex = 0;
			this.passwordField.UseSelectable = true;
			this.passwordField.UseSystemPasswordChar = true;
			this.passwordField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.passwordField_KeyPress);
			// 
			// passwordTimer
			// 
			this.passwordTimer.Interval = 500;
			this.passwordTimer.Tick += new System.EventHandler(this.passwordTimer_Tick);
			// 
			// authenticatorList
			// 
			this.authenticatorList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.authenticatorList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.authenticatorList.CurrentItem = null;
			this.authenticatorList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.authenticatorList.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.authenticatorList.IntegralHeight = false;
			this.authenticatorList.ItemHeight = 64;
			this.authenticatorList.Location = new System.Drawing.Point(20, 60);
			this.authenticatorList.Name = "authenticatorList";
			this.authenticatorList.ReadOnly = false;
			this.authenticatorList.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.authenticatorList.Size = new System.Drawing.Size(380, 62);
			this.authenticatorList.TabIndex = 0;
			this.authenticatorList.TabStop = false;
			this.authenticatorList.Visible = false;
			this.authenticatorList.ItemRemoved += new WinAuth.AuthenticatorListItemRemovedHandler(this.authenticatorList_ItemRemoved);
			// 
			// WinAuthForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.ClientSize = new System.Drawing.Size(420, 180);
			this.Controls.Add(this.passwordPanel);
			this.Controls.Add(this.introLabel);
			this.Controls.Add(this.commandPanel);
			this.Controls.Add(this.authenticatorList);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(420, 1080);
			this.MinimumSize = new System.Drawing.Size(420, 180);
			this.Name = "WinAuthForm";
			this.Resizable = false;
			this.StyleManager = this.metroStyleManager;
			this.Text = "WinAuth";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinAuthForm_FormClosing);
			this.Load += new System.EventHandler(this.WinAuthForm_Load);
			this.Shown += new System.EventHandler(this.WinAuthForm_Shown);
			this.ResizeEnd += new System.EventHandler(this.WinAuthForm_ResizeEnd);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WinAuthForm_MouseDown);
			this.Resize += new System.EventHandler(this.WinAuthForm_Resize);
			this.authenticatorMenu.ResumeLayout(false);
			this.commandPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
			this.passwordPanel.ResumeLayout(false);
			this.passwordPanel.PerformLayout();
			this.ResumeLayout(false);

    }

    #endregion

		private MetroFramework.Components.MetroStyleManager metroStyleManager;
		private MetroFramework.Components.MetroStyleExtender metroStyleExtender;
		private AuthenticatorListBox authenticatorList;
		private System.Windows.Forms.Timer mainTimer;
		private System.Windows.Forms.ContextMenuStrip authenticatorMenu;
		private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
		private MetroFramework.Controls.MetroPanel commandPanel;
		private MetroFramework.Controls.MetroButton addAuthenticatorButton;
		private System.Windows.Forms.ContextMenuStrip addAuthenticatorMenu;
		private MetroFramework.Controls.MetroButton optionsButton;
		private System.Windows.Forms.ContextMenuStrip optionsMenu;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private MetroFramework.Controls.MetroLabel introLabel;
		private MetroFramework.Controls.MetroPanel passwordPanel;
		private MetroFramework.Controls.MetroTextBox passwordField;
		private MetroFramework.Controls.MetroButton passwordButton;
		private MetroFramework.Controls.MetroLabel passwordErrorLabel;
		private System.Windows.Forms.Timer passwordTimer;
		private MetroFramework.Controls.MetroLabel passwordLabel;
		private System.Windows.Forms.ContextMenuStrip notifyMenu;

  }
}

