using System.Collections.Generic;
using System.Reflection;

namespace AutoDI
{
	public interface IAssemblyProvider
	{
		List<Assembly> GetAssemblies();
	}
}