using System;

namespace AutoDI.AspNetCore
{
	public class Configuration
	{
		internal LifeTime DefaultLifeTime { get; set; }
		internal Type NamingConventionProviderType { get; set; }
		internal bool DoOpenGenericWireups { get; set; }

		public Configuration Singleton()
		{
			DefaultLifeTime = LifeTime.Singleton;
			return this;
		}

		public Configuration Transient()
		{
			DefaultLifeTime = LifeTime.Transient;
			return this;
		}

		public Configuration WithNamingConvention<TNamingConventionProvider>()
			where TNamingConventionProvider : IServiceTypeNameFilter, new()
		{
			NamingConventionProviderType = typeof( TNamingConventionProvider );
			return this;
		}

		public Configuration WithOpenGenericWireups()
		{
			DoOpenGenericWireups = true;
			return this;
		}
	}
}