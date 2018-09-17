using System;
using System.Collections.Generic;

namespace AutoDI
{
	internal interface ITypeCache
	{
		List<Type> GetTypes( AutoType autoType );
	}
}