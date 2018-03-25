using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetrospectiveHelper;
using RetrospectiveHelper.Controllers;

namespace RetrospectiveHelper.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Przygotowanie
            HomeController controller = new HomeController();

            // Wykonanie
            ViewResult result = controller.Index() as ViewResult;

            // Sprawdzenie
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
