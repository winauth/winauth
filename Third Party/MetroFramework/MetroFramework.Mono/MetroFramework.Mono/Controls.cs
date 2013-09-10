using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MetroFramework
{
	public enum MetroLabelSize
	{
		Small
	}

	public enum MetroCheckBoxSize
	{
		Medium
	}

	public enum MetroLabelWeight
	{
		Regular
	}

	public class AssemblyRef
	{
		public const string MetroFrameworkDesignSN = "";
	}
}

namespace MetroFramework.Components
{
	public class MetroStyleManager : Control
	{
		public MetroStyleManager(IContainer components)
		{
			Components = components;
		}

		public IContainer Components { get; set; }

		public Form Owner { get; set; }
	}
	public class MetroStyleExtender : Control
	{
		public MetroStyleExtender(IContainer components)
		{
			Components = components;
		}

		public IContainer Components { get; set; }
	}
}
namespace MetroFramework.Drawing
{
}
namespace MetroFramework.Drawing.Html
{
	public class HtmlToolTip : ToolTip
	{
	}
	public class HtmlLabel : Label
	{
		public Size AutoScrollMinSize { get; set; }
		public bool AutoScroll { get; set; }
	}
}

	namespace MetroFramework.Interfaces
{
}
namespace MetroFramework.Controls
{
	public interface IMetroControl
	{
	}
	public class MetroLabel : Label
	{
		public MetroLabelSize FontSize { get; set; }
		public bool UseCustomForeColor { get; set; }
	}
	public class MetroButton : Button
	{
		public bool UseSelectable { get; set; }
	}
	public class MetroCheckBox : CheckBox
	{
		public MetroCheckBoxSize FontSize { get; set; }
		public bool UseSelectable { get; set; }
	}
	public class MetroRadioButton : RadioButton
	{
		public bool UseSelectable { get; set; }
	}
	public class MetroToggle : CheckBox
	{
		public bool UseSelectable { get; set; }
	}
	public class MetroTextBox : TextBox
	{
		public bool UseSelectable { get; set; }
		public string PromptText { get; set; }
	}
	public class MetroComboBox : ComboBox
	{
		public bool UseSelectable { get; set; }
	}
	public class MetroTabControl : TabControl
	{
		public bool UseSelectable { get; set; }
	}
	public class MetroTabPage : TabPage
	{
		public bool HorizontalScrollbarBarColor { get; set; }
		public bool HorizontalScrollbarHighlightOnWheel { get; set; }
		public int HorizontalScrollbarSize { get; set; }

		public int VerticalScrollbarSize { get; set; }
		public bool VerticalScrollbarHighlightOnWheel { get; set; }
		public bool VerticalScrollbarBarColor { get; set; }
	}
	public class MetroPanel : Panel
	{
		public bool HorizontalScrollbarBarColor { get; set; }
		public bool HorizontalScrollbarHighlightOnWheel { get; set; }
		public int HorizontalScrollbarSize { get; set; }

		public int VerticalScrollbarSize { get; set; }
		public bool VerticalScrollbarHighlightOnWheel { get; set; }
		public bool VerticalScrollbarBarColor { get; set; }
	}
	public class MetroLink : Label
	{
		public bool UseSelectable { get; set; }
	}
}
namespace MetroFramework.Forms
{
	public enum MetroFormBorderStyle
	{
		None,
		FixedSingle
	}

	public class MetroForm : Form
	{
		public MetroFormBorderStyle BorderStyle { get; set; }

		public bool Resizable { get; set; }

		public MetroFramework.Components.MetroStyleManager StyleManager { get; set; }
	}
}

namespace MetroFramework
{
	public class MetroFonts
	{
		public static Font Label(MetroLabelSize size, MetroLabelWeight weight)
		{
			return System.Drawing.SystemFonts.DefaultFont;
		}
	}
}
