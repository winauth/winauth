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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// An actual working OpenFileDialog since the CF one can't go up
	/// </summary>
	public partial class OpenFileDialogFTFY : Form
	{
		/// <summary>
		/// static piparray for splitting filters
		/// </summary>
		private static char[] PIPEARRAY = new char[] { '|' };

		/// <summary>
		/// Default filter if none passed in
		/// </summary>
		private const string DEFAULT_FILTER = "*.*|All Files (*.*)";

		/// <summary>
		/// Internalf flag to say we are loading list so we don't process events
		/// </summary>
		private bool IsLoadingFiles { get; set; }

		/// <summary>
		/// Current selected directory
		/// </summary>
		protected DirectoryInfo CurrentDirectory { get; set; }

		/// <summary>
		/// Current selected filename
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Initial directory to populate
		/// </summary>
		public string InitialDirectory { get; set; }

		/// <summary>
		/// Filter string, e.g. "*.*|All Files (*.*)|*.doc|Doc Files (*.doc)"
		/// </summary>
		public string Filter { get; set; }

		/// <summary>
		/// Initial filter index
		/// </summary>
		public int FilterIndex { get; set; }

		/// <summary>
		/// Create a new OpenFileDialog
		/// </summary>
		public OpenFileDialogFTFY()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Build a string string, e.g. 456B, 234K, or 34M
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		private static string ToFileStringString(long filesize)
		{
			decimal size = Convert.ToDecimal(filesize);

			if (size < 1000)
			{
				return size.ToString() + "B";
			}
			else if (size < (99*1024))
			{
				return (size / 1024).ToString("0.#") + "K";
			}
			else if (size < (99 * 1024 * 1024))
			{
				return (size / (1024*1024)).ToString("0.#") + "M";
			}
			else
			{
				return (size / (1024*1024*1024)).ToString("0.#") + "G";
			}
		}

		/// <summary>
		/// Get the current filer, e.g. "*.*"
		/// </summary>
		/// <returns></returns>
		private string GetFilter()
		{
			string[] filters = (string.IsNullOrEmpty(Filter) == false ? Filter.Split(PIPEARRAY) : DEFAULT_FILTER.Split(PIPEARRAY));
			if (FilterIndex * 2 > filters.Length)
			{
				throw new IndexOutOfRangeException();
			}
			return filters[FilterIndex * 2];
		}

		/// <summary>
		/// Build the list of directories and files
		/// </summary>
		private void PopulateFiles()
		{
			// say we're loading
			IsLoadingFiles = true;

			// clear and the the directories dropdown
			dirDropDown.Items.Clear();

			// get the parent if there is one
			DirectoryInfo parent = (CurrentDirectory.Parent != null ? CurrentDirectory.Parent : CurrentDirectory);
			if (parent == CurrentDirectory)
			{
				dirDropDown.Items.Add("My Device");
				dirDropDown.SelectedIndex = 0;
			}
			else if (parent.Parent != null)
			{
				dirDropDown.Items.Add(parent);
			}
			else
			{
				dirDropDown.Items.Add("My Device");
			}
			// get all siblings
			foreach (DirectoryInfo dir in parent.GetDirectories())
			{
				dirDropDown.Items.Add(dir);
				if (CurrentDirectory.FullName == dir.FullName)
				{
					dirDropDown.SelectedItem = dir;
				}
			}

			// clear and load the files list
			fileListiew.Items.Clear();
			foreach (DirectoryInfo di in CurrentDirectory.GetDirectories())
			{
				ListViewItem lvi = new ListViewItem(di.Name);
				lvi.Tag = di;
				fileListiew.Items.Add(lvi);
			}
			foreach (FileInfo fi in CurrentDirectory.GetFiles(GetFilter()))
			{
				string[] parts = new string[] { fi.Name, fi.LastWriteTime.ToShortDateString(), ToFileStringString(fi.Length) };
				ListViewItem lvi = new ListViewItem(parts);
				lvi.Tag = fi;
				fileListiew.Items.Add(lvi);
			}

			// ok to select
			IsLoadingFiles = false;
		}

		/// <summary>
		/// Select an item: directory or file
		/// </summary>
		/// <param name="fileItem"></param>
		private void SelectFile(object fileItem)
		{
			if (fileItem == null)
			{
				// this is the root
				DirectoryInfo path = CurrentDirectory;
				while (path.Parent != null)
				{
					path = path.Parent;
				}
				CurrentDirectory = path;
				PopulateFiles();
			}
			else if (fileItem is DirectoryInfo)
			{
				CurrentDirectory = (DirectoryInfo)fileItem;
				PopulateFiles();
			}
			else if (fileItem is FileInfo)
			{
				FileName = ((FileInfo)fileItem).FullName;
			}
		}

		/// <summary>
		/// Form event on loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OpenFileDialogFTFY_Load(object sender, EventArgs e)
		{
			// set current directory
			if (string.IsNullOrEmpty(InitialDirectory) == true)
			{
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			CurrentDirectory = new DirectoryInfo(InitialDirectory);

			// set widths
			int width = this.Width;
			fileListiew.Columns[0].Width = width * 50 / 100;
			fileListiew.Columns[1].Width = width * 25 / 100;
			fileListiew.Columns[2].Width = -2;

			// get the files
			PopulateFiles();
		}

		/// <summary>
		/// Hande the paint to hide the default OK button that WM creates
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			//WinAPI.HideDoneButton(this.Handle);
			WinAPI.HideXButton(this.Handle);
			base.OnPaint(e);
		}

		/// <summary>
		/// Select an item from the file list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fileListiew_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (fileListiew.SelectedIndices.Count > 0 && fileListiew.Items[fileListiew.SelectedIndices[0]] != null)
			{
				SelectFile(fileListiew.Items[fileListiew.SelectedIndices[0]].Tag);
			}
		}

		/// <summary>
		/// Click the Up menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpMenuItem_Click(object sender, EventArgs e)
		{
			SelectFile(CurrentDirectory.Parent != null ? CurrentDirectory.Parent : CurrentDirectory);
		}

		/// <summary>
		/// Click the Cancel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelMenuItem_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		/// <summary>
		/// Select a differnt Directory
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dirDropDown_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (IsLoadingFiles == true)
			{
				return;
			}

			object item = dirDropDown.SelectedItem;
			if (item != null)
			{
				if (item is DirectoryInfo)
				{
					SelectFile(item);
				}
				else if (item is string)
				{
					SelectFile(null);
				}
			}
		}
	}
}