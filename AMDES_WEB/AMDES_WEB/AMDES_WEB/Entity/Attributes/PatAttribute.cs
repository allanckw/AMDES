using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AMDES_KBS.Entity
{
    class PatAttribute : IEquatable<PatAttribute>, IComparable<PatAttribute>
    {
        public static string dataPath = System.Web.HttpContext.Current.Server.MapPath(@"Data\Add\PatAttributes.xml");

        public enum AttributeType
        {
            NUMERIC,
            CATEGORICAL
        }
        private AttributeType attrType;
        private string attributeName;
        private List<string> categoricalVals;

        private int minNumericValue;


        private int maxNumericValue;

        public PatAttribute(AttributeType t, string name)
        {
            this.attributeName = name.ToUpper();
            this.attrType = t;
            categoricalVals = new List<string>();
        }

        public PatAttribute()
        {
            categoricalVals = new List<string>();
        }


        public AttributeType AttrType
        {
            get { return attrType; }
            set { attrType = value; }
        }


        public string AttributeName
        {
            get { return attributeName; }
            set { attributeName = value.ToUpper(); }
        }

        public List<string> CategoricalVals
        {
            get
            {
                if (this.AttrType == AttributeType.CATEGORICAL)
                    return this.categoricalVals;
                else
                    return new List<string>();
            }
            set
            {
                if (this.AttrType == AttributeType.NUMERIC)
                    categoricalVals = new List<string>();
                else
                    this.categoricalVals = value;
            }
        }

        public string getCategoryByID(int i)
        {
            if (this.attrType == AttributeType.CATEGORICAL)
            {
                if (i > 0 && categoricalVals.Count > 0 && i < categoricalVals.Count)
                {
                    return categoricalVals[i];
                }
                else
                {
                    return null;
                }
            }
            else
                return null;

        }

        public void addCategoricalValue(string v)
        {
            if (this.AttrType == AttributeType.NUMERIC)
                throw new InvalidOperationException("Cannot set categorical values for numeric type attributes");
            else
            {
                if (!this.categoricalVals.Contains(v))
                    this.categoricalVals.Add(v);
            }
        }

        public void removeCategoricalValue(string v)
        {
            if (this.AttrType == AttributeType.NUMERIC)
                throw new InvalidOperationException("Cannot set categorical values for numeric type attributes");
            else
            {
                if (this.categoricalVals.Contains(v))
                    this.categoricalVals.Remove(v);
            }
        }

        public void clearCategoricalValue()
        {
            this.categoricalVals.Clear();
        }

        public AttributeType getAttributeTypeNUM()
        {
            return attrType;
        }

        public int getAttributeType()
        {
            return (int)attrType;
        }

        public void setAttributeType(int x)
        {
            if (typeof(AttributeType).IsEnumDefined(x))
            {
                attrType = (AttributeType)x;
            }
            else
            {
                throw new InvalidOperationException("Invalid Type!");
            }
        }

        public int MaxNumericValue
        {
            get
            {
                if (this.AttrType == AttributeType.NUMERIC)
                    return maxNumericValue;
                else
                    return -1;
            }
            set
            {
                if (this.AttrType == AttributeType.NUMERIC)
                    maxNumericValue = value;
                else
                    maxNumericValue = -1;
            }
        }

        public int MinNumericValue
        {
            get
            {
                if (this.AttrType == AttributeType.NUMERIC)
                    return minNumericValue;
                else
                    return -1;
            }
            set
            {
                if (this.AttrType == AttributeType.NUMERIC)
                    minNumericValue = value;
                else
                    minNumericValue = -1;
            }
        }

        public bool Equals(PatAttribute pa)
        {
            return this.attributeName.ToUpper().CompareTo(pa.attributeName.ToUpper()) == 0;
        }

        public int CompareTo(PatAttribute pa)
        {
            return this.attributeName.ToUpper().CompareTo(pa.attributeName.ToUpper());
        }
    }
}
