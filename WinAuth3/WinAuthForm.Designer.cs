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
			this.addAuthenticatorChoiceButton = new MetroFramework.Controls.MetroButton();
			this.addAuthenticatorButton = new MetroFramework.Controls.MetroButton();
			this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
			this.metroStyleExtender = new MetroFramework.Components.MetroStyleExtender(this.components);
			this.addAuthenticatorMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.authenticatorList = new WinAuth.AuthenticatorListBox();
			this.authenticatorMenu.SuspendLayout();
			this.commandPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
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
			this.commandPanel.Controls.Add(this.addAuthenticatorChoiceButton);
			this.commandPanel.Controls.Add(this.addAuthenticatorButton);
			this.commandPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.commandPanel.HorizontalScrollbarBarColor = true;
			this.commandPanel.HorizontalScrollbarHighlightOnWheel = false;
			this.commandPanel.HorizontalScrollbarSize = 10;
			this.commandPanel.Location = new System.Drawing.Point(20, 194);
			this.commandPanel.Name = "commandPanel";
			this.commandPanel.Size = new System.Drawing.Size(380, 53);
			this.commandPanel.TabIndex = 1;
			this.commandPanel.VerticalScrollbarBarColor = true;
			this.commandPanel.VerticalScrollbarHighlightOnWheel = false;
			this.commandPanel.VerticalScrollbarSize = 10;
			this.commandPanel.Click += new System.EventHandler(this.addAuthenticatorChoiceButton_Click);
			// 
			// addAuthenticatorChoiceButton
			// 
			this.addAuthenticatorChoiceButton.Location = new System.Drawing.Point(69, 14);
			this.addAuthenticatorChoiceButton.Name = "addAuthenticatorChoiceButton";
			this.addAuthenticatorChoiceButton.Size = new System.Drawing.Size(24, 23);
			this.addAuthenticatorChoiceButton.TabIndex = 0;
			this.addAuthenticatorChoiceButton.Text = "▼";
			this.addAuthenticatorChoiceButton.UseSelectable = true;
			// 
			// addAuthenticatorButton
			// 
			this.addAuthenticatorButton.Location = new System.Drawing.Point(0, 14);
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
			// authenticatorList
			// 
			this.authenticatorList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.authenticatorList.CurrentItem = null;
			this.authenticatorList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.authenticatorList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.authenticatorList.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.authenticatorList.IntegralHeight = false;
			this.authenticatorList.ItemHeight = 64;
			this.authenticatorList.Location = new System.Drawing.Point(20, 60);
			this.authenticatorList.Name = "authenticatorList";
			this.authenticatorList.ReadOnly = false;
			this.authenticatorList.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.authenticatorList.Size = new System.Drawing.Size(380, 187);
			this.authenticatorList.TabIndex = 0;
			this.authenticatorList.TabStop = false;
			// 
			// WinAuthForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(420, 267);
			this.Controls.Add(this.commandPanel);
			this.Controls.Add(this.authenticatorList);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(420, 1080);
			this.MinimumSize = new System.Drawing.Size(420, 39);
			this.Name = "WinAuthForm";
			this.StyleManager = this.metroStyleManager;
			this.Text = "WinAuth";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.WinAuthForm_Load);
			this.Resize += new System.EventHandler(this.WinAuthForm_Resize);
			this.authenticatorMenu.ResumeLayout(false);
			this.commandPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
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
		private MetroFramework.Controls.MetroButton addAuthenticatorChoiceButton;
		private System.Windows.Forms.ContextMenuStrip addAuthenticatorMenu;

  }
}

