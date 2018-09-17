using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoDI
{
	internal class ImplementationsFinder : IImplementationsFinder
	{
		private readonly ITypeCache _typeCache;

		public ImplementationsFinder( ITypeCache typeCache )
		{
			_typeCache = typeCache;
		}

		public List<Type> FindImplemenationsOf( Type serviceType )
		{
			return _typeCache
				.GetTypes( AutoType.Implementation )
				.Where( x => !x.IsAbstract )
				.Where( IsServiceImplementation ).ToList();
				
			bool IsServiceImplementation( Type concreteType )
			{
				var interfaces = concreteType.GetInterfaces();

				if ( !interfaces.Any() )
					return false;

				return interfaces.Any( x => x == serviceType ) 
				       || interfaces.Any( @interface => IsGenericTypeImplementation( serviceType, @interface ) );
			}
		}

		private static bool IsGenericTypeImplementation( Type serviceType, Type @interface )
		{
			return @interface.IsGenericType && serviceType.IsGenericType
			                                && @interface.GetGenericTypeDefinition() == serviceType.GetGenericTypeDefinition();
		}
	}
}