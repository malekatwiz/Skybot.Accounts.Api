using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skybot.Accounts.Api.Models;
using Skybot.Accounts.Api.Services.Accounts;

namespace Skybot.Accounts.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("check/{phoneNumber}")]
        [ProducesResponseType((int)HttpStatusCode.Found)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet]
        public IActionResult CheckAccount(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var account = _accountService.GetByPhoneNumber(phoneNumber);

                return account == null ? NotFound() : StatusCode((int) HttpStatusCode.Found);
            }
            return BadRequest();
        }

        [Route("create")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPut]
        public async Task<IActionResult> Create([FromBody]UserAccountModel model)
        {
            if (ModelIsValid(model))
            {
                var account = await _accountService.New(model);

                if (account != null && !account.Id.Equals(Guid.Empty))
                {
                    //TODO: Change Uri
                    return new CreatedResult(new Uri($"https://accounts.skybot.io/api/accounts/{account.Id}"), account);
                }
            }
            return BadRequest();
        }

        [Route("{phoneNumber}")]
        [ProducesResponseType((int)HttpStatusCode.Found)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet]
        public IActionResult GetByPhoneNumber(string phoneNumber)
        {
            var account = _accountService.GetByPhoneNumber(phoneNumber);

            if (account != null)
            {
                return new ObjectResult(account) {StatusCode = (int) HttpStatusCode.Found};
            }
            return NotFound();
        }

        [Route("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var account = await _accountService.Get(id);

            if (account != null)
            {
                return Ok(account);
            }

            return NotFound();
        }

        [Route("generatetoken")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody]VerificationCodeModel model)
        {
            var userAccount = _accountService.GetByPhoneNumber(model.PhoneNumber);
            if (userAccount != null && !userAccount.Id.Equals(Guid.Empty))
            {
                var accessCode = await _accountService.GenerateCode(userAccount.Id);
                if (!string.IsNullOrEmpty(accessCode))
                {
                    return Ok(accessCode);
                }
            }

            return new NotFoundResult();
        }


        [Route("validatetoken")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotAcceptable)]
        [HttpPost]
        public IActionResult ValidateToken([FromBody] VerificationCodeModel model)
        {
            if (_accountService.ValidateToken(model.PhoneNumber, model.Code))
            {
                return new AcceptedResult();
            }

            return new ObjectResult(HttpStatusCode.NotAcceptable);
        }

        private static bool ModelIsValid(UserAccountModel model)
        {
            return model != null && !string.IsNullOrEmpty(model.PhoneNumber);
        }
    }
}