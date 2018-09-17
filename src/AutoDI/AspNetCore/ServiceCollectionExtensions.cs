using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AutoDI.AspNetCore
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddAutoDI<TAssemblyProvider>( this IServiceCollection serviceCollection, Action<Configuration> configureWireUp )
			where TAssemblyProvider : IAssemblyProvider, new()
		{
			return serviceCollection.AddAutoDI( new TAssemblyProvider(), configureWireUp );
		}

		public static IServiceCollection AddAutoDI( this IServiceCollection serviceCollection, Assembly assembly, Action<Configuration> configureWireUp )
		{
			return serviceCollection.AddAutoDI( new AssemblyProviderAdapter( assembly ), configureWireUp );
		}

		internal static IServiceCollection AddAutoDI( this IServiceCollection serviceCollection, IAssemblyProvider assemblyProvider, Action<Configuration> configureWireUp )
		{
			var typeCache = new TypeCache( assemblyProvider );
			var configuration = new Configuration
			{
				DefaultLifeTime = LifeTime.Transient
			};

			configureWireUp( configuration );

			var serviceCollectionPopulator = new ServiceCollectionPopulator(
				typeCache,
				new ImplementationsFinder( typeCache ),
				CreateNameFilter() );

			serviceCollectionPopulator.Populate( serviceCollection, configuration.DefaultLifeTime, configuration.DoOpenGenericWireups );
			return serviceCollection;

			IServiceTypeNameFilter CreateNameFilter()
			{
				if ( configuration.NamingConventionProviderType != null )
					return (IServiceTypeNameFilter)Activator.CreateInstance( configuration.NamingConventionProviderType );

				return new SameNameServiceTypeNameFilter();
			}
		}
	}
}