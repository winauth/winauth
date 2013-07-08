using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Components
{
	[ProvideProperty("ApplyMetroTheme", typeof(Control))]
    public sealed class MetroStyleExtender : Component, IExtenderProvider, IMetroComponent
	{
        #region Interface

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroColorStyle Style
        {
            get { throw new NotSupportedException(); }
            set { }
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroThemeStyle.Default)]
        public MetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || metroTheme != MetroThemeStyle.Default)
                {
                    return metroTheme;
                }

                if (StyleManager != null && metroTheme == MetroThemeStyle.Default)
                {
                    return StyleManager.Theme;
                }
                if (StyleManager == null && metroTheme == MetroThemeStyle.Default)
                {
                    return MetroDefaults.Theme;
                }

                return metroTheme;
            }
            set 
            { 
                metroTheme = value;
                UpdateTheme();
            }
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set 
            { 
                metroStyleManager = value;
                UpdateTheme();
            }
        }

        #endregion

        #region Fields

        private readonly List<Control> extendedControls = new List<Control>();

        #endregion

        #region Constructor

        public MetroStyleExtender()
        {
        
        }

        public MetroStyleExtender(IContainer parent)
            : this()
        {
            if (parent != null)
            {
                parent.Add(this);
            }
        }

        #endregion

        #region Management Methods

        private void UpdateTheme()
        {
            Color backColor = MetroPaint.BackColor.Form(Theme);
            Color foreColor = MetroPaint.ForeColor.Label.Normal(Theme);

            foreach (Control ctrl in extendedControls)
            {
                if (ctrl != null)
                {
                    try
                    {
                        ctrl.BackColor = backColor;
                    }
                    catch { }

                    try
                    {
                        ctrl.ForeColor = foreColor;
                    }
                    catch { }
                }
            }
        }

        #endregion

        #region IExtenderProvider

        bool IExtenderProvider.CanExtend(object target)
		{
		    return target is Control && !(target is IMetroControl || target is IMetroForm);
		}

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [Description("Apply Metro Theme BackColor and ForeColor.")]
        public bool GetApplyMetroTheme(Control control)
		{
		    return control != null && extendedControls.Contains(control);
		}

        public void SetApplyMetroTheme(Control control, bool value)
        {
            if (control == null)
            {
                return;
            }

            if (extendedControls.Contains(control))
            {
                if (!value)
                {
                    extendedControls.Remove(control);
                }
            }
            else
            {
                if (value)
                {
                    extendedControls.Add(control);
                }
            }
        }

        #endregion
    }
}
