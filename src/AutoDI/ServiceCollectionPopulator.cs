using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace AutoDI
{
	internal class ServiceCollectionPopulator : IServiceCollectionPopulator
	{
		private readonly ITypeCache _typeCache;
		private readonly IImplementationsFinder _implementationsFinder;
		private readonly IServiceTypeNameFilter _serviceTypeNameFilter;

		public ServiceCollectionPopulator( ITypeCache typeCache, IImplementationsFinder implementationsFinder, IServiceTypeNameFilter serviceTypeNameFilter )
		{
			_typeCache = typeCache;
			_implementationsFinder = implementationsFinder;
			_serviceTypeNameFilter = serviceTypeNameFilter;
		}

		public void Populate( IServiceCollection serviceCollection, LifeTime lifeTime, bool includeOpenGenericWireup )
		{
			foreach ( var abstraction in _typeCache.GetTypes( AutoType.Abstraction ) )
			{
				var types = _implementationsFinder.FindImplemenationsOf( abstraction );
				if ( !includeOpenGenericWireup && types.All( x => x.IsGenericType ) )
					return;

				var dependencyType = types.FirstOrDefault( type => _serviceTypeNameFilter.IsMatch( abstraction, type ) );

				switch ( lifeTime )
				{
					case LifeTime.Singleton:
						RegisterDependency( serviceCollection.AddSingleton, abstraction, dependencyType );
						break;
					case LifeTime.Transient:
						RegisterDependency( serviceCollection.AddTransient, abstraction, dependencyType );
						break;
				}
			}
		}

		private static void RegisterDependency( Func<Type, Type, IServiceCollection> addDependency, Type service, Type implementation )
		{
			if ( service.IsGenericType && !implementation.IsGenericType )
			{
				var @interface = implementation
					                 .GetInterfaces()
					                 .FirstOrDefault( x => x.Name == service.Name ) ?? service;

				addDependency( @interface, implementation );
			}
			else
				addDependency( service, implementation );
		}
	}
}