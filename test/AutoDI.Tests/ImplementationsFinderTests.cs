using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using Xunit;

namespace AutoDI.Tests
{
	public class ImplementationsFinderTests
	{
		private readonly Mock<IAssemblyProvider> _assemblyProvider;

		public ImplementationsFinderTests()
		{
			_assemblyProvider = new Mock<IAssemblyProvider>();
			_assemblyProvider.Setup( x => x.GetAssemblies() )
				.Returns( new List<Assembly>
				{
					typeof( ImplementationsFinderTests ).Assembly
				} );
		}

		[Fact]
		public void FindImplementations_ReturnsSingleConcreteImplementation()
		{
			var singleImplementation = new ImplementationsFinder( _assemblyProvider.Object )
				.FindImplemenationsOf( typeof( ISingle ) ).Single();

			Assert.Equal( typeof( Single ), singleImplementation );
		}

		[Fact]
		public void FindImplementations_ReturnsMultipleConcreteImplementations()
		{
			var implementations = new ImplementationsFinder( _assemblyProvider.Object )
				.FindImplemenationsOf( typeof( IMany ) );

			var targetTypes = new[] {typeof( FirstOfMany ), typeof( SecondOfMany )};

			Assert.True( targetTypes.All( x => implementations.Contains( x ) ) );
		}

		[Fact]
		public void FindImplementations_DoesNotReturnAsbtractImplementations()
		{
			var implementations = new ImplementationsFinder( _assemblyProvider.Object )
				.FindImplemenationsOf( typeof( IAbstract ) );

			Assert.Empty( implementations );
		}

		[Fact]
		public void FindImplementations_DoesNotReturnNonPublicImplementations()
		{
			var @internal = new ImplementationsFinder( _assemblyProvider.Object )
				.FindImplemenationsOf( typeof( IInternal ) );

			Assert.Empty( @internal );
		}

		[Fact]
		public void FindImplementations_ReturnsOpenGenericTypes_ForSingleTypeParam()
		{
			var singleGenericImplementation = new ImplementationsFinder( _assemblyProvider.Object )
				.FindImplemenationsOf( typeof( ISingleOpenGeneric<> ) )
				.Single();

			Assert.Equal( typeof( SingleOpenGeneric<> ), singleGenericImplementation );
		}

		[Fact]
		public void FindImplementations_DoesCacheExportedTypes_ForSubsequentCalls()
		{
			var implementationsFinder = new ImplementationsFinder( _assemblyProvider.Object );

			Enumerable.Range( 0, 2 )
				.ToList()
				.ForEach( x => implementationsFinder.FindImplemenationsOf( typeof( IMany ) ) );

			_assemblyProvider.Verify( x => x.GetAssemblies(), Times.Once );
		}
	}

	public interface ISingle
	{
	}

	public class Single : ISingle
	{
	}

	public interface IMany
	{
	}

	public class FirstOfMany : IMany
	{
	}

	public class SecondOfMany : IMany
	{
	}

	public interface IAbstract
	{
	}

	public abstract class Abstract : IAbstract
	{
	}

	public interface IInternal
	{
	}

	internal class Internal : IInternal
	{
	}

	public interface ISingleOpenGeneric<T>
	{
	}

	public class SingleOpenGeneric<T> : ISingleOpenGeneric<T>
	{
	}
}