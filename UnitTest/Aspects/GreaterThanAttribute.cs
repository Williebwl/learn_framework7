using System;
using System.Reflection;

namespace BIFramework.Test.Aspects
{
	public class GreaterThanAttribute : Attribute
{
	private readonly int _value;

	public bool NetAspectAttribute = true;

	public GreaterThanAttribute (int value)
	{
		_value = value;
	}

	public void BeforeMethodForParameter (ParameterInfo parameter, int parameterValue)
	{
		BeforeMethodForParameter (parameter, (long)parameterValue);
	}

	public void BeforeMethodForParameter (ParameterInfo parameter, long parameterValue)
	{
		if (parameterValue <= _value)
			throw new Exception (string.Format ("{0} must be greater than {1}", parameter.Name, _value));
	}
}

}