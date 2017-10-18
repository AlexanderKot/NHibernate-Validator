using NHibernate.Validator.Engine;
using NHibernate.Validator.Exceptions;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.CustomValidator
{
	[TestFixture]
	public class BadValidatorFixture : BaseValidatorFixture
	{
		public class Foo
		{
			[BadValidator]
			private string SomeField;
		}

		public class Foo2
		{
			[BadValidatorMessageIsNull]
			private string SomeField;
		}


		[Test]
		public void ExceptionMustBeThrown()
		{
			Executing.This(() => new ClassValidator(typeof(Foo))).Should().Throw<HibernateValidatorException>();
		}

		/// <summary>
		/// Expected an HibernateValidatorException instead of a NullReferenceException.
		/// </summary>
		[Test]
		public void MessageNull()
		{
			Executing.This(() => new ClassValidator(typeof(Foo2))).Should().Throw<HibernateValidatorException>();
		}
	}
}
