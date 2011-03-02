/*
 * Copyright (C) 2010 Colin Mackie.
 * This software is distributed under the terms of the GNU General Public License.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Form to configure the auto login hotkey behaviour
	/// </summary>
	public partial class AutoLoginForm : Form
	{
		#region Data Members

		/// <summary>
		/// Dictionary set of modifiers
		/// </summary>
		public static Dictionary<WinAPI.KeyModifiers, string> m_modifiers = new Dictionary<WinAPI.KeyModifiers,string>() {
			{WinAPI.KeyModifiers.None, WinAPI.KeyModifiers.None.ToString()},
			{WinAPI.KeyModifiers.Alt, WinAPI.KeyModifiers.Alt.ToString()},
			{WinAPI.KeyModifiers.Control, WinAPI.KeyModifiers.Control.ToString()},
			{WinAPI.KeyModifiers.Shift, WinAPI.KeyModifiers.Shift.ToString()}
		};

		/// <summary>
		/// List of comboboxes that contain modifiers
		/// </summary>
		private List<ComboBox> m_modCombos = null;

		/// <summary>
		/// Get/set the Sequence we are editing
		/// </summary>
		public HoyKeySequence Sequence { get; set; }

		#endregion


		#region Initializers

		/// <summary>
		/// Create the form
		/// </summary>
		public AutoLoginForm()
		{
			InitializeComponent();

			// set out array of modifer combos
			m_modCombos = new List<ComboBox> { this.cbHotKeyMod1, this.cbHotKeyMod2, this.cbHotKeyMod3 };
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Set up for the form based on overall enabled setting
		/// </summary>
		/// <param name="enabled"></param>
		private void SetEnabled(bool enabled)
		{
			// set the main checkbox
			ckHotKey.Checked = enabled;

			// enable or disabled the fields
			cbHotKey.Enabled = enabled;
			cbHotKeyMod1.Enabled = enabled;
			cbHotKeyMod2.Enabled = enabled;
			cbHotKeyMod3.Enabled = enabled;
			tbWindowTitle.Enabled = enabled;
			ckAdvanced.Enabled = enabled;
			tbAdvanced.Enabled = ckAdvanced.Checked;

			// default the script
			if (enabled == true && ckAdvanced.Checked == false)
			{
				tbAdvanced.Text = "{CODE}{ENTER}";
			}
		}

		/// <summary>
		/// Load the sequence object and set up all the fields
		/// </summary>
		private void LoadHotKeySequence()
		{
			// enable all the fields
			SetEnabled(this.Sequence != null);

			// if we enabled set the dropdowns and fields
			if (this.Sequence != null)
			{
				// set the modifiers
				int comboxIndex = 0;
				foreach (WinAPI.KeyModifiers modifier in m_modifiers.Keys)
				{
					if ((this.Sequence.Modifiers & modifier) != 0)
					{
						m_modCombos[comboxIndex++].SelectedItem = m_modifiers[modifier];
					}
				}

				// set the hotkey
				cbHotKey.SelectedItem = this.Sequence.HotKey.ToString().Substring(3);

				// set the title
				tbWindowTitle.Text = this.Sequence.WindowTitle;
				ckRegex.Checked = this.Sequence.WindowTitleRegex;
				tbProcessName.Text = this.Sequence.ProcessName;

				// set the advaned
				ckAdvanced.Checked = this.Sequence.Advanced;
				if (this.Sequence.Advanced == true)
				{
					this.tbAdvanced.Text = this.Sequence.AdvancedScript ?? string.Empty;
				}
			}
			else
			{
				// set defaults that actually work (e.g. Ctrl-Alt-W)
				cbHotKeyMod1.SelectedItem = WinAPI.KeyModifiers.Control.ToString();
				cbHotKeyMod2.SelectedItem = WinAPI.KeyModifiers.Alt.ToString();
				cbHotKeyMod3.SelectedItem = WinAPI.KeyModifiers.None.ToString();
				cbHotKey.SelectedItem = WinAPI.VirtualKeyCode.VK_W.ToString().Substring(3);
			}
		}

		/// <summary>
		/// Save the fields into the sequence object
		/// </summary>
		/// <returns>return false if validation failed</returns>
		private bool SaveHotKeySequence()
		{
			// disabled?
			if (ckHotKey.Checked == false)
			{
				this.Sequence = null;
				return true;
			}

			// verify
			string noneModiifer = WinAPI.KeyModifiers.None.ToString();
			if ((string)this.cbHotKeyMod1.SelectedItem == noneModiifer
				&& (string)this.cbHotKeyMod2.SelectedItem == noneModiifer
				&& (string)this.cbHotKeyMod3.SelectedItem == noneModiifer)
			{
				MessageBox.Show(this, "You must select at least one modifier (e.g. Alt, Ctrl or Shift)", "Auto Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			if (this.cbHotKey.SelectedItem == null || (string)this.cbHotKey.SelectedItem == string.Empty)
			{
				MessageBox.Show(this, "You must select a hotkey", "Auto Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.cbHotKey.Focus();
				return false;
			}
			if (this.ckAdvanced.Checked == true && this.tbAdvanced.Text.Trim().Length == 0)
			{
				MessageBox.Show(this, "You must select enter some advanced key commands", "Auto Login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				tbAdvanced.Focus();
				return false;
			}

			// enabled and craerte HoTKetSequence if necccessary
			HoyKeySequence sequence = this.Sequence ?? new HoyKeySequence();
			if (this.Sequence == null)
			{
				this.Sequence = sequence;
			}

			// get the modifiers
			WinAPI.KeyModifiers modifier = WinAPI.KeyModifiers.None;
			modifier |= FindModifier((string)cbHotKeyMod1.SelectedItem);
			modifier |= FindModifier((string)cbHotKeyMod2.SelectedItem);
			modifier |= FindModifier((string)cbHotKeyMod3.SelectedItem);
			sequence.Modifiers = modifier;

			// get the key
			string key = "VK_" + ((string)cbHotKey.SelectedItem);
			sequence.HotKey = (WinAPI.VirtualKeyCode)Enum.Parse(typeof(WinAPI.VirtualKeyCode), key, true);

			// get the title
			sequence.WindowTitle = this.tbWindowTitle.Text;
			sequence.WindowTitleRegex = this.ckRegex.Checked;
			sequence.ProcessName = this.tbProcessName.Text;

			// get the script
			sequence.Advanced = ckAdvanced.Checked;
			sequence.AdvancedScript = tbAdvanced.Text;

			return true;
		}

		/// <summary>
		/// Get the right modifier based on a string
		/// </summary>
		/// <param name="modifiername"></param>
		/// <returns></returns>
		private static WinAPI.KeyModifiers FindModifier(string modifiername)
		{
			foreach (KeyValuePair<WinAPI.KeyModifiers, string> modifier in m_modifiers)
			{
				if (modifier.Value == modifiername)
				{
					return modifier.Key;
				}
			}

			return WinAPI.KeyModifiers.None;
		}

		#endregion

		#region Form events

		/// <summary>
		/// Change the main hotkeyt checkbox enabling or disabling the fields
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ckHotKey_CheckedChanged(object sender, EventArgs e)
		{
			SetEnabled(ckHotKey.Checked);
		}

		/// <summary>
		/// Change the advanced checkedbox allowing typing into script field
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ckAdvanced_CheckedChanged(object sender, EventArgs e)
		{
			tbAdvanced.Enabled = ckAdvanced.Checked;
		}

		/// <summary>
		/// Load the form. Set up combos and load current sequence
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OptionsForm_Load(object sender, EventArgs e)
		{
			// set tooltips
			ResourceManager rm = new ResourceManager("WindowsAuthenticator.Properties.Resources", typeof(AutoLoginForm).Assembly);
			tooltip.SetToolTip(ckAdvanced, rm.GetString("AUTOLOGIN_SCRIPT_TOOLTIP"));
			tooltip.SetToolTip(tbAdvanced, rm.GetString("AUTOLOGIN_TBADVANCED_TOOLTIP"));

			// load up the combos
			foreach (ComboBox cb in m_modCombos)
			{
				cb.Items.Clear();
				foreach (WinAPI.KeyModifiers modifier in m_modifiers.Keys)
				{
					cb.Items.Add(m_modifiers[modifier]);
				}
				cb.SelectedItem = cb.Items[0];
			}
			//
			cbHotKey.Items.Clear();
			foreach (WinAPI.VirtualKeyCode vk in Enum.GetValues(typeof(WinAPI.VirtualKeyCode)))
			{
				if (vk >= WinAPI.VirtualKeyCode.VK_SPACE)
				{
					cbHotKey.Items.Add(vk.ToString().Substring(3));
				}
			}
			cbHotKey.SelectedItem = cbHotKey.Items[0];

			// load current sequence
			LoadHotKeySequence();
		}

		/// <summary>
		/// Click the OK button to save the sequence
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (SaveHotKeySequence() == true)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}

		#endregion

	}
}
