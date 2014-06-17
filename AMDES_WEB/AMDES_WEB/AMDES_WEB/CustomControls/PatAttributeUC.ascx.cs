using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMDES_KBS.Entity;

namespace AMDES_WEB.CustomControls
{
    public partial class PatAttributeUC : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //private PatAttribute p;
        //public PatAttribute Attribute
        //{
        //    get { return this.p; }
        //    set { this.p = value; }
        //}

        public bool IsEmailField
        {
            get { return this.regEmail.Enabled; }
            set { this.regEmail.Enabled = value; }
        }

        public bool isNumeric
        {
            get
            {
                return this.regRange.Enabled;
            }
            set
            {
                this.regRange.Enabled = value;
                this.ddlSelections.Visible = !value;
                this.txtFieldResult.Visible = value;
            }
        }

        public string MinValue
        {
            get
            {
                if (!isNumeric)
                {
                    return "-1";
                }
                else return regRange.MinimumValue;
            }
            set
            {
                regRange.MinimumValue = value;
            }
        }

        public string MaxValue
        {
            get
            {
                if (!isNumeric)
                {
                    return "-1";
                }
                else return regRange.MaximumValue;
            }
            set
            {
                regRange.MaximumValue = value;
            }
        }

        public List<string> Selections
        {
            get { return (List<string>)ddlSelections.DataSource; }
            set
            {
                ddlSelections.DataSource = value;
                ddlSelections.DataBind();
            }
        }

        private int SelectedIndex
        {
            get
            {
                if (isNumeric)
                    return -1;
                else
                    return ddlSelections.SelectedIndex;
            }
        }

        public bool IsRequired
        {
            get { return this.reqValidator.Enabled; }
            set
            {

                if (!isNumeric)
                {
                    this.reqValidator.Enabled = false;
                }
                else
                {
                    this.reqValidator.Enabled = value;
                }
            }
        }

        public string FieldLabelString
        {
            get { return this.lblFieldName.Text.Trim(); }
            set { this.lblFieldName.Text = value; }
        }

        private string TextBoxValue
        {
            get { return this.txtFieldResult.Text.Trim(); }
            set { this.txtFieldResult.Text = value; }

        }

        public double Value
        {
            get {
                if (isNumeric)
                {
                    return double.Parse(this.TextBoxValue);
                }
                else
                {
                    return this.SelectedIndex;
                }
            }

        }

        public string HelpText
        {
            get
            {
                return this.txtFieldResult.ToolTip;
            }
            set
            {
                if (value != null)
                {
                    if (value.Length > 0)
                    {
                        this.txtFieldResult.ToolTip = value;
                        ddlSelections.ToolTip = value;
                    }
                }

            }
        }
    }
}