using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using InternalPortal.Models;
using InternalPortal.Models.Helpers;
namespace portal.tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
						var firstDate = new DateTime(2017,1,28);	
						var secondDate = new DateTime(2017,12,28);	
						var fy1 = FiscalYear.GetFiscalYearByDateTime(firstDate);
						var fy2 = FiscalYear.GetFiscalYearByDateTime(secondDate);
						Assert.AreEqual(fy1, "20162017");
						Assert.AreEqual(fy2, "20172018");
        }
				[TestMethod]
				public void TestMethod2() 
				{
			  		var startDate = new DateTime(2015,1,12);
						var endDate = new DateTime(2018,12,31);		
						var fys = FiscalYear.GetFiscalYearsFromTwoDates(startDate, endDate);

						Assert.AreEqual(fys, ["20152016,20162017,20172018,2018,2019"]);
				}
    }
}
