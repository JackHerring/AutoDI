using System;

namespace AutoDI
{
	internal class SameNameServiceTypeNameFilter : IServiceTypeNameFilter
	{
		public bool IsMatch( Type serviceType, Type implementationType )
		{
			var serviceTypeName = serviceType.Name.ToLower();
			return serviceTypeName.StartsWith( "i" )
			       && serviceTypeName.TrimStart( 'i' ) == implementationType.Name.ToLower();
		}
	}
}