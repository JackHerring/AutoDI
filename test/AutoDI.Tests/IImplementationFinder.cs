using System;
using System.Collections.Generic;

namespace AutoDI.Tests
{
	public interface IImplementationFinder
	{
		List<Type> FindImplemenationsOf( Type serviceType );
	}
}