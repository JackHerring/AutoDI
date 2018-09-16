using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AutoDI.Tests
{
    public class ServiceCollectionPopulatorTests
    {
    }

	internal class Configuration
	{
		internal ServiceType DefaultServiceType { get; set; }
	}

	internal enum ServiceType
	{
		Transient,
		Singleton,
		Scoped
	}

	internal interface IServiceCollectionPopulator
	{
		void Populate( IServiceCollection serviceCollection, Configuration configuration );
	}
}
