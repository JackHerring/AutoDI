using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AutoDI.Tests
{
	public class ServiceCollectionPopulatorTests
	{
		[Theory]
		[InlineData( LifeTime.Transient )]
		[InlineData( LifeTime.Singleton )]
		public void Populate_DoesRegisterTypesForCorrectLifetime( LifeTime lifeTime )
		{
			var serviceProvider = CreateTestServiceProvider<ISingle, Single>( lifeTime );
			var implementation = serviceProvider.GetService<ISingle>();

			Assert.Equal( typeof( Single ), implementation.GetType() );
			var isSameObject = implementation == serviceProvider.GetService<ISingle>();
			Assert.Equal( lifeTime == LifeTime.Singleton, isSameObject );
		}

		[Fact]
		public void Populate_DoesRegisterOpenGenericTypes()
		{
			var dependency = CreateTestServiceProvider( typeof( ISingleGeneric<> ), typeof( SingleOpenGeneric<> ) )
				.GetService<ISingleGeneric<string>>();

			Assert.NotNull( dependency );
			Assert.Equal( dependency.GetType().Name, typeof( SingleOpenGeneric<string> ).Name );
		}

		[Fact]
		public void Populate_DoesRegisterClosedGenericTypes()
		{
			var dependency = CreateTestServiceProvider( typeof( ISingleGeneric<> ), typeof( SingleGeneric ) )
				.GetService<ISingleGeneric<string>>();

			Assert.NotNull( dependency );
			Assert.Equal( typeof( SingleGeneric ), dependency.GetType() );
		}

		[Fact]
		public void Populate_WhenNotConfiguredToRegisterOpenGenericTypes_CannotResolveService()
		{
			var dependency = CreateTestServiceProvider( typeof( ISingleGeneric<> ), typeof( SingleOpenGeneric<> ), LifeTime.Transient, doOpenGenericWireups: false )
				.GetService<ISingleGeneric<string>>();

			Assert.Null( dependency );
		}

		private static IServiceProvider CreateTestServiceProvider<TServiceType, TImplementationType>( LifeTime lifeTime = LifeTime.Transient, bool doOpenGenericWireups = true )
		{
			return CreateTestServiceProvider( typeof( TServiceType ), typeof( TImplementationType ), lifeTime, doOpenGenericWireups );
		}

		private static IServiceProvider CreateTestServiceProvider( Type serviceType, Type implementationType, LifeTime lifeTime = LifeTime.Transient, bool doOpenGenericWireups = true )
		{
			var typeCacheMock = new Mock<ITypeCache>();
			var implementationsFinderMock = new Mock<IImplementationsFinder>();

			SetupTypeCache( AutoType.Implementation, implementationType );
			SetupTypeCache( AutoType.Abstraction, serviceType );

			implementationsFinderMock
				.Setup( x => x.FindImplemenationsOf( It.Is<Type>( t => t == serviceType ) ) )
				.Returns( new List<Type>
				{
					implementationType
				} );

			var services = new ServiceCollection();

			var serviceCollectionPopulator = new ServiceCollectionPopulator(
				typeCacheMock.Object,
				implementationsFinderMock.Object,
				new SuperLiberalTypeNameFilter() );

			serviceCollectionPopulator.Populate( services, lifeTime, doOpenGenericWireups );
			return services.BuildServiceProvider();

			void SetupTypeCache( AutoType autoType, Type type )
			{
				typeCacheMock.Setup( x => x.GetTypes( It.Is<AutoType>( t => t == autoType ) ) )
					.Returns( new List<Type>
					{
						type
					} );
			}
		}
	}

	internal class SuperLiberalTypeNameFilter : IServiceTypeNameFilter
	{
		public bool IsMatch( Type serviceType, Type implementationType ) => true;
	}
}