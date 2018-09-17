using System.Linq;
using Moq;
using Xunit;

namespace AutoDI.Tests
{
	public class ImplementationsFinderTests
	{
		private readonly Mock<ITypeCache> _typeCacheMock;

		public ImplementationsFinderTests()
		{
			_typeCacheMock = new Mock<ITypeCache>();
			_typeCacheMock.Setup( x => x.GetTypes( It.IsAny<AutoType>() ) )
				.Returns( typeof( ImplementationsFinderTests ).Assembly.GetTypes().ToList() );
		}

		[Fact]
		public void FindImplementations_ReturnsSingleConcreteImplementation()
		{
			var singleImplementation = new ImplementationsFinder( _typeCacheMock.Object )
				.FindImplemenationsOf( typeof( ISingle ) ).Single();

			Assert.Equal( typeof( Single ), singleImplementation );
		}

		[Fact]
		public void FindImplementations_ReturnsMultipleConcreteImplementations()
		{
			var implementations = new ImplementationsFinder( _typeCacheMock.Object )
				.FindImplemenationsOf( typeof( IMany ) );

			var targetTypes = new[] {typeof( FirstOfMany ), typeof( SecondOfMany )};

			Assert.True( targetTypes.All( x => implementations.Contains( x ) ) );
		}

		[Fact]
		public void FindImplementations_DoesNotReturnAsbtractImplementations()
		{
			var implementations = new ImplementationsFinder( _typeCacheMock.Object )
				.FindImplemenationsOf( typeof( IAbstract ) );

			Assert.Empty( implementations );
		}

		[Fact]
		public void FindImplementations_ReturnsGenericTypes_ForSingleTypeParameter()
		{
			var implementations = new ImplementationsFinder( _typeCacheMock.Object )
				.FindImplemenationsOf( typeof( ISingleGeneric<> ) );

			var genericTypes = new[] {typeof( SingleOpenGeneric<> ), typeof( SingleGeneric )};
			Assert.True( genericTypes.All( x => implementations.Contains( x ) ) );
		}
	}
}