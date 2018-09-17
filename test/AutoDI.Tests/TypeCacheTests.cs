using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using Xunit;

namespace AutoDI.Tests
{
	public class TypeCacheTests
	{
		private readonly Mock<IAssemblyProvider> _assemblyProviderMock;

		public TypeCacheTests()
		{
			_assemblyProviderMock = new Mock<IAssemblyProvider>();
			_assemblyProviderMock.Setup( x => x.GetAssemblies() )
				.Returns( new List<Assembly>
				{
					typeof( ImplementationsFinderTests ).Assembly
				} );
		}

		[Fact]
		public void FindImplementations_DoesCacheAssemblies_ForSubsequentCalls()
		{
			var typeCache = new TypeCache( _assemblyProviderMock.Object );

			Enumerable.Range( 0, 2 )
				.ToList()
				.ForEach( x => typeCache.GetTypes( AutoType.Implementation ) );

			_assemblyProviderMock.Verify( x => x.GetAssemblies(), Times.Once );
		}

		[Fact]
		public void FindImplementations_DoesNotReturnNonPublicImplementations()
		{
			var typeCache = new TypeCache( _assemblyProviderMock.Object );
			var @internal = typeCache
				.GetTypes( AutoType.Implementation )
				.FirstOrDefault( x => x == typeof( IInternal ) );

			Assert.Null( @internal );
		}
	}
}