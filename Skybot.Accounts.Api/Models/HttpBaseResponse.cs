using System;

namespace Skybot.Accounts.Api.Models
{
    [Serializable]
    public class HttpBaseResponse
    {
        public bool Success { get; set; }
        public string Object { get; set; }
    }
}
