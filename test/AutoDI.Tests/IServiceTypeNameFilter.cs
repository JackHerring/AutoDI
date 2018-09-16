using System;

namespace AutoDI.Tests
{
	public interface IServiceTypeNameFilter
	{
		bool IsMatch( Type serviceType, Type implementationType );
	}
}