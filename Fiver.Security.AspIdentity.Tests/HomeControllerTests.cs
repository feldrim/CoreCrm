using Fiver.Security.AspIdentity.Controllers;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fiver.Security.AspIdentity.Tests
{
   [TestClass]
   public class HomeControllerTests
   {
      [TestMethod]
      public void Index_Action_Returns_View()
      {
         // Arrange
         var controller = new HomeController();

         // Act
         var result = controller.Index();

         // Assert
         result.Should().BeViewResult();
      }
   }
}
