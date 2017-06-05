using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using InternalPortal.Models;
using InternalPortal.Models.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using InternalPortal.Controllers;
using Moq;
using InternalPortal.Models.Portal.Interfaces;
using System.Net.Http;
using InternalPortal.Models.Portal.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace portal.tests
{
    [TestClass]
    public class HelperMethodsTests
    {
        [TestMethod]
        public void GetFiscalYearByDateTime()
        {
		var firstDate = new DateTime(2017,1,28);	
		var secondDate = new DateTime(2017,12,28);	
		var fy1 = FiscalYear.GetFiscalYearByDateTime(firstDate);
		var fy2 = FiscalYear.GetFiscalYearByDateTime(secondDate);
		Assert.AreEqual(fy1, "20162017");
		Assert.AreEqual(fy2, "20172018");
        }
        [TestMethod]
        public void GetFiscalYearByDateTimeRange()
        {
            var testFys = new List<string>
            {
                "20162017",
                "20172018",
                "20182019"
            };
            var firstDate = new DateTime(2017,1,28);	
		    var secondDate = new DateTime(2018,12,28);	
		    List<string> fys = FiscalYear.GetFiscalYearByDateTimeRange(firstDate, secondDate);            
		    CollectionAssert.AreEqual(testFys, fys);
        }
        [TestMethod]
        public void GetTest()
        {
            int[] actualArray = { 1, 3, 7 };
            CollectionAssert.AreEqual(new int[] { 1, 3, 7 }, actualArray);
        }
    }
    [TestClass]
    public class DAL
    {
        public List<Project> fakeProjects;
        public Project fakeProject;
        [TestInitialize]
        public void Setup()
        {
            fakeProjects = new List<Project>
            {
                new Project
                {
                    GcimsClientId = "GCIMSUnit",
                    Title = "Test Title"
                },
                new Project
                {
                    GcimsClientId = "GCIMSUnit",
                    Title = "Test Title1"
                }

            };
            fakeProject = new Project
            {
                GcimsClientId = "GCIMSUnit",
                Title = "Test Title2"
            };
        }
        [TestMethod]
        public void GetAllProject()
        {
            
          

            Mock<IProjectRepository> projectRepo = new Mock<IProjectRepository>();
            projectRepo.Setup(x => x.GetAll()).Returns(fakeProjects);

            CollectionAssert.AreEqual(fakeProjects, projectRepo.Object.GetAll().ToList());
           
        }
        [TestMethod]
        public void AddProject()
        {

            var mock = FakeUnitOfWork();
            var p1 = new Project { Title = "fred" };
            var p2 = new Project { Title = "fred1" };
            var p3 = new Project { Title = "fred2" };
            mock.Object.Projects.Add(p1);
            mock.Object.Projects.Add(p2);
            mock.Object.Projects.Add(p3);
            mock.Object.Projects.RemoveRange(new List<Project>() { p1, p2 });
           
            Assert.IsTrue(mock.Object.Projects.GetAll().Count() == 1);           
        }

        public Mock<IUnitOfWork> FakeUnitOfWork()
        {
            var allProjects = new List<Project>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(x => x.Projects.Add(It.IsAny<Project>())).Callback<Project>((p) =>
            {
                allProjects.Add(p);

            });
            mockUnitOfWork.Setup(x => x.Projects.Remove(It.IsAny<Project>())).Callback<Project>((p) =>
            {
                allProjects.Remove(p);
            });
            mockUnitOfWork.Setup(x => x.Projects.RemoveRange(It.IsAny<List<Project>>())).Callback<List<Project>>((p) =>
            {
                allProjects.RemoveAll(p.Contains);
            });


            mockUnitOfWork.Setup(x => x.Projects.GetAll()).Returns(allProjects);
            return mockUnitOfWork;
        }
        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }




    }
}
