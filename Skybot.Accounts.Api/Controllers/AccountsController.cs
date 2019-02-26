using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpGet]
        public IActionResult CheckAccount(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var account = _accountService.GetByPhoneNumber(phoneNumber);

                return Ok(new HttpBaseResponse
                {
                    Success = account != null
                });
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet]
        public IActionResult GetByPhoneNumber(string phoneNumber)
        {
            var account = _accountService.GetByPhoneNumber(phoneNumber);

            var response = new HttpBaseResponse
            {
                Success = false
            };
            if (account != null)
            {
                response.Success = true;
                response.Object = JsonConvert.SerializeObject(account);
            }

            return Ok(response);
        }

        [Route("{id}/details")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var account = await _accountService.Get(id);
            var response = new HttpBaseResponse
            {
                Success = false
            };
            if (account != null)
            {
                response.Success = true;
                response.Object = JsonConvert.SerializeObject(account);
            }

            return Ok(response);
        }

        [Route("GenerateAccessCode")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<IActionResult> GenerateAccessCode([FromBody]VerificationCodeModel model)
        {
            var userAccount = _accountService.GetByPhoneNumber(model.PhoneNumber);
            var response = new HttpBaseResponse
            {
                Success = false
            };
            if (!string.IsNullOrEmpty(userAccount?.Id))
            {
                var accessCode = await _accountService.GenerateAccessCode(userAccount.Id);
                if (!string.IsNullOrEmpty(accessCode))
                {
                    response.Success = true;
                    response.Object = JsonConvert.SerializeObject(accessCode);
                }
            }

            return Ok(response);
        }


        [Route("ValidateAccessCode")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpPost]
        public IActionResult ValidateAccessCode([FromBody] VerificationCodeModel model)
        {
            var response = new HttpBaseResponse
            {
                Success = false
            };
            if (_accountService.ValidateAccessCode(model.PhoneNumber, model.Code))
            {
                response.Success = true;
            }

            return Ok(response);
        }

        private static bool ModelIsValid(UserAccountModel model)
        {
            return model != null && !string.IsNullOrEmpty(model.PhoneNumber);
        }
    }
}