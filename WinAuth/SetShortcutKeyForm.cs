/*
 * Copyright (C) 2013 Colin Mackie.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WinAuth.Resources;

namespace WinAuth
{
	public partial class SetShortcutKeyForm : ResourceForm
	{
		/// <summary>
		/// Inner class to put into the dropdown to represent the keys
		/// </summary>
		class KeyItem
		{
			/// <summary>
			/// The virtual Key code
			/// </summary>
			public WinAPI.VirtualKeyCode Key { get; set; }

			/// <summary>
			/// Create the new item
			/// </summary>
			/// <param name="key"></param>
			public KeyItem(WinAPI.VirtualKeyCode key)
			{
				Key = key;
			}

			/// <summary>
			/// Get the string representation, ignoring the VK_
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return Key.ToString().Substring(3);
			}
		}

		/// <summary>
		/// Get/set the Sequence we are editing
		/// </summary>
		public HotKey Hotkey { get; set; }

		/// <summary>
		/// Create the form
		/// </summary>
		public SetShortcutKeyForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Load the form and set up the choices
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SetShortcutKeyForm_Load(object sender, EventArgs e)
		{
			// set the tooltip
			tooltip.SetToolTip(advancedTextbox, strings._SetShortcutKeyForm_advancedTextbox_tooltip);

			// load the key list
			keyCombo.Items.Clear();
			KeyItem selected = null;
			keyCombo.Items.Add("None");
			foreach (WinAPI.VirtualKeyCode vk in Enum.GetValues(typeof(WinAPI.VirtualKeyCode)))
			{
				if (vk >= WinAPI.VirtualKeyCode.VK_SPACE)
				{
					var item = new KeyItem(vk);
					keyCombo.Items.Add(item);
					if (Hotkey != null && Hotkey.Key == vk)
					{
						selected = item;
					}
				}
			}
			if (selected != null)
			{
				keyCombo.SelectedItem = selected;
			}
			else
			{
				keyCombo.SelectedIndex = 0;
			}

			// set the modifiers
			if (this.Hotkey != null)
			{
				shiftToggle.Checked = ((this.Hotkey.Modifiers & WinAPI.KeyModifiers.Shift) != 0);
				ctrlToggle.Checked = ((this.Hotkey.Modifiers & WinAPI.KeyModifiers.Control) != 0);
				altToggle.Checked = ((this.Hotkey.Modifiers & WinAPI.KeyModifiers.Alt) != 0);

				this.injectRadioButton.Enabled = true;
				this.injectRadioButton.Checked = (this.Hotkey.Action == HotKey.HotKeyActions.Inject);
				this.injectTextbox.Enabled = this.injectRadioButton.Checked;
				this.injectTextbox.Text = (this.injectRadioButton.Checked == true ? this.Hotkey.Window : string.Empty);
				//
				this.pasteRadioButton.Enabled = true;
				this.pasteRadioButton.Checked = (this.Hotkey.Action == HotKey.HotKeyActions.Copy);
				//
				this.advancedRadioButton.Enabled = true;
				this.advancedRadioButton.Checked = (this.Hotkey.Action == HotKey.HotKeyActions.Advanced);
				this.advancedTextbox.Enabled = this.advancedRadioButton.Checked;
				this.advancedTextbox.Text = (this.advancedRadioButton.Checked == true ? this.Hotkey.Advanced : string.Empty);
			}

		}

		/// <summary>
		/// Click the OK button to save the keys
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			WinAPI.VirtualKeyCode key = (keyCombo.SelectedItem as KeyItem != null ? ((KeyItem)keyCombo.SelectedItem).Key : default(WinAPI.VirtualKeyCode));
			if (key == 0)
			{
				this.Hotkey = null;
			}
			else if (this.Hotkey == null)
			{
				this.Hotkey = new HotKey();
			}

			if (this.Hotkey != null)
			{
				this.Hotkey.Key = key;
				WinAPI.KeyModifiers modifiers = WinAPI.KeyModifiers.None;
				if (shiftToggle.Checked)
				{
					modifiers |= WinAPI.KeyModifiers.Shift;
				}
				if (ctrlToggle.Checked)
				{
					modifiers |= WinAPI.KeyModifiers.Control;
				}
				if (altToggle.Checked)
				{
					modifiers |= WinAPI.KeyModifiers.Alt;
				}
				this.Hotkey.Modifiers = modifiers;

				if (injectRadioButton.Checked == true)
				{
					this.Hotkey.Action = HotKey.HotKeyActions.Inject;
					this.Hotkey.Window = this.injectTextbox.Text;
					this.Hotkey.Advanced = null;
				}
				else if (pasteRadioButton.Checked == true)
				{
					this.Hotkey.Action = HotKey.HotKeyActions.Copy;
					this.Hotkey.Window = null;
					this.Hotkey.Advanced = null;
				}
				else if (advancedRadioButton.Checked == true)
				{
					this.Hotkey.Action = HotKey.HotKeyActions.Advanced;
					this.Hotkey.Window = null;
					this.Hotkey.Advanced = this.advancedTextbox.Text;
				}
			}
		}

		/// <summary>
		/// Fired when the key selection is changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void keyCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			// clear the modifiers if we have cleared the key
			WinAPI.VirtualKeyCode key = (keyCombo.SelectedItem as KeyItem != null ? ((KeyItem)keyCombo.SelectedItem).Key : default(WinAPI.VirtualKeyCode));
			if (key == 0)
			{
				shiftToggle.Checked = false;
				ctrlToggle.Checked = false;
				altToggle.Checked = false;

				this.injectRadioButton.Enabled = false;
				this.pasteRadioButton.Enabled = false;
				this.advancedRadioButton.Enabled = false;
			}
			else
			{
				this.injectRadioButton.Enabled = true;
				this.pasteRadioButton.Enabled = true;
				this.advancedRadioButton.Enabled = true;
			}
		}

		/// <summary>
		/// Click the inject radio
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void injectRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			this.injectTextbox.Enabled = this.injectRadioButton.Checked;
		}

		/// <summary>
		/// Click the Advanced radio
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void advancedRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			this.advancedTextbox.Enabled = this.advancedRadioButton.Checked;
		}

		/// <summary>
		/// Click to manually show the Advanced tooltip
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void advancedLink_Click(object sender, EventArgs e)
		{
			tooltip.Show(tooltip.GetToolTip(advancedTextbox), advancedTextbox);
		}
	}
}
