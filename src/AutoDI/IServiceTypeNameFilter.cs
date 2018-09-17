using System;

namespace AutoDI
{
	public interface IServiceTypeNameFilter
	{
		bool IsMatch( Type serviceType, Type implementationType );
	}
}