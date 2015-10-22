using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;
using AMDES_WEB.CustomControls;
using System.IO;

namespace AMDES_WEB
{
    public partial class patienttStart : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnStart.UniqueID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["dicSectionPage"] = null;
            Session["Result"] = null;
            CLIPSWebController clp = new CLIPSWebController();
            clp.ApplicationContext = WebApplicationContextController.setApplicationContext(Request.QueryString["appID"]);
            Session["clp"] = clp;

            if (Session["dob"] != null)
                dpFrom.Date = (DateTime)Session["dob"];

            AddRegistrationField();
        }

        public Control GetPostBackControl(Page page)
        {
            Control control = null;

            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if ((ctrlname != null) & ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        private void AddRegistrationField()
        {
            //Determine which control fired the postback event. 
            Control c = GetPostBackControl(Page);

            //Be sure everything in the placeholder control is cleared out
            this.phRegister.Controls.Clear();

            int ControlID = 0;

            string appID = Request.QueryString["appID"];

            lblEventName.InnerText = App.readAppTitle(appID);

            string dir = @"Data/" + appID;


            if (appID != null && appID.Length > 0 && Directory.Exists(Server.MapPath(dir)))
            {
                Literal l1 = new Literal();
                l1.Mode = LiteralMode.PassThrough;
                l1.Text = "<table>";
                this.phRegister.Controls.Add(l1);
                ControlID += 1;

                CLIPSWebController clp = (CLIPSWebController)Session["clp"];

                List<PatAttribute> attrList = PatAttributeController.getAllAttributes(clp.ApplicationContext);

                for (int i = 0; i < attrList.Count; i++)
                {
                    PatAttribute attr = attrList[i];
                    PatAttributeUC regField = (PatAttributeUC)LoadControl(@"~/CustomControls\PatAttributeUC.ascx");
                    regField.IsEmailField = false;

                    regField.FieldLabelString = attr.AttributeName;

                    switch (attr.AttrType)
                    {
                        case PatAttribute.AttributeType.CATEGORICAL:
                            regField.isNumeric = false;
                            regField.IsRequired = false;
                            regField.Selections = attr.CategoricalVals;


                            break;
                        case PatAttribute.AttributeType.NUMERIC:
                            regField.isNumeric = true;
                            regField.MinValue = attr.MinNumericValue.ToString();
                            regField.MaxValue = attr.MaxNumericValue.ToString();
                            break;
                    }


                    this.phRegister.Controls.Add(regField);
                    ControlID += 1;

                }

                Literal l2 = new Literal();
                l2.Mode = LiteralMode.PassThrough;
                l2.Text = "</table>";
                this.phRegister.Controls.Add(l2);
                ControlID += 1;


            }
            else
            {
                Response.Redirect("~/default.aspx");
            }

        }


        protected void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                //ccJoin.ValidateCaptcha(txtCaptcha.Text.Trim());
                //if (ccJoin.UserValidated) turn off for debug purpose
                if (true)
                {

                    Patient p = new Patient();

                    p.AssessmentDate = DateTime.Today;
                    p.DOB = dpFrom.Date;
                    p.NRIC = "ANON_" + DateTime.Now.Ticks.ToString(); //Unique Identfier based on time
                    p.Doctor = new Assessor("Doctor", "Clinic");

                    foreach (Control c in this.phRegister.Controls)
                    {
                        if (c is PatAttributeUC)
                        {
                            PatAttributeUC regField = ((PatAttributeUC)c);
                            if (regField.isNumeric)
                            {
                                p.createAttribute(regField.FieldLabelString, regField.Value);
                            }
                            else
                            {
                                p.createAttribute(regField.FieldLabelString, regField.Value);
                            }
                        }

                    }

                    CLIPSWebController clp = (CLIPSWebController)Session["clp"];
                    clp.CurrentPatient = p;
                    clp.clearAndLoadNew();

                    Session["clp"] = clp;
                    Session["dob"] = dpFrom.Date;

                    txtCaptcha.Text = "";
                    Response.Redirect("~/Questionnaire.aspx");

                }
                else
                {
                    Alert.Show("Invalid Captcha, please try again!");
                    txtCaptcha.Text = "";
                    txtCaptcha.Focus();
                }
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}