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
		    var isMatch = new SameNameServiceTypeNameFilter()
			    .IsMatch( typeof( IThing ), typeof( Thing ) );

		    Assert.True( isMatch );
	    }
    }

	internal interface IThing
	{
	}

	internal class Thing
	{
	}
}
