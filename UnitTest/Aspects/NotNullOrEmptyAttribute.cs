using System;
using System.Reflection;

namespace BIFramework.Test.Aspects
{
	public class NotNullOrEmptyAttribute : Attribute
{
	public bool NetAspectAttribute = true;

	public void BeforeMethodForParameter (ParameterInfo parameter, string parameterValue)
	{
		if (string.IsNullOrEmpty (parameterValue))
			throw new ArgumentNullException (parameter.Name);
	}
}

}