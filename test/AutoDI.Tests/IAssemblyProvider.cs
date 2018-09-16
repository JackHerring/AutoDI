using System.Collections.Generic;
using System.Reflection;

namespace AutoDI.Tests
{
	public interface IAssemblyProvider
	{
		List<Assembly> GetAssemblies();
	}
}