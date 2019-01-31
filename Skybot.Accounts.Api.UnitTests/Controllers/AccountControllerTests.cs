using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skybot.Accounts.Api.Controllers;
using Skybot.Accounts.Api.Data;
using Skybot.Accounts.Api.Models;
using System.Net;
using System.Threading.Tasks;

namespace Skybot.Accounts.Api.UnitTests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        const string phoneNumber = "+10001112222";

        [TestMethod]
        public async Task Check_ReturnsNotFound_WhenPhoneNumberDoesNotExist()
        {
            var accountsRepositoryMock = new Mock<IAccountsRepository>();
            accountsRepositoryMock.Setup(x => x.GetAccountByPhoneNumber(It.IsAny<string>()))
                .Returns(Task.FromResult((UserAccountModel)null));

            var accountsController = new AccountsController(accountsRepositoryMock.Object);

            var result = await accountsController.CheckAccount(new UserAccountModel
            {
                PhoneNumber = phoneNumber
            });

            var notFoundResult = result as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task Check_ReturnsBadRequest_WhenModelIsNull()
        {
            var accountsRepositoryMock = new Mock<IAccountsRepository>();

            var accountsController = new AccountsController(accountsRepositoryMock.Object);

            var result = await accountsController.CheckAccount(null);
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Check_ReturnsBadRequest_WhenPhoneNumberisMissing()
        {
            var accountsRepositoryMock = new Mock<IAccountsRepository>();

            var accountsController = new AccountsController(accountsRepositoryMock.Object);

            var result = await accountsController.CheckAccount(new UserAccountModel());
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Check_ReturnsOk_WhenPhoneNumberExists()
        {
            var accountsRepositoryMock = new Mock<IAccountsRepository>();
            accountsRepositoryMock.Setup(x => x.GetAccountByPhoneNumber(phoneNumber))
                .Returns(Task.FromResult(new UserAccountModel { PhoneNumber = phoneNumber}));

            var accountsController = new AccountsController(accountsRepositoryMock.Object);

            var result = await accountsController.CheckAccount(
                new UserAccountModel { PhoneNumber = phoneNumber });
            var okResult = result as OkResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(okResult.StatusCode, (int)HttpStatusCode.OK);
        }
    }
}
