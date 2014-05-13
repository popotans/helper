using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Reflection;

namespace Helper
{
    public class WebFormBinder
    {
       // public static readonly WebFormBinder Current = new WebFormBinder();

        private IPropertyCore core;

        public WebFormBinder()
        {
            core = new DefaultPropertyCore();
        }

        public WebFormBinder(IPropertyCore proertyhelper)
        {
            core = proertyhelper;
        }

        public void BindModel(object t, Page page)
        {
            foreach (PropertyInfo pi in t.GetType().GetProperties())
            {
                if (pi.CanWrite)
                {
                    Control ctrl = page.FindControl(pi.Name);
                    if (ctrl != null)
                    {
                        object ctrlVal = GetControl(ctrl);
                        try
                        {
                            core.SetPropertyValue(t, pi, ctrlVal);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        public void BindControl(object t, Page page)
        {
            foreach (PropertyInfo pi in t.GetType().GetProperties())
            {
                if (pi.CanRead)
                {
                    Control ctrl = page.FindControl(pi.Name);
                    if (ctrl != null)
                    {
                        object value = core.GetPropertyValue(t, pi);
                        SetControl(ctrl, value);
                    }
                }
            }
        }

        private object GetControl(Control ctrl)
        {
            object t = null;
            if (ctrl is ITextControl || ctrl is Label || ctrl is Literal)
            {
                t = (ctrl as ITextControl).Text;
            }
            else if (ctrl is IButtonControl)
            {
                t = (ctrl as IButtonControl).Text;
            }
            else if (ctrl is HiddenField)
            {
                t = (ctrl as HiddenField).Value;
            }
            else if (ctrl is DropDownList)
            {
                t = (ctrl as DropDownList).SelectedValue;
            }
            return t;
        }

        private void SetControl(Control ctrl, object val)
        {
            if (val == null) return;
            if (ctrl is ITextControl || ctrl is Label || ctrl is Literal)
            {
                (ctrl as ITextControl).Text = val.ToString();
            }
            else if (ctrl is IButtonControl)
            {
                (ctrl as IButtonControl).Text = val.ToString();
            }
            else if (ctrl is HiddenField)
            {
                (ctrl as HiddenField).Value = val.ToString();
            }
            else if (ctrl is ListControl)
            {
                (ctrl as ListControl).SelectedValue = val.ToString();
            }
        }
    }
}