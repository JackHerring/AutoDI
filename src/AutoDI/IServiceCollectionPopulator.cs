using Microsoft.Extensions.DependencyInjection;

namespace AutoDI
{
	internal interface IServiceCollectionPopulator
	{
		void Populate( IServiceCollection serviceCollection, LifeTime lifeTime, bool includeOpenGenericWireup );
	}
}