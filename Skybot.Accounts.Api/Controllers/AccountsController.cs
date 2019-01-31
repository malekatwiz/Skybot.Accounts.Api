using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skybot.Accounts.Api.Data;
using Skybot.Accounts.Api.Models;

namespace Skybot.Accounts.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsRepository _accountsRepository;

        public AccountsController(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        [Route("check")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpPost]
        public async Task<IActionResult> CheckAccount([FromBody]UserAccountModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.PhoneNumber))
            {
                return BadRequest();
            }

            var account = await _accountsRepository.GetAccountByPhoneNumber(model.PhoneNumber);

            return account == null ? (IActionResult) NotFound() : Ok();
        }
    }
}