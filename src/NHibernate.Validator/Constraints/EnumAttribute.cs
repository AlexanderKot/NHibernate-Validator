using System;
using System.Collections;
using System.Linq;
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

		//Next method is taken there
		//http://www.codeproject.com/Tips/194804/See-if-a-Flags-enum-is-valid
		public static bool IsFlagsValid(System.Type enumType, long value)
		{
			if (enumType.IsEnum && enumType.IsDefined(typeof(FlagsAttribute), false))
			{
				long compositeValues = Enum.GetValues(enumType)
										.Cast<object>()
										.Aggregate<object, long>(0, (current, flag) => current | Convert.ToInt64(flag));

				return ((~compositeValues & value) == 0);
			}
			else
			{
				return false;
			}
		}

		public bool IsValid(object value, IConstraintValidatorContext validatorContext)
		{
			if (value == null) return true;

			var tp = value.GetType();

			return tp.GetCustomAttributes(typeof (FlagsAttribute), false).Length > 0 
				? IsFlagsValid(tp, ((long)Convert.ChangeType(value, typeof(long)))) 
				: Enum.IsDefined(tp, value);
		}

		#endregion

		#region IPropertyConstraint Members

		public void Apply(Property property)
		{
			IEnumerator ie = property.ColumnIterator.GetEnumerator();
			ie.MoveNext();
			var col = (Column)ie.Current;


			if (property.Type.ReturnedClass.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0) return;

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
