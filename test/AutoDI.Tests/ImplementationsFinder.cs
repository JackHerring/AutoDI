using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoDI.Tests
{
	public class ImplementationsFinder : IImplementationFinder
	{
		private readonly IAssemblyProvider _assemblyProvider;
		private List<Type> _concretePublicTypes;

		//todo: tidy this up, makes my eyes hurt
		private List<Type> ConcretePublicTypes
		{
			get
			{
				return _concretePublicTypes ?? ( _concretePublicTypes = _assemblyProvider
					       .GetAssemblies()
					       .SelectMany( x => x.GetExportedTypes() )
					       .Where( x => !x.IsAbstract && x.IsPublic ).ToList() );
			}
		}

		public ImplementationsFinder( IAssemblyProvider assemblyProvider )
		{
			_assemblyProvider = assemblyProvider;
		}

		public List<Type> FindImplemenationsOf( Type serviceType )
		{
			return ConcretePublicTypes.Where( IsServiceImplementation ).ToList();

			bool IsServiceImplementation( Type concreteType )
			{
				var interfaces = concreteType.GetInterfaces();

				if ( !interfaces.Any() )
					return false;

				return interfaces.Any( x => x == serviceType ) || interfaces.Any( i => IsGenericTypeImplementation( serviceType, i ) );
			}
		}

		private static bool IsGenericTypeImplementation( Type serviceType, Type @interface )
		{
			return @interface.IsGenericType && serviceType.IsGenericType
			                                && @interface.GetGenericTypeDefinition() == serviceType.GetGenericTypeDefinition();
		}
	}
}