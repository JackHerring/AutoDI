using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AutoDI.Tests
{
    public class SameNameServiceTypeNameFilterTests
    {
	    [Fact]
	    public void ForImplementationTypeWithSameName_IsMatch()
	    {
		    Assert.True( new SameNameServiceTypeNameFilter().IsMatch( typeof( IThing ), typeof( Thing ) ) );
	    }
    }

	internal interface IThing
	{
	}

	internal class Thing
	{
	}
}
