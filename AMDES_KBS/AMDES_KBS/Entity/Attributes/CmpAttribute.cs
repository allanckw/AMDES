using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMDES_KBS.Entity
{
    public class CmpAttribute
    {
        private string key;
        private AttributeCmpType type;

        private int value;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public AttributeCmpType CompareType
        {
            get { return type; }
            set { type = value; }
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public CmpAttribute()
        {
        }

        public CmpAttribute(string key, AttributeCmpType type, int value)
        {
            this.key = key;
            this.type = type;
            this.value = value;
        }

        public CmpAttribute(string key, int type, int value)
        {
            this.key = key;
            if (typeof(AttributeCmpType).IsEnumDefined(type))
            {
                this.type = (AttributeCmpType)type;
            }
            else
            {
                throw new InvalidOperationException("Invalid type!");
            }
            this.value = value;
        }

        public AttributeCmpType getAttributeTypeENUM()
        {
            return type;
        }

        public int getCompareType()
        {
            return (int)type;
        }

        public void setAttributeType(int x)
        {
            if (typeof(AttributeCmpType).IsEnumDefined(x))
            {
                type = (AttributeCmpType)x;
            }
            else
            {
                throw new InvalidOperationException("Invalid Type!");
            }
        }
    }
}
