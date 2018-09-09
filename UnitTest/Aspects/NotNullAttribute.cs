using System;
using System.Reflection;

namespace BIFramework.Test.Aspects
{
	public class NotNullAttribute : Attribute
{
	public bool NetAspectAttribute = true;

	public void BeforeMethodForParameter (ParameterInfo parameter, object parameterValue)
	{
		if (parameterValue == null)
			throw new ArgumentNullException (parameter.Name);
	}
}

}