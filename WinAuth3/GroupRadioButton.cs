using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinAuth
{
	public class GroupRadioButton : RadioButton
	{
		public GroupRadioButton()
			: base()
		{
		}

		public GroupRadioButton(string group)
			: base()
		{
			Group = group;
		}

		public string Group { get; set; }

		protected override void OnCheckedChanged(EventArgs e)
		{
			base.OnCheckedChanged(e);

			string group = this.Group;
			if (string.IsNullOrEmpty(group))
			{
				return;
			}

			bool check = this.Checked;
			Form form = FindParentControl<Form>();
			GroupRadioButton[] radios = FindAllControls<GroupRadioButton>(form);
			foreach (GroupRadioButton grb in radios)
			{
				if (grb != this && check && grb.Group == group && grb.Checked)
				{
					grb.Checked = false;
				}
			}
		}

		private T FindParentControl<T>() where T : Control
		{
			Control parent = this.Parent;
			while (parent != null && !(parent is T))
			{
				parent = parent.Parent;
			}
			return (T)parent;
		}

		private static T[] FindAllControls<T>(Control parent) where T : Control
		{
			List<T> controls = new List<T>();
			foreach (Control c in parent.Controls)
			{
				if (c is T == true)
				{
					controls.Add((T)c);
					if (c.Controls != null && c.Controls.Count != 0)
					{
						controls.AddRange(FindAllControls<T>(c));
					}
				}
			}

			return controls.ToArray();
		}
	}
}
