using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Helper
{
    public static class DelegateHelper
    {
        static DelegateHelper()
        {

        }

        /// <summary>
        /// 委托-设置
        /// </summary>
        /// <param name="c"></param>
        /// <param name="s"></param>
        public delegate void DSet(Control c, string s);
        /// <summary>
        /// 委托 取值
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public delegate object DGet(Control c);

        public static void SetVal(Control c, string s)
        {
            if (c is Label)
            {
                ((Label)c).Text = s;
            }
            else if (c is TextBox)
            {
                ((TextBox)c).Text = s;
            }

            else if (c is ListBox)
            {
                (c as ListBox).SelectedValue = s;
            }
            else if (c is Button)
            {
                (c as Button).Text = s;
            }
        }

        public static object GetVal(Control c)
        {
            object s = string.Empty;

            if (c is Label)
            {
                s = ((Label)c).Text;
            }
            else if (c is TextBox)
            {
                s = ((TextBox)c).Text;
            }
            else if (c is Button)
            {
                s = (c as Button).Text;
            }
            else if (c is ComboBox)
            {
                s = ((ComboBox)c).SelectedValue;
            }
            else if (c is ListBox)
            {
                s = ((ListBox)c).SelectedValue;
            }
            else if (c is RichTextBox)
            {
                s = (c as RichTextBox).Text;
            }

            return s;
        }
    }
}
