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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Interfaces;

namespace MetroFramework.Design.Components
{
    internal class MetroStyleManagerDesigner : ComponentDesigner
    {
        DesignerVerbCollection designerVerbs;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (designerVerbs != null)
                {
                    return designerVerbs;
                }

                designerVerbs = new DesignerVerbCollection();
                designerVerbs.Add(new DesignerVerb("Reset Styles to Default", OnResetStyles));

                return designerVerbs;
            }
        }

        private IDesignerHost designerHost;
        public IDesignerHost DesignerHost
        {
            get
            {
                if (designerHost != null)
                {
                    return designerHost;
                }

                designerHost = (IDesignerHost)(GetService(typeof(IDesignerHost)));

                return designerHost;
            }
        }

        private IComponentChangeService componentChangeService;
        public IComponentChangeService ComponentChangeService
        {
            get
            {
                if (componentChangeService != null)
                {
                    return componentChangeService;
                }

                componentChangeService = (IComponentChangeService)(GetService(typeof(IComponentChangeService)));
                
                return componentChangeService;
            }
        }

        private void OnResetStyles(object sender, EventArgs args)
        {
            MetroStyleManager styleManager = Component as MetroStyleManager;
            if (styleManager != null)
            {
                if (styleManager.Owner == null)
                {
                    MessageBox.Show("StyleManager needs the Owner property assigned to before it can reset styles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            ResetStyles(styleManager, styleManager.Owner as Control);
        }

        private void ResetStyles(MetroStyleManager styleManager, Control control)
        {
            IMetroForm container = control as IMetroForm;
            if (container != null && !ReferenceEquals(styleManager, container.StyleManager))
            {
                return;
            }

            if (control is IMetroControl)
            {
                ResetProperty(control, "Style", MetroColorStyle.Default);
                ResetProperty(control, "Theme", MetroThemeStyle.Default);
            }
            else if (control is IMetroComponent)
            {
                ResetProperty(control, "Style", MetroColorStyle.Default);
                ResetProperty(control, "Theme", MetroThemeStyle.Default);
            }

            if (control.ContextMenuStrip != null)
            {
                ResetStyles(styleManager, control.ContextMenuStrip);
            }

            TabControl tabControl = control as TabControl;
            if (tabControl != null)
            {
                foreach (TabPage tp in tabControl.TabPages)
                {
                    ResetStyles(styleManager, tp);
                }
            }

            if (control.Controls != null)
            {
                foreach (Control child in control.Controls)
                {
                    ResetStyles(styleManager, child);
                }
            }
        }

        private void ResetProperty(Control control, string name, object newValue)
        {
            var typeDescriptor = TypeDescriptor.GetProperties(control)[name];
            if (typeDescriptor == null)
            {
                return;
            }

            object oldValue = typeDescriptor.GetValue(control);

            if (newValue.Equals(oldValue))
            {
                return;
            }

            ComponentChangeService.OnComponentChanging(control, typeDescriptor);
            typeDescriptor.SetValue(control, newValue);
            ComponentChangeService.OnComponentChanged(control, typeDescriptor, oldValue, newValue);
        }
    }
}
