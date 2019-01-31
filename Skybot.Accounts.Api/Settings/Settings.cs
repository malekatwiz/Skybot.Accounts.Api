using System;
using Microsoft.Extensions.Configuration;

namespace Skybot.Accounts.Api.Settings
{
    public class Settings : ISettings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Uri SkybotDbEndpoint => new Uri(_configuration["SkybotDb:Uri"]);

        public string SkybotDbAuthKey => _configuration["SkybotDb:AuthKey"];

        public string SkybotAccountsCollectionId => "Accounts";

        public string SkybotDbId => "Skybot";
    }
}
