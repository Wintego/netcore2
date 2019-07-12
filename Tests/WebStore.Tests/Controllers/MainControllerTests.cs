using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Controllers;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class MainControllerTests
    {
        private MainController controller;

        [TestInitialize]
        public void Initialize()
        {
            controller = new MainController();
        }

        [TestMethod]
        public void Index_Returns_View()
        {
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void ContactUs_Returns_View()
        {
            var result = controller.ContactUs();
            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void BlogSingle_Returns_View()
        {
            var result = controller.BlogSingle();
            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void Blog_Returns_View()
        {
            var result = controller.Blog();
            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void NotFound_Returns_View()
        {
            var result = controller.NotFound();
            Assert.IsType<ViewResult>(result);
        }
        [TestMethod]
        public void ErrorStatusCode_404_Redirect_to_Error404()
        {
            var result = controller.ErrorStatusCode("404");
            var redirect_to_action = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirect_to_action.ControllerName);
            Assert.Equal(nameof(controller.NotFound), redirect_to_action.ActionName);
        }
    }
}
