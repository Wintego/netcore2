using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.Interfaces.Api;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebApiControllerTests
    {
        private WebApiController _Controller;

        [TestInitialize]
        public void Initialize()
        {
            var value_service_mock = new Mock<IValuesService>();
            value_service_mock.Setup(service => service.GetAsync()).ReturnsAsync(new[] { "123", "456", "789" });
            _Controller = new WebApiController(value_service_mock.Object);
        }
        [TestMethod]
        public async Task Index_Method_returns_View_With_Value()
        {
            var result = await _Controller.Index();
            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);

            const int expected_count = 3;
            Assert.Equal(expected_count, model.Count());
        }
    }
}
