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

        [Route("check")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPost]
        public async Task<IActionResult> CheckAccount([FromBody]UserAccountModel model)
        {
            if (ModelIsValid(model))
            {
                var account = await _accountService.GetAccountByPhoneNumber(model.PhoneNumber);

                return account == null ? (IActionResult)NotFound() : Ok();
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
                var account = await _accountService.NewAccount(model);

                if (account != null && !account.Id.Equals(Guid.Empty))
                {
                    //TODO: Change Uri
                    return new CreatedResult(new Uri($"https://accounts.skybot.io/api/accounts/{account.Id}"), account);
                }
            }
            return BadRequest();
        }

        [Route("account/{phoneNumber}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetByPhoneNumber(string phoneNumber)
        {
            var account = await _accountService.GetAccountByPhoneNumber(phoneNumber);

            if (account != null)
            {
                return Ok(account);
            }
            return NotFound();
        }

        [Route("account/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var account = await _accountService.GeyById(id);

            if (account != null)
            {
                return Ok(account);
            }

            return NotFound();
        }

        private static bool ModelIsValid(UserAccountModel model)
        {
            return model != null && !string.IsNullOrEmpty(model.PhoneNumber);
        }
    }
}