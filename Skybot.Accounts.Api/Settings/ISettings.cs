using System;

namespace Skybot.Accounts.Api.Settings
{
    public interface ISettings
    {
        Uri SkybotDbEndpoint { get; }
        string SkybotDbAuthKey { get; }
        string SkybotAccountsCollectionId { get; }
        string SkybotDbId { get; }
    }
}
