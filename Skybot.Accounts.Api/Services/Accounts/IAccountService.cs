﻿using System;
using System.Threading.Tasks;
using Skybot.Accounts.Api.Data;
using Skybot.Accounts.Api.Models;

namespace Skybot.Accounts.Api.Services.Accounts
{
    public interface IAccountService
    {
        Task<UserAccount> New(UserAccountModel model);
        UserAccount GetByPhoneNumber(string phoneNumber);
        Task<UserAccount> Get(string id);
        Task<string> GenerateAccessCode(string phoneNumber);
        bool ValidateAccessCode(string phoneNumber, string accessCode);
    }
}
