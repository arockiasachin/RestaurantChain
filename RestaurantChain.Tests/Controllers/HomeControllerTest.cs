using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantChain;
using RestaurantChain.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RestaurantChain.Models;
namespace RestaurantChain.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            TestingController controller = new TestingController();

            // Act
            ViewResult result = controller.Index() as ViewResult;
           


            // Assert
            Assert.IsNotNull(result);
           
        }
        [TestMethod]
        public void CartTesting()
        {
            TestingController controller = new TestingController();
            ViewResult result = controller.CartTest() as ViewResult;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void CategoryTesting()
        {
            TestingController controller = new TestingController();
            ViewResult result = controller.CategoryTest() as ViewResult;
            Assert.IsNotNull(result);
        }

    }
}
