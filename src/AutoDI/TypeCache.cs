using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoDI
{
	internal class TypeCache : ITypeCache
	{
		private List<Type> _typeCache;
		private readonly IAssemblyProvider _assemblyProvider;

		public TypeCache( IAssemblyProvider assemblyProvider )
		{
			_assemblyProvider = assemblyProvider;
		}

		private List<Type> GetPublicExportedTypes()
		{
			return _typeCache ?? ( _typeCache = _assemblyProvider
				       .GetAssemblies()
				       .SelectMany( x => x.GetExportedTypes() )
				       .Where( x => x.IsPublic ).ToList() );
		}

		public List<Type> GetTypes( AutoType autoType )
		{
			var types = GetPublicExportedTypes();
			return autoType == AutoType.Abstraction 
				? types.Where( x => x.IsInterface ).ToList()
				: types;
		}
	}
}