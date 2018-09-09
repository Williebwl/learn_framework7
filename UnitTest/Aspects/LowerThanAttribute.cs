using System;
using System.Reflection;

namespace BIFramework.Test.Aspects
{
	public class LowerThanAttribute : Attribute
{
	private readonly int _value;

	public bool NetAspectAttribute = true;

	public LowerThanAttribute (int value)
	{
		_value = value;
	}

	public void BeforeMethodForParameter (ParameterInfo parameter, int parameterValue)
	{
		BeforeMethodForParameter (parameter, (long)parameterValue);
	}

	public void BeforeMethodForParameter (ParameterInfo parameter, long parameterValue)
	{
		if (parameterValue >= _value)
			throw new Exception (string.Format ("{0} must be lower than {1}", parameter.Name, _value));
	}
}

}