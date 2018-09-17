using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDI.Tests
{
	public interface ISingle
	{
	}

	public class Single : ISingle
	{
	}

	public interface IMany
	{
	}

	public class FirstOfMany : IMany
	{
	}

	public class SecondOfMany : IMany
	{
	}

	public interface IAbstract
	{
	}

	public abstract class Abstract : IAbstract
	{
	}

	public interface IInternal
	{
	}

	internal class Internal : IInternal
	{
	}

	public interface ISingleGeneric<T>
	{
	}

	public class SingleOpenGeneric<T> : ISingleGeneric<T>
	{
	}

	public class SingleGeneric : ISingleGeneric<string>
	{
	}
}
