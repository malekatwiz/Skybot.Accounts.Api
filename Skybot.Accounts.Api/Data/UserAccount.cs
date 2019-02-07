using System;
using Newtonsoft.Json;

namespace Skybot.Accounts.Api.Data
{
    public class UserAccount
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string AccessCode { get; set; }
        public DateTime AccessCodeExpiry { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
