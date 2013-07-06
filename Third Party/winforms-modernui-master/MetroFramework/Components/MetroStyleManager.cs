/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

using MetroFramework.Interfaces;

namespace MetroFramework.Components
{
    [Designer("MetroFramework.Design.Components.MetroStyleManagerDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    public sealed class MetroStyleManager : Component, ICloneable, ISupportInitialize
    {
        #region Fields

        private readonly IContainer parentContainer;

        private MetroColorStyle metroStyle = MetroDefaults.Style;
        [DefaultValue(MetroDefaults.Style)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroColorStyle Style
        {
            get { return metroStyle; }
            set
            {
                if (value == MetroColorStyle.Default)
                {
                    value = MetroDefaults.Style;
                }

                metroStyle = value;

                if (!isInitializing)
                {
                    Update();
                }
            }
        }

        private MetroThemeStyle metroTheme = MetroDefaults.Theme;
        [DefaultValue(MetroDefaults.Theme)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroThemeStyle Theme
        {
            get { return metroTheme; }
            set
            {
                if (value == MetroThemeStyle.Default)
                {
                    value = MetroDefaults.Theme;
                }

                metroTheme = value;

                if (!isInitializing)
                {
                    Update();
                }
            }
        }

        private ContainerControl owner;
        public ContainerControl Owner
        {
            get { return owner; }
            set
            {
                if (owner != null)
                {
                    owner.ControlAdded -= ControlAdded;
                }

                owner = value;

                if (value != null)
                {
                    owner.ControlAdded += ControlAdded;

                    if (!isInitializing)
                    {
                        UpdateControl(value);
                    }
                }
            }
        }

        #endregion

        #region Constructor

        public MetroStyleManager()
        {
        
        }

        public MetroStyleManager(IContainer parentContainer)
            : this()
        {
            if (parentContainer != null)
            {
                this.parentContainer = parentContainer;
                this.parentContainer.Add(this);
            }
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            MetroStyleManager newStyleManager = new MetroStyleManager();
            newStyleManager.metroTheme = Theme;
            newStyleManager.metroStyle = Style;
            return newStyleManager;
        }

        public object Clone(ContainerControl owner)
        {
            MetroStyleManager clonedManager = Clone() as MetroStyleManager;

            if (owner is IMetroForm)
            {
                clonedManager.Owner = owner;
            }

            return clonedManager;
        }

        #endregion

        #region ISupportInitialize

        private bool isInitializing;

        void ISupportInitialize.BeginInit()
        {
            isInitializing = true;
        }

        void ISupportInitialize.EndInit()
        {
            isInitializing = false;
            Update();
        }

        #endregion

        #region Management Methods

        private void ControlAdded(object sender, ControlEventArgs e)
        {
            if (!isInitializing)
            {
                UpdateControl(e.Control);
            }
        }

        public void Update()
        {
            if (owner != null)
            {
                UpdateControl(owner);
            }

            if (parentContainer == null || parentContainer.Components == null)
            {
                return;
            }

            foreach (Object obj in parentContainer.Components)
            {
                if (obj is IMetroComponent)
                {
                    ApplyTheme((IMetroComponent)obj);
                }
            }
        }

        private void UpdateControl(Control ctrl)
        {
            if (ctrl == null)
            {
                return;
            }

            IMetroControl metroControl = ctrl as IMetroControl;
            if (metroControl != null)
            {
                ApplyTheme(metroControl);
            }

            IMetroComponent metroComponent = ctrl as IMetroComponent;
            if (metroComponent != null)
            {
                ApplyTheme(metroComponent);
            }

            TabControl tabControl = ctrl as TabControl;
            if (tabControl != null)
            {
                foreach (TabPage tp in ((TabControl)ctrl).TabPages)
                {
                    UpdateControl(tp);
                }
            }

            if (ctrl.Controls != null)
            {
                foreach (Control child in ctrl.Controls)
                {
                    UpdateControl(child);
                }
            }

            if (ctrl.ContextMenuStrip != null)
            {
                UpdateControl(ctrl.ContextMenuStrip);
            }

            ctrl.Refresh();
        }

        private void ApplyTheme(IMetroControl control)
        {
            control.StyleManager = this;
        }

        private void ApplyTheme(IMetroComponent component)
        {
            component.StyleManager = this;
        }

        #endregion
    }
}
