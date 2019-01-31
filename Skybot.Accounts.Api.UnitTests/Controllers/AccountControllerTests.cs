using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skybot.Accounts.Api.Controllers;
using Skybot.Accounts.Api.Data;
using Skybot.Accounts.Api.Models;
using Skybot.Accounts.Api.Services.Accounts;
using System;
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
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountByPhoneNumber(It.IsAny<string>()))
                .Returns(Task.FromResult((UserAccount)null));

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.CheckAccount(new UserAccountModel
            {
                PhoneNumber = phoneNumber
            });

            var notFoundResult = result as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task Check_ReturnsBadRequest_WhenModelIsEmpty()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.CheckAccount(null);
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Check_ReturnsBadRequest_WhenPhoneNumberisMissing()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.CheckAccount(new UserAccountModel());
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Check_ReturnsOk_WhenPhoneNumberExists()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountByPhoneNumber(phoneNumber))
                .Returns(Task.FromResult(new UserAccount { PhoneNumber = phoneNumber }));

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.CheckAccount(
                new UserAccountModel { PhoneNumber = phoneNumber });
            var okResult = result as OkResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(okResult.StatusCode, (int)HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Create_ReturnsBadRequest_WhenModelIsEmpty()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.Create(null);
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Create_ReturnsBadRequest_WhenPhoneNumberIsMissing()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.Create(new UserAccountModel());
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Create_ReturnsInternalServerError_WhenServiceFailsToCreate()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.NewAccount(It.IsAny<UserAccountModel>()))
                .Returns(Task.FromResult((UserAccount)null));

            var accountController = new AccountsController(accountServiceMock.Object);

            var result = await accountController.Create(new UserAccountModel
            {
                PhoneNumber = phoneNumber
            });
            var objectResult = result as ObjectResult;

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(objectResult.StatusCode, (int)HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        public async Task Create_ReturnsUserAccount_WhenAccountIsCreatedSuccessfully()
        {
            var testAccountModel = new UserAccountModel
            {
                PhoneNumber = phoneNumber
            };
            var testUserAccount = new UserAccount
            {
                Id = new Guid().ToString(),
                PhoneNumber = testAccountModel.PhoneNumber,
            };

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.NewAccount(testAccountModel))
                .Returns(Task.FromResult(testUserAccount));

            var accountController = new AccountsController(accountServiceMock.Object);

            var result = await accountController.Create(new UserAccountModel
            {
                PhoneNumber = phoneNumber
            });

            var okObjectResult = result as OkObjectResult;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(okObjectResult.Value, testUserAccount);
        }
    }
}
