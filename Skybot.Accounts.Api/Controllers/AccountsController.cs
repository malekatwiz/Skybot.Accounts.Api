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
        [HttpPut]
        public async Task<IActionResult> Create([FromBody] UserAccountModel model)
        {
            if (ModelIsValid(model))
            {
                var account = await _accountService.NewAccount(model);

                if (string.IsNullOrEmpty(account?.Id))
                {
                    return new ObjectResult(account)
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }

                return Ok(account);
            }
            return BadRequest();
        }

        private static bool ModelIsValid(UserAccountModel model)
        {
            return model != null && !string.IsNullOrEmpty(model.PhoneNumber);
        }
    }
}