using System.Collections.Generic;
using System.Reflection;

namespace AutoDI.AspNetCore
{
	internal class AssemblyProviderAdapter : IAssemblyProvider
	{
		private readonly Assembly _assembly;

		public AssemblyProviderAdapter( Assembly assembly )
		{
			_assembly = assembly;
		}

		public List<Assembly> GetAssemblies()
		{
			return new List<Assembly>
			{
				_assembly
			};
		}
	}
}