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
        const string PhoneNumber = "+10001112222";

        [TestMethod]
        public async Task Check_ReturnsNotFound_WhenPhoneNumberDoesNotExist()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountByPhoneNumber(It.IsAny<string>()))
                .Returns(Task.FromResult((UserAccount)null))
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.CheckAccount(new UserAccountModel
            {
                PhoneNumber = PhoneNumber
            });

            var notFoundResult = result as NotFoundResult;

            accountServiceMock.Verify(x => x.GetAccountByPhoneNumber(It.IsAny<string>()), Times.Exactly(1));
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

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Check_ReturnsBadRequest_WhenPhoneNumberisMissing()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.CheckAccount(new UserAccountModel());
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Check_ReturnsOk_WhenPhoneNumberExists()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountByPhoneNumber(PhoneNumber))
                .Returns(Task.FromResult(CreateTestAccount()))
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.CheckAccount(
                new UserAccountModel { PhoneNumber = PhoneNumber });
            var okResult = result as OkResult;

            accountServiceMock.Verify(x => x.GetAccountByPhoneNumber(PhoneNumber), Times.Exactly(1));

            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, (int)HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Create_ReturnsBadRequest_WhenModelIsEmpty()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.Create(null);
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Create_ReturnsBadRequest_WhenPhoneNumberIsMissing()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.Create(new UserAccountModel());
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Create_ReturnsBadRequest_WhenServiceFailsToCreateAccount()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.NewAccount(It.IsAny<UserAccountModel>()))
                .Returns(Task.FromResult((UserAccount)null))
                .Verifiable();

            var accountController = new AccountsController(accountServiceMock.Object);

            var result = await accountController.Create(new UserAccountModel
            {
                PhoneNumber = PhoneNumber
            });
            var badRequestResult = result as BadRequestResult;

            accountServiceMock.Verify(x => x.NewAccount(It.IsAny<UserAccountModel>()), Times.Exactly(1));

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Create_ReturnsUserAccount_WhenAccountIsCreatedSuccessfully()
        {
            var testAccountModel = new UserAccountModel
            {
                PhoneNumber = PhoneNumber
            };
            var testUserAccount = CreateTestAccount();

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.NewAccount(testAccountModel))
                .Returns(Task.FromResult(testUserAccount))
                .Verifiable();

            var accountController = new AccountsController(accountServiceMock.Object);

            var result = await accountController.Create(testAccountModel);

            var createdObjectResult = result as CreatedResult;

            accountServiceMock.Verify(x => x.NewAccount(testAccountModel), Times.Exactly(1));

            //TODO: Check for returned Uri
            Assert.IsNotNull(createdObjectResult);
            Assert.AreEqual(createdObjectResult.StatusCode, (int)HttpStatusCode.Created);
            Assert.AreEqual(createdObjectResult.Value, testUserAccount);
        }

        [TestMethod]
        public async Task GetByPhoneNumber_ReturnsAccount_WhenAccountIsFound()
        {
            var testAccount = CreateTestAccount();

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetAccountByPhoneNumber(PhoneNumber))
                .Returns(Task.FromResult(testAccount))
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.GetByPhoneNumber(PhoneNumber);
            var okObjectResult = result as OkObjectResult;

            accountServiceMock.Verify(x => x.GetAccountByPhoneNumber(PhoneNumber), Times.Exactly(1));

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(okObjectResult.Value, testAccount);
        }

        [TestMethod]
        public async Task GetByPhoneNumber_ReturnsNotFound_WhenAccountDoesNotExist()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GeyById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((UserAccount) null))
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.GetById(It.IsAny<Guid>());
            var notFoundResult = result as NotFoundResult;

            accountServiceMock.Verify(x => x.GeyById(It.IsAny<Guid>()), Times.Exactly(1));

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task GetById_ReturnsAccount_WhenAccountIsFound()
        {
            var id = Guid.NewGuid();
            var testUserAccount = CreateTestAccount();

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GeyById(id))
                .Returns(Task.FromResult(testUserAccount))
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.GetById(id);
            var okObjectResult = result as OkObjectResult;

            accountServiceMock.Verify(x => x.GeyById(id), Times.Exactly(1));

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(okObjectResult.Value, testUserAccount);
        }

        [TestMethod]
        public async Task GetById_ReturnsNotFound_WhenAccountDoesNotExist()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GeyById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((UserAccount)null))
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.GetById(It.IsAny<Guid>());
            var notFoundResult = result as NotFoundResult;

            accountServiceMock.Verify(x => x.GeyById(It.IsAny<Guid>()), Times.Exactly(1));

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }

        private UserAccount CreateTestAccount()
        {
            return new UserAccount
            {
                Id = Guid.NewGuid(),
                PhoneNumber = PhoneNumber
            };
        }
    }
}
