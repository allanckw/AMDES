using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AMDES_KBS.Controllers;
using AMDES_KBS.Entity;
using System.Windows.Input;


namespace AMDES_KBS
{
    /// <summary>
    /// Interaction logic for frmAttributeSetting.xaml
    /// </summary>
    public partial class frmAttributeSetting : Page
    {

        int currIdx = -1;

        public frmAttributeSetting()
        {
            InitializeComponent();
            loadAttrType();
            loadAllAttribute();
        }

        private void loadAttrType()
        {
            cboType.Items.Clear();
            cboType.Items.Add(PatAttribute.AttributeType.NUMERIC.ToString());
            cboType.Items.Add(PatAttribute.AttributeType.CATEGORICAL.ToString());
            cboType.SelectedIndex = 0;
        }

        private void loadAllAttribute()
        {
            List<PatAttribute> lstAttr = PatAttributeController.getAllAttributes();
            lstAttrList.ItemsSource = null;
            lstAttrList.ItemsSource = lstAttr;
        }

        private void clearAttrData()
        {
            txtAttrName.Tag = "new";
            txtAttrName.Text = "";
            txtValue.Text = "";
            cboType.SelectedIndex = 0;
            lvCategoryValue.Items.Clear();
            txtMaxValue.Text = "";
            txtMinValue.Text = "";
        }

        private void loadAttrData(PatAttribute attr)
        {
            clearAttrData();
            txtAttrName.Text = attr.AttributeName;
            txtAttrName.Tag = attr.AttributeName;
            cboType.SelectedIndex = attr.getAttributeType();

            if (attr.getAttributeTypeNUM()==PatAttribute.AttributeType.CATEGORICAL)
            {
                stkpnlCategoryEntry.Visibility = Visibility.Visible;
                stkpnlNumericalEntry.Visibility = Visibility.Collapsed;
                loadAttrCategoryValue(attr);
            }
            else if (attr.getAttributeTypeNUM() == PatAttribute.AttributeType.NUMERIC)
            {
                stkpnlCategoryEntry.Visibility = Visibility.Collapsed;
                stkpnlNumericalEntry.Visibility = Visibility.Visible;
                loadAttrNumericalValue(attr);
            }
            else
            {
            }
        }

        private void loadAttrCategoryValue(PatAttribute attr)
        {
            lvCategoryValue.Items.Clear();
            foreach (string item in attr.CategoricalVals)
            {
                lvCategoryValue.Items.Add(item);
            }
        }

        private void loadAttrNumericalValue(PatAttribute attr)
        {
            txtMinValue.Text = attr.MinNumericValue.ToString();
            txtMaxValue.Text = attr.MaxNumericValue.ToString();
        }

        private void cboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sidx=cboType.SelectedIndex;
            if (sidx == -1)
            {
                return;
            }

            string sCategory = cboType.Items[sidx].ToString();

            if (sCategory.CompareTo(PatAttribute.AttributeType.CATEGORICAL.ToString()) == 0)
            {
                stkpnlCategoryEntry.Visibility = Visibility.Visible;
                stkpnlNumericalEntry.Visibility = Visibility.Collapsed;
            }
            else if (sCategory.CompareTo(PatAttribute.AttributeType.NUMERIC.ToString()) == 0)
            {
                stkpnlCategoryEntry.Visibility = Visibility.Collapsed;
                stkpnlNumericalEntry.Visibility = Visibility.Visible;
            }
            else
            {
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < lvCategoryValue.Items.Count; i++)
            {
                int cValue = txtValue.Text.ToUpper().Trim().CompareTo(lvCategoryValue.Items[i].ToString());
                if (cValue==0)
                {
                    return;
                }
            }
            lvCategoryValue.Items.Add(txtValue.Text.ToUpper().Trim());
            lvCategoryValue.SelectedIndex = lvCategoryValue.Items.Count - 1;
        }

        private void btnDeleteValue_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lvCategoryValue.SelectedIndex;
            if (sidx==-1)
            {
                return;
            }

            lvCategoryValue.Items.RemoveAt(sidx);
        }

        private void btnAddNewAttr_Click(object sender, RoutedEventArgs e)
        {
            lstAttrList.SelectedIndex = -1;
            clearAttrData();
        }

        private void btnDeleteAttr_Click(object sender, RoutedEventArgs e)
        {
            int sidx = lstAttrList.SelectedIndex;
            if (sidx==-1)
            {
                return;
            }
            PatAttribute sAttr=(PatAttribute)lstAttrList.Items[sidx];
            PatAttributeController.deleteAttribute(sAttr.AttributeName);
            loadAllAttribute();
        }

        private void btnSaveAttr_Click(object sender, RoutedEventArgs e)
        {
            PatAttribute attr;
            int sidx = cboType.SelectedIndex;
            if (sidx==-1)
            {
                return;
            }

            if (txtAttrName.Tag.ToString().CompareTo("new") == 0)
            {
                attr = new PatAttribute();
            }
            else
            {
                attr = (PatAttribute)lstAttrList.Items[lstAttrList.SelectedIndex];
            }

            attr.AttributeName = txtAttrName.Text.ToUpper().Trim();
            attr.setAttributeType(sidx);

            if (attr.getAttributeTypeNUM()==PatAttribute.AttributeType.CATEGORICAL)
            {
                foreach (string item in lvCategoryValue.Items)
                {
                    attr.addCategoricalValue(item);
                }
            }
            else if (attr.getAttributeTypeNUM()==PatAttribute.AttributeType.NUMERIC)
            {
                attr.MinNumericValue = int.Parse(txtMinValue.Text);
                attr.MaxNumericValue = int.Parse(txtMaxValue.Text);
            }

            PatAttributeController.updateAttribute(attr);
            loadAllAttribute();
        }

        private void lstAttrList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sidx = lstAttrList.SelectedIndex;
            if (sidx==-1)
            {
                return;
            }

            PatAttribute attr = (PatAttribute)lstAttrList.Items[sidx];
            loadAttrData(attr);
        }

        private void txtMinValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = App.NumericValidationTextBox(e.Text);
        }

        private void txtMaxValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = App.NumericValidationTextBox(e.Text);
        }

    }
}
