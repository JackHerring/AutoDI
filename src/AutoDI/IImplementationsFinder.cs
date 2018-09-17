using System;
using System.Collections.Generic;

namespace AutoDI
{
	internal interface IImplementationsFinder
	{
		List<Type> FindImplemenationsOf( Type serviceType );
	}
}