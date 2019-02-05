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
        const string PhoneNumber = "10001112222";

        [TestMethod]
        public void Check_ReturnsNotFound_WhenPhoneNumberDoesNotExist()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetByPhoneNumber(It.IsAny<string>()))
                .Returns((UserAccount)null)
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = accountsController.CheckAccount(PhoneNumber);

            var notFoundResult = result as NotFoundResult;

            accountServiceMock.Verify(x => x.GetByPhoneNumber(It.IsAny<string>()), Times.Exactly(1));
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void Check_ReturnsBadRequest_WhenModelIsEmpty()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = accountsController.CheckAccount(null);
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Check_ReturnsBadRequest_WhenPhoneNumberisMissing()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = accountsController.CheckAccount(string.Empty);
            var badRequestResult = result as BadRequestResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(badRequestResult.StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void Check_ReturnsFoundStatusCode_WhenPhoneNumberExists()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetByPhoneNumber(PhoneNumber))
                .Returns(CreateTestAccount())
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = accountsController.CheckAccount(PhoneNumber);
            var statusCodeResult = result as StatusCodeResult;

            accountServiceMock.Verify(x => x.GetByPhoneNumber(PhoneNumber), Times.Exactly(1));

            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(statusCodeResult.StatusCode, (int)HttpStatusCode.Found);
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
            accountServiceMock.Setup(x => x.New(It.IsAny<UserAccountModel>()))
                .Returns(Task.FromResult((UserAccount)null))
                .Verifiable();

            var accountController = new AccountsController(accountServiceMock.Object);

            var result = await accountController.Create(new UserAccountModel
            {
                PhoneNumber = PhoneNumber
            });
            var badRequestResult = result as BadRequestResult;

            accountServiceMock.Verify(x => x.New(It.IsAny<UserAccountModel>()), Times.Exactly(1));

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
            accountServiceMock.Setup(x => x.New(testAccountModel))
                .Returns(Task.FromResult(testUserAccount))
                .Verifiable();

            var accountController = new AccountsController(accountServiceMock.Object);

            var result = await accountController.Create(testAccountModel);

            var createdObjectResult = result as CreatedResult;

            accountServiceMock.Verify(x => x.New(testAccountModel), Times.Exactly(1));

            //TODO: Check for returned Uri
            Assert.IsNotNull(createdObjectResult);
            Assert.AreEqual(createdObjectResult.StatusCode, (int)HttpStatusCode.Created);
            Assert.AreEqual(createdObjectResult.Value, testUserAccount);
        }

        [TestMethod]
        public void GetByPhoneNumber_ReturnsAccount_WhenAccountIsFound()
        {
            var testAccount = CreateTestAccount();

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetByPhoneNumber(PhoneNumber))
                .Returns(testAccount)
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = accountsController.GetByPhoneNumber(PhoneNumber);
            var objectResult = result as ObjectResult;

            accountServiceMock.Verify(x => x.GetByPhoneNumber(PhoneNumber), Times.Exactly(1));

            Assert.IsNotNull(objectResult);
            Assert.AreEqual(objectResult.StatusCode, (int)HttpStatusCode.Found);
            Assert.AreEqual(objectResult.Value, testAccount);
        }

        [TestMethod]
        public async Task GetByPhoneNumber_ReturnsNotFound_WhenAccountDoesNotExist()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.Gey(It.IsAny<Guid>()))
                .Returns(Task.FromResult((UserAccount) null))
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.GetById(It.IsAny<Guid>());
            var notFoundResult = result as NotFoundResult;

            accountServiceMock.Verify(x => x.Gey(It.IsAny<Guid>()), Times.Exactly(1));

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, (int)HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task GetById_ReturnsAccount_WhenAccountIsFound()
        {
            var id = Guid.NewGuid();
            var testUserAccount = CreateTestAccount();

            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.Gey(id))
                .Returns(Task.FromResult(testUserAccount))
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.GetById(id);
            var okObjectResult = result as OkObjectResult;

            accountServiceMock.Verify(x => x.Gey(id), Times.Exactly(1));

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.AreEqual(okObjectResult.Value, testUserAccount);
        }

        [TestMethod]
        public async Task GetById_ReturnsNotFound_WhenAccountDoesNotExist()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.Gey(It.IsAny<Guid>()))
                .Returns(Task.FromResult((UserAccount)null))
                .Verifiable();

            var accountsController = new AccountsController(accountServiceMock.Object);

            var result = await accountsController.GetById(It.IsAny<Guid>());
            var notFoundResult = result as NotFoundResult;

            accountServiceMock.Verify(x => x.Gey(It.IsAny<Guid>()), Times.Exactly(1));

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
