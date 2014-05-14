using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Helper.Web
{
    public class AscxContainer : System.Web.UI.Control, System.Web.UI.INamingContainer
    {
        private string _virtualPath;
        private Control _ascx;

        public string VirtualPath
        {
            get { return this._virtualPath; }
            set
            {
                string oldValue = this._virtualPath;
                this._virtualPath = value;
                if (this.Page != null && value != oldValue)
                {
                    this.ChildControlsCreated = false;
                    this.EnsureChildControls();
                }
            }
        }
        public Control Ascx
        {
            get { return this._ascx; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!string.IsNullOrEmpty(this.VirtualPath))
            {
                this.EnsureChildControls();
            }
        }

        protected override void LoadViewState(object savedState)
        {
            Pair pair = savedState as Pair;
            if (pair != null)
            {
                base.LoadViewState(pair.First);
                this.VirtualPath = pair.Second as string;
            }
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            if (string.IsNullOrEmpty(this.VirtualPath))
            {
                return;
            }

            this._ascx = this.Page.LoadControl(this.VirtualPath);
            if (this._ascx == null)
            {
                throw new Exception("ViewVirtualPath cannot be loaded.");
            }

            this._ascx.ID = "ascx";
            this.ClearChildState();
            this.Controls.Add(this._ascx);
        }

        protected override object SaveViewState()
        {
            return new Pair(base.SaveViewState(), this.VirtualPath);
        }
    }
}
