using System;

namespace NHibernate.Validator.Tests.Integration
{
	public class FromNhibMetadata
	{
		public virtual int Id { get; set; }

		public virtual DateTime?  DateNotNull { get; set; }
		
		public virtual string StrValue { get; set; }
	
		public virtual decimal Dec { get; set; }
	
		public virtual En1 EnumV { get; set; }
	}

	public enum En1
	{
		v1 = 0,
		v2 = 1
	}
}
