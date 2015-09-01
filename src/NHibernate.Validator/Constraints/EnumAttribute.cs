using System;
using System.Collections;

using NHibernate.Mapping;
using NHibernate.Validator.Engine;

namespace NHibernate.Validator.Constraints
{
    /// <summary>
    /// Value assigned to enum property must be a valid enum value
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EnumAttribute : EmbeddedRuleArgsAttribute, IRuleArgs, IValidator, IPropertyConstraint
    {
        private string message = "{validator.enum}";

        public EnumAttribute()
        {
        }

        #region IRuleArgs Members

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        #endregion

        #region IValidator Members

        public bool IsValid(object value, IConstraintValidatorContext validatorContext)
        {
            if (value == null) return true;

            return Enum.IsDefined(value.GetType(), value);
        }

        #endregion

        #region IPropertyConstraint Members

        public void Apply(Property property)
        {
            IEnumerator ie = property.ColumnIterator.GetEnumerator();
            ie.MoveNext();
            var col = (Column)ie.Current;

            var svals = String.Empty;  
            foreach(var v in Enum.GetValues(property.Type.ReturnedClass))
            {
                if (svals.Length > 0) svals = svals + ", ";
                svals = svals + ((long)Convert.ChangeType(v, typeof(long)));
            }

            col.CheckConstraint = col.Name + " in (" + svals + ")";
        }

        #endregion
    }
}
