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

                if (!string.IsNullOrEmpty(account?.Id))
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

        [Route("{id}/details")]
        [ProducesResponseType((int)HttpStatusCode.Found)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var account = await _accountService.Get(id);

            if (account != null)
            {
                return new ObjectResult(account) { StatusCode = (int)HttpStatusCode.Found };
            }

            return NotFound();
        }

        [Route("GenerateAccessCode")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPost]
        public async Task<IActionResult> GenerateAccessCode([FromBody]VerificationCodeModel model)
        {
            var userAccount = _accountService.GetByPhoneNumber(model.PhoneNumber);
            if (!string.IsNullOrEmpty(userAccount?.Id))
            {
                var accessCode = await _accountService.GenerateAccessCode(userAccount.Id);
                if (!string.IsNullOrEmpty(accessCode))
                {
                    return Ok(accessCode);
                }
            }

            return new NotFoundResult();
        }


        [Route("ValidateAccessCode")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotAcceptable)]
        [HttpPost]
        public IActionResult ValidateAccessCode([FromBody] VerificationCodeModel model)
        {
            if (_accountService.ValidateAccessCode(model.PhoneNumber, model.Code))
            {
                return new AcceptedResult();
            }

            return new ObjectResult(null){StatusCode = (int)HttpStatusCode.NotAcceptable };
        }

        private static bool ModelIsValid(UserAccountModel model)
        {
            return model != null && !string.IsNullOrEmpty(model.PhoneNumber);
        }
    }
}